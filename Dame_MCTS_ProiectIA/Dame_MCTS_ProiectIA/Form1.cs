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
    public enum CellType { White, Black, BlackWithX, BlackWithY, BlackWithPossibleMove, BlackWithPieceConflict };
    public enum GameOverType { No, WinHuman, WinComputer};
    public enum ActionType { ToSelect, ToMove };
    public partial class Form1 : Form
    {
        private ActionType action;
        private GameOverType gameOver;
        public bool humanTurn;
        private Point currentPos, newPos;
        private Random rand;
        private List<Point> availableMoves;
        private List<Point> confruntation;
        public int humanPieces, computerPieces;

        public CellType[,] getBoard() { return this.boardGame.Board; }
        public BoardGame getBoardGame() { return this.boardGame; }
        public Form1()
        {
            InitializeComponent();
            this.NewGame();
        }
        private void BoardGame_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            CellType[,] board = boardGame.Board;
            if (this.gameOver == GameOverType.No)
            {
                int line = e.Location.Y / 70;
                int column = e.Location.X / 70;
                if (action == ActionType.ToSelect)
                {
                    currentPos.X = line;
                    currentPos.Y = column;
                    if (confruntation.Count() > 0)
                    {
                        if (humanTurn && board[currentPos.X, currentPos.Y] == CellType.BlackWithPieceConflict)
                        {
                            UnsetPieceConflict(ref board, confruntation);
                            availableMoves = GetAvailableMovesForPiece(board, currentPos.X, currentPos.Y, CellType.BlackWithX);
                            SetAvailableMoves(ref board, availableMoves);
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
                        availableMoves = GetAvailableMovesForPiece(board, currentPos.X, currentPos.Y, CellType.BlackWithX);
                        SetAvailableMoves(ref board, availableMoves);
                        action = ActionType.ToMove;
                        labelOutputAction.Text = "Muta piesa";
                    }
                }
                else if (action == ActionType.ToMove)
                {
                    newPos.X = line;
                    newPos.Y = column;
                    if (IsValidMove(board, currentPos, newPos))
                    {
                        UnsetAvailableMoves(ref board, availableMoves);
                        availableMoves.Clear();
                        MakeMove(ref board, currentPos, newPos, PlayerTurnSimulation.Human, ref this.gameOver);

                        action = ActionType.ToSelect;
                        labelPlayerTurn.Text = "Randul calculatorului";
                        humanTurn = false;
                        boardGame.DrawBoard();
                        if (!IsGameOver(board, ref this.gameOver))
                        {
                            ComputerTurn(ref board);
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
                            confruntation = SearchConfruntation(board, CellType.BlackWithX);

                            if (confruntation.Count > 0)
                            {
                                SetPieceConflict(ref board, confruntation);
                                action = ActionType.ToSelect;
                                labelOutputAction.Text = "Selecteaza piesa pentru \nconfruntare";
                            }
                        }
                    }
                    else if (humanTurn && confruntation.Count() == 0 && board[newPos.X, newPos.Y] == CellType.BlackWithX)
                    {
                        currentPos = newPos;
                        if (availableMoves.Count() > 0)
                        {
                            UnsetAvailableMoves(ref board, availableMoves);
                            availableMoves.Clear();
                        }
                        availableMoves = GetAvailableMovesForPiece(board, currentPos.X, currentPos.Y, CellType.BlackWithX);
                        SetAvailableMoves(ref board, availableMoves);
                        action = ActionType.ToMove;
                        labelOutputAction.Text = "Muta piesa";
                    }
                    else
                    {
                        labelOutputAction.Text = "Muta piesa";
                        action = ActionType.ToMove;
                    }
                }
                boardGame.Board = board;
                boardGame.DrawBoard();
            }
        }
        private void NewGame()
        {
            CellType[,] board = new CellType[8, 8] {
                { CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY},
                { CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White},
                { CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY, CellType.White, CellType.BlackWithY},
                { CellType.Black, CellType.White, CellType.Black, CellType.White, CellType.Black, CellType.White, CellType.Black, CellType.White},
                { CellType.White, CellType.Black, CellType.White, CellType.Black, CellType.White, CellType.Black, CellType.White, CellType.Black},
                { CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White},
                { CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX},
                { CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White, CellType.BlackWithX, CellType.White},
            };
            this.boardGame.LoadImages();
            this.boardGame.Board = board;

            this.rand = new Random();
            this.action = ActionType.ToSelect;
            this.humanTurn = true;
            this.humanPieces = 12;
            this.computerPieces = 12;
            this.gameOver = GameOverType.No;
            this.confruntation = new List<Point>();
            labelPlayerTurn.Text = "Randul tau";
            labelOutputAction.Text = "Selecteaza o piesa";
            boardGame.DrawBoard();
        }
        
        public void getCatch(CellType[,] board, List<Point> moves,CellType opponent, int line, int column, int direction)
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
                         }
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
        public List<Point> GetAvailableMovesForPiece(CellType[,] board, int line, int column, CellType player)
        {
            List<Point> moves = new List<Point>();
            if (player == CellType.BlackWithX)
            {
                getCatch(board,moves, CellType.BlackWithY, line, column,1);
                getCatch(board, moves, CellType.BlackWithY, line, column,2);
                getCatch(board, moves, CellType.BlackWithY, line, column,3);
                getCatch(board, moves, CellType.BlackWithY, line, column,4);
                
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
                getCatch(board, moves, CellType.BlackWithX, line, column, 1);
                getCatch(board,moves, CellType.BlackWithX, line, column, 2);
                getCatch(board,moves, CellType.BlackWithX, line, column, 3);
                getCatch(board, moves, CellType.BlackWithX, line, column, 4);
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

        public void MakeMove(ref CellType[,] board, Point currentPos, Point newPos, PlayerTurnSimulation player , ref GameOverType gameO)
        {

            if (Math.Abs(currentPos.X - newPos.X) == 2 || Math.Abs(currentPos.Y - newPos.Y) == 2)
            {
                int X, Y;
                X = currentPos.X - ((currentPos.X - newPos.X) / 2);
                Y = currentPos.Y - ((currentPos.Y - newPos.Y) / 2);
                board[X, Y] = CellType.Black;
                if (player == Dame_MCTS_ProiectIA.PlayerTurnSimulation.Machine)
                {
                    this.humanPieces--;
                    if (this.humanPieces == 0)
                    {
                        gameO = GameOverType.WinComputer;
                    }
                }
                else if (player == Dame_MCTS_ProiectIA.PlayerTurnSimulation.Human)
                {
                    this.computerPieces--;
                    if (this.computerPieces == 0)
                    {
                        gameO = GameOverType.WinHuman;
                    }
                }
            }


            CellType aux;
            aux = board[currentPos.X, currentPos.Y];
            board[currentPos.X, currentPos.Y] = board[newPos.X, newPos.Y];
            board[newPos.X, newPos.Y] = aux;
        }


        public List<Point> SearchConfruntation(CellType[,] board, CellType player)
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

        public List<Point> AvailablePiece(CellType[,] board, CellType player)
        {
            List<Point> pieces = new List<Point>();
            
            for (int line = 0; line < 8; line++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (board[line, column] == player)
                    {

                        if (GetAvailableMovesForPiece(board, line, column, player).Count() > 0)
                        {
                            pieces.Add(new Point(line, column));
                        }
                    }
                }
            }
            return pieces;
        }

        public void ComputerTurn(ref CellType[,] board)
        {
            MonteCarloTreeSearch monte = new MonteCarloTreeSearch(this);
            Point moveOfComputer = new Point();
            Point currentPos = new Point();
            //bool isValidPiece, isValid;
          
            board = monte.GetBestMove();
            if (availableMoves.Count() > 0)
            {
                UnsetAvailableMoves(ref board, availableMoves);
                availableMoves.Clear();
            }
            
        }

        public bool IsValidMove(CellType[,] board, Point currentPos, Point newPos)
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

        public void SetAvailableMoves(ref CellType[,] board, List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.BlackWithPossibleMove;
            }
        }

        public void SetPieceConflict(ref CellType[,] board, List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.BlackWithPieceConflict;
            }
        }

        public void UnsetPieceConflict(ref CellType[,] board, List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.BlackWithX;
            }
        }
        public void ButtonNewGame_Click(object sender, EventArgs e)
        {
            this.NewGame();
        }

        public void UnsetAvailableMoves(ref CellType[,] board, List<Point> points)
        {
            foreach (Point item in points)
            {
                board[item.X, item.Y] = CellType.Black;
            }
        }

        public bool IsGameOver(CellType[,] board, ref GameOverType gameO)
        {
            bool isOver = false;
            int distanceHuman,distanceComputer;
            int availabePiecesHuman = AvailablePiece(board, CellType.BlackWithX).Count();
            int availabePiecesComputer = AvailablePiece(board, CellType.BlackWithY).Count();
            if (this.computerPieces == 0)
            {
                gameO = GameOverType.WinHuman;
            }
            else if(this.humanPieces == 0)
            {
                gameO = GameOverType.WinComputer;
            }
            else if (availabePiecesHuman == 0 && availabePiecesComputer == 0)
            {
                if(this.humanPieces < this.computerPieces)
                {
                    gameO = GameOverType.WinComputer;
                }
                else if (this.humanPieces > this.computerPieces)
                {
                    gameO = GameOverType.WinHuman;
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
                        gameO = GameOverType.WinComputer;
                    }
                    else
                    {
                        gameO = GameOverType.WinHuman;
                    }
                }

            }
            else if (availabePiecesHuman == 0)
            {
                gameO = GameOverType.WinComputer;
            }
            else if (availabePiecesComputer == 0)
            {
                gameO = GameOverType.WinHuman;
            }
            if(gameO != GameOverType.No)
            {
                isOver = true;
            }
            return isOver;

        }
    }
}