using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
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

        public Point getBestMove()
        {
            int x = rand.Next() % 8;
            int y = rand.Next() % 8;
            return new Point(x, y);
        }
    }
}
