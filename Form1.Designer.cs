
namespace HungryHorace
{
    partial class Window
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.Runtime = new System.Windows.Forms.Timer(this.components);
            this.ChaseChillStates = new System.Windows.Forms.Timer(this.components);
            this.FearState = new System.Windows.Forms.Timer(this.components);
            this.Title = new System.Windows.Forms.TextBox();
            this.Score = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.buttonStart.Font = new System.Drawing.Font("Sitka Small", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonStart.Location = new System.Drawing.Point(283, 145);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(292, 190);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "PLAY";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // Runtime
            // 
            this.Runtime.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ChaseChillStates
            // 
            this.ChaseChillStates.Interval = 5000;
            this.ChaseChillStates.Tick += new System.EventHandler(this.ChaseChillState_Tick);
            // 
            // FearState
            // 
            this.FearState.Interval = 10001;
            this.FearState.Tick += new System.EventHandler(this.FearState_Tick);
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Title.Font = new System.Drawing.Font("Showcard Gothic", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Title.ForeColor = System.Drawing.Color.Blue;
            this.Title.Location = new System.Drawing.Point(219, 48);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(400, 61);
            this.Title.TabIndex = 1;
            this.Title.Text = "Hungry Horace";
            this.Title.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Score
            // 
            this.Score.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Score.Font = new System.Drawing.Font("SimSun", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Score.Location = new System.Drawing.Point(209, 203);
            this.Score.Name = "Score";
            this.Score.Size = new System.Drawing.Size(429, 99);
            this.Score.TabIndex = 2;
            this.Score.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Window
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(863, 516);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.Score);
            this.Name = "Window";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Timer Runtime;
        private System.Windows.Forms.Timer ChaseChillStates;
        private System.Windows.Forms.Timer FearState;
        private System.Windows.Forms.TextBox Title;
        private System.Windows.Forms.TextBox Score;
    }
}

