using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicDoubleTests
    {
        private static readonly ExpressionTestCase[] s_testCases = new ExpressionTestCase[]
        {
            //LeftToDouble
            new ExpressionTestCase()
            {
                Infix = "2 + 1.5",
                ExpectedOutput = "(+ 2 1.5)",
            },
            //RightToDouble
            new ExpressionTestCase()
            {
                Infix = "1.5 + 2",
                ExpectedOutput = "(+ 1.5 2)",
            },
            //SinCosTan
            new ExpressionTestCase()
            {
                Infix = "sin(1.0) + cos(1.0) + tan(1.0)",
                ExpectedOutput = "(+ (+ ($ sin 1.0) ($ cos 1.0)) ($ tan 1.0))",
            },
            //MaxIntDouble
            new ExpressionTestCase()
            {
                Infix = "max(2, b)",
                ExpectedOutput = "($ max 2 b)",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(ExpressionTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
