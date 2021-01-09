using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dame_MCTS_ProiectIA
{
    class MonteCarloTreeSearch
    {
        private Node tree;
        private Random rand;
        public MonteCarloTreeSearch()
        {
            tree = new Node();
            rand = new Random();
        }
        public void selection()
        {

        }

        public void expanding()
        {

        }

        public void simulation()
        {

        }

        public void backpropagation()
        {

        }

        public Point getBestMove(List<Point> points)
        {
            int move = rand.Next() % points.Count();
            return points[move];
        }
    }
}
