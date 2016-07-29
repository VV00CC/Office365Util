namespace CompareDataTool
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.srcClearBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.desClearBtn = new System.Windows.Forms.Button();
            this.listSettingsCbx = new System.Windows.Forms.CheckBox();
            this.publicViewCbx = new System.Windows.Forms.CheckBox();
            this.listColumnCbx = new System.Windows.Forms.CheckBox();
            this.listSecurityCbx = new System.Windows.Forms.CheckBox();
            this.itemCbx = new System.Windows.Forms.CheckBox();
            this.AttachmentCbx = new System.Windows.Forms.CheckBox();
            this.itemSecurityCbx = new System.Windows.Forms.CheckBox();
            this.compareBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ListWorflowsCbx = new System.Windows.Forms.CheckBox();
            this.compareProcessBar = new System.Windows.Forms.ProgressBar();
            this.desPasswordTpr = new CompareDataTool.Customize.TextPoper();
            this.desUserNameTpr = new CompareDataTool.Customize.TextPoper();
            this.desListTitleTpr = new CompareDataTool.Customize.TextPoper();
            this.desSiteUrlTpr = new CompareDataTool.Customize.TextPoper();
            this.srcPasswordTpr = new CompareDataTool.Customize.TextPoper();
            this.srcUserNameTpr = new CompareDataTool.Customize.TextPoper();
            this.srcListTitleTpr = new CompareDataTool.Customize.TextPoper();
            this.srcSiteUrlTpr = new CompareDataTool.Customize.TextPoper();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.srcPasswordTpr);
            this.groupBox1.Controls.Add(this.srcUserNameTpr);
            this.groupBox1.Controls.Add(this.srcListTitleTpr);
            this.groupBox1.Controls.Add(this.srcSiteUrlTpr);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.srcClearBtn);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(44, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 443);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SourceData";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 288);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "UserName:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "ListTitle:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "SiteUrl:";
            // 
            // srcClearBtn
            // 
            this.srcClearBtn.Location = new System.Drawing.Point(176, 390);
            this.srcClearBtn.Name = "srcClearBtn";
            this.srcClearBtn.Size = new System.Drawing.Size(75, 23);
            this.srcClearBtn.TabIndex = 8;
            this.srcClearBtn.Text = "Clear";
            this.srcClearBtn.UseVisualStyleBackColor = true;
            this.srcClearBtn.Click += new System.EventHandler(this.srcClearBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.desPasswordTpr);
            this.groupBox2.Controls.Add(this.desUserNameTpr);
            this.groupBox2.Controls.Add(this.desListTitleTpr);
            this.groupBox2.Controls.Add(this.desSiteUrlTpr);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.desClearBtn);
            this.groupBox2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(370, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(286, 443);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DestinationData";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(43, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 14);
            this.label8.TabIndex = 6;
            this.label8.Text = "Password:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 14);
            this.label7.TabIndex = 4;
            this.label7.Text = "UserName:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 14);
            this.label6.TabIndex = 2;
            this.label6.Text = "ListTitle:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 14);
            this.label5.TabIndex = 0;
            this.label5.Text = "SiteUrl:";
            // 
            // desClearBtn
            // 
            this.desClearBtn.Location = new System.Drawing.Point(188, 390);
            this.desClearBtn.Name = "desClearBtn";
            this.desClearBtn.Size = new System.Drawing.Size(75, 23);
            this.desClearBtn.TabIndex = 8;
            this.desClearBtn.Text = "Clear";
            this.desClearBtn.UseVisualStyleBackColor = true;
            this.desClearBtn.Click += new System.EventHandler(this.desClearBtn_Click);
            // 
            // listSettingsCbx
            // 
            this.listSettingsCbx.AutoSize = true;
            this.listSettingsCbx.Checked = true;
            this.listSettingsCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.listSettingsCbx.Location = new System.Drawing.Point(45, 30);
            this.listSettingsCbx.Name = "listSettingsCbx";
            this.listSettingsCbx.Size = new System.Drawing.Size(87, 18);
            this.listSettingsCbx.TabIndex = 2;
            this.listSettingsCbx.Text = "ListSettings";
            this.listSettingsCbx.UseVisualStyleBackColor = true;
            // 
            // publicViewCbx
            // 
            this.publicViewCbx.AutoSize = true;
            this.publicViewCbx.Checked = true;
            this.publicViewCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.publicViewCbx.Location = new System.Drawing.Point(45, 62);
            this.publicViewCbx.Name = "publicViewCbx";
            this.publicViewCbx.Size = new System.Drawing.Size(92, 18);
            this.publicViewCbx.TabIndex = 3;
            this.publicViewCbx.Text = "PublicViews";
            this.publicViewCbx.UseVisualStyleBackColor = true;
            // 
            // listColumnCbx
            // 
            this.listColumnCbx.AutoSize = true;
            this.listColumnCbx.Checked = true;
            this.listColumnCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.listColumnCbx.Location = new System.Drawing.Point(45, 95);
            this.listColumnCbx.Name = "listColumnCbx";
            this.listColumnCbx.Size = new System.Drawing.Size(92, 18);
            this.listColumnCbx.TabIndex = 4;
            this.listColumnCbx.Text = "ListColumns";
            this.listColumnCbx.UseVisualStyleBackColor = true;
            // 
            // listSecurityCbx
            // 
            this.listSecurityCbx.AutoSize = true;
            this.listSecurityCbx.Checked = true;
            this.listSecurityCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.listSecurityCbx.Location = new System.Drawing.Point(45, 160);
            this.listSecurityCbx.Name = "listSecurityCbx";
            this.listSecurityCbx.Size = new System.Drawing.Size(87, 18);
            this.listSecurityCbx.TabIndex = 5;
            this.listSecurityCbx.Text = "ListSecurity";
            this.listSecurityCbx.UseVisualStyleBackColor = true;
            // 
            // itemCbx
            // 
            this.itemCbx.AutoSize = true;
            this.itemCbx.Checked = true;
            this.itemCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.itemCbx.Location = new System.Drawing.Point(45, 216);
            this.itemCbx.Name = "itemCbx";
            this.itemCbx.Size = new System.Drawing.Size(51, 18);
            this.itemCbx.TabIndex = 6;
            this.itemCbx.Text = "Item";
            this.itemCbx.UseVisualStyleBackColor = true;
            // 
            // AttachmentCbx
            // 
            this.AttachmentCbx.AutoSize = true;
            this.AttachmentCbx.Checked = true;
            this.AttachmentCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AttachmentCbx.Location = new System.Drawing.Point(45, 258);
            this.AttachmentCbx.Name = "AttachmentCbx";
            this.AttachmentCbx.Size = new System.Drawing.Size(88, 18);
            this.AttachmentCbx.TabIndex = 7;
            this.AttachmentCbx.Text = "Attachment";
            this.AttachmentCbx.UseVisualStyleBackColor = true;
            // 
            // itemSecurityCbx
            // 
            this.itemSecurityCbx.AutoSize = true;
            this.itemSecurityCbx.Checked = true;
            this.itemSecurityCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.itemSecurityCbx.Location = new System.Drawing.Point(45, 301);
            this.itemSecurityCbx.Name = "itemSecurityCbx";
            this.itemSecurityCbx.Size = new System.Drawing.Size(93, 18);
            this.itemSecurityCbx.TabIndex = 8;
            this.itemSecurityCbx.Text = "ItemSecurity";
            this.itemSecurityCbx.UseVisualStyleBackColor = true;
            // 
            // compareBtn
            // 
            this.compareBtn.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.compareBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.compareBtn.Location = new System.Drawing.Point(720, 419);
            this.compareBtn.Name = "compareBtn";
            this.compareBtn.Size = new System.Drawing.Size(211, 73);
            this.compareBtn.TabIndex = 9;
            this.compareBtn.Text = "Compare";
            this.compareBtn.UseVisualStyleBackColor = true;
            this.compareBtn.Click += new System.EventHandler(this.compareBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ListWorflowsCbx);
            this.groupBox3.Controls.Add(this.listSettingsCbx);
            this.groupBox3.Controls.Add(this.publicViewCbx);
            this.groupBox3.Controls.Add(this.itemSecurityCbx);
            this.groupBox3.Controls.Add(this.listColumnCbx);
            this.groupBox3.Controls.Add(this.AttachmentCbx);
            this.groupBox3.Controls.Add(this.listSecurityCbx);
            this.groupBox3.Controls.Add(this.itemCbx);
            this.groupBox3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(720, 49);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(211, 344);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Advance";
            // 
            // ListWorflowsCbx
            // 
            this.ListWorflowsCbx.AutoSize = true;
            this.ListWorflowsCbx.Checked = true;
            this.ListWorflowsCbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ListWorflowsCbx.Location = new System.Drawing.Point(45, 127);
            this.ListWorflowsCbx.Name = "ListWorflowsCbx";
            this.ListWorflowsCbx.Size = new System.Drawing.Size(100, 18);
            this.ListWorflowsCbx.TabIndex = 9;
            this.ListWorflowsCbx.Text = "ListWorkflows";
            this.ListWorflowsCbx.UseVisualStyleBackColor = true;
            // 
            // compareProcessBar
            // 
            this.compareProcessBar.Location = new System.Drawing.Point(44, 510);
            this.compareProcessBar.Name = "compareProcessBar";
            this.compareProcessBar.Size = new System.Drawing.Size(887, 23);
            this.compareProcessBar.TabIndex = 11;
            // 
            // desPasswordTpr
            // 
            this.desPasswordTpr.BackColor = System.Drawing.Color.Transparent;
            this.desPasswordTpr.fieldText = "";
            this.desPasswordTpr.listRecord = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("desPasswordTpr.listRecord")));
            this.desPasswordTpr.Location = new System.Drawing.Point(44, 323);
            this.desPasswordTpr.Name = "desPasswordTpr";
            this.desPasswordTpr.Size = new System.Drawing.Size(219, 29);
            this.desPasswordTpr.TabIndex = 12;
            // 
            // desUserNameTpr
            // 
            this.desUserNameTpr.BackColor = System.Drawing.Color.Transparent;
            this.desUserNameTpr.fieldText = "";
            this.desUserNameTpr.listRecord = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("desUserNameTpr.listRecord")));
            this.desUserNameTpr.Location = new System.Drawing.Point(44, 236);
            this.desUserNameTpr.Name = "desUserNameTpr";
            this.desUserNameTpr.Size = new System.Drawing.Size(219, 29);
            this.desUserNameTpr.TabIndex = 11;
            // 
            // desListTitleTpr
            // 
            this.desListTitleTpr.BackColor = System.Drawing.Color.Transparent;
            this.desListTitleTpr.fieldText = "";
            this.desListTitleTpr.listRecord = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("desListTitleTpr.listRecord")));
            this.desListTitleTpr.Location = new System.Drawing.Point(44, 156);
            this.desListTitleTpr.Name = "desListTitleTpr";
            this.desListTitleTpr.Size = new System.Drawing.Size(219, 31);
            this.desListTitleTpr.TabIndex = 10;
            // 
            // desSiteUrlTpr
            // 
            this.desSiteUrlTpr.BackColor = System.Drawing.Color.Transparent;
            this.desSiteUrlTpr.fieldText = "";
            this.desSiteUrlTpr.listRecord = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("desSiteUrlTpr.listRecord")));
            this.desSiteUrlTpr.Location = new System.Drawing.Point(44, 78);
            this.desSiteUrlTpr.Name = "desSiteUrlTpr";
            this.desSiteUrlTpr.Size = new System.Drawing.Size(219, 35);
            this.desSiteUrlTpr.TabIndex = 9;
            // 
            // srcPasswordTpr
            // 
            this.srcPasswordTpr.BackColor = System.Drawing.SystemColors.Control;
            this.srcPasswordTpr.fieldText = "";
            this.srcPasswordTpr.listRecord = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("srcPasswordTpr.listRecord")));
            this.srcPasswordTpr.Location = new System.Drawing.Point(30, 323);
            this.srcPasswordTpr.Name = "srcPasswordTpr";
            this.srcPasswordTpr.Size = new System.Drawing.Size(221, 29);
            this.srcPasswordTpr.TabIndex = 12;
            // 
            // srcUserNameTpr
            // 
            this.srcUserNameTpr.BackColor = System.Drawing.SystemColors.Control;
            this.srcUserNameTpr.fieldText = "";
            this.srcUserNameTpr.listRecord = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("srcUserNameTpr.listRecord")));
            this.srcUserNameTpr.Location = new System.Drawing.Point(30, 236);
            this.srcUserNameTpr.Name = "srcUserNameTpr";
            this.srcUserNameTpr.Size = new System.Drawing.Size(221, 29);
            this.srcUserNameTpr.TabIndex = 11;
            // 
            // srcListTitleTpr
            // 
            this.srcListTitleTpr.BackColor = System.Drawing.SystemColors.Control;
            this.srcListTitleTpr.fieldText = "";
            this.srcListTitleTpr.listRecord = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("srcListTitleTpr.listRecord")));
            this.srcListTitleTpr.Location = new System.Drawing.Point(30, 156);
            this.srcListTitleTpr.Name = "srcListTitleTpr";
            this.srcListTitleTpr.Size = new System.Drawing.Size(221, 29);
            this.srcListTitleTpr.TabIndex = 10;
            // 
            // srcSiteUrlTpr
            // 
            this.srcSiteUrlTpr.BackColor = System.Drawing.SystemColors.Control;
            this.srcSiteUrlTpr.fieldText = "";
            this.srcSiteUrlTpr.listRecord = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("srcSiteUrlTpr.listRecord")));
            this.srcSiteUrlTpr.Location = new System.Drawing.Point(31, 78);
            this.srcSiteUrlTpr.Name = "srcSiteUrlTpr";
            this.srcSiteUrlTpr.Size = new System.Drawing.Size(221, 29);
            this.srcSiteUrlTpr.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 545);
            this.Controls.Add(this.compareProcessBar);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.compareBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "DataCompareTool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button srcClearBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button desClearBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox listSettingsCbx;
        private System.Windows.Forms.CheckBox publicViewCbx;
        private System.Windows.Forms.CheckBox listColumnCbx;
        private System.Windows.Forms.CheckBox listSecurityCbx;
        private System.Windows.Forms.CheckBox itemCbx;
        private System.Windows.Forms.CheckBox AttachmentCbx;
        private System.Windows.Forms.CheckBox itemSecurityCbx;
        private System.Windows.Forms.Button compareBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar compareProcessBar;
        private System.Windows.Forms.CheckBox ListWorflowsCbx;
        private Customize.TextPoper srcSiteUrlTpr;
        private Customize.TextPoper srcListTitleTpr;
        private Customize.TextPoper srcPasswordTpr;
        private Customize.TextPoper srcUserNameTpr;
        private Customize.TextPoper desSiteUrlTpr;
        private Customize.TextPoper desListTitleTpr;
        private Customize.TextPoper desUserNameTpr;
        private Customize.TextPoper desPasswordTpr;
    }
}

