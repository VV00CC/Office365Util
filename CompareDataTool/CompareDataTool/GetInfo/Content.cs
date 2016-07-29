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
    public class Content : ListProps
    {
        bool mAttachment;

        public bool Attachment
        {
            get { return this.mAttachment; }
            set { this.mAttachment = value; }
        }

        private Dictionary<string, object> GetSpecifyItemProps(string sGUID)
        {

            CamlQuery query = new CamlQuery();
            query.ViewXml = "<View><Query><Where><Eq><FieldRef Name='GUID' /><Value Type='Guid'>" + sGUID + "</Value></Eq></Where></View></Query>";
            ListItemCollection ListItem = L.GetItems(query);
            try
            {
                Context.Load(ListItem);
                Context.ExecuteQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Environment.Exit(0); 
            }
            

            Dictionary<string, object> itemprops = new Dictionary<string, object>();
            if (ListItem.Count == 0)
            {
                Console.WriteLine("No such item found."); //log
            }
            else
            {

                Console.WriteLine("Such item found:\n"); // log

                foreach (ListItem i in ListItem)
                {
                    itemprops = i.FieldValues;
                    try
                    {
                        Context.Load(L.Fields);
                        Context.ExecuteQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    foreach (Field f in L.Fields)
                    {
                        if (f.Hidden && itemprops.ContainsKey(f.InternalName))
                        {
                            itemprops.Remove(f.InternalName);
                        }
                        else if (!f.Hidden && itemprops.ContainsKey(f.InternalName))
                        {
                            switch (f.FieldTypeKind)
                            {
                                case FieldType.User:
                                    if (itemprops[f.InternalName] != null)
                                    {
                                        FieldLookupValue user = (FieldLookupValue)itemprops[f.InternalName];
                                        itemprops[f.InternalName] = user.LookupValue;
                                    }
                                    break;
                                case FieldType.Lookup:
                                    if (itemprops[f.InternalName] != null && itemprops[f.InternalName].GetType().ToString() != "System.String")
                                    {
                                        FieldLookupValue lp = (FieldLookupValue)itemprops[f.InternalName];
                                        itemprops[f.InternalName] = lp.LookupValue;
                                    }
                                    break;
                                case FieldType.URL:
                                    if (itemprops[f.InternalName] != null)
                                    {
                                        FieldUrlValue url = (FieldUrlValue)itemprops[f.InternalName];
                                        itemprops[f.InternalName] = url.Description + ":" + url.Url;
                                    }
                                    break;
                                case FieldType.Invalid:
                                    if (itemprops[f.InternalName] != null)
                                    {
                                        string t = "";
                                        foreach (KeyValuePair<string, object> k in (Dictionary<string, object>)itemprops[f.InternalName])
                                        {
                                            t = t + k.Value.ToString();
                                        }
                                        itemprops[f.InternalName] = t;
                                    }
                                    break;
                                default:

                                    break;

                            }

                        }
                    }
                    try
                    {
                        Context.Load(i.ContentType);
                        Context.ExecuteQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    itemprops.Add("ContentType", i.ContentType.Name);
                    itemprops.Add("GUID", sGUID);

                    if (mAttachment)
                    {
                        try
                        {
                            Context.Load(i.AttachmentFiles);
                            Context.ExecuteQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        if (i.AttachmentFiles.Count != 0)
                        {
                            itemprops.Add("Attachs", "");

                            foreach (Attachment at in i.AttachmentFiles)
                            {
                                Console.WriteLine(at.FileName);
                                Console.WriteLine(at.ServerRelativeUrl);
                                itemprops["Attachs"] = itemprops["Attachs"] + at.FileName + ":" + at.ServerRelativeUrl + ";";
                            }
                        }

                    }
                }

            }
            return itemprops;

        }

        public List<Dictionary<string, object>> GetAllItemProps()
        {
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            if (L.BaseType.ToString() == "DocumentLibrary")
            {
                FileCollection files = L.RootFolder.Files;
                try
                {
                    Context.Load(files);
                    Context.ExecuteQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    System.Environment.Exit(0); 
                }
                foreach (File f in files)
                {
                    items.Add(GetSpecifyFileProps(f.Name));
                }

            }
            else if (L.BaseType.ToString() == "GenericList" || L.BaseType.ToString() == "DiscussionBoard" || L.BaseType.ToString() == "Survey" || L.BaseType.ToString() == "Issue")
            {
                CamlQuery query = new CamlQuery();
                query.ViewXml = "<Where><Neq><FieldRef Name='GUID' /><Value Type='Guid'>-1</Value></NEq></Where>";
                ListItemCollection ListItem = L.GetItems(query);
                try
                {
                    Context.Load(ListItem);
                    Context.ExecuteQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    System.Environment.Exit(0); 
                }

                if (ListItem.Count != 0)
                {
                    foreach (ListItem i in ListItem)
                    {
                        items.Add(GetSpecifyItemProps(i["GUID"].ToString()));
                    }
                }

            }
            else
            {

            }

            return items;

        }

        private Dictionary<string, object> GetSpecifyFileProps(string sName)
        {
            CamlQuery query = new CamlQuery();
            query.ViewXml = "<Where><Neq><FieldRef Name='GUID' /><Value Type='Guid'>-1</Value></NEq></Where>";
            ListItemCollection ListItem = L.GetItems(query);
            try
            {
                Context.Load(ListItem);
                Context.ExecuteQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Environment.Exit(0); 
            }

            Dictionary<string, object> fileprops = new Dictionary<string, object>();

            foreach (ListItem item in ListItem)
            {
                try
                {
                    Context.Load(item.File);
                    Context.ExecuteQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    System.Environment.Exit(0); 
                }
                if (item.File.Name == sName)
                {
                    fileprops = item.FieldValues;
                    try
                    {
                        Context.Load(L.Fields);
                        Context.ExecuteQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    foreach (Field f in L.Fields)
                    {
                        if (fileprops.ContainsKey(f.InternalName))
                        {
                            if (f.Hidden)
                            {
                                fileprops.Remove(f.InternalName);
                            }
                            else if (f.InternalName == "ImageWidth" || f.InternalName == "ImageHeight" || f.InternalName == "ImageCreateDate" ||
                                f.InternalName == "AlternateThumbnailUrl" || f.InternalName == "ID" || f.InternalName == "Created" || f.InternalName == "Author" ||
                                f.InternalName == "Modified" || f.InternalName == "Editor" || f.InternalName == "_CopySource" || f.InternalName == "CheckoutUser" || f.InternalName == "_CheckinComment" ||
                                f.InternalName == "_UIVersionString" || f.InternalName == "ParentVersionString" || f.InternalName == "ParentLeafName")
                            {
                                fileprops.Remove(f.InternalName);
                            }
                            else
                            {
                                switch (f.FieldTypeKind)
                                {
                                    case FieldType.User:
                                        if (fileprops[f.InternalName] != null)
                                        {
                                            FieldLookupValue user = (FieldLookupValue)fileprops[f.InternalName];
                                            fileprops[f.InternalName] = user.LookupValue;
                                        }
                                        break;
                                    case FieldType.Lookup:
                                        if (fileprops[f.InternalName] != null && fileprops[f.InternalName].GetType().ToString() != "System.String")
                                        {
                                            FieldLookupValue lp = (FieldLookupValue)fileprops[f.InternalName];
                                            fileprops[f.InternalName] = lp.LookupValue;
                                        }
                                        break;
                                    case FieldType.URL:
                                        if (fileprops[f.InternalName] != null)
                                        {
                                            FieldUrlValue url = (FieldUrlValue)fileprops[f.InternalName];
                                            fileprops[f.InternalName] = url.Description + ":" + url.Url;
                                        }
                                        break;
                                    case FieldType.Invalid:
                                        if (fileprops[f.InternalName] != null)
                                        {
                                            string t = "";
                                            foreach (KeyValuePair<string, object> k in (Dictionary<string, object>)fileprops[f.InternalName])
                                            {
                                                t = t + k.Value.ToString();
                                            }
                                            fileprops[f.InternalName] = t;
                                        }
                                        break;
                                    default:

                                        break;
                                }

                            }
                        }
                    }
                    try
                    {
                        Context.Load(item, i => i.ContentType);
                        Context.ExecuteQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    fileprops.Add("ContentType", item.ContentType.Name);
                    try
                    {
                        Context.Load(item.File, i => i.Author, i => i.CheckedOutByUser, i => i.ListItemAllFields, i => i.LockedByUser, i => i.ModifiedBy, i => i.Versions);
                        Context.ExecuteQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    try
                    {
                        fileprops.Add("Name", item.File.Name);
                    }
                    catch (Exception)
                    {
                        fileprops.Add("Name", "");
                    }
                    try
                    {
                        fileprops.Add("CreatedTime", item.File.TimeCreated.ToFileTimeUtc());
                    }
                    catch (Exception)
                    {

                        fileprops.Add("CreatedTime", 0);
                    }
                    try
                    {
                        fileprops.Add("Author", item.File.Author.LoginName);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("Author", "");
                    }
                    try
                    {
                        fileprops.Add("LastModifiedTime", item.File.TimeLastModified.ToFileTimeUtc());
                    }
                    catch (Exception)
                    {

                        fileprops.Add("LastModifiedTime", 0);
                    }
                    try
                    {
                        fileprops.Add("ModifiedBy", item.File.ModifiedBy.LoginName);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("ModifiedBy", "");
                    }
                    try
                    {
                        fileprops.Add("CheckOut/In Status", item.File.Level);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("CheckOut/In Status", 0);
                    }
                    try
                    {
                        fileprops.Add("CheckedOutBy", item.File.CheckedOutByUser.LoginName);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("CheckedOutBy", "");
                    }
                    try
                    {
                        fileprops.Add("CheckInComment", item.File.CheckInComment);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("CheckInComment", "");
                    }
                    try
                    {
                        fileprops.Add("Size", item.File.Length);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("Size", 0);
                    }
                    try
                    {
                        fileprops.Add("Version", item.File.UIVersionLabel);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("Version", "");
                    }
                    try
                    {
                        fileprops.Add("MajorVersion", item.File.MajorVersion);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("MajorVersion", -1);
                    }
                    try
                    {
                        fileprops.Add("MinorVersion", item.File.MinorVersion);
                    }
                    catch (Exception)
                    {

                        fileprops.Add("MinorVersion", -1);
                    }


                }


            }
            return fileprops;
        }

        public List<Dictionary<string, object>> GetAllItemSecurity()
        {
            CamlQuery query = new CamlQuery();
            query.ViewXml = "<Where><Neq><FieldRef Name='GUID' /><Value Type='Guid'>-1</Value></NEq></Where>";
            ListItemCollection ListItem = L.GetItems(query);
            try
            {
                Context.Load(ListItem);
                Context.ExecuteQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Environment.Exit(0); 
            }

            List<Dictionary<string, object>> users = new List<Dictionary<string, object>>();

            if (ListItem.Count != 0)
            {
                foreach (ListItem i in ListItem)
                {
                    Dictionary<string, object> user = new Dictionary<string, object>();
                    if (L.BaseType.ToString() == "DocumentLibrary")
                    {
                        try
                        {
                            Context.Load(i.File);
                            Context.ExecuteQuery();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        user.Add("ItemName", i.File.Name);
                    }
                    else if (L.BaseType.ToString() == "GenericList" || L.BaseType.ToString() == "DiscussionBoard" || L.BaseType.ToString() == "Survey" || L.BaseType.ToString() == "Issue")
                    {
                        user.Add("ItemName", i["GUID"]);
                    }
                    try
                    {
                        Context.Load(i, ii => ii.HasUniqueRoleAssignments, ii => ii.RoleAssignments);
                        Context.ExecuteQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    user.Add("HasUniqueRoleAssignments", i.HasUniqueRoleAssignments);

                    foreach (RoleAssignment r in i.RoleAssignments)
                    {
                        string P = "";
                        try
                        {
                            Context.Load(r.Member);
                            Context.ExecuteQuery();
                            Context.Load(r.RoleDefinitionBindings);
                            Context.ExecuteQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message); 
                        }
                        foreach (RoleDefinition rd in r.RoleDefinitionBindings)
                        {
                            P = P + rd.Name + ";";
                        }
                        user.Add(r.Member.LoginName + r.Member.PrincipalType.ToString(), P);
                    }
                    users.Add(user);
                }
            }

            return users;

        }
    }
}