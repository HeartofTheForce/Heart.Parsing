using Heart.Parsing;
using NUnit.Framework;

namespace Heart.Tests.ExpressionPatternTests
{
    [TestFixture]
    public class PrecedenceTests
    {
        private static readonly PatternParser s_parser = ParsingHelper.BuildPatternParser("./TestGrammars/precedence.hg");

        // 8 valid non-nullary operator arrangements, 3^2 - 1(PostPre is invalid)
        // 3 cases per arrangement
        // left_0 right_0
        // left_1 right_0
        // right_0 left_1
        private static readonly ExpressionTestCase[] s_testCases = new ExpressionTestCase[]
        {
            //PrePre
            new ExpressionTestCase()
            {
                Infix = "pre_a0 pre_b0 x",
                ExpectedOutput = "(pre_a0 (pre_b0 x))",
            },
            new ExpressionTestCase()
            {
                Infix = "pre_a1 pre_b0 x",
                ExpectedOutput = "(pre_a1 (pre_b0 x))",
            },
            new ExpressionTestCase()
            {
                Infix = "pre_a0 pre_b1 x",
                ExpectedOutput = "(pre_a0 (pre_b1 x))",
            },
            //PrePost
            new ExpressionTestCase()
            {
                Infix = "pre_a0 x 0post_b",
                ExpectedOutput = "(0post_b (pre_a0 x))",
            },
            new ExpressionTestCase()
            {
                Infix = "pre_a1 x 0post_b",
                ExpectedOutput = "(pre_a1 (0post_b x))",
            },
            new ExpressionTestCase()
            {
                Infix = "pre_a0 x 1post_b",
                ExpectedOutput = "(1post_b (pre_a0 x))",
            },
            //PreIn
            new ExpressionTestCase()
            {
                Infix = "pre_a0 x 0in_b0 y",
                ExpectedOutput = "(0in_b0 (pre_a0 x) y)",
            },
            new ExpressionTestCase()
            {
                Infix = "pre_a1 x 0in_b0 y",
                ExpectedOutput = "(pre_a1 (0in_b0 x y))",
            },
            new ExpressionTestCase()
            {
                Infix = "pre_a0 x 1in_b1 y",
                ExpectedOutput = "(1in_b1 (pre_a0 x) y)",
            },
            //PostPost
            new ExpressionTestCase()
            {
                Infix = "x 0post_a 0post_b",
                ExpectedOutput = "(0post_b (0post_a x))",
            },
            new ExpressionTestCase()
            {
                Infix = "x 1post_a 0post_b",
                ExpectedOutput = "(0post_b (1post_a x))",
            },
            new ExpressionTestCase()
            {
                Infix = "x 0post_a 1post_b",
                ExpectedOutput = "(1post_b (0post_a x))",
            },
            //PostIn
            new ExpressionTestCase()
            {
                Infix = "x 0post_a 0in_b0 y",
                ExpectedOutput = "(0in_b0 (0post_a x) y)",
            },
            new ExpressionTestCase()
            {
                Infix = "x 1post_a 0in_b0 y",
                ExpectedOutput = "(0in_b0 (1post_a x) y)",
            },
            new ExpressionTestCase()
            {
                Infix = "x 0post_a 1in_b1 y",
                ExpectedOutput = "(1in_b1 (0post_a x) y)",
            },
            //InPre
            new ExpressionTestCase()
            {
                Infix = "x 0in_a0 pre_b0 y",
                ExpectedOutput = "(0in_a0 x (pre_b0 y))",
            },
            new ExpressionTestCase()
            {
                Infix = "x 1in_a1 pre_b0 y",
                ExpectedOutput = "(1in_a1 x (pre_b0 y))",
            },
            new ExpressionTestCase()
            {
                Infix = "x 0in_a0 pre_b1 y",
                ExpectedOutput = "(0in_a0 x (pre_b1 y))",
            },
            //InPost
            new ExpressionTestCase()
            {
                Infix = "x 0in_a0 y 0post_b",
                ExpectedOutput = "(0post_b (0in_a0 x y))",
            },
            new ExpressionTestCase()
            {
                Infix = "x 1in_a1 y 0post_b",
                ExpectedOutput = "(1in_a1 x (0post_b y))",
            },
            new ExpressionTestCase()
            {
                Infix = "x 0in_a0 y 1post_b",
                ExpectedOutput = "(1post_b (0in_a0 x y))",
            },
            //InIn
            new ExpressionTestCase()
            {
                Infix = "x 0in_a0 y 0in_b0 z",
                ExpectedOutput = "(0in_b0 (0in_a0 x y) z)",
            },
            new ExpressionTestCase()
            {
                Infix = "x 1in_a1 y 0in_b0 z",
                ExpectedOutput = "(1in_a1 x (0in_b0 y z))",
            },
            new ExpressionTestCase()
            {
                Infix = "x 0in_a0 y 1in_b1 z",
                ExpectedOutput = "(1in_b1 (0in_a0 x y) z)",
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(ExpressionTestCase testCase)
        {
            testCase.Execute(s_parser);
        }
    }
}
