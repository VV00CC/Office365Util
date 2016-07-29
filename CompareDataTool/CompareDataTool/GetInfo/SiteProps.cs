using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using System.Windows.Forms;

namespace CompareDataTool
{
    public class SiteProps
    {
        ClientContext mContext;
        Web mWeb;

        public ClientContext Context
        {
            get { return this.mContext; }
            set { this.mContext = value; }
        }

        public Web W
        {
            get { return this.mWeb; }
        }

        private ClientContext ContextConnector(string SiteUrl, string username, string pwd)
        {
            ClientContext context = new ClientContext(SiteUrl);
            SecureString pw = new SecureString();
            foreach (char c in pwd)
            {
                pw.AppendChar(c);
            }
            SharePointOnlineCredentials oCredential = new SharePointOnlineCredentials(username, pw);
            context.Credentials = oCredential;
            return context;
        }

        public void InitialWeb(string SiteUrl, string username, string pwd)
        {
            mContext = ContextConnector(SiteUrl,username,pwd);
            try
            {
                if (Context.Web == null)
                {
                    mWeb = Context.Site.RootWeb;
                }
                else
                {
                    mWeb = Context.Web;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Environment.Exit(0); 
            }

        }
 
        
    }
}
