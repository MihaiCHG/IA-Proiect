using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dame_MCTS_ProiectIA
{
    enum CellType { White, Black, BlackWithX, BlackWithY, BlackWithPossibleMove };
    enum ActionType { ToSelect, ToMove };
    public partial class Form1 : Form
    {
        private CellType[,] board;
        private ActionType action;
        private bool humanTurn;
        private Point currentPos, newPos;
        private Random rand;
        private List<Point> availableMoves;
        public Form1()
        {
            InitializeComponent();
            board = new CellType[8, 8] {
                { CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY},
                { CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White},
                { CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY},
                { CellType.Black, CellType.White, CellType.Black, CellType.White, CellType.Black, CellType.White, CellType.Black, CellType.White},
                { CellType.White, CellType.Black, CellType.White, CellType.Black, CellType.White, CellType.Black, CellType.White, CellType.Black},
                { CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White},
                { CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX},
                { CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White},
            };
            this.rand = new Random();
            this.action = ActionType.ToSelect;
            this.humanTurn = true;
        }

        private void drawBoard()
        {
            pictureBox1.Image = new Bitmap("images\\board.png");
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        switch (board[j, i])
                        {
                            case CellType.Black:
                                g.DrawImage(new Bitmap("images\\blackBox.png"), new Point(i * 70, j * 70));
                                break;
                            case CellType.BlackWithX:
                                g.DrawImage(new Bitmap("images\\blackBox.png"), new Point(i * 70, j * 70));
                                g.DrawImage(new Bitmap("images\\pieceX.png"), new Point(i * 70, j * 70));
                                break;
                            case CellType.BlackWithY:
                                g.DrawImage(new Bitmap("images\\blackBox.png"), new Point(i * 70, j * 70));
                                g.DrawImage(new Bitmap("images\\pieceY.png"), new Point(i * 70, j * 70));
                                break;
                            case CellType.BlackWithPossibleMove:
                                g.DrawImage(new Bitmap("images\\blackBox.png"), new Point(i * 70, j * 70));
                                g.DrawImage(new Bitmap("images\\possibleMove.png"), new Point(i * 70, j * 70));
                                break;
                        }
                    }
                }
            }
            pictureBox1.Image = bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            drawBoard();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private List<Point> getAvailableMovesForPiece(int line, int column)
        {
            List<Point> moves = new List<Point>();
            if (humanTurn)
            {
                if (line - 1 >= 0 && column - 1 >= 0 && board[line - 1, column - 1] == CellType.Black)
                    moves.Add(new Point(line-1, column-1));
                if (line - 1 >= 0 && column + 1 < 8 && board[line - 1, column + 1] == CellType.Black)
                    moves.Add(new Point(line - 1, column + 1));
                if(line - 2 >= 0 && column - 2 >= 0 && board[line-1, column-1] == CellType.BlackWithY && board[line - 2, column - 2] == CellType.Black)
                    moves.Add(new Point(line - 2, column - 2));
                if (line - 2 >= 0 && column + 2 < 8 && board[line - 1, column + 1] == CellType.BlackWithY && board[line - 2, column + 2] == CellType.Black)
                    moves.Add(new Point(line - 2, column + 2));
            }
            else
            {
                if (line + 1 < 8 && column - 1 >= 0 && board[line + 1, column - 1] == CellType.Black)
                    moves.Add(new Point(line + 1, column - 1));
                if (line + 1 < 8 && column + 1 < 8 && board[line + 1, column + 1] == CellType.Black)
                    moves.Add(new Point(line + 1, column + 1));
                if (line + 2 < 8 && column - 2 >= 0 && board[line + 1, column - 1] == CellType.BlackWithX && board[line + 2, column - 2] == CellType.Black)
                    moves.Add(new Point(line + 2, column - 2));
                if (line + 2 < 8 && column + 2 < 8 && board[line + 1, column + 1] == CellType.BlackWithX && board[line + 2, column + 2] == CellType.Black)
                    moves.Add(new Point(line + 2, column + 2));
            }
            return moves;
        }

        private void makeMove(Point currentPos, Point newPos)
        {
            CellType aux;
            aux = board[currentPos.X, currentPos.Y];
            board[currentPos.X, currentPos.Y] = board[newPos.X, newPos.Y];
            board[newPos.X, newPos.Y] = aux;
        }

        private void computerTurn()
        {
            MonteCarloTreeSearch monte = new MonteCarloTreeSearch();
            Point moveOfComputer = new Point();
            Point currentPos = new Point();
            bool isValidSecondPiece = false;
            bool isValidPiece;
            bool isValid;
            while (!isValidSecondPiece)
            {
                isValidPiece = false;
                while (!isValidPiece)
                {
                    currentPos.X = rand.Next() % 8;
                    currentPos.Y = rand.Next() % 8;
                    if (board[currentPos.X, currentPos.Y] == CellType.BlackWithY)
                        isValidPiece = true;
                }
                availableMoves = getAvailableMovesForPiece(currentPos.X, currentPos.Y);
                if (availableMoves.Count() > 0)
                {
                    setAvailableMoves(availableMoves);
                    isValidSecondPiece = true;
                }
            }
            isValid = false;
            while (!isValid)
            {
                moveOfComputer = monte.getBestMove();
                labelPlayerTurn.Text = moveOfComputer.X + " " + moveOfComputer.Y;
                isValid = isValidMove(currentPos, moveOfComputer);
            }
            if (availableMoves.Count() > 0)
            {
                unsetAvailableMoves(availableMoves);
                availableMoves.Clear();
            }
            makeMove(currentPos, moveOfComputer);
        }

        private bool isValidMove(Point currentPos, Point newPos)
        {
            if (board[currentPos.X, currentPos.Y] == CellType.White || board[newPos.X, newPos.Y] == CellType.White)
                return false;
            if (humanTurn && board[currentPos.X, currentPos.Y] != CellType.BlackWithX)
                return false;
            if (!humanTurn && board[currentPos.X, currentPos.Y] != CellType.BlackWithY)
                return false;
            if (board[newPos.X, newPos.Y] != CellType.BlackWithPossibleMove)
                return false;
            return true;
        }

        private void setAvailableMoves(List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.BlackWithPossibleMove;
            }
        }

        private void unsetAvailableMoves(List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.Black;
            }
        }
        private void pictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int line = e.Location.Y / 70;
            int column = e.Location.X / 70;
            if (action == ActionType.ToSelect)
            {
                currentPos.X = line;
                currentPos.Y = column;
                if (humanTurn && board[currentPos.X, currentPos.Y] != CellType.BlackWithX)
                {
                    action = ActionType.ToSelect;
                    labelOutputAction.Text = "Selecteaza o piesa";
                }
                else if (humanTurn && board[currentPos.X, currentPos.Y] == CellType.BlackWithX)
                {
                    availableMoves = getAvailableMovesForPiece(currentPos.X, currentPos.Y);
                    setAvailableMoves(availableMoves);
                    action = ActionType.ToMove;
                    labelOutputAction.Text = "Muta piesa";
                }
            }
            else if (action == ActionType.ToMove)
            {
                newPos.X = line;
                newPos.Y = column;
                if (isValidMove(currentPos, newPos))
                {
                    unsetAvailableMoves(availableMoves);
                    availableMoves.Clear();
                    makeMove(currentPos, newPos);
                    action = ActionType.ToSelect;
                    labelPlayerTurn.Text = "Randul calculatorului";
                    humanTurn = false;
                    computerTurn();
                    humanTurn = true;
                    labelPlayerTurn.Text = "Randul tau";
                }
                else if(humanTurn && board[newPos.X, newPos.Y] == CellType.BlackWithX)
                {
                    currentPos = newPos;
                    if(availableMoves.Count() > 0)
                    {
                        unsetAvailableMoves(availableMoves);
                        availableMoves.Clear();
                    }
                    availableMoves = getAvailableMovesForPiece(currentPos.X, currentPos.Y);
                    setAvailableMoves(availableMoves);
                    action = ActionType.ToMove;
                    labelOutputAction.Text = "Muta piesa";
                }
                else
                {
                    labelOutputAction.Text = "Muta piesa";
                    action = ActionType.ToMove;
                }
            }
            drawBoard();
        }
    }
}

