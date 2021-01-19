using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dame_MCTS_ProiectIA
{
    class Node
    {
        public Node()
        {
            this.W = 0;
            this.N = 0;
            this.TN = 0;
            this.Children = new List<Node>();
        }
        public CellType[,] Board { get; set; }
        public int W { get; set; }
        public int N { get; set; }
        public int TN { get; set; }
        public double Value { get; set; }
        public PlayerTurnSimulation Player { get; set; }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; }
        public void AddChild(Node child)
        {
            this.Children.Add(child);
        }
        public void RemoveChild(Node child)
        {
            this.Children.Remove(child);
        }

        public void CalcUCTS()
        {
            Value = ((double)W / (double)N) + 1.41 * Math.Sqrt(Math.Log(TN) / (double)N);
        }
    }
}
