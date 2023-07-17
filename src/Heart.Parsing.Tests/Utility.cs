using Heart.Parsing;

namespace Heart.Tests
{
    public static class Utility
    {
        public static readonly PatternParser Parser = ParsingHelper.BuildPatternParser("./TestGrammars/test.hg");
    }
}
