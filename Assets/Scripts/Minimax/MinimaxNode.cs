using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax
{
    public abstract class MinimaxNode
    {
        public MinimaxNode() { }

        public abstract MinimaxNode[] GetChildren();
        public abstract double GetScore();
        public abstract MinimaxStep GetPathToChild(MinimaxNode child);
    }
}
