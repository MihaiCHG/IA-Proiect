﻿namespace Dame_MCTS_ProiectIA
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonNewGame = new System.Windows.Forms.Button();
            this.labelOutputAction = new System.Windows.Forms.Label();
            this.labelPlayerTurn = new System.Windows.Forms.Label();
            this.boardGame = new Dame_MCTS_ProiectIA.BoardGame();
            this.SuspendLayout();
            // 
            // buttonNewGame
            // 
            this.buttonNewGame.Location = new System.Drawing.Point(627, 162);
            this.buttonNewGame.Name = "buttonNewGame";
            this.buttonNewGame.Size = new System.Drawing.Size(75, 23);
            this.buttonNewGame.TabIndex = 1;
            this.buttonNewGame.Text = "New Game";
            this.buttonNewGame.UseVisualStyleBackColor = true;
            this.buttonNewGame.Click += new System.EventHandler(this.ButtonNewGame_Click);
            // 
            // labelOutputAction
            // 
            this.labelOutputAction.AutoSize = true;
            this.labelOutputAction.Location = new System.Drawing.Point(594, 87);
            this.labelOutputAction.Name = "labelOutputAction";
            this.labelOutputAction.Size = new System.Drawing.Size(97, 13);
            this.labelOutputAction.TabIndex = 2;
            this.labelOutputAction.Text = "Selecteaza o piesa";
            // 
            // labelPlayerTurn
            // 
            this.labelPlayerTurn.AutoSize = true;
            this.labelPlayerTurn.Location = new System.Drawing.Point(594, 117);
            this.labelPlayerTurn.Name = "labelPlayerTurn";
            this.labelPlayerTurn.Size = new System.Drawing.Size(121, 13);
            this.labelPlayerTurn.TabIndex = 3;
            this.labelPlayerTurn.Text = "Randul jucatorului uman";
            // 
            // boardGame
            // 
            this.boardGame.Location = new System.Drawing.Point(12, 12);
            this.boardGame.Name = "boardGame";
            this.boardGame.Size = new System.Drawing.Size(560, 560);
            this.boardGame.TabIndex = 0;
            this.boardGame.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BoardGame_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 596);
            this.Controls.Add(this.boardGame);
            this.Controls.Add(this.labelPlayerTurn);
            this.Controls.Add(this.labelOutputAction);
            this.Controls.Add(this.buttonNewGame);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BoardGame boardGame;
        private System.Windows.Forms.Button buttonNewGame;
        private System.Windows.Forms.Label labelOutputAction;
        private System.Windows.Forms.Label labelPlayerTurn;
    }
}

