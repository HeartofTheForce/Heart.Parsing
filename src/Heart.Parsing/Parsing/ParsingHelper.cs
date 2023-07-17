using System;
using System.Collections.Generic;
using System.IO;
using Heart.Parsing.Patterns;

namespace Heart.Parsing
{
    public static class ParsingHelper
    {
        private static readonly TerminalPattern s_regex = TerminalPattern.FromRegex("`(?:``|[^`])*`");
        private static readonly TerminalPattern s_plainText = TerminalPattern.FromRegex("'(?:''|[^'])*'");
        private static readonly TerminalPattern s_identifier = TerminalPattern.FromRegex("[_a-zA-Z]\\w*");
        private static readonly TerminalPattern s_digits = TerminalPattern.FromRegex("\\d+");
        private static readonly TerminalPattern s_none = TerminalPattern.FromPlainText("none");

        private static IPattern Label(this IPattern pattern, string label)
        {
            return LabelPattern.Create(label, pattern);
        }

        public static PatternParser BuildPatternParser(string path)
        {
            string input = File.ReadAllText(path);

            var pegParser = CreatePegParser();
            var pattern = LookupPattern.Create("rule").MinOrMore(1).Trim();
            var result = pegParser.MatchComplete(pattern, input);

            var output = new PatternParser();
            var rules = (QuantifierNode)result;
            foreach (var child in rules.Children)
            {
                var sequenceNode = (SequenceNode)child;

                var ruleHeadNode = (SequenceNode)sequenceNode.Children[0];
                var ruleNameNode = (ValueNode)ruleHeadNode.Children[0];
                string ruleName = ruleNameNode.Value;

                var labelNode = (LabelNode)sequenceNode.Children[1];
                IPattern rulePattern;
                switch (labelNode.Label)
                {
                    case "expr": rulePattern = BuildExpr(labelNode.Node); break;
                    case "choice": rulePattern = BuildChoice(labelNode.Node); break;
                    default: throw new NotImplementedException();
                }

                string? label = TryParseLabel(ruleHeadNode.Children[1]);
                if (label != null)
                    rulePattern = LabelPattern.Create(label, rulePattern);

                output.Patterns[ruleName] = rulePattern;
            }

            return output;
        }

        private static PatternParser CreatePegParser()
        {
            var parser = new PatternParser();

            parser.Patterns["_"] =
                ChoicePattern.Create()
                    .Or(TerminalPattern.FromRegex("#.*"))
                    .Or(TerminalPattern.FromRegex("\\s+"))
                    .MinOrMore(0);

            parser.Patterns["rule"] = SequencePattern.Create()
                .Then(LookupPattern.Create("rule_head"))
                .Then(ChoicePattern.Create()
                    .Or(LookupPattern.Create("expr").Label("expr"))
                    .Or(LookupPattern.Create("choice").Label("choice")));

            parser.Patterns["rule_head"] = SequencePattern.Create()
                .Then(s_identifier)
                .Then(LookupPattern.Create("label"))
                .Then(TerminalPattern.FromPlainText("->"));

            parser.Patterns["choice"] = SequencePattern.Create()
                .Then(LookupPattern.Create("sequence"))
                .Then(SequencePattern.Create()
                    .Then(TerminalPattern.FromPlainText("/"))
                    .Then(LookupPattern.Create("sequence"))
                    .At(1)
                    .MinOrMore(0));

            parser.Patterns["sequence"] = SequencePattern.Create()
                .Then(LookupPattern.Create("predicate"))
                .Then(LookupPattern.Create("label"))
                .MinOrMore(1);

            parser.Patterns["label"] = SequencePattern.Create()
                .Then(TerminalPattern.FromPlainText(":"))
                .Then(s_plainText)
                .At(1)
                .Optional();

            parser.Patterns["predicate"] = SequencePattern.Create()
               .Then(ChoicePattern.Create()
                    .Or(TerminalPattern.FromPlainText("&"))
                    .Or(TerminalPattern.FromPlainText("!"))
                    .Optional())
                .Then(LookupPattern.Create("quantifier"));

            parser.Patterns["quantifier"] = SequencePattern.Create()
                .Then(LookupPattern.Create("term"))
                .Then(ChoicePattern.Create()
                    .Or(TerminalPattern.FromPlainText("?"))
                    .Or(TerminalPattern.FromPlainText("*"))
                    .Or(TerminalPattern.FromPlainText("+"))
                    .Optional());

            parser.Patterns["term"] = SequencePattern.Create()
                .Then(PredicatePattern.Negative(LookupPattern.Create("rule_head")))
                .Then(PredicatePattern.Negative(LookupPattern.Create("expr_head")))
                .Then(ChoicePattern.Create()
                    .Or(ChoicePattern.Create()
                        .Or(s_regex)
                        .Or(s_plainText)
                        .Label("terminal"))
                    .Or(SequencePattern.Create()
                        .Then(TerminalPattern.FromPlainText("("))
                        .Then(LookupPattern.Create("choice"))
                        .Then(TerminalPattern.FromPlainText(")"))
                        .At(1)
                        .Label("parenthesis"))
                    .Or(s_identifier.Label("lookup")))
                .At(2);

            parser.Patterns["expr_head"] = SequencePattern.Create()
                .Then(s_plainText)
                .Then(ChoicePattern.Create()
                    .Or(s_digits)
                    .Or(s_none))
                .Then(ChoicePattern.Create()
                    .Or(s_digits)
                    .Or(s_none));

            parser.Patterns["expr"] = SequencePattern.Create()
                .Then(TerminalPattern.FromPlainText("["))
                .Then(SequencePattern.Create()
                    .Then(LookupPattern.Create("expr_head"))
                    .Then(LookupPattern.Create("choice"))
                    .MinOrMore(0))
                .Then(TerminalPattern.FromPlainText("]"))
                .At(1);

            return parser;
        }

        private static string? TryParseLabel(IParseNode node)
        {
            var optional = (QuantifierNode)node;
            if (optional.Children.Count == 0)
                return null;

            var valueNode = (ValueNode)optional.Children[0];
            return valueNode.Value[1..^1].Replace("''", "'");
        }

        private static IPattern BuildChoice(IParseNode node)
        {
            var sequenceNode = (SequenceNode)node;
            var leftPattern = BuildSequence(sequenceNode.Children[0]);

            var minOrMoreNode = (QuantifierNode)sequenceNode.Children[1];
            if (minOrMoreNode.Children.Count == 0)
                return leftPattern;

            var output = ChoicePattern.Create()
               .Or(leftPattern);

            foreach (var child in minOrMoreNode.Children)
            {
                output.Or(BuildSequence(child));
            }

            return output;
        }

        private static IPattern BuildSequence(IParseNode node)
        {
            var quantifierNode = (QuantifierNode)node;
            if (quantifierNode.Children.Count == 1)
                return BuildSequenceStep(quantifierNode.Children[0]);

            var output = SequencePattern.Create();
            foreach (var child in quantifierNode.Children)
            {
                output.Then(BuildSequenceStep(child));
            }

            return output;
        }

        private static IPattern BuildSequenceStep(IParseNode node)
        {
            var sequenceNode = (SequenceNode)node;
            var pattern = BuildPredicate(sequenceNode.Children[0]);
            string? label = TryParseLabel(sequenceNode.Children[1]);
            if (label != null)
                pattern = LabelPattern.Create(label, pattern);

            return pattern;
        }

        private static IPattern BuildPredicate(IParseNode node)
        {
            var sequenceNode = (SequenceNode)node;
            var optional = (QuantifierNode)sequenceNode.Children[0];
            var pattern = BuildQuantifier(sequenceNode.Children[1]);

            if (optional.Children.Count == 0)
                return pattern;

            var valueNode = (ValueNode)optional.Children[0];
            switch (valueNode.Value)
            {
                case "&": return PredicatePattern.Positive(pattern);
                case "!": return PredicatePattern.Negative(pattern);
                default: throw new NotImplementedException();
            };
        }

        private static IPattern BuildQuantifier(IParseNode node)
        {
            var sequenceNode = (SequenceNode)node;
            var pattern = BuildTerm(sequenceNode.Children[0]);
            var optional = (QuantifierNode)sequenceNode.Children[1];

            if (optional.Children.Count == 0)
                return pattern;

            var valueNode = (ValueNode)optional.Children[0];
            switch (valueNode.Value)
            {
                case "?": return pattern.Optional();
                case "*": return pattern.MinOrMore(0);
                case "+": return pattern.MinOrMore(1);
                default: throw new NotImplementedException();
            };
        }

        private static IPattern BuildTerm(IParseNode node)
        {
            var root = (LabelNode)node;

            switch (root.Label)
            {
                case "terminal":
                    {
                        var valueNode = (ValueNode)root.Node;
                        string pattern = valueNode.Value[1..^1];

                        if (valueNode.Value.StartsWith('`'))
                            return TerminalPattern.FromRegex(pattern);

                        if (valueNode.Value.StartsWith('\''))
                            return TerminalPattern.FromPlainText(pattern);

                        throw new NotImplementedException();
                    };
                case "parenthesis":
                    return BuildChoice(root.Node);
                case "lookup":
                    {
                        var valueNode = (ValueNode)root.Node;
                        return LookupPattern.Create(valueNode.Value);
                    }
                default: throw new NotImplementedException();
            }
        }

        private static IPattern BuildExpr(IParseNode node)
        {
            var quantifierNode = (QuantifierNode)node;

            var operatorInfos = new List<OperatorInfo>();
            foreach (var child in quantifierNode.Children)
            {
                var sequenceNode = (SequenceNode)child;

                var headNode = (SequenceNode)sequenceNode.Children[0];
                var keyNode = (ValueNode)headNode.Children[0];
                string key = keyNode.Value[1..^1];

                var leftNode = (ValueNode)headNode.Children[1];
                uint? leftPrecedence;
                if (leftNode.Value == "none")
                    leftPrecedence = null;
                else
                    leftPrecedence = uint.Parse(leftNode.Value);

                var rightNode = (ValueNode)headNode.Children[2];
                uint? rightPrecedence;
                if (rightNode.Value == "none")
                    rightPrecedence = null;
                else
                    rightPrecedence = uint.Parse(rightNode.Value);

                var patternNode = sequenceNode.Children[1];
                var pattern = BuildChoice(patternNode);

                var operatorInfo = new OperatorInfo(key, pattern, leftPrecedence, rightPrecedence);
                operatorInfos.Add(operatorInfo);
            }

            return new ExpressionPattern(operatorInfos);
        }
    }
}
