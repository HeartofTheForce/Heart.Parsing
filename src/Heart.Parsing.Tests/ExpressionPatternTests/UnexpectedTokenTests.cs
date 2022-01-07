using NUnit.Framework;

namespace Heart.Tests.ExpressionPatternTests
{
    [TestFixture]
    public class UnexpectedTokenTests
    {
        private static readonly UnexpectedTokenTestCase[] s_testCases = new UnexpectedTokenTestCase[]
        {
            //ConstantConstant
            new UnexpectedTokenTestCase()
            {
                Infix = "1 1",
                ExpectedTextOffset = 2,
                ExpectedPattern = "EOF",
            },
            //IdentifierConstant
            new UnexpectedTokenTestCase()
            {
                Infix = "a 1",
                ExpectedTextOffset = 2,
                ExpectedPattern = "EOF",
            },
            //ConstantIdentifier
            new UnexpectedTokenTestCase()
            {
                Infix = "1 a",
                ExpectedTextOffset = 2,
                ExpectedPattern = "EOF",
            },
            //IdentifierIdentifier
            new UnexpectedTokenTestCase()
            {
                Infix = "a a",
                ExpectedTextOffset = 2,
                ExpectedPattern = "EOF",
            },
            //RoundCloseIdentifier
            new UnexpectedTokenTestCase()
            {
                Infix = "(1) a",
                ExpectedTextOffset = 4,
                ExpectedPattern = "EOF",
            },
            //RoundCloseConstant
            new UnexpectedTokenTestCase()
            {
                Infix = "(a) 1",
                ExpectedTextOffset = 4,
                ExpectedPattern = "EOF",
            },
            //UnexpectedClose
            new UnexpectedTokenTestCase()
            {
                Infix = "(1+2))",
                ExpectedTextOffset = 5,
                ExpectedPattern = "EOF",
            },
            //BracketMissingClose
            new UnexpectedTokenTestCase()
            {
                Infix = "(1+2",
                ExpectedTextOffset = 4,
                ExpectedPattern = ")",
            },
            //CallMissingClose
            new UnexpectedTokenTestCase()
            {
                Infix = "max(1,2",
                ExpectedTextOffset = 7,
                ExpectedPattern = ")",
            },
            //UnexpectedComma
            new UnexpectedTokenTestCase()
            {
                Infix = "(1,)",
                ExpectedTextOffset = 2,
                ExpectedPattern = ")",
            },
            //TooFewOperands
            new UnexpectedTokenTestCase()
            {
                Infix = "a ? b",
                ExpectedTextOffset = 5,
                ExpectedPattern = ":",
            },
            //TooManyOperands
            new UnexpectedTokenTestCase()
            {
                Infix = "a ? b : c : d",
                ExpectedTextOffset = 10,
                ExpectedPattern = "EOF",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(UnexpectedTokenTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
