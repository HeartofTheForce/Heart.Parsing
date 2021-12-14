using System.Collections.Generic;

namespace Heart.Parsing
{
    public interface IParseNode
    {
        int TextOffset { get; }
        IEnumerable<IParseNode> GetChildren();
    }
}
