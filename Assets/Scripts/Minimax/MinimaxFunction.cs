using System;
using System.Collections.Generic;
using System.Linq;

namespace Minimax
{
    public static class MinimaxFunction
    {
        public const double MINIMUM = Double.MinValue;
        public const double MAXIMUM = Double.MaxValue;

#nullable enable
        public static MinimaxOutput Minimax(MinimaxNode node, bool maximize, int depth, double alpha = MINIMUM, double beta = MAXIMUM, List<MinimaxStep>? steps = null)
        {
#nullable disable
            steps ??= new();

            List<MinimaxNode> children = node.GetChildren();

            if (depth == 0 || children.Count == 0)
            {
                return new MinimaxOutput(node.GetScore(), steps);
            }

            List<MinimaxStep> retSteps = steps.Take(steps.Count).ToList();

            double best = maximize ? MINIMUM : MAXIMUM;

            foreach (MinimaxNode child in children)
            {
                MinimaxStep newStep = node.GetPathToChild(child);
                List<MinimaxStep> newSteps = steps.Take(steps.Count).ToList();
                newSteps.Add(newStep);

                var nextMaximize = !maximize;

                if (child is CheckerBoard)
                {
                    nextMaximize = (child as CheckerBoard).CurrentPlayer == CheckerColor.BLACK;
                }


                
                MinimaxOutput output = Minimax(child, nextMaximize, depth - 1, alpha, beta, newSteps);
                newSteps = output.steps.Take(output.steps.Count).ToList();

                best = maximize ? Math.Max(best, output.score) : Math.Min(best, output.score);

                if (best == output.score)
                {
                    retSteps = newSteps.Take(newSteps.Count).ToList();
                }

                if ((maximize && best > beta) || (!maximize && best < alpha))
                {
                    break;
                }

                if (maximize)
                {
                    alpha = Math.Max(alpha, best);
                }
                else
                {
                    beta = Math.Min(beta, best);
                }
            }

            return new MinimaxOutput(best, retSteps);
        }
    }
}