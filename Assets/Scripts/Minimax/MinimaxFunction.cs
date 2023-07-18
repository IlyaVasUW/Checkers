using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax
{
    public class MinimaxFunction
    {
        public readonly double MINIMUM = Double.MinValue;
        public readonly double MAXIMUM = Double.MaxValue;
        public MinimaxOutput Minimax(MinimaxNode node, bool maximize, int depth, double alpha, double beta, MinimaxStep[] steps)
        {
            MinimaxNode[] children = node.GetChildren();
            if(depth == 0 || children.Length == 0)
            {
                return new MinimaxOutput(node.GetScore(), steps);
            }

            MinimaxStep[] retSteps = { };

            double best = maximize ? MINIMUM : MAXIMUM;

            foreach(MinimaxNode child in children)
            {
                MinimaxStep newStep = node.GetPathToChild(child);
                MinimaxStep[] newSteps = (MinimaxStep[])steps.Clone();
                newSteps.Append(newStep);

                MinimaxOutput output = Minimax(child, !maximize, depth - 1, alpha, beta, newSteps);

                if(maximize)
                {
                    best = best >= output.score ? best : output.score;
                    alpha = best >= alpha ? best : alpha;
                } else
                {
                    best = best <= output.score ? best : output.score; ;
                    beta = best <= beta ? best : beta;
                }

                if(best == output.score)
                {
                    retSteps = (MinimaxStep[])newSteps.Clone();
                }

                if(beta <= alpha)
                {
                    break;
                }
            }

            return new MinimaxOutput(best, retSteps);
        }
    }

    public class MinimaxOutput
    {
        public double score;
        public MinimaxStep[] steps;

        public MinimaxOutput(double score, MinimaxStep[] steps)
        {
            this.score = score;
            this.steps = steps;
        }
    }
}
