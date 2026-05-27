namespace VirusX
{
    partial class SpawnMonster
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpawnMonster));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.GetInfo = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.MaxSpawn = new System.Windows.Forms.TextBox();
            this.XYSpawn = new System.Windows.Forms.TextBox();
            this.AutoXY = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listView3 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.Info;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(239, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(126, 21);
            this.comboBox1.TabIndex = 38;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // GetInfo
            // 
            this.GetInfo.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.GetInfo.ForeColor = System.Drawing.Color.Black;
            this.GetInfo.Location = new System.Drawing.Point(-1, 141);
            this.GetInfo.Name = "GetInfo";
            this.GetInfo.Size = new System.Drawing.Size(191, 31);
            this.GetInfo.TabIndex = 10;
            this.GetInfo.Text = "AddMonster";
            this.GetInfo.UseVisualStyleBackColor = false;
            this.GetInfo.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button5.ForeColor = System.Drawing.Color.Black;
            this.button5.Location = new System.Drawing.Point(371, 10);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(41, 23);
            this.button5.TabIndex = 42;
            this.button5.Text = "R";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.label8.Font = new System.Drawing.Font("Microsoft Tai Le", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(12, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 63;
            this.label8.Text = "X,Y Spawn";
            // 
            // MaxSpawn
            // 
            this.MaxSpawn.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.MaxSpawn.BackColor = System.Drawing.SystemColors.Info;
            this.MaxSpawn.Location = new System.Drawing.Point(88, 67);
            this.MaxSpawn.Name = "MaxSpawn";
            this.MaxSpawn.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MaxSpawn.Size = new System.Drawing.Size(100, 20);
            this.MaxSpawn.TabIndex = 0;
            // 
            // XYSpawn
            // 
            this.XYSpawn.BackColor = System.Drawing.SystemColors.Info;
            this.XYSpawn.Location = new System.Drawing.Point(88, 37);
            this.XYSpawn.Name = "XYSpawn";
            this.XYSpawn.Size = new System.Drawing.Size(100, 20);
            this.XYSpawn.TabIndex = 62;
            // 
            // AutoXY
            // 
            this.AutoXY.Enabled = true;
            this.AutoXY.Interval = 1;
            this.AutoXY.Tick += new System.EventHandler(this.AutoGetXY);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "MaxCount";
            // 
            // textBox1
            // 
            this.textBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.Location = new System.Drawing.Point(88, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 65;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "IDMonster";
            // 
            // listView3
            // 
            this.listView3.Location = new System.Drawing.Point(418, 7);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(533, 361);
            this.listView3.TabIndex = 69;
            this.listView3.ReadOnly = false;
            this.listView3.RowHeadersVisible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(196, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 31);
            this.button1.TabIndex = 70;
            this.button1.Text = "CopyListView";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button2_Click);
            // 
            // SpawnMonster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(952, 368);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.XYSpawn);
            this.Controls.Add(this.GetInfo);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.MaxSpawn);
            this.Controls.Add(this.comboBox1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpawnMonster";
            this.Text = "SpawnMonster";
            this.Load += new System.EventHandler(this.Control_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listView3)).EndInit();
        }

        #endregion
        private System.Windows.Forms.TextBox MaxSpawn;
        public System.Windows.Forms.Timer AutoXY;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button GetInfo;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox XYSpawn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView listView3;
        private System.Windows.Forms.Button button1;

    }
}