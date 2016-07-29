using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using Microsoft.SharePoint;
using System.Reflection;
using System.Xml;

namespace CompareDataTool.CompareCore
{
    class CompareColumns
    {
        string compareResult = "";
        bool caseReslt = true;
        Reporter report = new Reporter();
        public bool CompareColumnCollection(FieldCollection Soufields, FieldCollection Desfields)
        {
            foreach (Field sou in Soufields)
            {
                if (!sou.Hidden)
                {
                    var desCollection = from d in Desfields.Where(d => d.Hidden == false)
                                        where sou.Id == d.Id
                                        select d;
                    if (desCollection != null)
                    {
                        switch (desCollection.Count())
                        {
                            case 1:
                                GetCompareFieldElement(sou, desCollection.First());
                                break;
                            default:
                                caseReslt = false;
                                compareResult += sou.Title + ":\n";
                                compareResult += "The column named has more than one in the destination, can not compare.\n";
                                break;
                        }
                    }
                    else
                    {
                        caseReslt = false;
                        compareResult += sou.Title + ":\n";
                        compareResult += "The column named \"" + sou.TypeDisplayName + "\" Can not find in the destination.\n";
                    }
                }
                
            }
            report.updateXslDetails(compareResult);
            return caseReslt;
        }

        private void GetCompareFieldElement(Field sourceField,Field desField)
        {
            try
            {
                XmlDocument sourceDoc = new XmlDocument();
                XmlDocument desDoc = new XmlDocument();
                sourceDoc.LoadXml(sourceField.SchemaXml);
                desDoc.LoadXml(desField.SchemaXml);
                XmlElement sourceEle = sourceDoc.DocumentElement;
                XmlElement desEle = desDoc.DocumentElement;
                compareFieldXml(sourceEle, desEle);
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occored while GetCompareFieldElement,error message:"+ex.Message);
            }
            
        }

        private void compareFieldXml(XmlElement sourceXmlEle,XmlElement desXmlEle)
        {
            XmlAttributeCollection sourceAtt = sourceXmlEle.Attributes;
            XmlAttributeCollection desAtt = desXmlEle.Attributes;
            try
            {
                if(sourceAtt["Type"].Value!=desAtt["Type"].Value)
                {
                    caseReslt = false;
                    compareResult += sourceAtt["DisplayName"].Value+":\n";
                    compareResult += "The Column named has different type between source and destination.Cannot compare.\n";
                    return;
                }
                if (sourceAtt["ColName"] != null)
                {
                    sourceAtt.Remove(sourceAtt["ColName"]);
                }
                if(sourceAtt["SourceID"]!=null)
                {
                    sourceAtt.Remove(sourceAtt["SourceID"]);
                }
                if(sourceAtt["DisplayName"].Value.Length>5)
                {
                    if (sourceAtt["DisplayName"].Value.Substring(0, 6).Equals("Lookup"))
                    {
                        sourceAtt.Remove(sourceAtt["List"]);
                    }
                }
                foreach(XmlAttribute s in sourceAtt)
                {
                    foreach(XmlAttribute d in desAtt)
                    {
                        if(s.Name==d.Name)
                        {
                            if(s.Value==d.Value)
                            {
                                //log
                                break;
                            }
                            else
                            {
                                caseReslt = false;
                                compareResult += sourceXmlEle.Attributes["DisplayName"].Value + "\n";
                                compareResult += "The setting named "+s.Name+" does not matched between source and destination.\n";
                                return;
                            }
                        }
                    }
                }
                if((sourceXmlEle.InnerXml!=null)&&(desXmlEle.InnerXml!=null))
                {
                    if (sourceXmlEle.InnerXml.Equals(desXmlEle.InnerXml))
                    {
                        //log
                    }
                    else
                    {
                        caseReslt = false;
                        compareResult += sourceXmlEle.Attributes["DisplayName"].Value + "\n";
                        compareResult += "The Column setting does not matched between source and destination.\n";
                    }
                }
                else if(sourceXmlEle.InnerXml!=desXmlEle.InnerXml)
                {
                    caseReslt = false;
                    compareResult += sourceXmlEle.Attributes["DisplayName"].Value + "\n";
                    compareResult += "The Column setting does not matched between source and destination.\n";
                    return;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("An error occored while compareFieldXml, Column named " + sourceXmlEle.Attributes["DisplayName"].Value + ",error message: " + e.Message);
            }
            return;
        }
 
    }
}
