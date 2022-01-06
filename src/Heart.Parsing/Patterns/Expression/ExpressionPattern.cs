using System.Collections.Generic;

namespace Heart.Parsing.Patterns
{
    public class ExpressionPattern : IPattern
    {
        private readonly IEnumerable<OperatorInfo> _wantOperandOperators;
        private readonly IEnumerable<OperatorInfo> _haveOperandOperators;

        public ExpressionPattern(IEnumerable<OperatorInfo> operators)
        {
            var wantOperandOperators = new List<OperatorInfo>();
            var haveOperandOperators = new List<OperatorInfo>();

            foreach (var op in operators)
            {
                if (op.LeftPrecedence == null)
                    wantOperandOperators.Add(op);
                else
                    haveOperandOperators.Add(op);
            }

            _wantOperandOperators = wantOperandOperators;
            _haveOperandOperators = haveOperandOperators;
        }

        public static IParseNode Parse(PatternParser parser, ParserContext ctx)
        {
            var pattern = parser.Patterns["expr"].Trim(parser.Patterns["_"]);
            return pattern.MatchComplete(parser, ctx);
        }

        public IParseNode? Match(PatternParser parser, ParserContext ctx)
        {
            var nodeBuilders = new Stack<ExpressionNodeBuilder>();
            ExpressionNode? operand = null;

            var nonSignificantHelper = new NonSignificantHelper();
            while (true)
            {
                nonSignificantHelper.PreMatch(parser, ctx);
                var right = TryGetNodeBuilder(operand == null, parser, ctx);
                nonSignificantHelper.PostMatch(right != null, ctx);
                if (right == null)
                {
                    if (operand == null)
                        return null;

                    while (nodeBuilders.Count > 0)
                    {
                        operand = nodeBuilders.Pop().FeedOperandRight(operand);
                    }

                    return operand;
                }

                while (nodeBuilders.TryPeek(out var left) && IsEvaluatedBefore(left.OperatorInfo, right.OperatorInfo))
                {
                    operand = nodeBuilders.Pop().FeedOperandRight(operand);
                }

                operand = right.FeedOperandLeft(operand);
                if (operand == null)
                    nodeBuilders.Push(right);
            }
        }

        private ExpressionNodeBuilder? TryGetNodeBuilder(bool wantOperand, PatternParser parser, ParserContext ctx)
        {
            int localOffset = ctx.Offset;

            IEnumerable<OperatorInfo> operators;
            if (wantOperand)
                operators = _wantOperandOperators;
            else
                operators = _haveOperandOperators;

            foreach (var op in operators)
            {
                var result = op.Pattern.TryMatch(parser, ctx);
                if (result != null)
                {
                    if (localOffset == ctx.Offset)
                        throw new ZeroLengthMatchException(ctx.Offset);

                    return new ExpressionNodeBuilder(op, result);
                }
            }

            if (wantOperand)
                ctx.LogException(new ExpressionTermException(ctx.Offset));

            return null;
        }

        private static bool IsEvaluatedBefore(OperatorInfo left, OperatorInfo right)
        {
            if (left.RightPrecedence == null || right.LeftPrecedence == null)
                return left.RightPrecedence == null;

            return left.RightPrecedence <= right.LeftPrecedence;
        }
    }

    public class ExpressionTermException : PatternException
    {
        public ExpressionTermException(int textOffset) : base(textOffset, 1, $"Invalid Expression Term @ {textOffset}")
        {
        }
    }
}
