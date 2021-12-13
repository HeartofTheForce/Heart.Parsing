using Heart.Parsing;
using NUnit.Framework;

namespace Heart.Tests.ExpressionPatternTests
{
    [TestFixture]
    public class OperatorExpressionZeroTests
    {
        private static readonly PatternParser s_parser= ParsingHelper.BuildPatternParser("./TestGrammars/operator-expression-zero.hg");

        private static readonly IExpressionTestCase[] s_testCases = new IExpressionTestCase[]
        {
            //{}
            new ExpressionTestCase()
            {
                Infix = "{}",
                ExpectedOutput = "({)",
            },
            new UnexpectedTokenTestCase()
            {
                Infix = "{x}",
                ExpectedTextOffset = 1,
                ExpectedPattern = "}",
            },
            //[]
            new ExpressionTestCase()
            {
                Infix = "[]",
                ExpectedOutput = "([)",
            },
            new UnexpectedTokenTestCase()
            {
                Infix = "[x]",
                ExpectedTextOffset = 1,
                ExpectedPattern = "]",
            },
            //?:
            new ExpressionTestCase()
            {
                Infix = "?",
                ExpectedOutput = "(?)",
            },
            new UnexpectedTokenTestCase()
            {
                Infix = "? x",
                ExpectedTextOffset = 2,
                ExpectedPattern = "EOF",
            },
            //|
            new ExpressionTestCase()
            {
                Infix = "|",
                ExpectedOutput = "(|)",
            },
            new UnexpectedTokenTestCase()
            {
                Infix = "| x",
                ExpectedTextOffset = 2,
                ExpectedPattern = "EOF",
            },
            //&*
            new ExpressionTestCase()
            {
                Infix = "& *",
                ExpectedOutput = "(&)",
            },
            new UnexpectedTokenTestCase()
            {
                Infix = "& x *",
                ExpectedTextOffset = 2,
                ExpectedPattern = "*",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(IExpressionTestCase testCase)
        {
            testCase.Execute(s_parser);
        }
    }
}
