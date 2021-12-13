using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicTernaryTests
    {
        private static readonly BasicTestCase[] s_testCases = new BasicTestCase[]
        {
            //ChainedTernary
            new BasicTestCase()
            {
                Infix = "a ? b : c ? d : e",
                ExpectedNodeString = "(?: a b (?: c d e))",
            },
            //TernaryBrackets
            new BasicTestCase()
            {
                Infix = "(a ? b : c) ? d : e",
                ExpectedNodeString = "(?: (?: a b c) d e)",
            },
            //TernaryBinary
            new BasicTestCase()
            {
                Infix = "a ? b : c + 1",
                ExpectedNodeString = "(?: a b (+ c 1))",
            },
            //NestedTernary
            new BasicTestCase()
            {
                Infix = "a ? b ? c : d : e",
                ExpectedNodeString = "(?: a (?: b c d) e)",
            },
            //InfixTernary
            new BasicTestCase()
            {
                Infix = "a * b ? c : d",
                ExpectedNodeString = "(?: (* a b) c d)",
            },
            //PrefixTernary
            new BasicTestCase()
            {
                Infix = "-a ? b : c",
                ExpectedNodeString = "(?: (u- a) b c)",
            },
            //TernaryPostfix
            new BasicTestCase()
            {
                Infix = "a ? b : c!",
                ExpectedNodeString = "(?: a b (! c))",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(BasicTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
