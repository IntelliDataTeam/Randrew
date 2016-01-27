namespace Randrew
{
    partial class MainGUI
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
            this.menuFile = new System.Windows.Forms.ComboBox();
            this.statusText = new System.Windows.Forms.TextBox();
            this.dataOutput = new System.Windows.Forms.DataGridView();
            this.Bunny = new System.Windows.Forms.TextBox();
            this.tab_menu = new System.Windows.Forms.TabControl();
            this.tab_main = new System.Windows.Forms.TabPage();
            this.tab_option = new System.Windows.Forms.TabPage();
            this.CheckList = new System.Windows.Forms.CheckedListBox();
            this.CheckingLabel = new System.Windows.Forms.Label();
            this.UpdateList = new System.Windows.Forms.CheckedListBox();
            this.UpdateLabel = new System.Windows.Forms.Label();
            this.tab_queries = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dataOutput)).BeginInit();
            this.tab_menu.SuspendLayout();
            this.tab_main.SuspendLayout();
            this.tab_option.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuFile
            // 
            this.menuFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.menuFile.FormattingEnabled = true;
            this.menuFile.Items.AddRange(new object[] {
            "Import CSV",
            "Check CSV",
            "PN Scan",
            "Update Source",
            "Set New Config",
            "Reset Settings",
            "Exit"});
            this.menuFile.Location = new System.Drawing.Point(6, 6);
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(121, 21);
            this.menuFile.TabIndex = 0;
            this.menuFile.SelectedIndexChanged += new System.EventHandler(this.menuFile_SelectedIndexChanged);
            // 
            // statusText
            // 
            this.statusText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusText.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusText.Cursor = System.Windows.Forms.Cursors.No;
            this.statusText.Enabled = false;
            this.statusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusText.Location = new System.Drawing.Point(133, 6);
            this.statusText.Multiline = true;
            this.statusText.Name = "statusText";
            this.statusText.ReadOnly = true;
            this.statusText.Size = new System.Drawing.Size(926, 92);
            this.statusText.TabIndex = 1;
            this.statusText.Text = "Hi, I\'m Randrew!";
            this.statusText.TextChanged += new System.EventHandler(this.statusText_TextChanged);
            // 
            // dataOutput
            // 
            this.dataOutput.AllowUserToAddRows = false;
            this.dataOutput.AllowUserToDeleteRows = false;
            this.dataOutput.AllowUserToOrderColumns = true;
            this.dataOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataOutput.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataOutput.Location = new System.Drawing.Point(6, 104);
            this.dataOutput.Name = "dataOutput";
            this.dataOutput.Size = new System.Drawing.Size(1053, 466);
            this.dataOutput.TabIndex = 2;
            // 
            // Bunny
            // 
            this.Bunny.BackColor = System.Drawing.SystemColors.Info;
            this.Bunny.Location = new System.Drawing.Point(6, 33);
            this.Bunny.Multiline = true;
            this.Bunny.Name = "Bunny";
            this.Bunny.ReadOnly = true;
            this.Bunny.Size = new System.Drawing.Size(121, 65);
            this.Bunny.TabIndex = 3;
            this.Bunny.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tab_menu
            // 
            this.tab_menu.Controls.Add(this.tab_main);
            this.tab_menu.Controls.Add(this.tab_option);
            this.tab_menu.Controls.Add(this.tab_queries);
            this.tab_menu.Location = new System.Drawing.Point(12, 12);
            this.tab_menu.Name = "tab_menu";
            this.tab_menu.SelectedIndex = 0;
            this.tab_menu.Size = new System.Drawing.Size(1073, 602);
            this.tab_menu.TabIndex = 4;
            // 
            // tab_main
            // 
            this.tab_main.Controls.Add(this.menuFile);
            this.tab_main.Controls.Add(this.dataOutput);
            this.tab_main.Controls.Add(this.Bunny);
            this.tab_main.Controls.Add(this.statusText);
            this.tab_main.Location = new System.Drawing.Point(4, 22);
            this.tab_main.Name = "tab_main";
            this.tab_main.Padding = new System.Windows.Forms.Padding(3);
            this.tab_main.Size = new System.Drawing.Size(1065, 576);
            this.tab_main.TabIndex = 0;
            this.tab_main.Text = "Main";
            this.tab_main.UseVisualStyleBackColor = true;
            // 
            // tab_option
            // 
            this.tab_option.Controls.Add(this.CheckList);
            this.tab_option.Controls.Add(this.CheckingLabel);
            this.tab_option.Controls.Add(this.UpdateList);
            this.tab_option.Controls.Add(this.UpdateLabel);
            this.tab_option.Location = new System.Drawing.Point(4, 22);
            this.tab_option.Name = "tab_option";
            this.tab_option.Padding = new System.Windows.Forms.Padding(3);
            this.tab_option.Size = new System.Drawing.Size(1065, 576);
            this.tab_option.TabIndex = 1;
            this.tab_option.Text = "Options";
            this.tab_option.UseVisualStyleBackColor = true;
            // 
            // CheckList
            // 
            this.CheckList.FormattingEnabled = true;
            this.CheckList.Location = new System.Drawing.Point(276, 42);
            this.CheckList.Name = "CheckList";
            this.CheckList.Size = new System.Drawing.Size(186, 304);
            this.CheckList.TabIndex = 3;
            // 
            // CheckingLabel
            // 
            this.CheckingLabel.AutoSize = true;
            this.CheckingLabel.Location = new System.Drawing.Point(273, 26);
            this.CheckingLabel.Name = "CheckingLabel";
            this.CheckingLabel.Size = new System.Drawing.Size(103, 13);
            this.CheckingLabel.TabIndex = 2;
            this.CheckingLabel.Text = "Customize Checking";
            // 
            // UpdateList
            // 
            this.UpdateList.FormattingEnabled = true;
            this.UpdateList.Location = new System.Drawing.Point(28, 42);
            this.UpdateList.Name = "UpdateList";
            this.UpdateList.Size = new System.Drawing.Size(185, 304);
            this.UpdateList.TabIndex = 1;
            // 
            // UpdateLabel
            // 
            this.UpdateLabel.AutoSize = true;
            this.UpdateLabel.Location = new System.Drawing.Point(25, 26);
            this.UpdateLabel.Name = "UpdateLabel";
            this.UpdateLabel.Size = new System.Drawing.Size(98, 13);
            this.UpdateLabel.TabIndex = 0;
            this.UpdateLabel.Text = "Customize Updates";
            // 
            // tab_queries
            // 
            this.tab_queries.Location = new System.Drawing.Point(4, 22);
            this.tab_queries.Name = "tab_queries";
            this.tab_queries.Padding = new System.Windows.Forms.Padding(3);
            this.tab_queries.Size = new System.Drawing.Size(1065, 576);
            this.tab_queries.TabIndex = 2;
            this.tab_queries.Text = "Queries";
            this.tab_queries.UseVisualStyleBackColor = true;
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 626);
            this.Controls.Add(this.tab_menu);
            this.Name = "MainGUI";
            this.Text = "Randrew";
            this.Load += new System.EventHandler(this.MainGUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataOutput)).EndInit();
            this.tab_menu.ResumeLayout(false);
            this.tab_main.ResumeLayout(false);
            this.tab_main.PerformLayout();
            this.tab_option.ResumeLayout(false);
            this.tab_option.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox menuFile;
        private System.Windows.Forms.TextBox statusText;
        private System.Windows.Forms.DataGridView dataOutput;
        private System.Windows.Forms.TextBox Bunny;
        private System.Windows.Forms.TabControl tab_menu;
        private System.Windows.Forms.TabPage tab_main;
        private System.Windows.Forms.TabPage tab_option;
        private System.Windows.Forms.Label UpdateLabel;
        private System.Windows.Forms.CheckedListBox CheckList;
        private System.Windows.Forms.Label CheckingLabel;
        private System.Windows.Forms.CheckedListBox UpdateList;
        private System.Windows.Forms.TabPage tab_queries;
    }
}

