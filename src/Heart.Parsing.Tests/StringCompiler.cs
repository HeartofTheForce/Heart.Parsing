using System;
using System.Collections.Generic;
using System.Linq;
using Heart.Parsing;
using Heart.Parsing.Patterns;

namespace Heart.Tests
{
    public static class StringCompiler
    {
        private static readonly Dictionary<string, Func<ExpressionNode, string>> s_overrideCompilers = new Dictionary<string, Func<ExpressionNode, string>>()
        {
            ["()"] = (node) =>
            {
                var sequenceNode = (SequenceNode)node.MidNode;
                return Compile(sequenceNode.Children[1]);
            },
            ["real"] = (node) =>
            {
                var valueNode = (ValueNode)node.MidNode;
                return valueNode.Value;
            },
            ["integral"] = (node) =>
            {
                var valueNode = (ValueNode)node.MidNode;
                return valueNode.Value;
            },
            ["boolean"] = (node) =>
            {
                var choiceNode = (ChoiceNode)node.MidNode;
                var valueNode = (ValueNode)choiceNode.Node;
                return valueNode.Value;
            },
            ["identifier"] = (node) =>
            {
                var valueNode = (ValueNode)node.MidNode;
                return valueNode.Value;
            },
        };

        public static string Compile(IParseNode node)
        {
            switch (node)
            {
                case ExpressionNode expressionNode:
                    {
                        if (s_overrideCompilers.TryGetValue(expressionNode.Key, out var compiler))
                            return compiler(expressionNode);

                        IEnumerable<ExpressionNode> children = ParseNodeHelper.GetChildrenRecursive<ExpressionNode>(expressionNode);

                        if (children.Any())
                        {
                            string parameters = string.Join(' ', children.Select(x => Compile(x)));
                            return $"({expressionNode.Key} {parameters})";
                        }

                        return $"({expressionNode.Key})";
                    }
                case ValueNode valueNode:
                    {
                        return valueNode.Value;
                    }
                default:
                    {
                        var children = ParseNodeHelper.GetChildren(node).Select(x => Compile(x));
                        return string.Join(' ', children);
                    }
            }

            throw new NotImplementedException();
        }
    }
}
