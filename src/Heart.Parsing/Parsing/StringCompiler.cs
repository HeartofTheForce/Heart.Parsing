using System;
using System.Collections.Generic;
using System.Linq;
using Heart.Parsing.Patterns;

namespace Heart.Parsing
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
                var valueNode = (ValueNode)node.MidNode;
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

                        var children = expressionNode
                            .FindChildren<ExpressionNode>()
                            .Select(x => Compile(x));

                        if (children.Any())
                        {
                            string parameters = string.Join(' ', children);
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
                        var children = node
                            .GetChildren()
                            .Select(x => Compile(x));

                        return string.Join(' ', children);
                    }
            }

            throw new NotImplementedException();
        }
    }
}
