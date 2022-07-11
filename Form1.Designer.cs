
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
            this.GameModeButton = new System.Windows.Forms.Button();
            this.TutorialButton = new System.Windows.Forms.Button();
            this.Normal = new System.Windows.Forms.Button();
            this.Hardcore = new System.Windows.Forms.Button();
            this.NormalLabel = new System.Windows.Forms.Label();
            this.HardcoreLabel = new System.Windows.Forms.Label();
            this.GameOverLabel = new System.Windows.Forms.Label();
            this.MenuButton = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.Score = new System.Windows.Forms.Label();
            this.VictoryLabel = new System.Windows.Forms.Label();
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
            this.buttonStart.Location = new System.Drawing.Point(298, 172);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(292, 190);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "PLAY";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // Runtime
            // 
            this.Runtime.Interval = 150;
            this.Runtime.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ChaseChillStates
            // 
            this.ChaseChillStates.Interval = 5000;
            this.ChaseChillStates.Tick += new System.EventHandler(this.ChaseChillState_Tick);
            // 
            // FearState
            // 
            this.FearState.Interval = 5001;
            this.FearState.Tick += new System.EventHandler(this.FearState_Tick);
            // 
            // GameModeButton
            // 
            this.GameModeButton.BackColor = System.Drawing.Color.SteelBlue;
            this.GameModeButton.Font = new System.Drawing.Font("Segoe UI Black", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.GameModeButton.Location = new System.Drawing.Point(379, 405);
            this.GameModeButton.Name = "GameModeButton";
            this.GameModeButton.Size = new System.Drawing.Size(142, 42);
            this.GameModeButton.TabIndex = 3;
            this.GameModeButton.Text = "Game mode";
            this.GameModeButton.UseVisualStyleBackColor = false;
            this.GameModeButton.Click += new System.EventHandler(this.GameModeButton_Click);
            // 
            // TutorialButton
            // 
            this.TutorialButton.BackColor = System.Drawing.Color.SteelBlue;
            this.TutorialButton.Font = new System.Drawing.Font("Segoe UI Black", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TutorialButton.Location = new System.Drawing.Point(379, 453);
            this.TutorialButton.Name = "TutorialButton";
            this.TutorialButton.Size = new System.Drawing.Size(142, 42);
            this.TutorialButton.TabIndex = 4;
            this.TutorialButton.Text = "Tutorial";
            this.TutorialButton.UseVisualStyleBackColor = false;
            // 
            // Normal
            // 
            this.Normal.BackColor = System.Drawing.Color.Red;
            this.Normal.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Normal.Location = new System.Drawing.Point(390, 453);
            this.Normal.Name = "Normal";
            this.Normal.Size = new System.Drawing.Size(121, 37);
            this.Normal.TabIndex = 5;
            this.Normal.Text = "Normal";
            this.Normal.UseVisualStyleBackColor = false;
            this.Normal.Click += new System.EventHandler(this.Normal_Click);
            this.Normal.MouseLeave += new System.EventHandler(this.Normal_MouseLeave);
            this.Normal.MouseHover += new System.EventHandler(this.Normal_MouseHover);
            // 
            // Hardcore
            // 
            this.Hardcore.BackColor = System.Drawing.Color.SteelBlue;
            this.Hardcore.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Hardcore.Location = new System.Drawing.Point(390, 496);
            this.Hardcore.Name = "Hardcore";
            this.Hardcore.Size = new System.Drawing.Size(121, 38);
            this.Hardcore.TabIndex = 6;
            this.Hardcore.Text = "Hardcore";
            this.Hardcore.UseVisualStyleBackColor = false;
            this.Hardcore.Click += new System.EventHandler(this.Hardcore_Click);
            this.Hardcore.MouseLeave += new System.EventHandler(this.Hardcore_MouseLeave);
            this.Hardcore.MouseHover += new System.EventHandler(this.Hardcore_MouseHover);
            // 
            // NormalLabel
            // 
            this.NormalLabel.AutoSize = true;
            this.NormalLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NormalLabel.Font = new System.Drawing.Font("Book Antiqua", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.NormalLabel.Location = new System.Drawing.Point(153, 473);
            this.NormalLabel.Name = "NormalLabel";
            this.NormalLabel.Size = new System.Drawing.Size(231, 22);
            this.NormalLabel.TabIndex = 7;
            this.NormalLabel.Text = "Unlimited number of lives";
            // 
            // HardcoreLabel
            // 
            this.HardcoreLabel.AutoSize = true;
            this.HardcoreLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.HardcoreLabel.Font = new System.Drawing.Font("Book Antiqua", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.HardcoreLabel.Location = new System.Drawing.Point(147, 495);
            this.HardcoreLabel.Name = "HardcoreLabel";
            this.HardcoreLabel.Size = new System.Drawing.Size(237, 22);
            this.HardcoreLabel.TabIndex = 8;
            this.HardcoreLabel.Text = "Limited number of lives (3)";
            // 
            // GameOverLabel
            // 
            this.GameOverLabel.AutoSize = true;
            this.GameOverLabel.Font = new System.Drawing.Font("Showcard Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GameOverLabel.Location = new System.Drawing.Point(205, 202);
            this.GameOverLabel.Name = "GameOverLabel";
            this.GameOverLabel.Size = new System.Drawing.Size(478, 98);
            this.GameOverLabel.TabIndex = 9;
            this.GameOverLabel.Text = "Game Over";
            this.GameOverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MenuButton
            // 
            this.MenuButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.MenuButton.Font = new System.Drawing.Font("Verdana", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.MenuButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MenuButton.Location = new System.Drawing.Point(270, 466);
            this.MenuButton.Name = "MenuButton";
            this.MenuButton.Size = new System.Drawing.Size(370, 86);
            this.MenuButton.TabIndex = 10;
            this.MenuButton.Text = "Go to menu";
            this.MenuButton.UseVisualStyleBackColor = false;
            this.MenuButton.Click += new System.EventHandler(this.MenuButton_Click);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Showcard Gothic", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Title.ForeColor = System.Drawing.Color.Blue;
            this.Title.Location = new System.Drawing.Point(237, 57);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(430, 59);
            this.Title.TabIndex = 11;
            this.Title.Text = "Hungry Horace";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Score
            // 
            this.Score.AutoSize = true;
            this.Score.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Score.Location = new System.Drawing.Point(338, 259);
            this.Score.Name = "Score";
            this.Score.Size = new System.Drawing.Size(147, 54);
            this.Score.TabIndex = 12;
            this.Score.Text = "SCORE";
            this.Score.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VictoryLabel
            // 
            this.VictoryLabel.AutoSize = true;
            this.VictoryLabel.Font = new System.Drawing.Font("Showcard Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VictoryLabel.ForeColor = System.Drawing.Color.Gold;
            this.VictoryLabel.Location = new System.Drawing.Point(237, 57);
            this.VictoryLabel.Name = "VictoryLabel";
            this.VictoryLabel.Size = new System.Drawing.Size(425, 98);
            this.VictoryLabel.TabIndex = 13;
            this.VictoryLabel.Text = "VICTORY!";
            // 
            // Window
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(911, 623);
            this.Controls.Add(this.Score);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.HardcoreLabel);
            this.Controls.Add(this.NormalLabel);
            this.Controls.Add(this.Hardcore);
            this.Controls.Add(this.Normal);
            this.Controls.Add(this.GameModeButton);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.GameOverLabel);
            this.Controls.Add(this.MenuButton);
            this.Controls.Add(this.TutorialButton);
            this.Controls.Add(this.VictoryLabel);
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
        private System.Windows.Forms.Button GameModeButton;
        private System.Windows.Forms.Button TutorialButton;
        private System.Windows.Forms.Button Normal;
        private System.Windows.Forms.Button Hardcore;
        private System.Windows.Forms.Label NormalLabel;
        private System.Windows.Forms.Label HardcoreLabel;
        private System.Windows.Forms.Label GameOverLabel;
        private System.Windows.Forms.Button MenuButton;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label Score;
        private System.Windows.Forms.Label VictoryLabel;
    }
}

