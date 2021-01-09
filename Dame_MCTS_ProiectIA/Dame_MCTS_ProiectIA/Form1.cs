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

    enum GameOverType { No, WinHuman, WinComputer};
    enum ActionType { ToSelect, ToMove };
    public partial class Form1 : Form
    {
        private CellType[,] board;
        private ActionType action;
        private bool humanTurn;
        private Point currentPos, newPos;
        private Random rand;
        private List<Point> availableMoves;
        private int humanPieces, computerPieces;
        private GameOverType gameOver;
        public Form1()
        {
            InitializeComponent();
            this.newGame();
        }

        private void newGame()
        {
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
            this.humanPieces = 12;
            this.computerPieces = 12;
            this.gameOver = GameOverType.No;
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
            this.newGame();
        }

        private List<Point> getAvailableMovesForPiece(int line, int column)
        {
            List<Point> moves = new List<Point>();
            if (humanTurn)
            {

                if (line - 2 >= 0 && column - 2 >= 0 && board[line - 1, column - 1] == CellType.BlackWithY && board[line - 2, column - 2] == CellType.Black)
                    moves.Add(new Point(line - 2, column - 2));
                if(line + 2 <8  && column + 2 <8 && board[line + 1, column + 1] == CellType.BlackWithY && board[line + 2, column + 2] == CellType.Black)
                    moves.Add(new Point(line + 2, column + 2));
                if(line - 2 >= 0 && column + 2 < 8 && board[line - 1, column + 1] == CellType.BlackWithY && board[line - 2, column + 2] == CellType.Black)
                        moves.Add(new Point(line - 2, column + 2));
                if(line + 2 <8 && column - 2 >=0 && board[line + 1, column - 1] == CellType.BlackWithY && board[line + 2, column - 2] == CellType.Black)
                    moves.Add(new Point(line + 2, column - 2));
                if(moves.Count()==0)
                {
                    if (line - 1 >= 0 && column - 1 >= 0 && board[line - 1, column - 1] == CellType.Black)
                        moves.Add(new Point(line - 1, column - 1));
                    if (line - 1 >= 0 && column + 1 < 8 && board[line - 1, column + 1] == CellType.Black)
                        moves.Add(new Point(line - 1, column + 1));

                }
            }
            else
            {
                if (line - 2 >= 0 && column - 2 >= 0 && board[line - 1, column - 1] == CellType.BlackWithX && board[line - 2, column - 2] == CellType.Black)
                    moves.Add(new Point(line - 2, column - 2));
                if (line + 2 < 8 && column + 2 < 8 && board[line + 1, column + 1] == CellType.BlackWithX && board[line + 2, column + 2] == CellType.Black)
                    moves.Add(new Point(line + 2, column + 2));
                if (line - 2 >= 0 && column + 2 < 8 && board[line - 1, column + 1] == CellType.BlackWithX && board[line - 2, column + 2] == CellType.Black)
                    moves.Add(new Point(line - 2, column + 2));
                if (line + 2 < 8 && column - 2 >= 0 && board[line + 1, column - 1] == CellType.BlackWithX && board[line + 2, column - 2] == CellType.Black)
                    moves.Add(new Point(line + 2, column - 2));
                if (moves.Count() == 0)
                {
                    if (line + 1 < 8 && column - 1 >= 0 && board[line + 1, column - 1] == CellType.Black)
                        moves.Add(new Point(line + 1, column - 1));
                    if (line + 1 < 8 && column + 1 < 8 && board[line + 1, column + 1] == CellType.Black)
                        moves.Add(new Point(line + 1, column + 1));

                }
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


        private List<Point> searchConfruntation(CellType player)
        {
            List<Point> pieces = new List<Point>();
            CellType opponent= CellType.BlackWithY;
            if (player == CellType.BlackWithX)
            {
                opponent = CellType.BlackWithY;
            }
            else if (player == CellType.BlackWithY)
            {
                opponent = CellType.BlackWithX;
            }
            for (int line=0;line<8;line++)
            {
                for(int column=0;column<8;column++)
                {
                    if (board[line, column] == player)
                    {
                        if (line - 2 >= 0 && column - 2 >= 0 && board[line - 1, column - 1] == opponent && board[line - 2, column - 2] == CellType.Black
                            || line + 2 < 8 && column + 2 < 8 && board[line + 1, column + 1] == opponent && board[line + 2, column + 2] == CellType.Black
                            || line - 2 >= 0 && column + 2 < 8 && board[line - 1, column + 1] == opponent && board[line - 2, column + 2] == CellType.Black
                            || line + 2 < 8 && column - 2 >= 0 && board[line + 1, column - 1] == opponent && board[line + 2, column - 2] == CellType.Black)
                            pieces.Add(new Point(line, column));   
                    }
                }
            }
            return pieces;
        }

        private void computerTurn()
        {
            MonteCarloTreeSearch monte = new MonteCarloTreeSearch();
            Point moveOfComputer = new Point();
            Point currentPos = new Point();
            bool isValidPiece;
            bool isValid;
            List<Point> confruntation = searchConfruntation(CellType.BlackWithY);
            if (confruntation.Count > 0)
            {
                int piece = rand.Next() % confruntation.Count();
                currentPos = confruntation[piece];
                availableMoves = getAvailableMovesForPiece(currentPos.X, currentPos.Y);
            }
            else
            {
                int i;
                for (i = 0; i < this.computerPieces; i++)
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
                        break;
                    }
                }
                if (i >= this.computerPieces)
                {
                    this.gameOver = GameOverType.WinHuman;
                    return;
                }
            }
            moveOfComputer = monte.getBestMove(availableMoves);
            if (availableMoves.Count() > 0)
            {
                unsetAvailableMoves(availableMoves);
                availableMoves.Clear();
            }
            if (Math.Abs(currentPos.X - moveOfComputer.X) == 2 || Math.Abs(currentPos.Y - moveOfComputer.Y) == 2)
            {
                int X, Y;
                X = currentPos.X - ((currentPos.X - moveOfComputer.X) / 2);
                Y = currentPos.Y - ((currentPos.Y - moveOfComputer.Y) / 2);
                board[X, Y] = CellType.Black;
                if (this.humanPieces == 0)
                {
                    this.gameOver = GameOverType.WinComputer;
                }
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
            if (this.gameOver == GameOverType.No)
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
                        if (Math.Abs(currentPos.X - newPos.X) == 2 || Math.Abs(currentPos.Y - newPos.Y) == 2)
                        {
                            int X, Y;
                            X = currentPos.X - ((currentPos.X - newPos.X) / 2);
                            Y = currentPos.Y - ((currentPos.Y - newPos.Y) / 2);
                            board[X, Y] = CellType.Black;

                        }
                        unsetAvailableMoves(availableMoves);
                        availableMoves.Clear();
                        makeMove(currentPos, newPos);
                        action = ActionType.ToSelect;
                        labelPlayerTurn.Text = "Randul calculatorului";
                        humanTurn = false;
                        if (this.computerPieces == 0)
                        {
                            this.gameOver = GameOverType.WinHuman;
                        }
                        else
                        {
                            computerTurn();
                            humanTurn = true;
                            labelPlayerTurn.Text = "Randul tau";
                        }
                        if (this.gameOver == GameOverType.WinHuman)
                        {
                            labelOutputAction.Text = "Joc incheiat, ai castigat!";
                        }
                        else if (this.gameOver == GameOverType.WinComputer)
                        {
                            labelOutputAction.Text = "Joc incheiat, a castigat calculatorul!";
                        }
                        else
                        {
                            List<Point> confruntation = searchConfruntation(CellType.BlackWithX);
                            if (confruntation.Count > 0)
                            {
                                int piece = rand.Next() % confruntation.Count();
                                currentPos = confruntation[piece];
                                availableMoves = getAvailableMovesForPiece(currentPos.X, currentPos.Y);
                                setAvailableMoves(availableMoves);
                                action = ActionType.ToMove;
                            }
                        }
                    }
                    else if (humanTurn && board[newPos.X, newPos.Y] == CellType.BlackWithX)
                    {
                        currentPos = newPos;
                        if (availableMoves.Count() > 0)
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
}

