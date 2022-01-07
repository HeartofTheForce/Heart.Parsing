using NUnit.Framework;

namespace Heart.Tests.ExpressionPatternTests
{
    [TestFixture]
    public class ExpressionTermTests
    {
        private static readonly ExpressionTermTestCase[] s_testCases = new ExpressionTermTestCase[]
        {
            //EmptyDelimiterClose
            new ExpressionTermTestCase()
            {
                Infix = "max(1, 2, )",
                ExpectedTextOffset = 10,
            },
            //EmptyDelimiterDelimiter
            new ExpressionTermTestCase()
            {
                Infix = "max(1, ,3)",
                ExpectedTextOffset = 7,
            },
            //EmptyOpenDelimiter
            new ExpressionTermTestCase()
            {
                Infix = "max( , 2, 3)",
                ExpectedTextOffset = 5,
            },
            //EmptyDelimiterEndOfString
            new ExpressionTermTestCase()
            {
                Infix = "max(1, 2, ",
                ExpectedTextOffset = 10,
            },
            //EmptyBrackets
            new ExpressionTermTestCase()
            {
                Infix = "()",
                ExpectedTextOffset = 1,
            },
            //EmptyBinaryRight
            new ExpressionTermTestCase()
            {
                Infix = "1 +",
                ExpectedTextOffset = 3,
            },
            //EmptyUnaryRight
            new ExpressionTermTestCase()
            {
                Infix = "-",
                ExpectedTextOffset = 1,
            },
            //NestedEmptyBinaryRight
            new ExpressionTermTestCase()
            {
                Infix = "(1 + ) 2",
                ExpectedTextOffset = 5,
            },
        };

        [TestCaseSource(nameof(s_testCases))]
        public void TestCases(ExpressionTermTestCase testCase)
        {
            testCase.Execute(Utility.Parser);
        }
    }
}
