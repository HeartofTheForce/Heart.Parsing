using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicNonSignificantTests
    {
        private static readonly BasicTestCase[] s_testCases = new BasicTestCase[]
        {
            //Leading Whitespace
            new BasicTestCase()
            {
                Infix = " 1",
                ExpectedNodeString = "1",
            },
            //Trailing Whitespace
            new BasicTestCase()
            {
                Infix = "1 ",
                ExpectedNodeString = "1",
            },
            //Expression Whitespace
            new BasicTestCase()
            {
                Infix = "1 + 2",
                ExpectedNodeString = "(+ 1 2)",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(BasicTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
