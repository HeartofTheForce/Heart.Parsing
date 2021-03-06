namespace Heart.Parsing.Patterns
{
    public class LookupPattern : IPattern
    {
        private readonly string _key;

        private LookupPattern(string key)
        {
            _key = key;
        }

        public static LookupPattern Create(string key)
        {
            return new LookupPattern(key);
        }

        public IParseNode? Match(PatternParser parser, ParserContext ctx)
        {
            var result = parser.Patterns[_key].Match(parser, ctx);
            return result;
        }
    }
}
