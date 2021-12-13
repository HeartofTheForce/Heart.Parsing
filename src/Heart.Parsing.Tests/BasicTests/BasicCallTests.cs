using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicCallTests
    {
        private static readonly BasicTestCase[] s_testCases = new BasicTestCase[]
        {
            //Call
            new BasicTestCase()
            {
                Infix = "sin(a) + cos(b)",
                ExpectedNodeString = "(+ ($ sin a) ($ cos b))",
            },
            //CallExpressionParameter
            new BasicTestCase()
            {
                Infix = "tan(b + c)",
                ExpectedNodeString = "($ tan (+ b c))",
            },
            //CallChained
            new BasicTestCase()
            {
                Infix = "sin(cos(b))",
                ExpectedNodeString = "($ sin ($ cos b))",
            },
            //CallMultiParameter
            new BasicTestCase()
            {
                Infix = "max(a,b) + min(b,c)",
                ExpectedNodeString = "(+ ($ max a b) ($ min b c))",
            },
            //CallMultiExpressionParameter
            new BasicTestCase()
            {
                Infix = "max(a + b, b + c)",
                ExpectedNodeString = "($ max (+ a b) (+ b c))",
            },
            //CallNestedMultiExpressionParameter
            new BasicTestCase()
            {
                Infix = "max((a + b) * 2, (b / c))",
                ExpectedNodeString = "($ max (* (+ a b) 2) (/ b c))",
            },
            //CallChainedMultiParameter
            new BasicTestCase()
            {
                Infix = "max(min(c, b), max(c, a))",
                ExpectedNodeString = "($ max ($ min c b) ($ max c a))",
            },
            //CallChainedMultiParameterUnary
            new BasicTestCase()
            {
                Infix = "max(min(c, b), +a)",
                ExpectedNodeString = "($ max ($ min c b) (u+ a))",
            },
            //CallNestedExpressionParameter
            new BasicTestCase()
            {
                Infix = "sin(min(c, b) - a)",
                ExpectedNodeString = "($ sin (- ($ min c b) a))",
            },
            //PostfixInfixUnary
            new BasicTestCase()
            {
                Infix = "clamp(a,b,c) + - d",
                ExpectedNodeString = "(+ ($ clamp a b c) (u- d))",
            },
            //Postfix
            new BasicTestCase()
            {
                Infix = "clamp(a + b, b - c, c * d)",
                ExpectedNodeString = "($ clamp (+ a b) (- b c) (* c d))",
            },
            //UnaryCall
            new BasicTestCase()
            {
                Infix = "-max(2, b)",
                ExpectedNodeString = "(u- ($ max 2 b))",
            },
            //EmptyCall
            new BasicTestCase()
            {
                Infix = "max()",
                ExpectedNodeString = "($ max)",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(BasicTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
