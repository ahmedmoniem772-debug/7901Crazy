namespace VirusX
{
    partial class Packets
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ConvertBtn = new System.Windows.Forms.Button();
            this.ConverterBox = new System.Windows.Forms.GroupBox();
            this.hexval = new System.Windows.Forms.Label();
            this.stringVal = new System.Windows.Forms.Label();
            this.decVal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.PacketViewerGroupBox = new System.Windows.Forms.GroupBox();
            this.CopyPacket = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ClearListBtn = new System.Windows.Forms.Button();
            this.Client2ServerBox = new System.Windows.Forms.GroupBox();
            this.ClientAutoScrollBox = new System.Windows.Forms.CheckBox();
            this.clientPackets = new System.Windows.Forms.DataGridView();
            this.PacketNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LenghtColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serverPackets = new System.Windows.Forms.DataGridView();
            this.PacketNameColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LenghtColomn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Server2ClientBox = new System.Windows.Forms.GroupBox();
            this.ServerAutoScrollBox = new System.Windows.Forms.CheckBox();
            this.AddFltrBtn = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.FilterBox = new System.Windows.Forms.GroupBox();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.PacketViewerBoxx = new Be.Windows.Forms.HexBox();
            this.ConverterBox.SuspendLayout();
            this.PacketViewerGroupBox.SuspendLayout();
            this.Client2ServerBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientPackets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverPackets)).BeginInit();
            this.Server2ClientBox.SuspendLayout();
            this.FilterBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(178, 408);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(235, 73);
            this.button2.TabIndex = 22;
            this.button2.Text = "Test";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(52, 27);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 20);
            this.textBox1.TabIndex = 12;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // ConvertBtn
            // 
            this.ConvertBtn.Location = new System.Drawing.Point(189, 24);
            this.ConvertBtn.Name = "ConvertBtn";
            this.ConvertBtn.Size = new System.Drawing.Size(75, 23);
            this.ConvertBtn.TabIndex = 11;
            this.ConvertBtn.Text = "Convert";
            this.ConvertBtn.UseVisualStyleBackColor = true;
            // 
            // ConverterBox
            // 
            this.ConverterBox.Controls.Add(this.textBox1);
            this.ConverterBox.Controls.Add(this.ConvertBtn);
            this.ConverterBox.Controls.Add(this.hexval);
            this.ConverterBox.Controls.Add(this.stringVal);
            this.ConverterBox.Controls.Add(this.decVal);
            this.ConverterBox.Location = new System.Drawing.Point(697, 408);
            this.ConverterBox.Name = "ConverterBox";
            this.ConverterBox.Size = new System.Drawing.Size(267, 113);
            this.ConverterBox.TabIndex = 21;
            this.ConverterBox.TabStop = false;
            this.ConverterBox.Text = "Converter";
            // 
            // hexval
            // 
            this.hexval.AutoSize = true;
            this.hexval.Font = new System.Drawing.Font("Impact", 12F);
            this.hexval.ForeColor = System.Drawing.Color.Maroon;
            this.hexval.Location = new System.Drawing.Point(7, 23);
            this.hexval.Name = "hexval";
            this.hexval.Size = new System.Drawing.Size(41, 20);
            this.hexval.TabIndex = 9;
            this.hexval.Text = "Hex : ";
            // 
            // stringVal
            // 
            this.stringVal.AutoSize = true;
            this.stringVal.Font = new System.Drawing.Font("Impact", 12F);
            this.stringVal.ForeColor = System.Drawing.Color.Maroon;
            this.stringVal.Location = new System.Drawing.Point(6, 84);
            this.stringVal.Name = "stringVal";
            this.stringVal.Size = new System.Drawing.Size(46, 20);
            this.stringVal.TabIndex = 8;
            this.stringVal.Text = "ASCII:";
            // 
            // decVal
            // 
            this.decVal.AutoSize = true;
            this.decVal.Font = new System.Drawing.Font("Impact", 12F);
            this.decVal.ForeColor = System.Drawing.Color.Maroon;
            this.decVal.Location = new System.Drawing.Point(6, 53);
            this.decVal.Name = "decVal";
            this.decVal.Size = new System.Drawing.Size(71, 20);
            this.decVal.TabIndex = 0;
            this.decVal.Text = "Decimal : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Impact", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(6, 334);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Current Offest : 0";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Location = new System.Drawing.Point(6, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(143, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Don\'t Show \\ Show Only";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(477, 340);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "From Index : ";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(548, 338);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(28, 20);
            this.textBox2.TabIndex = 15;
            this.textBox2.Text = "4";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(349, 336);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "7-bit ---> Normal";
            this.button1.UseVisualStyleBackColor = true;
            this.ConvertBtn.Click += new System.EventHandler(this.button1_Click_3);
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.PacketViewerBoxx.SelectionStartChanged += new System.EventHandler(this.PacketViewerBoxx_SelectionStartChanged);
            this.CopyPacket.Click += new System.EventHandler(this.button8_Click);
            this.comboBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AddPacketID);
            this.AddFltrBtn.Click += new System.EventHandler(this.button3_Click);
            this.ClearListBtn.Click += new System.EventHandler(this.button6_Click);
            this.ClearBtn.Click += new System.EventHandler(this.button5_Click);
            this.serverPackets.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serverPackets_CellContentClick);
            this.serverPackets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serverPackets_CellContentClick);
            this.serverPackets.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serverPackets_CellContentClick);
            this.serverPackets.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serverPackets_CellContentClick);
            this.serverPackets.KeyUp += new System.Windows.Forms.KeyEventHandler(this.serverPacket_Up);

            this.clientPackets.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clientPackets_CellContentClick);
            this.clientPackets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clientPackets_CellContentClick);
            this.clientPackets.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clientPackets_CellContentClick);
            this.clientPackets.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clientPackets_CellContentClick);
            this.clientPackets.KeyUp += new System.Windows.Forms.KeyEventHandler(this.clientPackets_KeyUp);
            // 
            // PacketViewerGroupBox
            // 
            this.PacketViewerGroupBox.Controls.Add(this.PacketViewerBoxx);
            this.PacketViewerGroupBox.Controls.Add(this.label2);
            this.PacketViewerGroupBox.Controls.Add(this.textBox2);
            this.PacketViewerGroupBox.Controls.Add(this.button1);
            this.PacketViewerGroupBox.Controls.Add(this.label1);
            this.PacketViewerGroupBox.Location = new System.Drawing.Point(16, 16);
            this.PacketViewerGroupBox.Name = "PacketViewerGroupBox";
            this.PacketViewerGroupBox.Size = new System.Drawing.Size(583, 366);
            this.PacketViewerGroupBox.TabIndex = 20;
            this.PacketViewerGroupBox.TabStop = false;
            this.PacketViewerGroupBox.Text = "Packet Viewer";
            // 
            // CopyPacket
            // 
            this.CopyPacket.Location = new System.Drawing.Point(849, 321);
            this.CopyPacket.Name = "CopyPacket";
            this.CopyPacket.Size = new System.Drawing.Size(225, 25);
            this.CopyPacket.TabIndex = 19;
            this.CopyPacket.Text = "Copy Packet";
            this.CopyPacket.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ClearListBtn
            // 
            this.ClearListBtn.Location = new System.Drawing.Point(375, 17);
            this.ClearListBtn.Name = "ClearListBtn";
            this.ClearListBtn.Size = new System.Drawing.Size(97, 23);
            this.ClearListBtn.TabIndex = 4;
            this.ClearListBtn.Text = "Clear List";
            this.ClearListBtn.UseVisualStyleBackColor = true;
            // 
            // Client2ServerBox
            // 
            this.Client2ServerBox.Controls.Add(this.ClientAutoScrollBox);
            this.Client2ServerBox.Controls.Add(this.clientPackets);
            this.Client2ServerBox.Location = new System.Drawing.Point(605, 16);
            this.Client2ServerBox.Name = "Client2ServerBox";
            this.Client2ServerBox.Size = new System.Drawing.Size(237, 299);
            this.Client2ServerBox.TabIndex = 15;
            this.Client2ServerBox.TabStop = false;
            this.Client2ServerBox.Text = "Client -> Server";
            // 
            // ClientAutoScrollBox
            // 
            this.ClientAutoScrollBox.AutoSize = true;
            this.ClientAutoScrollBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ClientAutoScrollBox.Location = new System.Drawing.Point(60, 276);
            this.ClientAutoScrollBox.Name = "ClientAutoScrollBox";
            this.ClientAutoScrollBox.Size = new System.Drawing.Size(108, 17);
            this.ClientAutoScrollBox.TabIndex = 11;
            this.ClientAutoScrollBox.Text = "Auto Scroll Down";
            this.ClientAutoScrollBox.UseVisualStyleBackColor = true;
            // 
            // clientPackets
            // 
            this.clientPackets.AllowUserToAddRows = false;
            this.clientPackets.AllowUserToResizeColumns = false;
            this.clientPackets.AllowUserToResizeRows = false;
            this.clientPackets.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.clientPackets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.clientPackets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientPackets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PacketNameColumn,
            this.dataGridViewTextBoxColumn4,
            this.LenghtColumn,
            this.Column2});
            this.clientPackets.Location = new System.Drawing.Point(6, 19);
            this.clientPackets.MultiSelect = false;
            this.clientPackets.Name = "clientPackets";
            this.clientPackets.ReadOnly = true;
            this.clientPackets.RowHeadersVisible = false;
            this.clientPackets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.clientPackets.Size = new System.Drawing.Size(219, 250);
            this.clientPackets.TabIndex = 1;
            // 
            // PacketNameColumn
            // 
            this.PacketNameColumn.FillWeight = 115F;
            this.PacketNameColumn.HeaderText = "Packet Name";
            this.PacketNameColumn.Name = "PacketNameColumn";
            this.PacketNameColumn.ReadOnly = true;
            this.PacketNameColumn.Width = 115;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 45F;
            this.dataGridViewTextBoxColumn4.HeaderText = "ID";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 45;
            // 
            // LenghtColumn
            // 
            this.LenghtColumn.FillWeight = 45F;
            this.LenghtColumn.HeaderText = "Lenght";
            this.LenghtColumn.Name = "LenghtColumn";
            this.LenghtColumn.ReadOnly = true;
            this.LenghtColumn.Width = 45;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "boundData";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            // 
            // serverPackets
            // 
            this.serverPackets.AllowUserToAddRows = false;
            this.serverPackets.AllowUserToResizeColumns = false;
            this.serverPackets.AllowUserToResizeRows = false;
            this.serverPackets.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.serverPackets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.serverPackets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serverPackets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PacketNameColumn2,
            this.dataGridViewTextBoxColumn2,
            this.LenghtColomn,
            this.Column1});
            this.serverPackets.Location = new System.Drawing.Point(6, 20);
            this.serverPackets.MultiSelect = false;
            this.serverPackets.Name = "serverPackets";
            this.serverPackets.ReadOnly = true;
            this.serverPackets.RowHeadersVisible = false;
            this.serverPackets.RowHeadersWidth = 10;
            this.serverPackets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.serverPackets.Size = new System.Drawing.Size(213, 250);
            this.serverPackets.TabIndex = 0;
            // 
            // PacketNameColumn2
            // 
            this.PacketNameColumn2.FillWeight = 115F;
            this.PacketNameColumn2.HeaderText = "Packet Name";
            this.PacketNameColumn2.Name = "PacketNameColumn2";
            this.PacketNameColumn2.ReadOnly = true;
            this.PacketNameColumn2.Width = 115;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 45F;
            this.dataGridViewTextBoxColumn2.HeaderText = "ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 45;
            // 
            // LenghtColomn
            // 
            this.LenghtColomn.FillWeight = 45F;
            this.LenghtColomn.HeaderText = "Lenght";
            this.LenghtColomn.Name = "LenghtColomn";
            this.LenghtColomn.ReadOnly = true;
            this.LenghtColomn.Width = 45;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "boundData";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // Server2ClientBox
            // 
            this.Server2ClientBox.Controls.Add(this.ServerAutoScrollBox);
            this.Server2ClientBox.Controls.Add(this.serverPackets);
            this.Server2ClientBox.Location = new System.Drawing.Point(849, 16);
            this.Server2ClientBox.Name = "Server2ClientBox";
            this.Server2ClientBox.Size = new System.Drawing.Size(225, 299);
            this.Server2ClientBox.TabIndex = 16;
            this.Server2ClientBox.TabStop = false;
            this.Server2ClientBox.Text = "Server -> Client";
            // 
            // ServerAutoScrollBox
            // 
            this.ServerAutoScrollBox.AutoSize = true;
            this.ServerAutoScrollBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ServerAutoScrollBox.Location = new System.Drawing.Point(61, 276);
            this.ServerAutoScrollBox.Name = "ServerAutoScrollBox";
            this.ServerAutoScrollBox.Size = new System.Drawing.Size(108, 17);
            this.ServerAutoScrollBox.TabIndex = 12;
            this.ServerAutoScrollBox.Text = "Auto Scroll Down";
            this.ServerAutoScrollBox.UseVisualStyleBackColor = true;
            // 
            // AddFltrBtn
            // 
            this.AddFltrBtn.Location = new System.Drawing.Point(271, 17);
            this.AddFltrBtn.Name = "AddFltrBtn";
            this.AddFltrBtn.Size = new System.Drawing.Size(97, 23);
            this.AddFltrBtn.TabIndex = 2;
            this.AddFltrBtn.Text = "Add-Remove";
            this.AddFltrBtn.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(153, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(112, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // FilterBox
            // 
            this.FilterBox.Controls.Add(this.checkBox1);
            this.FilterBox.Controls.Add(this.ClearListBtn);
            this.FilterBox.Controls.Add(this.AddFltrBtn);
            this.FilterBox.Controls.Add(this.comboBox1);
            this.FilterBox.Location = new System.Drawing.Point(605, 350);
            this.FilterBox.Name = "FilterBox";
            this.FilterBox.Size = new System.Drawing.Size(469, 47);
            this.FilterBox.TabIndex = 17;
            this.FilterBox.TabStop = false;
            this.FilterBox.Text = "Filter";
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(605, 321);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(235, 25);
            this.ClearBtn.TabIndex = 18;
            this.ClearBtn.Text = "Clear Rows";
            this.ClearBtn.UseVisualStyleBackColor = true;
            // 
            // PacketViewerBoxx
            // 
            this.PacketViewerBoxx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketViewerBoxx.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PacketViewerBoxx.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PacketViewerBoxx.LineInfoForeColor = System.Drawing.Color.Empty;
            this.PacketViewerBoxx.Location = new System.Drawing.Point(6, 19);
            this.PacketViewerBoxx.Name = "PacketViewerBoxx";
            this.PacketViewerBoxx.ReadOnly = true;
            this.PacketViewerBoxx.SelectionBackColor = System.Drawing.Color.DimGray;
            this.PacketViewerBoxx.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.PacketViewerBoxx.Size = new System.Drawing.Size(566, 310);
            this.PacketViewerBoxx.StringViewVisible = true;
            this.PacketViewerBoxx.TabIndex = 17;
            this.PacketViewerBoxx.UseFixedBytesPerLine = true;
            this.PacketViewerBoxx.VScrollBarVisible = true;
            this.PacketViewerBoxx.Click += new System.EventHandler(this.PacketViewerBoxx_Click);
            // 
            // Packets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 537);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ConverterBox);
            this.Controls.Add(this.PacketViewerGroupBox);
            this.Controls.Add(this.CopyPacket);
            this.Controls.Add(this.Client2ServerBox);
            this.Controls.Add(this.Server2ClientBox);
            this.Controls.Add(this.FilterBox);
            this.Controls.Add(this.ClearBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Packets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packets";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Packets_FormClosed);
            this.Load += new System.EventHandler(this.Packets_Load);
            this.ConverterBox.ResumeLayout(false);
            this.ConverterBox.PerformLayout();
            this.PacketViewerGroupBox.ResumeLayout(false);
            this.PacketViewerGroupBox.PerformLayout();
            this.Client2ServerBox.ResumeLayout(false);
            this.Client2ServerBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientPackets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverPackets)).EndInit();
            this.Server2ClientBox.ResumeLayout(false);
            this.Server2ClientBox.PerformLayout();
            this.FilterBox.ResumeLayout(false);
            this.FilterBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Button ConvertBtn;
        public System.Windows.Forms.GroupBox ConverterBox;
        public System.Windows.Forms.Label hexval;
        public System.Windows.Forms.Label stringVal;
        public System.Windows.Forms.Label decVal;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.GroupBox PacketViewerGroupBox;
        public System.Windows.Forms.Button CopyPacket;
        public System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Button ClearListBtn;
        public System.Windows.Forms.GroupBox Client2ServerBox;
        public System.Windows.Forms.CheckBox ClientAutoScrollBox;
        public System.Windows.Forms.DataGridView clientPackets;
        public System.Windows.Forms.DataGridViewTextBoxColumn PacketNameColumn;
        public System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        public System.Windows.Forms.DataGridViewTextBoxColumn LenghtColumn;
        public System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        public System.Windows.Forms.DataGridView serverPackets;
        public System.Windows.Forms.DataGridViewTextBoxColumn PacketNameColumn2;
        public System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        public System.Windows.Forms.DataGridViewTextBoxColumn LenghtColomn;
        public System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        public System.Windows.Forms.GroupBox Server2ClientBox;
        public System.Windows.Forms.CheckBox ServerAutoScrollBox;
        public System.Windows.Forms.Button AddFltrBtn;
        public System.Windows.Forms.ComboBox comboBox1;
        public System.Windows.Forms.GroupBox FilterBox;
        public System.Windows.Forms.Button ClearBtn;
        private Be.Windows.Forms.HexBox PacketViewerBoxx;
    }
}