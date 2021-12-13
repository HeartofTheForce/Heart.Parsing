using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicIntTests
    {
        private static readonly BasicTestCase[] s_testCases = new BasicTestCase[]
        {
            //ReturnDouble2Int
            new BasicTestCase()
            {
                Infix = "2.5 + 3.3",
                ExpectedNodeString = "(+ 2.5 3.3)",
            },
            //IntOnlyBitwise
            new BasicTestCase()
            {
                Infix = "~(1 | 4)",
                ExpectedNodeString = "(~ (| 1 4))",
            },
            //BinaryPostfix
            new BasicTestCase()
            {
                Infix = "2 * 1!",
                ExpectedNodeString = "(* 2 (! 1))",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(BasicTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
