namespace Heart.Parsing.Patterns
{
    public class NonSignificantHelper
    {
        private int _resetOffset;
        private int _cnt;

        public NonSignificantHelper()
        {
            _resetOffset = 0;
            _cnt = 0;
        }

        public void PreMatch(PatternParser parser, ParserContext ctx)
        {
            _resetOffset = ctx.Offset;
            if (_cnt != 0)
                parser.Patterns["_"].TryMatch(parser, ctx);
        }

        public void PostMatch(bool success, ParserContext ctx)
        {
            if (success)
                _cnt++;
            else
                ctx.Offset = _resetOffset;
        }
    }
}
