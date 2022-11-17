using System;

namespace Heart.Parsing.Patterns
{
    public abstract class PatternException : Exception
    {
        public int TextOffset { get; }
        public int Priority { get; }

        public PatternException(int textOffset, int priority, string message) : base(message)
        {
            TextOffset = textOffset;
            Priority = priority;
        }

        public PatternException(int textOffset, string message) : this(textOffset, 0, message)
        {
        }
    }

    public class ZeroLengthMatchException : PatternException
    {
        public ZeroLengthMatchException(int textOffset) : base(textOffset, "Unexpected 0 length match")
        {
        }
    }

    public class ExpressionTermException : PatternException
    {
        public ExpressionTermException(int textOffset) : base(textOffset, 1, $"Invalid Expression Term @ {textOffset}")
        {
        }
    }

    public class UnexpectedTokenException : PatternException
    {
        public string ExpectedPattern { get; }

        public UnexpectedTokenException(int textOffset, string expectedPattern) : base(textOffset, $"Unexpected Token @ {textOffset} expected {expectedPattern}")
        {
            ExpectedPattern = expectedPattern;
        }

        public UnexpectedTokenException(int textOffset, TerminalPattern terminalPattern) : this(textOffset, terminalPattern.ToString())
        {
        }
    }
}