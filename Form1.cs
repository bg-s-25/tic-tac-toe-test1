/**
 * Tic-Tac-Toe Test 1 -> Class FORM1
 * Author: Bobby Georgiou
 * Date: Jun 2018
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Tic_Tac_Toe_Test_1
{
    public partial class Form1 : Form
    {
        // Fields

        string APP_DIR = Application.StartupPath;
        string currentTurnName; // Player or CPU
        int turnNum = 0; // turn number
        string CPUSymbol; // X or O
        string playerSymbol; // X or O
        int delay_boxNum;
        string delay_symbol;
        ArrayList board = new ArrayList();
        List<int[]> triples = new List<int[]>();
        bool CPUFirstTurnDone = false;

        // Form Init

        public Form1()
        {
            InitializeComponent();
            BackColor = Color.Black;
            ShowMenu();
        }

        // Shared Methods

        public void ShowMenu()
        {
            pb_box1.Visible = false; pb_box2.Visible = false;
            pb_box3.Visible = false; pb_box4.Visible = false;
            pb_box5.Visible = false; pb_box6.Visible = false;
            pb_box7.Visible = false; pb_box8.Visible = false;
            pb_box9.Visible = false; l_status.Visible = false;
            b_quitGame.Visible = false;
            b_newGame.Visible = true; l_Title.Visible = true; l_Author.Visible = true;
        }

        public void NewGame()
        {
            for (int i = 0; i < 9; i++)
            {
                board.Add("");
            }

            triples.Add(new int[] { 0, 1, 2 }); triples.Add(new int[] { 3, 4, 5 });
            triples.Add(new int[] { 6, 7, 8 }); triples.Add(new int[] { 0, 3, 6 });
            triples.Add(new int[] { 1, 4, 7 }); triples.Add(new int[] { 2, 5, 8 });
            triples.Add(new int[] { 2, 4, 6 }); triples.Add(new int[] { 0, 4, 8 });

            turnNum = 0;
            CPUFirstTurnDone = false;
            int rndindex;
            Random x = new Random();
            rndindex = x.Next(2); // select random player for first turn
            if (rndindex == 0) { currentTurnName = "CPU"; } else { currentTurnName = "Player"; }
            rndindex = x.Next(2); // randomize symbols
            if (rndindex == 0)
            {
                CPUSymbol = "O";
                playerSymbol = "X";
            } else
            {
                CPUSymbol = "X";
                playerSymbol = "O";
            }
            // update UI
            b_newGame.Visible = false; l_Title.Visible = false; l_Author.Visible = false;
            pb_box1.Visible = true; pb_box2.Visible = true;
            pb_box3.Visible = true; pb_box4.Visible = true;
            pb_box5.Visible = true; pb_box6.Visible = true;
            pb_box7.Visible = true; pb_box8.Visible = true;
            pb_box9.Visible = true; l_status.Visible = true;
            b_quitGame.Visible = true;
            NextTurn();
        }

        private void BoxClick(int boxNum)
        {
            if ((string)board[boxNum - 1] == "" && currentTurnName == "Player")
            {
                SetSymbol(boxNum, playerSymbol);
                board[boxNum - 1] = playerSymbol;
                NextTurn();
            }
        }

        public void SetSymbol(int boxNum, string symbol)
        {
            PictureBox pBox = (PictureBox)Controls["pb_box" + boxNum.ToString()];
            if (symbol == "X")
            {
                pBox.Image = Image.FromFile(APP_DIR + "/tic-tac-toe-X.png");
            }
            else if (symbol == "O")
            {
                pBox.Image = Image.FromFile(APP_DIR + "/tic-tac-toe-O.png");
            } else
            {
                pBox.Image = null;
            }
        }

        public void NextTurn()
        {
            if (XWin() && !(OWin())) // game finished, X wins
            {
                if (playerSymbol == "X")
                {
                    endGameMessage("Player Wins");
                } else
                {
                    endGameMessage("CPU Wins");
                }
                reset();
                ShowMenu();
            } else if (OWin() && !(XWin())) // game finished, O wins
            {
                if (playerSymbol == "O")
                {
                    endGameMessage("Player Wins");
                }
                else
                {
                    endGameMessage("CPU Wins");
                }
                reset();
                ShowMenu();
            } else if (boardFull()) // game finished, tie
            {
                endGameMessage("Tie Game");
                reset();
                ShowMenu();
            } else
            {
                if (currentTurnName == "CPU")
                {
                    currentTurnName = "Player";
                }
                else
                {
                    currentTurnName = "CPU";
                }
                turnNum++;
                l_status.Text = "[Turn " + turnNum + "] " + currentTurnName + "'s Turn";
                if (currentTurnName == "CPU") { CPUTurn(); }
            }
            Update();
        }

        void endGameMessage(string messageText)
        {
            MessageBox.Show(messageText, "Tic Tac Toe Test 1", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        bool boardFull()
        {
            return ((string)board[0] != "" && (string)board[1] != "" && (string)board[2] != "" && (string)board[3] != "" && (string)board[4] != "" && (string)board[5] != "" &&
                (string)board[6] != "" && (string)board[7] != "" && (string)board[8] != "");
        }

        int[] adjBoxIndices(int index)
        {
            switch (index) {
                case 0:
                    return new int[] { 1, 4, 3 };
                case 1:
                    return new int[] { 2, 4, 0 };
                case 2:
                    return new int[] { 1, 4, 5 };
                case 3:
                    return new int[] { 0, 4, 6 };
                case 4:
                    return new int[] { 0, 1, 2, 3, 5, 6, 7, 8 };
                case 5:
                    return new int[] { 2, 4, 8 };
                case 6:
                    return new int[] { 3, 4, 7 };
                case 7:
                    return new int[] { 6, 4, 8 };
                case 8:
                    return new int[] { 5, 4, 7 };
                default:
                    return new int[] { };
            }
        }

        private void CPUTurn()
        {
            List<int> emptyIndices = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if ((string)board[i] == "")
                {
                    emptyIndices.Add(i + 1);
                }
            }

            if (!(CPUFirstTurnDone))
            {
                // (0) use random empty spot
                Random x1 = new Random();
                int rndindex1 = x1.Next(emptyIndices.Count);
                delay_boxNum = emptyIndices[rndindex1];
                startDelay(); CPUFirstTurnDone = true; return;
            }

            CPUFirstTurnDone = true;

            // (1) complete triple where possible
            for (int i = 0; i < 8; i++)
            {
                int num1 = triples[i][0];
                int num2 = triples[i][1];
                int num3 = triples[i][2];
                if (((string)board[num1] == CPUSymbol && (string)board[num2] == CPUSymbol && (string)board[num3] == "") || ((string)board[num1] == CPUSymbol && (string)board[num2] == "" && (string)board[num3] == CPUSymbol) || ((string)board[num1] == "" && (string)board[num2] == CPUSymbol && (string)board[num3] == CPUSymbol))
                {
                    if ((string)board[num1] == "") { delay_boxNum = num1 + 1; startDelay(); return; }
                    if ((string)board[num2] == "") { delay_boxNum = num2 + 1; startDelay(); return; }
                    if ((string)board[num3] == "") { delay_boxNum = num3 + 1; startDelay(); return; }
                }
            }

            // (2) block player if has two in a row & empty spot
            for (int i = 0; i < 8; i++)
            {
                int num1 = triples[i][0];
                int num2 = triples[i][1];
                int num3 = triples[i][2];
                if (((string)board[num1] == playerSymbol && (string)board[num2] == playerSymbol && (string)board[num3] == "") || ((string)board[num1] == playerSymbol && (string)board[num2] == "" && (string)board[num3] == playerSymbol) || ((string)board[num1] == "" && (string)board[num2] == playerSymbol && (string)board[num3] == playerSymbol))
                {
                    if ((string)board[num1] == "") { delay_boxNum = num1 + 1; startDelay(); return; }
                    if ((string)board[num2] == "") { delay_boxNum = num2 + 1; startDelay(); return; }
                    if ((string)board[num3] == "") { delay_boxNum = num3 + 1; startDelay(); return; }
                }
            }

            // (3) place in empty spot adjacent to other CPU symbols or use random empty spot
            for (int i = 0; i < emptyIndices.Count; i++) {
                foreach (int adjIndex in adjBoxIndices(emptyIndices[i]))
                {
                    if ((string)board[adjIndex] != CPUSymbol)
                    {
                        emptyIndices.Remove(i);
                    }
                }
            }
            Random x2 = new Random();
            int rndindex2 = x2.Next(emptyIndices.Count);
            delay_boxNum = emptyIndices[rndindex2];
            startDelay();
        }

        private void startDelay()
        {
            delay_symbol = CPUSymbol;
            t_delaySymbol.Start();
        }

        public bool XWin()
        {
            return (((string)board[0] == (string)board[1] && (string)board[1] == (string)board[2] && (string)board[2] == "X") || ((string)board[3] == (string)board[4] && (string)board[4] == (string)board[5] && (string)board[5] == "X") ||
                ((string)board[6] == (string)board[7] && (string)board[7] == (string)board[8] && (string)board[8] == "X") || ((string)board[0] == (string)board[3] && (string)board[3] == (string)board[6] && (string)board[6] == "X") ||
                ((string)board[1] == (string)board[4] && (string)board[4] == (string)board[7] && (string)board[7] == "X") || ((string)board[2] == (string)board[5] && (string)board[5] == (string)board[8] && (string)board[8] == "X") ||
                ((string)board[0] == (string)board[4] && (string)board[4] == (string)board[8] && (string)board[8] == "X") || ((string)board[2] == (string)board[4] && (string)board[4] == (string)board[6] && (string)board[6] == "X"));
        }

        public bool OWin()
        {
            return (((string)board[0] == (string)board[1] && (string)board[1] == (string)board[2] && (string)board[2] == "O") || ((string)board[3] == (string)board[4] && (string)board[4] == (string)board[5] && (string)board[5] == "O") ||
                ((string)board[6] == (string)board[7] && (string)board[7] == (string)board[8] && (string)board[8] == "O") || ((string)board[0] == (string)board[3] && (string)board[3] == (string)board[6] && (string)board[6] == "O") ||
                ((string)board[1] == (string)board[4] && (string)board[4] == (string)board[7] && (string)board[7] == "O") || ((string)board[2] == (string)board[5] && (string)board[5] == (string)board[8] && (string)board[8] == "O") ||
                ((string)board[0] == (string)board[4] && (string)board[4] == (string)board[8] && (string)board[8] == "O") || ((string)board[2] == (string)board[4] && (string)board[4] == (string)board[6] && (string)board[6] == "O"));
        }

        public void reset()
        {
            for (int i = 1; i < 10; i++)
            {
                SetSymbol(i, "");
            }
            board.Clear();
        }

        // Control Methods

        private void b_newGame_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void pb_box1_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void pb_box2_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void pb_box3_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void pb_box4_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void pb_box5_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void pb_box6_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void pb_box7_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void pb_box8_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void pb_box9_Click(object sender, EventArgs e)
        {
            PictureBox senderPBox = (PictureBox)sender;
            BoxClick(Int32.Parse(senderPBox.Name.Substring(senderPBox.Name.Length - 1)));
        }

        private void t_delaySymbol_Tick(object sender, EventArgs e)
        {
            SetSymbol(delay_boxNum, delay_symbol);
            board[delay_boxNum - 1] = CPUSymbol;
            t_delaySymbol.Enabled = false;
            NextTurn();
            t_delaySymbol.Stop();
        }

        private void b_quitGame_Click(object sender, EventArgs e)
        {
            reset();
            ShowMenu();
        }
    }
}
