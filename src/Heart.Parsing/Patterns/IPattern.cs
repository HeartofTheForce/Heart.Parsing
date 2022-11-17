using System;

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

        public static IPattern Trim(this IPattern pattern)
        {
            return SequencePattern.Create()
                .Discard(QuantifierPattern.Optional(LookupPattern.Create("_")))
                .Then(pattern)
                .Discard(QuantifierPattern.Optional(LookupPattern.Create("_")));
        }
    }
}
