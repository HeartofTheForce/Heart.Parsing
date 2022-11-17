using System;
using System.Collections.Generic;
using Heart.Parsing.Patterns;

namespace Heart.Parsing
{
    public class PatternParser
    {
        public Dictionary<string, IPattern> Patterns { get; }

        public PatternParser()
        {
            Patterns = new Dictionary<string, IPattern>();
        }

        public IParseNode MatchComplete(IPattern pattern, string input)
        {
            var ctx = new ParserContext(input);
            var result = pattern.TryMatch(this, ctx);
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
    }
}
