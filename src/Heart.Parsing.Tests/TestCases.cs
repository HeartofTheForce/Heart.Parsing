using Heart.Parsing;
using Heart.Parsing.Patterns;
using NUnit.Framework;
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace Heart.Tests
{
    public interface IExpressionTestCase
    {
        void Execute(PatternParser parser);
    }

    public class ExpressionTestCase : IExpressionTestCase
    {
        public string Infix { get; set; }
        public string ExpectedOutput { get; set; }

        public void Execute(PatternParser parser)
        {
            var pattern = parser.Patterns["expr"].Trim();
            var result = parser.MatchComplete(pattern, Infix);

            Assert.AreEqual(ExpectedOutput, StringCompiler.Compile(result));
        }

        public override string ToString()
        {
            return $"\"{Infix}\"";
        }
    }

    public class ExpressionTermTestCase : IExpressionTestCase
    {
        public string Infix { get; set; }
        public int ExpectedTextOffset { get; set; }

        public void Execute(PatternParser parser)
        {
            var pattern = parser.Patterns["expr"].Trim();
            var ex = Assert.Throws<ExpressionTermException>(() => parser.MatchComplete(pattern, Infix));

            Assert.AreEqual(ExpectedTextOffset, ex.TextOffset);
        }

        public override string ToString()
        {
            return $"\"{Infix}\"";
        }
    }

    public class UnexpectedTokenTestCase : IExpressionTestCase
    {
        public string Infix { get; set; }
        public int ExpectedTextOffset { get; set; }
        public string ExpectedPattern { get; set; }

        public void Execute(PatternParser parser)
        {
            var pattern = parser.Patterns["expr"].Trim();
            var ex = Assert.Throws<UnexpectedTokenException>(() => parser.MatchComplete(pattern, Infix));

            Assert.AreEqual(ExpectedTextOffset, ex.TextOffset);
            Assert.AreEqual(ExpectedPattern, ex.ExpectedPattern);
        }

        public override string ToString()
        {
            return $"\"{Infix}\"";
        }
    }
}
