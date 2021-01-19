using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dame_MCTS_ProiectIA
{
    public enum PlayerTurnSimulation { Human, Machine };
    class MonteCarloTreeSearch
    {
        private Form1 parent;
        private Node tree;
        private Random rand;
        public CellType[,] Board { get; set; }
        public PlayerTurnSimulation playerTurn { get; set; }
        public MonteCarloTreeSearch(Form1 form)
        {
            this.parent = form;
            tree = new Node();
            tree.Board = copyBoard(parent.getBoard());
            tree.Player = playerTurn;
            if (parent.humanTurn) {
                playerTurn = PlayerTurnSimulation.Human;
            }
            else
            {
                playerTurn = PlayerTurnSimulation.Machine;
            }
            Board = tree.Board;
            rand = new Random();
        }

        private CellType[,] copyBoard(CellType[,] board)
        {
            CellType[,] b= new CellType[8, 8];
            for(int i=0;i<8;i++)
            {
                for(int j=0;j<8;j++)
                {
                    b[i, j] = board[i, j];
                }
            }
            return b;
        }

        private Point getRandomPosition(CellType[,] board, CellType player)
        {
            Point point = new Point();

            List<Point> pieces = parent.SearchConfruntation(board, player);
            if (pieces.Count() == 0) {
                pieces = parent.AvailablePiece(board, player);
            }
            point = pieces[rand.Next() % pieces.Count()];
            return point;
        }

        //functie care sa fie utilizata pentru determinarea unei pozitii random
        // pentru om/calculator
        private List<Point> GetRandomMove(PlayerTurnSimulation turn, CellType[,] board)
        {
            List<Point> move = new List<Point>();
            Point positionOfPiece;
            List<Point> availableMoves;
            CellType cellType;
            int randMove;
            if (turn == PlayerTurnSimulation.Human)
                cellType = CellType.BlackWithX;
            else
                cellType = CellType.BlackWithY;
           
            positionOfPiece = getRandomPosition(board, cellType);
            move.Add(positionOfPiece);
            availableMoves = parent.GetAvailableMovesForPiece(Board, positionOfPiece.X, positionOfPiece.Y, cellType);
            randMove = rand.Next() % availableMoves.Count();
            move.Add(availableMoves[randMove]);
            return move;
        }
        public Node Selection(Node startNode)
        {
            double maxValue;
            Node selected = null;
            int humanPieces, machinePieces;
            humanPieces = parent.humanPieces;
            machinePieces = parent.computerPieces;
            while (startNode.Children.Count() > 0)
            {
                maxValue = startNode.Children.First().Value;
                selected = startNode.Children.First();
                foreach (Node child in startNode.Children)
                {
                    if (child.Value > maxValue)
                    {
                        maxValue = child.Value;
                        selected = child;
                    }
                }
                startNode = selected;
            }
            parent.humanPieces = humanPieces;
            parent.computerPieces = machinePieces;
            return startNode;
        }

        public Node Expanding(Node node)
        {
            Node newChild = new Node
            {
                Value = double.PositiveInfinity
            };
            if (playerTurn == PlayerTurnSimulation.Human)
                playerTurn = PlayerTurnSimulation.Machine;
            else
                playerTurn = PlayerTurnSimulation.Human;
            newChild.Player = playerTurn;
            newChild.Board = copyBoard(node.Board);
            List<Point> randomMove = GetRandomMove(playerTurn, node.Board);
            parent.MakeMove(Board, randomMove[0], randomMove[1], playerTurn);

            node.AddChild(newChild);
            return newChild;
        }

        public GameOverType Simulation(Node node)
        {
            Board = copyBoard(node.Board);
            PlayerTurnSimulation turn = playerTurn;
            GameOverType gameO = GameOverType.No;
            bool isOver = false;
            int humanPieces, machinePieces;
            humanPieces = parent.humanPieces;
            machinePieces = parent.computerPieces;
            do
            {
                CellType cellType;
                if (turn == PlayerTurnSimulation.Human)
                    cellType = CellType.BlackWithX;
                else
                    cellType = CellType.BlackWithY;
                List<Point> randomMove = GetRandomMove(turn,Board);
                parent.MakeMove(Board, randomMove[0], randomMove[1], turn);
                if (turn == PlayerTurnSimulation.Human)
                    turn = PlayerTurnSimulation.Machine;
                else
                    turn = PlayerTurnSimulation.Human;
                
                isOver = parent.IsGameOver(Board, ref gameO);
                
            } while (isOver==false);
            node.TN++;
            parent.humanPieces = humanPieces;
            parent.computerPieces = machinePieces;
            return gameO;
        }

        public void Backpropagation(Node node, PlayerTurnSimulation player)
        {
            Node nodeProp = node;
            while (nodeProp != null)
            {
                nodeProp.N++;
                if (nodeProp.Player == player)
                {
                    nodeProp.W += 10;
                }
                nodeProp.CalcUCTS();
                nodeProp = nodeProp.Parent;
            }
        }

        public CellType[,] GetBestMove()
        {
            

            int i = 0;
            Node node;
            GameOverType gameO;
            PlayerTurnSimulation player = PlayerTurnSimulation.Human;
            while(i<2)
            {
                node = Selection(tree);
                node = Expanding(node);
                gameO = Simulation(node);
                if (gameO == GameOverType.WinHuman)
                    player = PlayerTurnSimulation.Human;
                else if (gameO == GameOverType.WinComputer)
                    player = PlayerTurnSimulation.Machine;
                Backpropagation(node, player);
                i++;
            }
            double maxValue = tree.Children.First().Value;
            Node selected = tree.Children.First();
            foreach (Node child in tree.Children)
            {
                if (child.Value > maxValue)
                {
                    maxValue = child.Value;
                    selected = child;
                }
            }
            return selected.Board;

        }
    }
}
