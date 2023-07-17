using System.Collections.Generic;

namespace Heart.Parsing.Patterns
{
    public class SequencePattern : IPattern
    {
        private readonly List<IPattern> _patterns;

        private SequencePattern()
        {
            _patterns = new List<IPattern>();
        }

        public static SequencePattern Create()
        {
            return new SequencePattern();
        }

        public SequencePattern Then(IPattern pattern)
        {
            _patterns.Add(pattern);
            return this;
        }

        public IParseNode? Match(PatternParser parser, ParserContext ctx)
        {
            int localOffset = ctx.Offset;

            var flowHelper = new FlowHelper();
            var output = new List<IParseNode>();
            foreach (var pattern in _patterns)
            {
                flowHelper.PreMatch(parser, ctx);
                var result = pattern.Match(parser, ctx);
                flowHelper.PostMatch(result != null, ctx);

                if (result == null)
                    return null;

                output.Add(result);
            }

            return new SequenceNode(localOffset, output);
        }
    }

    public class SequenceAtPattern : IPattern
    {
        private readonly SequencePattern _pattern;
        private readonly int _index;

        public SequenceAtPattern(SequencePattern pattern, int index)
        {
            _pattern = pattern;
            _index = index;
        }

        public IParseNode? Match(PatternParser parser, ParserContext ctx)
        {
            var result = _pattern.Match(parser, ctx);
            if (result != null)
                return ((SequenceNode)result).Children[_index];

            return null;
        }
    }

    public class SequenceNode : IParseNode
    {
        public int TextOffset { get; }
        public List<IParseNode> Children { get; }

        public SequenceNode(int textOffset, List<IParseNode> children)
        {
            TextOffset = textOffset;
            Children = children;
        }

        public IEnumerable<IParseNode> GetChildren()
        {
            return Children;
        }
    }
}
