using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Dame_MCTS_ProiectIA
{
    public class BoardGame : Control
    {
        private PictureBox pictureBoxBoard;
        private Bitmap background, blackBox, pieceX, pieceY, possibleMove, pieceConflict;
        
        private void PictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }
        public static int GetNumerOfPiecesForPlayer(CellType[,] board, CellType player)
        {
            int c = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == player)
                        c++;
                }
            }
            return c;
        }
        public BoardGame() : base()
        {
            this.pictureBoxBoard = new PictureBox();
            this.pictureBoxBoard.Size = new System.Drawing.Size(560, 560);
            this.pictureBoxBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseUp);
            this.Controls.Add(pictureBoxBoard);
        }
        public void LoadImages()
        {
            this.background = new Bitmap("images\\board.png");
            this.blackBox = new Bitmap("images\\blackBox.png");
            this.pieceX = new Bitmap("images\\pieceX.png");
            this.pieceY = new Bitmap("images\\pieceY.png");
            this.possibleMove = new Bitmap("images\\possibleMove.png");
            this.pieceConflict = new Bitmap("images\\pieceConflict.png");
        }
        public CellType[,] Board { set; get; }

        public void DrawBoard()
        {
            pictureBoxBoard.Image = this.background;
            Bitmap bmp = new Bitmap(pictureBoxBoard.Image);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        switch (Board[j, i])
                        {
                            case CellType.Black:
                                g.DrawImage(blackBox, new Point(i * 70, j * 70));
                                break;
                            case CellType.BlackWithX:
                                g.DrawImage(blackBox, new Point(i * 70, j * 70));
                                g.DrawImage(pieceX, new Point(i * 70, j * 70));
                                break;
                            case CellType.BlackWithY:
                                g.DrawImage(blackBox, new Point(i * 70, j * 70));
                                g.DrawImage(pieceY, new Point(i * 70, j * 70));
                                break;
                            case CellType.BlackWithPossibleMove:
                                g.DrawImage(blackBox, new Point(i * 70, j * 70));
                                g.DrawImage(possibleMove, new Point(i * 70, j * 70));
                                break;
                            case CellType.BlackWithPieceConflict:
                                g.DrawImage(blackBox, new Point(i * 70, j * 70));
                                g.DrawImage(pieceConflict, new Point(i * 70, j * 70));
                                break;
                        }
                    }
                }
            }
            pictureBoxBoard.Image = bmp;
        }
    }
}
