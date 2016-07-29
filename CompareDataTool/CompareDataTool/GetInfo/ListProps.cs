using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Workflow;
using System.Security;
using System.Windows.Forms;

namespace CompareDataTool
{
    public class ListProps:SiteProps
    {
         List mlist;
        
         public List L
         {
             get { return this.mlist; }
         }

         public Dictionary<string, object> GetListSettings(string ListTitle)
         {
             List list = W.Lists.GetByTitle(ListTitle);
             Context.Load(list, l => l.Title, l => l.Description, l => l.OnQuickLaunch, l => l.EnableModeration, l => l.EnableVersioning,
                     l => l.EnableMinorVersions, l => l.DraftVersionVisibility, l => l.ForceCheckout, l => l.AllowContentTypes, l => l.DocumentTemplateUrl,
                     l => l.EnableFolderCreation, l => l.IsSiteAssetsLibrary, l => l.ValidationFormula, l => l.ValidationMessage);
             Context.ExecuteQuery();

             Dictionary<string, object> Lproperties = new Dictionary<string, object>();
             Lproperties.Add("Title", list.Title);
             Lproperties.Add("Description", list.Description);
             Lproperties.Add("Navigation", list.OnQuickLaunch);
             Lproperties.Add("ContentApproval", list.EnableModeration);
             Lproperties.Add("Versioning", list.EnableVersioning);
             Lproperties.Add("MinorVersions", list.EnableMinorVersions);
             Lproperties.Add("DraftVersionVisibility", (int)list.DraftVersionVisibility);
             Lproperties.Add("RequireCheckOut", list.ForceCheckout);
             Lproperties.Add("AllowContentTypes", list.AllowContentTypes);
             Lproperties.Add("DocumentTemplateUrl", list.DocumentTemplateUrl);
             Lproperties.Add("FolderCreation", list.EnableFolderCreation);
             Lproperties.Add("SiteAssetsLibrary", list.IsSiteAssetsLibrary);
             Lproperties.Add("ValidationFormula", list.ValidationFormula);
             Lproperties.Add("ValidationMessage", list.ValidationMessage);

             return Lproperties;


         }

         public FieldCollection GetListColumns(string ListTitle)
         {
             List list = W.Lists.GetByTitle(ListTitle);
             Context.Load(list, l => l.Fields);
             Context.ExecuteQuery();

             return list.Fields;

         }

         public ViewCollection GetListViews(string ListTitle)
         {
             List list = W.Lists.GetByTitle(ListTitle);
             Context.Load(list, l => l.Views);
             Context.ExecuteQuery();

             return list.Views;
         }

         public WorkflowAssociationCollection GetListWorkflows(string ListTitle)
         {           
             List list = W.Lists.GetByTitle(ListTitle);
             Context.Load(list, l => l.WorkflowAssociations);
             Context.ExecuteQuery();

             return list.WorkflowAssociations;
         }

         public Dictionary<string, object> GetListSecurity(string ListTitle)
         {
             List list = W.Lists.GetByTitle(ListTitle);
             Context.Load(list, l => l.HasUniqueRoleAssignments, l => l.RoleAssignments);
             Context.ExecuteQuery();

             Dictionary<string, object> users = new Dictionary<string, object>();

             users.Add("HasUniqueRoleAssignments", list.HasUniqueRoleAssignments);

             foreach (RoleAssignment r in list.RoleAssignments)
             {
                 string P = "";
                 Context.Load(r.Member);
                 Context.ExecuteQuery();
                 Context.Load(r.RoleDefinitionBindings);
                 Context.ExecuteQuery();
                 foreach (RoleDefinition rd in r.RoleDefinitionBindings)
                 {
                     // Console.WriteLine(rd.Name);
                     P = P + rd.Name + ";";
                 }
                 users.Add(r.Member.LoginName + r.Member.PrincipalType.ToString(), P);
             }

             return users;

         }

         public void InitialList(string ListTitle)
         {
             List list = W.Lists.GetByTitle(ListTitle);
             try
             {
                 Context.Load(list);
                 Context.ExecuteQuery();
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
                 System.Environment.Exit(0); 
             }

             mlist = list; 
         }

    }
}
