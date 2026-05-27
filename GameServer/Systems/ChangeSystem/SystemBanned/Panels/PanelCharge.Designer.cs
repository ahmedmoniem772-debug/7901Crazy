namespace VirusX.Systems.ChangeSystem.SystemBanned.Panels
{
    partial class PanelCharge
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.User = new System.Windows.Forms.TextBox();
            this.USD = new System.Windows.Forms.TextBox();
            this.USDT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Donate$";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "UserName";
            // 
            // User
            // 
            this.User.Location = new System.Drawing.Point(83, 6);
            this.User.Name = "User";
            this.User.Size = new System.Drawing.Size(100, 21);
            this.User.TabIndex = 2;
            // 
            // USD
            // 
            this.USD.Location = new System.Drawing.Point(83, 54);
            this.USD.Name = "USD";
            this.USD.Size = new System.Drawing.Size(100, 21);
            this.USD.TabIndex = 3;
            // 
            // USDT
            // 
            this.USDT.Location = new System.Drawing.Point(187, 158);
            this.USDT.Name = "USDT";
            this.USDT.Size = new System.Drawing.Size(125, 33);
            this.USDT.TabIndex = 4;
            this.USDT.Text = "SendDonate";
            this.USDT.UseVisualStyleBackColor = true;
            this.USDT.Click += new System.EventHandler(this.button1_Click);
            // 
            // PanelCharge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(547, 203);
            this.Controls.Add(this.USDT);
            this.Controls.Add(this.USD);
            this.Controls.Add(this.User);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PanelCharge";
            this.Text = "Donate Panel @MTHero";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox User;
        private System.Windows.Forms.TextBox USD;
        private System.Windows.Forms.Button USDT;
    }
}