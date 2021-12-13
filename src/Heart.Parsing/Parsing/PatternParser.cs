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
    }
}
