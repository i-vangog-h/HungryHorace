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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Map map;
        Graphics g;
        private void buttonStart_Click(object sender, EventArgs e)
        {
            g = CreateGraphics();
            map = new Map("newplan.txt", "icons.png");
            this.Text = "Zbývá sebrat " + map.coinsLeft + " minci";

            timer1.Enabled = true;
            buttonStart.Visible = false;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            switch (map.state)
            {
                case State.running:
                    map.MoveObjects(pressedkey);
                    map.PrintOut(g, ClientSize.Width, ClientSize.Height);
                    this.Text = "Zbývá sebrat " + map.coinsLeft + " minci";
                    break;
                case State.win:
                    timer1.Enabled = false;
                    MessageBox.Show("You won mate!");
                    break;
                case State.lost:
                    timer1.Enabled = false;
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

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            this.pressedkey = PressedKey.none;
        }
    }
}
