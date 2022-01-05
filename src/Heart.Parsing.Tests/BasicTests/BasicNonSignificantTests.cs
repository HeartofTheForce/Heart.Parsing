using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicNonSignificantTests
    {
        private static readonly ExpressionTestCase[] s_testCases = new ExpressionTestCase[]
        {
            //LeadingWhitespace
            new ExpressionTestCase()
            {
                Infix = " 1",
                ExpectedOutput = "1",
            },
            //TrailingWhitespace
            new ExpressionTestCase()
            {
                Infix = "1 ",
                ExpectedOutput = "1",
            },
            //ExpressionWhitespace
            new ExpressionTestCase()
            {
                Infix = "1 + 2",
                ExpectedOutput = "(+ 1 2)",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(ExpressionTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
