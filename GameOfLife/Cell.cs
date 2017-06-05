using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class Cell
    {
        public bool IsAlive { get; private set; }

        public Cell(bool isAlive)
        {
            IsAlive = isAlive;
        }

        public void NextGeneration(List<Cell> neighbours)
        {
            var nrOfAliveNeighbours = neighbours.Count(n => n.IsAlive);

            if (IsOverCrowded(nrOfAliveNeighbours))
            {
                IsAlive = false;
            }
            else if (HasHealthyNumberOfNeighboursToStayAlive(nrOfAliveNeighbours))
            {
                IsAlive = true;
            }
            else if (HasHealthyNumberOfNeighboursToResurrect(nrOfAliveNeighbours))
            {
                IsAlive = true;
            }
            else
            {
                IsAlive = false;
            }
        }

        private bool HasHealthyNumberOfNeighboursToResurrect(int nrOfAliveNeighbours)
        {
            return !IsAlive && nrOfAliveNeighbours == 3;
        }

        private bool HasHealthyNumberOfNeighboursToStayAlive(int nrOfAliveNeighbours)
        {
            return IsAlive && nrOfAliveNeighbours >= 2 && nrOfAliveNeighbours <= 3;
        }

        private bool IsOverCrowded(int nrOfAliveNeighbours)
        {
            return IsAlive && nrOfAliveNeighbours > 3;
        }
    }
}