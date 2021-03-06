using System.Collections.Generic;

namespace Heart.Parsing.Patterns
{
    public class ChoicePattern : IPattern
    {
        private readonly List<IPattern> _patterns;

        private ChoicePattern()
        {
            _patterns = new List<IPattern>();
        }

        public static ChoicePattern Create()
        {
            return new ChoicePattern();
        }

        public ChoicePattern Or(IPattern pattern)
        {
            _patterns.Add(pattern);
            return this;
        }

        public IParseNode? Match(PatternParser parser, ParserContext ctx)
        {
            for (int i = 0; i < _patterns.Count; i++)
            {
                var result = _patterns[i].TryMatch(parser, ctx);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
