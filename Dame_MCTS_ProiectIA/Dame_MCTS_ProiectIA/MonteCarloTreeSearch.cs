using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Dame_MCTS_ProiectIA
{
    public enum PlayerTurnSimulation { Human, Machine };
    class MonteCarloTreeSearch
    {
        private Form1 parentForm;
        private Node tree;
        private Random rand;
        private GameOverType gameO;
        public CellType[,] Board;
        public PlayerTurnSimulation playerTurn { get; set; }
        public MonteCarloTreeSearch(Form1 form)
        {
            this.parentForm = form;
            tree = new Node();
            tree.Board = copyBoard(parentForm.getBoard());
           
            if (parentForm.humanTurn) {
                playerTurn = PlayerTurnSimulation.Human;
            }
            else
            {
                playerTurn = PlayerTurnSimulation.Machine;
            }
            tree.Player = playerTurn;
            Board = tree.Board;
            rand = new Random();
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
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

            List<Point> pieces = parentForm.SearchConfruntation(board, player);
            if (pieces.Count() == 0) {
                pieces = parentForm.AvailablePiece(board, player);
            }
            if (pieces.Count() > 0)
            {
                point = pieces[rand.Next() % pieces.Count()];
                return point;
            }
            else
            {
                parentForm.IsGameOver(board, ref gameO);
                return new Point(-1, -1);
            }
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
            if (positionOfPiece.X!=-1)
            {
                move.Add(positionOfPiece);
                //Thread.Sleep(50);
                availableMoves = parentForm.GetAvailableMovesForPiece(board, positionOfPiece.X, positionOfPiece.Y, cellType);
                randMove = rand.Next() % availableMoves.Count();

                move.Add(availableMoves[randMove]);
                return move;
            }
            else { return null; }
        }
        public Node Selection(Node startNode)
        {
            double maxValue;
            Node selected = null;
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
            Expanding(startNode);
            if (startNode.Children.Count()>0)
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
            return startNode;
        }

        public void Expanding(Node node)
        {
            Node newChild;
            CellType cellType;
            if (!parentForm.IsGameOver(Board, ref gameO))
            {
                if (node.Player == PlayerTurnSimulation.Human)
                    cellType = CellType.BlackWithX;
                else
                    cellType = CellType.BlackWithY;
                List<Point> pieces = parentForm.SearchConfruntation(node.Board, cellType);
                if (pieces.Count() == 0)
                {
                    pieces = parentForm.AvailablePiece(node.Board, cellType);
                }
                for (int i = 0; i < pieces.Count(); i++)
                {
                    List<Point> moves = parentForm.GetAvailableMovesForPiece(node.Board, pieces[i].X, pieces[i].Y, cellType);
                    for (int j = 0; j < moves.Count(); j++)
                    {
                        newChild = new Node
                        {
                            Value = double.PositiveInfinity
                        };

                        newChild.Board = copyBoard(node.Board);
                        if (!parentForm.IsGameOver(Board, ref gameO))
                        {
                            parentForm.MakeMove(ref newChild.Board, pieces[i], moves[j], node.Player, ref gameO);
                        }
                        if (node.Player == PlayerTurnSimulation.Human)
                            playerTurn = PlayerTurnSimulation.Machine;
                        else
                            playerTurn = PlayerTurnSimulation.Human;

                        newChild.Player = playerTurn;
                        newChild.Parent = node;
                        node.AddChild(newChild);
                    }
                }
            }
        }

        public GameOverType Simulation(Node node)
        {
            Board = copyBoard(node.Board);
            playerTurn = node.Player;
            PlayerTurnSimulation turn = playerTurn;
            GameOverType gameO = GameOverType.No;
            bool isOver = false;
            isOver = parentForm.IsGameOver(Board, ref gameO);
            while (isOver == false) 
            {
                /*CellType cellType;
                if (turn == PlayerTurnSimulation.Human)
                    cellType = CellType.BlackWithX;
                else
                    cellType = CellType.BlackWithY;*/
                List<Point> randomMove = GetRandomMove(turn,Board);
                if (randomMove != null)
                {
                    parentForm.MakeMove(ref Board, randomMove[0], randomMove[1], turn, ref gameO);
                    if (turn == PlayerTurnSimulation.Human)
                        turn = PlayerTurnSimulation.Machine;
                    else
                        turn = PlayerTurnSimulation.Human;

                    isOver = parentForm.IsGameOver(Board, ref gameO);
                }
            } 
            node.TN++;
            
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
                nodeProp.CalcUCT();
                nodeProp = nodeProp.Parent;
            }
        }

        public CellType[,] GetBestMove()
        {
            

            int i = 0;
            Node node;
            long end = CurrentTimeMillis() + 4000;
            PlayerTurnSimulation player = PlayerTurnSimulation.Human;
            while(CurrentTimeMillis()< end)
            {
                gameO = GameOverType.No;
                node = Selection(tree);
                if (gameO == GameOverType.No)
                {
                    gameO = Simulation(node);
                    if (gameO == GameOverType.WinHuman)
                        player = PlayerTurnSimulation.Human;
                    else if (gameO == GameOverType.WinComputer)
                        player = PlayerTurnSimulation.Machine;
                }
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
