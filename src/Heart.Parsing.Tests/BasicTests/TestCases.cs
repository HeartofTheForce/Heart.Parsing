using Heart.Parsing;
using Heart.Parsing.Patterns;
using NUnit.Framework;
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace Heart.Tests.BasicTests
{
    public class BasicTestCase
    {
        public string Infix { get; set; }
        public string ExpectedNodeString { get; set; }

        public void Execute(PatternParser parser)
        {
            var ctx = new ParserContext(Infix);

            var node = ExpressionPattern.Parse(parser, ctx);
            Assert.AreEqual(ExpectedNodeString, StringCompiler.Compile(node));
        }

        public override string ToString()
        {
            return $"\"{Infix}\"";
        }
    }
}
