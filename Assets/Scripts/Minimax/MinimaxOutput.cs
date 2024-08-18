using System.Collections.Generic;

namespace Minimax
{

    public class MinimaxOutput
    {
        public double score;
        public List<MinimaxStep> steps;

        public MinimaxOutput(double score, List<MinimaxStep> steps)
        {
            this.score = score;
            this.steps = steps;
        }
    }
}
