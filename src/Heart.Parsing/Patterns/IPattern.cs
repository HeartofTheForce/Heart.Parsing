namespace Heart.Parsing.Patterns
{
    public interface IPattern
    {
        IParseNode? Match(PatternParser parser, ParserContext ctx);
    }

    public static class PatternExtensions
    {
        public static IParseNode? TryMatch(this IPattern pattern, PatternParser parser, ParserContext ctx)
        {
            int localOffset = ctx.Offset;

            var result = pattern.Match(parser, ctx);
            if (result == null)
                ctx.Offset = localOffset;

            return result;
        }

        public static QuantifierPattern MinOrMore(this IPattern pattern, int min)
        {
            return new QuantifierPattern(min, null, pattern);
        }

        public static QuantifierPattern Optional(this IPattern pattern)
        {
            return new QuantifierPattern(0, 1, pattern);
        }

        public static SequenceAtPattern At(this SequencePattern sequencePattern, int index)
        {
            return new SequenceAtPattern(sequencePattern, index);
        }

        public static IPattern Trim(this IPattern pattern)
        {
            return SequencePattern.Create()
                .Then(LookupPattern.Create("_").Optional())
                .Then(pattern)
                .Then(LookupPattern.Create("_").Optional())
                .At(1);
        }
    }
}
