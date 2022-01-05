using NUnit.Framework;

namespace Heart.Tests.BasicTests
{
    [TestFixture]
    public class BasicTernaryTests
    {
        private static readonly ExpressionTestCase[] s_testCases = new ExpressionTestCase[]
        {
            //ChainedTernary
            new ExpressionTestCase()
            {
                Infix = "a ? b : c ? d : e",
                ExpectedOutput = "(?: a b (?: c d e))",
            },
            //TernaryBrackets
            new ExpressionTestCase()
            {
                Infix = "(a ? b : c) ? d : e",
                ExpectedOutput = "(?: (?: a b c) d e)",
            },
            //TernaryBinary
            new ExpressionTestCase()
            {
                Infix = "a ? b : c + 1",
                ExpectedOutput = "(?: a b (+ c 1))",
            },
            //NestedTernary
            new ExpressionTestCase()
            {
                Infix = "a ? b ? c : d : e",
                ExpectedOutput = "(?: a (?: b c d) e)",
            },
            //InfixTernary
            new ExpressionTestCase()
            {
                Infix = "a * b ? c : d",
                ExpectedOutput = "(?: (* a b) c d)",
            },
            //PrefixTernary
            new ExpressionTestCase()
            {
                Infix = "-a ? b : c",
                ExpectedOutput = "(?: (u- a) b c)",
            },
            //TernaryPostfix
            new ExpressionTestCase()
            {
                Infix = "a ? b : c!",
                ExpectedOutput = "(?: a b (! c))",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(ExpressionTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
