using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicIntTests
    {
        private static readonly ExpressionTestCase[] s_testCases = new ExpressionTestCase[]
        {
            //ReturnDouble2Int
            new ExpressionTestCase()
            {
                Infix = "2.5 + 3.3",
                ExpectedOutput = "(+ 2.5 3.3)",
            },
            //IntOnlyBitwise
            new ExpressionTestCase()
            {
                Infix = "~(1 | 4)",
                ExpectedOutput = "(~ (| 1 4))",
            },
            //BinaryPostfix
            new ExpressionTestCase()
            {
                Infix = "2 * 1!",
                ExpectedOutput = "(* 2 (! 1))",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(ExpressionTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
