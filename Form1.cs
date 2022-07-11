using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HungryHorace
{
    public partial class Window : Form
    {
        public int levelnumber;
        private int numberoflevels;
        public int score;
        public Window()
        {
            InitializeComponent();
            this.levelnumber = 1;
            System.IO.StreamReader sr = new System.IO.StreamReader("newplan.txt") ;
            this.numberoflevels = int.Parse(sr.ReadLine());
            sr.Close();
            this.score = 0;
            Score.Visible = false;
        }

        Map map;
        Graphics g;
        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartNewLevel();
            buttonStart.Visible = false;
            Title.Visible = false;
            Score.Visible = false;
            
        }

        private void StartNewLevel()
        {
            g = CreateGraphics();
            map = new Map("newplan.txt", "icons.png", levelnumber);
            this.Text = map.coinsLeft + " COINS LEFT";

            Runtime.Enabled = true;
            ChaseChillStates.Enabled = true;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            switch (map.state)
            {
                case State.running:
                    map.MoveObjects(pressedkey);
                    map.PrintOut(g, ClientSize.Width, ClientSize.Height);
                    this.Text = map.coinsLeft + " COINS LEFT";
                    break;
                case State.win:
                    Runtime.Enabled = false;
                    ChaseChillStates.Enabled = false;
                    if (levelnumber < numberoflevels)
                    {
                        levelnumber += 1;
                        score += map.score;
                        StartNewLevel();
                    }
                    else
                    {
                        Runtime.Enabled = false;
                        g.Clear(BackColor);
                        score += map.score;
                        Score.Text = "SCORE: " + score;
                        Score.Visible = true;
                    }
                    break;
                case State.lost:
                    Runtime.Enabled = false;
                    ChaseChillStates.Enabled = false;
                    MessageBox.Show("You lost!");
                    break;
                default:
                    break;
            }
        }

        PressedKey pressedkey = PressedKey.none;

        // HACK na odchyceni stisku sipek
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                pressedkey = PressedKey.up;
                return true;
            }
            if (keyData == Keys.Down)
            {
                pressedkey = PressedKey.down;
                return true;
            }
            if (keyData == Keys.Left)
            {
                pressedkey = PressedKey.left;
                return true;
            }
            if (keyData == Keys.Right)
            {
                pressedkey = PressedKey.right; 
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyUp_1(object sender, KeyEventArgs e)
        {
            pressedkey = PressedKey.none;
        }


        private DateTime starttime;
        private void ChaseChillState_Tick(object sender, EventArgs e)
        {
            
            foreach(Ghost ghost in map.Ghosts)
            {
                switch (ghost.state)
                {
                    case Map.GhostState.chase:
                        ghost.state = Map.GhostState.chill;
                        break;
                    case Map.GhostState.chill:
                        ghost.state = Map.GhostState.chase;
                        break;
                    case Map.GhostState.fear:
                        starttime = DateTime.Now;
                        FearState.Enabled = true;
                        ChaseChillStates.Enabled = false;
                        break;
                    default:
                        break;  
                }
            }
        }

        private void FearState_Tick(object sender, EventArgs e)
        {
            TimeSpan interval = DateTime.Now - starttime;
            if (interval.TotalMilliseconds > 10000)
            {
               map.RestoreGhosts();
               foreach (Ghost ghost in map.Ghosts)
                {
                    ghost.state = Map.GhostState.chill;
                }
                ChaseChillStates.Enabled = true;
                FearState.Enabled = false;
            }

        }
    }
}
