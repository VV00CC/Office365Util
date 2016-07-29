using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using System.IO;
using System.Xml;

namespace CompareDataTool.CompareCore
{
    class CompareSettings
    {
        static Reporter report = new Reporter();

        static public bool CompareDictionary(Dictionary<string, object> D1, Dictionary<string, object> D2)
        {
            int i = 0;
            string details = "";
            Dictionary<string, object> D3 = new Dictionary<string, object>(D2);

            foreach (KeyValuePair<string, object> d in D1)
            {
                if (!D3.ContainsKey(d.Key))
                {
                    details = details + d.Key + " NOT Exist in Destination" + ";\n";
                   
                    i++;
                }
                else
                {

                    object v1 = D1[d.Key];
                    object v2 = D3[d.Key];

                    if (!Equals(v1, v2))
                    {
                        details = details + d.Key + " UnMatch! Source: " + v1 + " / Des: " + v2 + ";\n";
                       
                        i++;
                    }
                    D3.Remove(d.Key);
                }
            }

            if (D3.Count != 0)
            {
                foreach (KeyValuePair<string, object> d in D3)
                {
                    details = details + d.Key + " NOT Exist in Source" + ";\n";
                   
                    i++;
                }
            }

            if (i == 0)
            {
                report.updateXslDetails(details);
                
                return true;
            }
            else
            {
                report.updateXslDetails(details);
                
                return false;
            }
        }

        static public bool CompareItems(List<Dictionary<string, object>> l1, List<Dictionary<string, object>> l2)
        {
            int i = 0;
            
            string details = "";
            foreach (Dictionary<string, object> d in l1)
            {
                foreach (Dictionary<string, object> de in l2)
                {
                    if (IsEqualItem(d, de))
                    {
                        if (!CompareDictionary(d, de))
                        {
                            i++;
                        }

                        l2.Remove(de);
                        

                        break;
                    }
                    else if (de.Equals(l2[l2.Count - 1]))
                    {
                        if (d.Keys.Contains("GUID"))
                        {
                            details = details + d["GUID"] + " NOT Exist in Destination" + ";\n";
                            
                        }
                        else
                        {
                            details = details + d["Name"] + " NOT Exist in Destination" + ";\n";
                            
                        }
                        i++;
                    }
                }
            }

            if (l2.Count != 0)
            {
                foreach (Dictionary<string, object> dd in l2)
                {
                    if (dd.Keys.Contains("GUID"))
                    {
                        details = details + dd["GUID"] + " NOT Exist in Source" + ";\n";
                        
                    }
                    else
                    {
                        details = details + dd["Name"] + " NOT Exist in Source" + ";\n";
                        
                    }
                }
            }

            if (i == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        static public bool CompareItemsSecurity(List<Dictionary<string, object>> l1, List<Dictionary<string, object>> l2)
        {
            int i = 0;

            string details = "";
            foreach (Dictionary<string, object> d in l1)
            {
                foreach (Dictionary<string, object> de in l2)
                {
                    if (IsEqualItemSecurity(d, de))
                    {
                        if (!CompareDictionary(d, de))
                        {
                            i++;
                        }

                        l2.Remove(de);                    

                        break;
                    }
                    else if (de.Equals(l2[l2.Count - 1]))
                    {
                        details = details + d["ItemName"] + " NOT Exist in Destination" + ";\n";
                        
                        i++;
                    }
                }
            }

            if (l2.Count != 0)
            {
                foreach (Dictionary<string, object> dd in l2)
                {
                    details = details + dd["ItemName"] + " NOT Exist in Source" + ";\n";
                    

                }
            }

            if (i == 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        static private bool IsEqualItem(Dictionary<string, object> D1, Dictionary<string, object> D2)
        {
            bool equalkey = false;
            if (D1.Keys.Contains("GUID"))
            {
                if (D1["GUID"].Equals(D2["GUID"]))
                {
                    equalkey = true;
                }

                return equalkey;
            }
            else if (D1.Keys.Contains("Name"))
            {
                if (D1["Name"].Equals(D2["Name"]))
                {
                    equalkey = true;
                }

                return equalkey;
            }

            return equalkey;
        }

        static private bool IsEqualItemSecurity(Dictionary<string, object> D1, Dictionary<string, object> D2)
        {
            bool equalkey = false;
            
                if (D1["ItemName"].Equals(D2["ItemName"]))
                {
                    equalkey = true;
                }

                return equalkey;
            

        }

        static public Dictionary<string, object> MergeSecurity(Dictionary<string, object> D1, Dictionary<string, object> D2)
        {
            Dictionary<string, object> D3 = new Dictionary<string, object>(D1);

            if (!D1["HasUniqueRoleAssignments"].Equals(D2["HasUniqueRoleAssignments"]))
            {
                D3["HasUniqueRoleAssignments"] = true;
            }

            foreach (string key in D2.Keys)
            {
                if (key != "HasUniqueRoleAssignments")
                {
                    if (D3.ContainsKey(key))
                    {
                        D3[key] = MergePermissions(D3[key].ToString(), D2[key].ToString());
                    }
                    else
                    {
                        D3.Add(key, D2[key]);
                    }
                }
            }
            return D3;
        }

        private static string MergePermissions(string s1, string s2)
        {

            string[] str1 = s1.Split(';');

            string[] str2 = s2.Split(';');

            string[] myData = Distill(str1, str2).ToString().Split(';');
            myData = RemoveDup(myData);
            string strtype = "";
            foreach (string x in myData)
            {
                if (x.ToString() != "")
                {
                    strtype += x.ToString() + ";";
                }

            }

            return strtype;
        }

        private static string Distill(string[] str1, string[] str2)
        {
            ArrayList al = new ArrayList();

            for (int index = 0; index < str1.Length - 1; index++)
            {

                al.Add(str1[index].ToString());
            }
            for (int index = 0; index < str2.Length - 1; index++)
            {
                al.Add(str2[index].ToString());
            }

            string strtype = "";
            for (int i = 0; i < al.Count; i++)
            {
                strtype += al[i].ToString() + ";";

            }

            return strtype;


        }

        private static string[] RemoveDup(string[] myData)
        {
            if (myData.Length > 0)
            {
                Array.Sort(myData);
                int size = 1;
                for (int i = 1; i < myData.Length; i++)
                    if (myData[i] != myData[i - 1])
                        size++;

                string[] myTempData = new string[size];
                int j = 0;
                myTempData[j++] = myData[0];

                for (int i = 1; i < myData.Length; i++)
                    if (myData[i] != myData[i - 1])
                        myTempData[j++] = myData[i];

                return myTempData;
            }


            return myData;
        }       
    }
}
