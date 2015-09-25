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
            ((System.ComponentModel.ISupportInitialize)(this.dataOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // menuFile
            // 
            this.menuFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.menuFile.FormattingEnabled = true;
            this.menuFile.Items.AddRange(new object[] {
            "Import CSV",
            "Check CSV",
            "Update Source",
            "Set New Config",
            "Reset Settings",
            "Exit"});
            this.menuFile.Location = new System.Drawing.Point(12, 12);
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
            this.statusText.Location = new System.Drawing.Point(139, 12);
            this.statusText.Multiline = true;
            this.statusText.Name = "statusText";
            this.statusText.ReadOnly = true;
            this.statusText.Size = new System.Drawing.Size(946, 92);
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
            this.dataOutput.Location = new System.Drawing.Point(12, 110);
            this.dataOutput.Name = "dataOutput";
            this.dataOutput.Size = new System.Drawing.Size(1073, 504);
            this.dataOutput.TabIndex = 2;
            // 
            // Bunny
            // 
            this.Bunny.BackColor = System.Drawing.SystemColors.Info;
            this.Bunny.Location = new System.Drawing.Point(12, 39);
            this.Bunny.Multiline = true;
            this.Bunny.Name = "Bunny";
            this.Bunny.ReadOnly = true;
            this.Bunny.Size = new System.Drawing.Size(121, 65);
            this.Bunny.TabIndex = 3;
            this.Bunny.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 626);
            this.Controls.Add(this.Bunny);
            this.Controls.Add(this.dataOutput);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.menuFile);
            this.Name = "MainGUI";
            this.Text = "Randrew";
            this.Load += new System.EventHandler(this.MainGUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataOutput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox menuFile;
        private System.Windows.Forms.TextBox statusText;
        private System.Windows.Forms.DataGridView dataOutput;
        private System.Windows.Forms.TextBox Bunny;
    }
}

