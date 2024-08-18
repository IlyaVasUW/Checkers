using System.Collections.Generic;

namespace Minimax
{
    public abstract class MinimaxNode
    {
        public MinimaxNode() { }

        public abstract List<MinimaxNode> GetChildren();
        public abstract double GetScore();
        public abstract MinimaxStep GetPathToChild(MinimaxNode child);
    }
}
