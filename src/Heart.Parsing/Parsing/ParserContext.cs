using System.Collections.Generic;
using Heart.Parsing.Patterns;

namespace Heart.Parsing
{
    public class ParserContext
    {
        public string Input { get; }
        public int Offset { get; set; }
        public PatternException? Exception { get; private set; }
        public bool IsEOF => Offset == Input.Length;

        private readonly List<PatternException> _patternExceptions;

        public ParserContext(string input)
        {
            Input = input;
            Offset = 0;
            Exception = null;
            _patternExceptions = new List<PatternException>();
        }

        public void LogException(PatternException ex)
        {
            _patternExceptions.Add(ex);

            if (Exception == null || ex.TextOffset > Exception.TextOffset || (ex.TextOffset == Exception.TextOffset && ex.Priority >= Exception.Priority))
                Exception = ex;
        }

        public bool IsComplete() => Offset == Input.Length;
    }
}
