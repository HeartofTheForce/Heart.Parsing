using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicCallTests
    {
        private static readonly ExpressionTestCase[] s_testCases = new ExpressionTestCase[]
        {
            //Call
            new ExpressionTestCase()
            {
                Infix = "sin(a) + cos(b)",
                ExpectedOutput = "(+ ($ sin a) ($ cos b))",
            },
            //CallExpressionParameter
            new ExpressionTestCase()
            {
                Infix = "tan(b + c)",
                ExpectedOutput = "($ tan (+ b c))",
            },
            //CallChained
            new ExpressionTestCase()
            {
                Infix = "sin(cos(b))",
                ExpectedOutput = "($ sin ($ cos b))",
            },
            //CallMultiParameter
            new ExpressionTestCase()
            {
                Infix = "max(a,b) + min(b,c)",
                ExpectedOutput = "(+ ($ max a b) ($ min b c))",
            },
            //CallMultiExpressionParameter
            new ExpressionTestCase()
            {
                Infix = "max(a + b, b + c)",
                ExpectedOutput = "($ max (+ a b) (+ b c))",
            },
            //CallNestedMultiExpressionParameter
            new ExpressionTestCase()
            {
                Infix = "max((a + b) * 2, (b / c))",
                ExpectedOutput = "($ max (* (+ a b) 2) (/ b c))",
            },
            //CallChainedMultiParameter
            new ExpressionTestCase()
            {
                Infix = "max(min(c, b), max(c, a))",
                ExpectedOutput = "($ max ($ min c b) ($ max c a))",
            },
            //CallChainedMultiParameterUnary
            new ExpressionTestCase()
            {
                Infix = "max(min(c, b), +a)",
                ExpectedOutput = "($ max ($ min c b) (u+ a))",
            },
            //CallNestedExpressionParameter
            new ExpressionTestCase()
            {
                Infix = "sin(min(c, b) - a)",
                ExpectedOutput = "($ sin (- ($ min c b) a))",
            },
            //PostfixInfixUnary
            new ExpressionTestCase()
            {
                Infix = "clamp(a,b,c) + - d",
                ExpectedOutput = "(+ ($ clamp a b c) (u- d))",
            },
            //Postfix
            new ExpressionTestCase()
            {
                Infix = "clamp(a + b, b - c, c * d)",
                ExpectedOutput = "($ clamp (+ a b) (- b c) (* c d))",
            },
            //UnaryCall
            new ExpressionTestCase()
            {
                Infix = "-max(2, b)",
                ExpectedOutput = "(u- ($ max 2 b))",
            },
            //EmptyCall
            new ExpressionTestCase()
            {
                Infix = "max()",
                ExpectedOutput = "($ max)",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(ExpressionTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
