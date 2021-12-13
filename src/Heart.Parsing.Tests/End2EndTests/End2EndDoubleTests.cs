using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicDoubleTests
    {
        private static readonly BasicTestCase[] s_testCases = new BasicTestCase[]
        {
            //LeftToDouble
            new BasicTestCase()
            {
                Infix = "2 + 1.5",
                ExpectedNodeString = "(+ 2 1.5)",
            },
            //RightToDouble
            new BasicTestCase()
            {
                Infix = "1.5 + 2",
                ExpectedNodeString = "(+ 1.5 2)",
            },
            //SinCosTan
            new BasicTestCase()
            {
                Infix = "sin(1.0) + cos(1.0) + tan(1.0)",
                ExpectedNodeString = "(+ (+ ($ sin 1.0) ($ cos 1.0)) ($ tan 1.0))",
            },
            //MaxIntDouble
            new BasicTestCase()
            {
                Infix = "max(2, b)",
                ExpectedNodeString = "($ max 2 b)",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(BasicTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
