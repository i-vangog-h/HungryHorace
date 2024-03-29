﻿// Mini arcade game inspired by Hungry Horace
// Ivan Turko, I. year, spec. Informatika (сz)
// Summer semestr 2021/22
// Prorgamování 2, NPRG031 (cz)

//////////////////////////////////////////////

/// RUNTIME AND GUI ///

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
    public partial class Form_1 : Form
    {
        public int levelnumber;
        private int numberoflevels;
        public int score;
        public Form_1()
        {
            InitializeComponent();

            System.IO.StreamReader sr = new System.IO.StreamReader("newplan.txt") ; //The first string of .txt file contatins the number of levels
            this.numberoflevels = int.Parse(sr.ReadLine());
            sr.Close();

            this.levelnumber = 1;
            this.score = 0;
            this.livesleft = 3;

            ActivateMenu();
            
            this.Text = "Hungry Horace Inspired";
        }

        Map map;
        Graphics g;


        // Handling Key Inputs

        PressedKey pressedkey = PressedKey.none;
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
        //Not used in the game, find it better (faster, more dynamic) without key up state, can be switched by setting form's property KeyPreview to true

        //

        /// RUNTIME ///

        private void buttonStart_Click(object sender, EventArgs e)
        {
            g = CreateGraphics();
            DeactivateMenu();
            StartNewLevel();
        }

        private void StartNewLevel()
        {
            map = new Map("newplan.txt", "icons.png", levelnumber);

            if (gamemode != Gamemode.hardcore)
            {
                this.Text = "COINS LEFT: " + map.coinsLeft + "    YOUR SCORE: " + score;
            }
            else
            {
                this.Text = "COINS LEFT: " + map.coinsLeft + "    LIVES LEFT: " + livesleft + "    YOUR SCORE: " + score;
            }

            Runtime.Enabled = true;
            ChaseChillStates.Enabled = true;
            PauseButton.Enabled = true;
            PauseButton.Visible = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Runtime_Tick(object sender, EventArgs e)
        {
            switch (map.state)
            {
                case State.running:
                    map.MoveObjects(pressedkey);
                    map.PrintOut(g, ClientSize.Width, ClientSize.Height);
                    if (gamemode != Gamemode.hardcore)
                    {
                        this.Text = "COINS LEFT: " + map.coinsLeft + "    YOUR SCORE: " + (score + map.score);
                    }
                    else
                    {
                        this.Text = "COINS LEFT: " + map.coinsLeft + "    LIVES LEFT: " + livesleft + "    YOUR SCORE: " + (score + map.score);
                    }
                    break;
                case State.win:
                    Runtime.Enabled = false;
                    ChaseChillStates.Enabled = false;
                    FearState.Enabled = false;
                    if (levelnumber < numberoflevels)
                    {
                        levelnumber += 1;
                        if (levelnumber == 3)
                        {
                            FearState.Interval = 10000;
                        }
                        score += map.score;
                        StartNewLevel();
                    }
                    else
                    {
                        g.Clear(BackColor);
                        score += map.score;
                        Score.Text = "SCORE: " + score;
                        score = 0;
                        /// VICTORY WINDOW
                        Score.Visible = true;
                        MenuButton.Visible = true;
                        VictoryLabel.Visible = true;
                        PauseButton.Visible = false;
                        /// 
                    }
                    break;
                case State.eaten:
                    ChaseChillStates.Enabled = false;
                    switch (gamemode)
                    {
                        case Gamemode.normal:
                            Runtime.Enabled = false;
                            StartNewLevel();
                            break;
                        case Gamemode.hardcore:
                            if (livesleft > 0)
                            {
                                livesleft -= 1;
                                StartNewLevel();
                            }
                            else
                            {
                                map.state = State.lost;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case State.lost:
                    g.Clear(BackColor);
                    Runtime.Enabled = false;
                    ChaseChillStates.Enabled = false;
                    /// GAME OVER WINDOW
                    GameOverLabel.Visible = true;
                    MenuButton.Visible = true;
                    ///
                    break;
                default:
                    break;
            }

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
            if (interval.TotalMilliseconds > FearState.Interval - 1)
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

        ///



        /// INTERFACE ///

        public enum Gamemode { normal, hardcore };
        Gamemode gamemode = Gamemode.normal;
        public int livesleft;

        // Methods 
        
        public void ActivateMenu()
        {
            Title.Visible = true;
            buttonStart.Visible = true;
            GameModeButton.Visible = true;
            TutorialButton.Visible = true;
            ExitButton.Visible = true;

            Score.Visible = false;
            Hardcore.Visible = false;
            Normal.Visible = false;
            NormalLabel.Visible = false;
            HardcoreLabel.Visible = false;
            GameOverLabel.Visible = false;
            MenuButton.Visible = false;
            VictoryLabel.Visible = false;
            TutorialLabel.Visible = false;
            PausePanel.Visible = false;
            PauseButton.Visible = false;
        }
        public void DeactivateMenu()
        {
            buttonStart.Visible = false;
            Title.Visible = false;
            Score.Visible = false;
            GameModeButton.Visible = false;
            TutorialButton.Visible = false;
            Normal.Visible = false;
            Hardcore.Visible = false;
            ExitButton.Visible = false;
        }


        // User Data Handling

        private void GameModeButton_Click(object sender, EventArgs e)
        {
            
            Hardcore.Visible = !Hardcore.Visible;
            Normal.Visible = !Normal.Visible;
            TutorialButton.Visible = !TutorialButton.Visible;

        }

        private void Hardcore_MouseHover(object sender, EventArgs e)
        {
            HardcoreLabel.Visible = true;
        }

        private void Hardcore_MouseLeave(object sender, EventArgs e)
        {
            HardcoreLabel.Visible = false;
        }

        private void Normal_MouseHover(object sender, EventArgs e)
        {
            NormalLabel.Visible = true;
        }

        private void Normal_MouseLeave(object sender, EventArgs e)
        {
            NormalLabel.Visible = false;
        }

        private void Normal_Click(object sender, EventArgs e)
        {
            gamemode = Gamemode.normal;
            Normal.BackColor = Color.Red;
            Hardcore.BackColor = Color.SteelBlue;
        }

        private void Hardcore_Click(object sender, EventArgs e)
        {
            gamemode = Gamemode.hardcore;
            Hardcore.BackColor = Color.Red;
            Normal.BackColor = Color.SteelBlue;
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            MenuButton.Visible = false;
            GameOverLabel.Visible = false;
            Score.Visible = false;
            VictoryLabel.Visible = false;
            PauseButton.Visible = false;
            ActivateMenu();
            levelnumber = 1;
            this.Text = "Hungry Horace Inspired";
        }

        private void TutorialButton_Click(object sender, EventArgs e)
        {
            TutorialLabel.Visible = !TutorialLabel.Visible;
            GameModeButton.Enabled = !GameModeButton.Enabled;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            Runtime.Stop();
            if (ChaseChillStates.Enabled)
                ChaseChillStates.Stop();
            else
                FearState.Stop();
            PausePanel.Visible = true;
            PauseButton.Enabled = false;
        }

        private void PauseMenuButton_Click(object sender, EventArgs e)
        {
            g.Clear(BackColor);
            Runtime.Enabled = false;
            ChaseChillStates.Enabled = false;
            FearState.Enabled = false;
            PausePanel.Visible = false;
            PauseButton.Visible = false;
            ActivateMenu();
            this.Text = "Hungry Horace Inspired";
            levelnumber = 1;
            
        }

        private void ResumePauseButton_Click(object sender, EventArgs e)
        {
            PauseButton.Enabled = true;
            PausePanel.Visible = false;
            Runtime.Start();
            if (ChaseChillStates.Enabled)
                ChaseChillStates.Start();
            else
                FearState.Start();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        ///
        
    }
}
