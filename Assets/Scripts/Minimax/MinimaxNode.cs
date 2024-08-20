using System;
using System.Collections.Generic;

namespace Minimax
{
    public abstract class MinimaxNode : IComparable<MinimaxNode>
    {
        public MinimaxNode() { }

        public abstract List<MinimaxNode> GetChildren();
        public abstract double GetScore();
        public abstract MinimaxStep GetPathToChild(MinimaxNode child);

        public virtual int CompareTo(MinimaxNode other)
        {
            return (int)((this.GetScore() - other.GetScore())*100);
        }
    }
}
