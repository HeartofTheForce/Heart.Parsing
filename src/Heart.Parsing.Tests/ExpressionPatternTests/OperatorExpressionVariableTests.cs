using Heart.Parsing;
using NUnit.Framework;

namespace Heart.Tests.ExpressionPatternTests
{
    [TestFixture]
    public class OperatorExpressionVariableTests
    {
        private static readonly PatternParser s_parser = ParsingHelper.BuildPatternParser("./TestGrammars/operator-expression-variable.hg");

        private static readonly IExpressionTestCase[] s_testCases = new IExpressionTestCase[]
        {
            //{}
            new ExpressionTestCase()
            {
                Infix = "{x, y, z}",
                ExpectedOutput = "({ x y z)",
            },
            new ExpressionTestCase()
            {
                Infix = "{}",
                ExpectedOutput = "({)",
            },
            new ExpressionTestCase()
            {
                Infix = "{x}",
                ExpectedOutput = "({ x)",
            },
            //[]
            new ExpressionTestCase()
            {
                Infix = "[x y z]",
                ExpectedOutput = "([ x y z)",
            },
            new ExpressionTestCase()
            {
                Infix = "[]",
                ExpectedOutput = "([)",
            },
            new ExpressionTestCase()
            {
                Infix = "[x]",
                ExpectedOutput = "([ x)",
            },
            //?:
            new ExpressionTestCase()
            {
                Infix = "? x : y : z",
                ExpectedOutput = "(? x y z)",
            },
            new ExpressionTestCase()
            {
                Infix = "?",
                ExpectedOutput = "(?)",
            },
            new ExpressionTestCase()
            {
                Infix = "? x",
                ExpectedOutput = "(? x)",
            },
            //|
            new ExpressionTestCase()
            {
                Infix = "| x y z",
                ExpectedOutput = "(| x y z)",
            },
            new ExpressionTestCase()
            {
                Infix = "|",
                ExpectedOutput = "(|)",
            },
            new ExpressionTestCase()
            {
                Infix = "| x",
                ExpectedOutput = "(| x)",
            },
            //&*
            new ExpressionTestCase()
            {
                Infix = "& x * y * z *",
                ExpectedOutput = "(& x y z)",
            },
            new ExpressionTestCase()
            {
                Infix = "& *",
                ExpectedOutput = "(&)",
            },
            new ExpressionTestCase()
            {
                Infix = "& x *",
                ExpectedOutput = "(& x)",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(IExpressionTestCase testCase)
        {
            testCase.Execute(s_parser);
        }
    }
}
