using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using System.Threading;
using CompareDataTool.CompareCore;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using CompareDataTool.Customize;

namespace CompareDataTool
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        const string textRecordName = "TextRecord.dat";
        public enum TextKeys
        {
            srcSiteUrl,
            srcListTitle,
            srcUserName,
            srcPassword,

            desSiteUrl,
            desListTitle,
            desUserName,
            desPassword,
        }

        Dictionary<int, Control> m_textControlsMap;

        public Form1()
        {
            InitializeComponent();
            m_recordMap = new Dictionary<int, HashSet<string>>();
            initTextControl();
        }
        private Dictionary<int, HashSet<string>> m_recordMap;
        public class InitialThread
        {
            private string srcSiteUrl;
            private string srcListTitle;
            private string srcUserName;
            private string srcPassword;

            private string desSiteUrl;
            private string desListTitle;
            private string desUserName;
            private string desPassword;

            public ThreadCallBackDelegate completeCallBack;
            public SetProcessValueCallback processCallBack;

            public InitialThread(string srcSiteUrl, string srcListTitle, string srcUserName, string srcPassword,
                                   string desSiteUrl, string desListTitle, string desUserName, string desPassword)
            {
                this.srcSiteUrl = srcSiteUrl;
                this.srcListTitle = srcListTitle;
                this.srcUserName = srcUserName;
                this.srcPassword = srcPassword;

                this.desSiteUrl = desSiteUrl;
                this.desListTitle = desListTitle;
                this.desUserName = desUserName;
                this.desPassword = desPassword;
            }

            public void ThreadProc()
            {
                processCallBack(10);
                Content src = new Content();
                src.InitialWeb(srcSiteUrl, srcUserName, srcPassword);
                src.InitialList(srcListTitle);

                processCallBack(20);
                Content des = new Content();
                des.InitialWeb(desSiteUrl, desUserName, desPassword);
                des.InitialList(desListTitle);

                processCallBack(30);
                completeCallBack(src, des, srcListTitle, desListTitle);
            }
        }
        public delegate void ThreadCallBackDelegate(Content src, Content des, string srcl, string desl);
        public delegate void SetProcessValueCallback(int value);
        public delegate void SetCompareBtnCallback(bool bEnabled);
        public delegate void IamFocused(int key);

        private void onOneTextFocused(int key)
        {
            foreach (int k in m_textControlsMap.Keys)
            {
                if (k != key)
                {
                    TextPoper tp = (TextPoper)m_textControlsMap[k];
                    tp.hidePopList();
                }
            }
        }

        private void srcClearBtn_Click(object sender, EventArgs e)
        {
            this.srcSiteUrlTpr.fieldText = "";
            this.srcListTitleTpr.fieldText = "";
            this.srcUserNameTpr.fieldText = "";
            this.srcPasswordTpr.fieldText = "";
        }

        private void desClearBtn_Click(object sender, EventArgs e)
        {
            this.desSiteUrlTpr.fieldText = "";
            this.desListTitleTpr.fieldText = "";
            this.desUserNameTpr.fieldText = "";
            this.desPasswordTpr.fieldText = "";
        }

        private void saveTextRecords()
        {
            m_recordMap[(int)TextKeys.srcSiteUrl].Add(this.srcSiteUrlTpr.fieldText);
            m_recordMap[(int)TextKeys.srcListTitle].Add(this.srcListTitleTpr.fieldText);
            m_recordMap[(int)TextKeys.srcUserName].Add(this.srcUserNameTpr.fieldText);
            m_recordMap[(int)TextKeys.srcPassword].Add(this.srcPasswordTpr.fieldText);

            m_recordMap[(int)TextKeys.desSiteUrl].Add(this.desSiteUrlTpr.fieldText);
            m_recordMap[(int)TextKeys.desListTitle].Add(this.desListTitleTpr.fieldText);
            m_recordMap[(int)TextKeys.desUserName].Add(this.desUserNameTpr.fieldText);
            m_recordMap[(int)TextKeys.desPassword].Add(this.desPasswordTpr.fieldText);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(textRecordName, FileMode.Create);
            formatter.Serialize(fs, m_recordMap);
            fs.Close();
        }

        private void initRecordMaps()
        {
            m_recordMap[(int)TextKeys.srcSiteUrl] = new HashSet<string>();
            m_recordMap[(int)TextKeys.srcListTitle] = new HashSet<string>();
            m_recordMap[(int)TextKeys.srcUserName] = new HashSet<string>();
            m_recordMap[(int)TextKeys.srcPassword] = new HashSet<string>();

            m_recordMap[(int)TextKeys.desSiteUrl] = new HashSet<string>();
            m_recordMap[(int)TextKeys.desListTitle] = new HashSet<string>();
            m_recordMap[(int)TextKeys.desUserName] = new HashSet<string>();
            m_recordMap[(int)TextKeys.desPassword] = new HashSet<string>();
        }

        private void loadTextRecords()
        {
            if (System.IO.File.Exists(textRecordName))
            {
                FileStream fs = new FileStream(textRecordName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                this.m_recordMap = (Dictionary<int, HashSet<string>>)formatter.Deserialize(fs);
                foreach (int key in m_textControlsMap.Keys)
                {
                    TextPoper tp = (TextPoper)m_textControlsMap[key];
                    tp.listRecord = m_recordMap[key];
                }
                fs.Close();
            }
            else 
            {
                initRecordMaps();
            }
        }

        private void initTextControl()
        {
            m_textControlsMap = new Dictionary<int, Control>();
            m_textControlsMap[(int)TextKeys.srcSiteUrl] = this.srcSiteUrlTpr;
            m_textControlsMap[(int)TextKeys.srcListTitle] = this.srcListTitleTpr;
            m_textControlsMap[(int)TextKeys.srcUserName] = this.srcUserNameTpr;
            m_textControlsMap[(int)TextKeys.srcPassword] = this.srcPasswordTpr;

            m_textControlsMap[(int)TextKeys.desSiteUrl] = this.desSiteUrlTpr;
            m_textControlsMap[(int)TextKeys.desListTitle] = this.desListTitleTpr;
            m_textControlsMap[(int)TextKeys.desUserName] = this.desUserNameTpr;
            m_textControlsMap[(int)TextKeys.desPassword] = this.desPasswordTpr;

            foreach (int key in m_textControlsMap.Keys)
            {
                TextPoper tp = (TextPoper)m_textControlsMap[key];
                tp.key = key;
                tp.focusedCallback = onOneTextFocused;
            }

            loadTextRecords();
        }

        private void compareBtn_Click(object sender, EventArgs e)
        {
            if (this.srcSiteUrlTpr.fieldText == "" || this.srcListTitleTpr.fieldText == "" || this.srcUserNameTpr.fieldText == "" ||
                this.srcPasswordTpr.fieldText == "" || this.desSiteUrlTpr.fieldText == "" || this.desListTitleTpr.fieldText == "" ||
                this.desUserNameTpr.fieldText == "" || this.desPasswordTpr.fieldText == "")
            {
                return;
            }
            saveTextRecords();
            Reporter report = new Reporter();
            report.NewXsl();
            InitialThread thd = new InitialThread(this.srcSiteUrlTpr.fieldText, this.srcListTitleTpr.fieldText, this.srcUserNameTpr.fieldText, this.srcPasswordTpr.fieldText,
                this.desSiteUrlTpr.fieldText, this.desListTitleTpr.fieldText, this.desUserNameTpr.fieldText, this.desPasswordTpr.fieldText);
            thd.completeCallBack = InitialListComplete;
            thd.processCallBack = setProcessValue;
            Thread t = new Thread(new ThreadStart(thd.ThreadProc));
            t.Start();
            setCompareBtn(false);
        }

        private void InitialListComplete(Content src, Content des, string srcl, string desl)
        {
            Reporter report = new Reporter();
            //try
            {
                if (this.listSettingsCbx.Checked)
                {
                    if (CompareSettings.CompareDictionary(src.GetListSettings(srcl), des.GetListSettings(desl)))
                    {
                        report.updateXslResult("List Settings Match!");

                    }
                    else
                    {
                        report.updateXslResult("List Settings UnMatch!");

                    }
                }
                setProcessValue(40);
                if (this.publicViewCbx.Checked)
                {
                    CompareViews cv = new CompareViews();
                    if (cv.CompareViewCollection(src.GetListViews(srcl), des.GetListViews(desl)))
                    {
                        report.updateXslResult("List Views Match!");
                    }
                    else
                    {
                        report.updateXslResult("List Views UnMatch!");
                    }

                }
                setProcessValue(50);
                if (this.listColumnCbx.Checked)
                {
                    CompareColumns cc = new CompareColumns();
                    if (cc.CompareColumnCollection(src.GetListColumns(srcl), des.GetListColumns(desl)))
                    {
                        report.updateXslResult("List Columns Match!");
                    }
                    else
                    {
                        report.updateXslResult("List Columns UnMatch!");
                    }
                }
                setProcessValue(60);
                if (this.ListWorflowsCbx.Checked)
                {
                    CompareWorkflows cw = new CompareWorkflows();
                    if (cw.CompareWorkflowCollection(src.GetListWorkflows(srcl), des.GetListWorkflows(desl)))
                    {
                        report.updateXslResult("List Workflows Match!");
                    }
                    else
                    {
                        report.updateXslResult("List Workflows UnMatch!");
                    }
                }
                setProcessValue(70);
                if (this.listSecurityCbx.Checked)
                {
                    if (CompareSettings.CompareDictionary(src.GetListSecurity(srcl), des.GetListSecurity(desl)))
                    {
                        report.updateXslResult("List Security Match!");

                    }
                    else
                    {
                        report.updateXslResult("List Security UnMatch!");

                    }
                }
                setProcessValue(80);
                if (this.itemCbx.Checked)
                {
                    if (this.AttachmentCbx.Checked)
                    {
                        src.Attachment = true;
                    }
                    else
                    {
                        src.Attachment = false;
                    }
                    if (CompareSettings.CompareItems(src.GetAllItemProps(), des.GetAllItemProps()))
                    {
                        report.updateXslResult("Items Match!");

                    }
                    else
                    {
                        report.updateXslResult("Items UnMatch!");

                    }
                }
                setProcessValue(90);
                if (this.itemSecurityCbx.Checked)
                {
                    if (CompareSettings.CompareItemsSecurity(src.GetAllItemSecurity(), des.GetAllItemSecurity()))
                    {
                        report.updateXslResult("Items Security Match!");

                    }
                    else
                    {
                        report.updateXslResult("Items Security UnMatch!");

                    }
                }
                setProcessValue(100);
                setCompareBtn(true);
            }
            //catch
            //{
            //    MessageBox.Show("catch a excption of comparing!");
            //    setProcessValue(0);
            //    setCompareBtn(true);
            //}
        }

        private void setCompareBtn(bool bEnabled)
        {
            if (this.compareBtn.InvokeRequired)
            {
                SetCompareBtnCallback cb = new SetCompareBtnCallback(setCompareBtn);
                this.Invoke(cb, new object[] { bEnabled });
            }
            else
            {
                this.compareBtn.Enabled = bEnabled;
            }
        }

        private void setProcessValue(int value)
        {
            if (this.compareProcessBar.InvokeRequired)
            {
                SetProcessValueCallback cb = new SetProcessValueCallback(setProcessValue);
                this.Invoke(cb, new object[] { value });
            }
            else
            {
                this.compareProcessBar.Value = value;
            }
        }
    }
}
