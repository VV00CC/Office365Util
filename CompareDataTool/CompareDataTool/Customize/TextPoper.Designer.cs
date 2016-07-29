namespace CompareDataTool.Customize
{
    partial class TextPoper
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.poplistBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(3, 3);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(213, 21);
            this.inputTextBox.TabIndex = 1;
            this.inputTextBox.TextChanged += new System.EventHandler(this.inputTextBox_TextChanged);
            this.inputTextBox.GotFocus += new System.EventHandler(this.textBox_GotFocus);
            this.inputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
            this.inputTextBox.LostFocus += new System.EventHandler(this.textBox_LostFocus);
            // 
            // poplistBox
            // 
            this.poplistBox.FormattingEnabled = true;
            this.poplistBox.ItemHeight = 12;
            this.poplistBox.Location = new System.Drawing.Point(3, 21);
            this.poplistBox.Name = "poplistBox";
            this.poplistBox.Size = new System.Drawing.Size(213, 52);
            this.poplistBox.TabIndex = 0;
            this.poplistBox.Visible = false;
            this.poplistBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.poplist_MouseClick);
            this.poplistBox.SelectedValueChanged += new System.EventHandler(this.poplistBox_SelectedValueChanged);
            this.poplistBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.poplistBox_KeyDown);
            // 
            // TextPoper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.poplistBox);
            this.Controls.Add(this.inputTextBox);
            this.Name = "TextPoper";
            this.Size = new System.Drawing.Size(219, 81);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.ListBox poplistBox;
    }
}
