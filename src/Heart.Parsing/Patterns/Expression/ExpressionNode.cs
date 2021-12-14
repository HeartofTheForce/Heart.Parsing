using System.Collections.Generic;

namespace Heart.Parsing.Patterns
{
    public class ExpressionNode : IParseNode
    {
        public string Key { get; }
        public int TextOffset { get; set; }

        public ExpressionNode? LeftNode { get; }
        public IParseNode MidNode { get; }
        public ExpressionNode? RightNode { get; }

        public ExpressionNode(string key, ExpressionNode? leftNode, IParseNode midNode, ExpressionNode? rightNode)
        {
            Key = key;

            LeftNode = leftNode;
            MidNode = midNode;
            RightNode = rightNode;

            if (leftNode != null)
                TextOffset = leftNode.TextOffset;
            else
                TextOffset = midNode.TextOffset;
        }

        public IEnumerable<IParseNode> GetChildren()
        {
            if (LeftNode != null)
                yield return LeftNode;

            yield return MidNode;

            if (RightNode != null)
                yield return RightNode;
        }
    }
}
