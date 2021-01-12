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
    enum CellType { White, Black, BlackWithX, BlackWithY, BlackWithPossibleMove, BlackWithPieceConflict };

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
        private List<Point> confruntation;
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
            this.confruntation = new List<Point>();
            labelPlayerTurn.Text = "Randul tau";
            labelOutputAction.Text = "Selecteaza o piesa";
            drawBoard();
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
                            case CellType.BlackWithPieceConflict:
                                g.DrawImage(new Bitmap("images\\blackBox.png"), new Point(i * 70, j * 70));
                                g.DrawImage(new Bitmap("images\\pieceConflict.png"), new Point(i * 70, j * 70));
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

        
        private void getCatch(List<Point> moves,CellType opponent, int line, int column, int direction)
        {
            int signLine, signColumn;
            signLine = signColumn = 0;
            switch(direction)
            {
                case 1:// - -
                    signLine = signColumn = -1;
                    break;
                case 2:// - +
                    signLine = 1;
                    signColumn = -1;
                    break;
                case 3:// + +
                    signLine = signColumn = 1;
                    break;
                case 4:// + -
                    signLine = -1;
                    signColumn = +1;
                    break;
            }
            for (int i = 2; i < 8; i += 2)
            {
                if (line + signLine * i >= 0 && column + signColumn * i >= 0 && line + signLine * i <8 && column + signColumn * i <8)//daca nu a iesit de pe tabla
                {
                    if (board[line + signLine*i - signLine*1, column + signColumn*i - signColumn*1] != opponent || board[line + signLine * i, column + signColumn * i] != CellType.Black)//daca nu este in confruntare sau urmatorul loc nu este liber
                    {
                        if (i > 2)
                        {
                            moves.Add(new Point(line + signLine * (i-2), column + signColumn * (i-2)));  
                        }
                        break;
                    }
                    else { 
                        Console.WriteLine("Test"); }
                }
                else
                {
                    if (i > 2)
                    {
                        moves.Add(new Point(line + signLine * (i - 2), column + signColumn * (i - 2)));
                    }
                    break;
                }

            }
        }


        private List<Point> getAvailableMovesForPiece(int line, int column)
        {
            List<Point> moves = new List<Point>();
            if (humanTurn)
            {
                getCatch(moves, CellType.BlackWithY, line, column,1);
                getCatch(moves, CellType.BlackWithY, line, column,2);
                getCatch(moves, CellType.BlackWithY, line, column,3);
                getCatch(moves, CellType.BlackWithY, line, column,4);
                
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
                getCatch(moves, CellType.BlackWithX, line, column, 1);
                getCatch(moves, CellType.BlackWithX, line, column, 2);
                getCatch(moves, CellType.BlackWithX, line, column, 3);
                getCatch(moves, CellType.BlackWithX, line, column, 4);
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

        private List<Point> availablePiece(CellType player)
        {
            List<Point> pieces = new List<Point>();
            
            for (int line = 0; line < 8; line++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (board[line, column] == player)
                    {

                        if (getAvailableMovesForPiece(line, column).Count() > 0)
                        {
                            pieces.Add(new Point(line, column));
                        }
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
            confruntation = searchConfruntation(CellType.BlackWithY);
            if (confruntation.Count > 0)
            {
                int piece = rand.Next() % confruntation.Count();
                currentPos = confruntation[piece];
                availableMoves = getAvailableMovesForPiece(currentPos.X, currentPos.Y);
            }
            else
            {
                List<Point> availablePieces = availablePiece(CellType.BlackWithY);
                if (availablePieces.Count() > 0)
                {
                    currentPos = availablePieces[rand.Next() % availablePieces.Count()];
                    availableMoves = getAvailableMovesForPiece(currentPos.X, currentPos.Y);
                    if (availableMoves.Count() > 0)
                    {
                        setAvailableMoves(availableMoves);
                    }
                }
                else
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
                this.humanPieces--;
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

        private void setPieceConflict(List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.BlackWithPieceConflict;
            }
        }

        private void unsetPieceConflict(List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.BlackWithX;
            }
        }
        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            this.newGame();
        }

        private void unsetAvailableMoves(List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.Black;
            }
        }

        private bool isGameOver()
        {
            bool isOver = false;
            int distanceHuman,distanceComputer;
            int availabePiecesHuman = availablePiece(CellType.BlackWithX).Count();
            int availabePiecesComputer = availablePiece(CellType.BlackWithY).Count();
            if (this.computerPieces == 0)
            {
                this.gameOver = GameOverType.WinHuman;
            }
            else if(this.humanPieces == 0)
            {
                this.gameOver = GameOverType.WinComputer;
            }
            else if (availabePiecesHuman == 0 && availabePiecesComputer == 0)
            {
                if(this.humanPieces < this.computerPieces)
                {
                    this.gameOver = GameOverType.WinComputer;
                }
                else if (this.humanPieces > this.computerPieces)
                {
                    this.gameOver = GameOverType.WinHuman;
                }
                else
                {
                    distanceHuman = distanceComputer = 0;
                    for (int line = 0; line < 8; line++)
                    {
                        for (int column = 0; column < 8; column++)
                        {
                            if (board[line, column] == CellType.BlackWithX)
                            {

                                distanceHuman += Math.Abs(line);
                            }
                            else if (board[line, column] == CellType.BlackWithY)
                            {

                                distanceComputer += Math.Abs(line-8);
                            }
                        }
                    }
                    if(distanceHuman>distanceComputer)
                    {
                        this.gameOver = GameOverType.WinComputer;
                    }
                    else
                    {
                        this.gameOver = GameOverType.WinHuman;
                    }
                }

            }
            else if (availabePiecesHuman == 0)
            {
                this.gameOver = GameOverType.WinComputer;
            }
            else if (availabePiecesComputer == 0)
            {
                this.gameOver = GameOverType.WinHuman;
            }
            if(this.gameOver!=GameOverType.No)
            {
                isOver = true;
            }
            return isOver;

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
                    if(confruntation.Count()>0)
                    {
                        if (humanTurn && board[currentPos.X, currentPos.Y] == CellType.BlackWithPieceConflict)
                        {
                            unsetPieceConflict(confruntation);
                            availableMoves = getAvailableMovesForPiece(currentPos.X, currentPos.Y);
                            setAvailableMoves(availableMoves);
                            action = ActionType.ToMove;
                            labelOutputAction.Text = "Muta piesa";
                        }
                    }
                    else if (humanTurn && board[currentPos.X, currentPos.Y] != CellType.BlackWithX)
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
                        if (Math.Abs(currentPos.X - newPos.X) >= 2 || Math.Abs(currentPos.Y - newPos.Y) >= 2)
                        {
                            int directionX = (currentPos.X - newPos.X) / Math.Abs(currentPos.X - newPos.X);
                            int directionY = (currentPos.Y - newPos.Y) / Math.Abs(currentPos.Y - newPos.Y);
                            for(int X= currentPos.X; Math.Abs(X-newPos.X)!=0; X-=directionX*2)
                            {
                                for (int Y = currentPos.Y; Math.Abs(Y - newPos.Y) != 0; Y -= directionY * 2)
                                {
                                    board[X - directionX, Y - directionY] = CellType.Black;
                                    this.computerPieces--;
                                }
                            }
                        }
                        unsetAvailableMoves(availableMoves);
                        availableMoves.Clear();
                        makeMove(currentPos, newPos);
                        action = ActionType.ToSelect;
                        labelPlayerTurn.Text = "Randul calculatorului";
                        humanTurn = false;
                        if (!isGameOver())
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
                            labelOutputAction.Text = "Joc incheiat, a \ncastigat calculatorul!";
                        }
                        else
                        {
                            confruntation = searchConfruntation(CellType.BlackWithX);
                            
                            if (confruntation.Count > 0)
                            {
                                setPieceConflict(confruntation);
                                action = ActionType.ToSelect;
                                labelOutputAction.Text = "Selecteaza piesa pentru \nconfruntare";
                            }
                        }
                    }
                    else if (humanTurn && confruntation.Count()==0 && board[newPos.X, newPos.Y] == CellType.BlackWithX)
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

