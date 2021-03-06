using System;

namespace Heart.Parsing.Patterns
{
    public class ExpressionNodeBuilder
    {
        public readonly OperatorInfo OperatorInfo;
        private readonly IParseNode _midNode;
        private ExpressionNode? _leftNode;
        private ExpressionNode? _rightNode;

        public ExpressionNodeBuilder(OperatorInfo operatorInfo, IParseNode midNode)
        {
            OperatorInfo = operatorInfo;
            _midNode = midNode;
        }

        public ExpressionNode? FeedOperandLeft(ExpressionNode? leftNode)
        {
            _leftNode = leftNode;
            return TryCompleteNode();
        }

        public ExpressionNode FeedOperandRight(ExpressionNode? rightNode)
        {
            _rightNode = rightNode;
            return TryCompleteNode() ?? throw new Exception($"{nameof(ExpressionNodeBuilder)} is incomplete");
        }

        private ExpressionNode? TryCompleteNode()
        {
            bool haveLeft = _leftNode != null;
            bool expectLeft = OperatorInfo.LeftPrecedence != null;
            if (haveLeft != expectLeft)
                return null;

            bool haveRight = _rightNode != null;
            bool expectRight = OperatorInfo.RightPrecedence != null;
            if (haveRight != expectRight)
                return null;

            return new ExpressionNode(OperatorInfo.Key, _leftNode, _midNode, _rightNode);
        }
    }
}
