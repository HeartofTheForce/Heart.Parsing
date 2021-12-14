using System;
using System.Collections.Generic;
using System.Linq;

namespace Heart.Parsing
{
    public static class ParseNodeHelper
    {
        public static List<T> FindChildren<T>(this IParseNode node)
        {
            var output = new List<T>();

            var nodeStack = new Stack<IParseNode>();
            foreach (var child in node.GetChildren().Reverse())
            {
                nodeStack.Push(child);
            }

            while (nodeStack.Count > 0)
            {
                var current = nodeStack.Pop();
                if (current is T typedNode)
                {
                    output.Add(typedNode);
                    continue;
                }

                foreach (var child in current.GetChildren().Reverse())
                {
                    nodeStack.Push(child);
                }
            }

            return output;
        }
    }
}
