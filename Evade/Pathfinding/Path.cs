using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evade.Pathfinding
{
    public class Path<node> : IEnumerable<node>
    {
        public node LastStep { get; private set; }
        public Path<node> PreviousSteps { get; private set; }
        public double TotalCost { get; private set; }

        private Path(node lastStep, Path<node> previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }

        public Path(node start) : this(start, null, 0) { }

        public Path<node> AddStep(node step, double stepCost)
        {
            return new Path<node>(step, this, TotalCost + stepCost);
        }

        public IEnumerator<node> GetEnumerator()
        {
            for (var p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }
}
