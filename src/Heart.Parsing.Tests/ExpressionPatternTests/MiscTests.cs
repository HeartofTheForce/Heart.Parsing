using Heart.Parsing;
using Heart.Parsing.Patterns;
using NUnit.Framework;

namespace Heart.Tests.ExpressionPatternTests
{
    [TestFixture]
    public class ErrorTests
    {
        private static readonly PatternParser s_parser = ParsingHelper.BuildPatternParser("./TestGrammars/misc.hg");

        [Test]
        public void ExceptCorrectPatternExceptionPriority()
        {
            var pattern = s_parser.Patterns["exception_priority"];
            var ex = Assert.Throws<UnexpectedTokenException>(() => s_parser.MatchComplete(pattern, "[[]"));
            Assert.AreEqual(3, ex?.TextOffset);
        }
    }
}
