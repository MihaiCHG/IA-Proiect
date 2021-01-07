﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Node
    {
        public Node()
        {
            this.W = 0;
            this.N = 0;
            this.Children = new List<Node>();
        }
        public CellType[,] Board { get; set; }
        public int W { get; set; }
        public int N { get; set; }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; }
        public void addChild(Node child)
        {
            this.Children.Add(child);
        }
        public void removeChild(Node child)
        {
            this.Children.Remove(child);
        }
    }
}