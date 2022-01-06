using System;

namespace Heart.Parsing.Patterns
{
    public interface IPattern
    {
        IParseNode? Match(PatternParser parser, ParserContext ctx);
    }

    public static class PatternExtensions
    {
        public static IParseNode MatchComplete(this IPattern pattern, PatternParser parser, ParserContext ctx)
        {
            var result = pattern.Match(parser, ctx);
            if (!ctx.IsComplete())
            {
                ctx.LogException(new UnexpectedTokenException(ctx.Offset, "EOF"));
                if (ctx.Exception != null)
                    throw ctx.Exception;
                else
                    throw new ArgumentException(nameof(ctx.Exception));
            }

            if (result == null)
                throw new ArgumentException(nameof(ctx.Exception));

            return result;
        }

        public static IParseNode? TryMatch(this IPattern pattern, PatternParser parser, ParserContext ctx)
        {
            int localOffset = ctx.Offset;

            var result = pattern.Match(parser, ctx);
            if (result == null)
                ctx.Offset = localOffset;

            return result;
        }

        public static IPattern Trim(this IPattern pattern, IPattern trimPattern)
        {
            return SequencePattern.Create()
                .Discard(trimPattern)
                .Then(pattern)
                .Discard(trimPattern);
        }
    }
}
