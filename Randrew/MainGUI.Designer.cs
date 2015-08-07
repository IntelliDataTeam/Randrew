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
            "Exit"});
            this.menuFile.Location = new System.Drawing.Point(12, 12);
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(121, 21);
            this.menuFile.TabIndex = 0;
            this.menuFile.SelectedIndexChanged += new System.EventHandler(this.menuFile_SelectedIndexChanged);
            // 
            // statusText
            // 
            this.statusText.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusText.Location = new System.Drawing.Point(163, 12);
            this.statusText.Multiline = true;
            this.statusText.Name = "statusText";
            this.statusText.ReadOnly = true;
            this.statusText.Size = new System.Drawing.Size(355, 70);
            this.statusText.TabIndex = 1;
            // 
            // dataOutput
            // 
            this.dataOutput.AllowUserToAddRows = false;
            this.dataOutput.AllowUserToDeleteRows = false;
            this.dataOutput.AllowUserToResizeColumns = false;
            this.dataOutput.AllowUserToResizeRows = false;
            this.dataOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataOutput.Location = new System.Drawing.Point(12, 88);
            this.dataOutput.Name = "dataOutput";
            this.dataOutput.Size = new System.Drawing.Size(525, 239);
            this.dataOutput.TabIndex = 2;
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 339);
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
    }
}

