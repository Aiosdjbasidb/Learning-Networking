namespace LidgrenChat
{
	partial class k
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
			this.writeBox = new System.Windows.Forms.TextBox();
			this.sendButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.ipBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.connectButton = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.startServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainChat = new System.Windows.Forms.ListBox();
			this.clientsListbox = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// writeBox
			// 
			this.writeBox.AcceptsReturn = true;
			this.writeBox.AcceptsTab = true;
			this.writeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.writeBox.Enabled = false;
			this.writeBox.Location = new System.Drawing.Point(12, 413);
			this.writeBox.Name = "writeBox";
			this.writeBox.Size = new System.Drawing.Size(549, 20);
			this.writeBox.TabIndex = 1;
			this.writeBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.writeBox_KeyDown);
			// 
			// sendButton
			// 
			this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.sendButton.Enabled = false;
			this.sendButton.Location = new System.Drawing.Point(568, 384);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(152, 49);
			this.sendButton.TabIndex = 2;
			this.sendButton.Text = "Send";
			this.sendButton.UseVisualStyleBackColor = true;
			this.sendButton.Click += new System.EventHandler(this.SendButtonClick);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(565, 274);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Server IP";
			// 
			// ipBox
			// 
			this.ipBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ipBox.Location = new System.Drawing.Point(568, 290);
			this.ipBox.Name = "ipBox";
			this.ipBox.Size = new System.Drawing.Size(152, 20);
			this.ipBox.TabIndex = 4;
			this.ipBox.Text = "127.0.0.1";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(565, 313);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Name";
			// 
			// nameBox
			// 
			this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.nameBox.Location = new System.Drawing.Point(568, 329);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(152, 20);
			this.nameBox.TabIndex = 4;
			this.nameBox.Text = "Skippy";
			this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
			// 
			// connectButton
			// 
			this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.connectButton.Location = new System.Drawing.Point(568, 355);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(152, 23);
			this.connectButton.TabIndex = 5;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.ConnectButtonClick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startServerToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(732, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// startServerToolStripMenuItem
			// 
			this.startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
			this.startServerToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
			this.startServerToolStripMenuItem.Text = "Start Server";
			this.startServerToolStripMenuItem.Click += new System.EventHandler(this.startServerToolStripMenuItem_Click);
			// 
			// mainChat
			// 
			this.mainChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mainChat.FormattingEnabled = true;
			this.mainChat.HorizontalScrollbar = true;
			this.mainChat.Location = new System.Drawing.Point(13, 28);
			this.mainChat.Name = "mainChat";
			this.mainChat.ScrollAlwaysVisible = true;
			this.mainChat.Size = new System.Drawing.Size(549, 368);
			this.mainChat.TabIndex = 7;
			// 
			// clientsListbox
			// 
			this.clientsListbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.clientsListbox.FormattingEnabled = true;
			this.clientsListbox.Location = new System.Drawing.Point(568, 44);
			this.clientsListbox.Name = "clientsListbox";
			this.clientsListbox.Size = new System.Drawing.Size(152, 225);
			this.clientsListbox.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(565, 28);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(92, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Connected clients";
			// 
			// k
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(732, 445);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.clientsListbox);
			this.Controls.Add(this.mainChat);
			this.Controls.Add(this.connectButton);
			this.Controls.Add(this.nameBox);
			this.Controls.Add(this.ipBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.sendButton);
			this.Controls.Add(this.writeBox);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "k";
			this.Text = "Lidgren Chat";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox writeBox;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox ipBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem startServerToolStripMenuItem;
		public System.Windows.Forms.ListBox mainChat;
		private System.Windows.Forms.ListBox clientsListbox;
		private System.Windows.Forms.Label label3;
	}
}

