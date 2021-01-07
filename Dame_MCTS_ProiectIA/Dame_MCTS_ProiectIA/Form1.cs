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
    public partial class Form1 : Form
    {
        private CellType[,] board;
        private ActionType action;
        private bool humanTurn;
        private Point currentPos, newPos;
        private enum CellType { White, Black, BlackWithX, BlackWithY };
        private enum ActionType { ToSelect, ToMove };
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private List<Point> getAvailableMovesForPiece(int line, int column)
        {
            List<Point> moves = new List<Point>();
            return moves;
        }

        private void makeMove(Point currentPos, Point newPos)
        {
            if (board[currentPos.X, currentPos.Y] != CellType.White && board[newPos.X, newPos.Y] != CellType.White)
            {
                CellType aux;
                aux = board[currentPos.X, currentPos.Y];
                board[currentPos.X, currentPos.Y] = board[newPos.X, newPos.Y];
                board[newPos.X, newPos.Y] = aux;
            }
        }

        private bool isValidMove(Point currentPos, Point newPos)
        {
            if (humanTurn && board[currentPos.X, currentPos.Y] != CellType.BlackWithX)
                return false;
            if (!humanTurn && board[currentPos.X, currentPos.Y] != CellType.BlackWithY)
                return false;
            if (board[newPos.X, newPos.Y] != CellType.Black)
                return false;
            return true;
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
                    action = ActionType.ToSelect;
                else if (!humanTurn && board[currentPos.X, currentPos.Y] != CellType.BlackWithY)
                    action = ActionType.ToSelect;
                else
                    action = ActionType.ToMove;
            }
            else if (action == ActionType.ToMove)
            {
                newPos.X = line;
                newPos.Y = column;
                if (isValidMove(currentPos, newPos))
                {
                    makeMove(currentPos, newPos);
                    drawBoard();
                    humanTurn = !humanTurn;
                    action = ActionType.ToSelect;
                }
                else
                    action = ActionType.ToMove;
            }
            if (action == ActionType.ToMove)
                labelOutputAction.Text = "Muta piesa";
            else
                labelOutputAction.Text = "Selecteaza o piesa";
            if (humanTurn)
                labelPlayerTurn.Text = "Randul jucatorului uman";
            else
                labelPlayerTurn.Text = "Randul calculatorului";
        }
    }
}

