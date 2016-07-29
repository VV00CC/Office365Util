using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompareDataTool.Customize
{
    public partial class TextPoper : UserControl
    {
        public CompareDataTool.Form1.IamFocused focusedCallback;
        private int m_key;
        public int key
        {
            get;
            set;
        }
        private HashSet<string> m_listRecord;
        public HashSet<string> listRecord
        {
            get
            {
                return this.m_listRecord;
            }
            set
            {
                this.m_listRecord = value;
            }
        }
        public void hidePopList()
        {
            this.poplistBox.Visible = false;
            this.poplistBox.Height = 0;
            this.Height = this.poplistBox.Height + this.inputTextBox.Height;
        }
        public TextPoper()
        {
            InitializeComponent();
            this.listRecord = new HashSet<string>();
        }
        public string fieldText 
        {
            get
            {
                return this.inputTextBox.Text;
            }
            set
            {
                this.inputTextBox.Text = value;
            }
        }
        private void textBox_GotFocus(object sender, EventArgs e)
        {
            if (this.poplistBox.Visible)
            {
                return;
            }
            if (listRecord.Count == 0)
            {
                return;
            }
            focusedCallback(this.key);
            this.poplistBox.Items.Clear();
            foreach (string s in this.m_listRecord)
            {
                this.poplistBox.Items.Add(s);
            }
            this.poplistBox.Height = this.poplistBox.ItemHeight * (this.poplistBox.Items.Count + 1);
            this.Height = this.poplistBox.Height + this.inputTextBox.Height;
            this.poplistBox.Visible = true;
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.inputTextBox.Focused)
            {
                List<string> list = new List<string>();
                this.poplistBox.Items.Clear();
                foreach (string i in this.listRecord)
                {
                    if (i.StartsWith(this.inputTextBox.Text))
                    {
                        this.poplistBox.Items.Add(i);
                    }
                }
                this.poplistBox.Height = this.poplistBox.ItemHeight * (this.poplistBox.Items.Count + 1);
                this.Height = this.poplistBox.Height + this.inputTextBox.Height;
                this.poplistBox.Visible = true;
            }

        }

        private void textBox_LostFocus(object sender, EventArgs e)
        {
            //if (listRecord.Count > 0)
            //{
            //    return;
            //}
            //hidePopList();
        }

        private void poplistBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.poplistBox.SelectedItem == null)
            {
                return;
            }
            this.inputTextBox.Text = this.poplistBox.SelectedItem.ToString();
            //hidePopList();
        }

        private void poplistBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.poplistBox.SelectedIndex != -1)
                {
                    this.poplistBox.SetSelected(this.poplistBox.SelectedIndex, true);
                    this.inputTextBox.Text = this.poplistBox.SelectedItem.ToString();
                    hidePopList();
                }
            }
        }

        private void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (this.poplistBox.Visible && this.poplistBox.Items.Count > 0) {
                    this.poplistBox.Select();
                    this.poplistBox.SelectedIndex = 0;
                }
            }
        }

        private void poplist_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.poplistBox.SelectedIndex < 0) // 无选中项
            {
                return;
            }
            Rectangle vRectangle = this.poplistBox.GetItemRectangle(
                this.poplistBox.SelectedIndex);
            if (vRectangle.Contains(e.Location))
            {
                this.poplistBox.SetSelected(this.poplistBox.SelectedIndex, true);
                hidePopList();
            }
        }
    }
}
