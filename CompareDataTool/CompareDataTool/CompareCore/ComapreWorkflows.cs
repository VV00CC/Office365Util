using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Xml;
using Microsoft.SharePoint.Client.Workflow;

namespace CompareDataTool.CompareCore
{
    class CompareWorkflows
    {
        private string compareResult = "";
        private bool caseResult = true;
        private XmlElement sourceWorkflowEle;
        private XmlElement desWorkflowEle;
        private XmlDocument sourceThreeStateXmlDoc = new XmlDocument();
        private XmlDocument desThreeStateXmlDoc = new XmlDocument();
        private XmlDocument sourceApprovalXmlDoc = new XmlDocument();
        private XmlDocument desApprovalXmlDoc = new XmlDocument();
        private Guid threeStateWorkflowGuid = new Guid("c6964bff-bf8d-41ac-ad5e-b61ec111731a");
        private Guid dispositionWorkflowGuid = new Guid("dd19a800-37c1-43c0-816d-f8eb5f4a4145");
        private Guid approvalWorkflowGuid = new Guid("8ad4d8f0-93a7-4941-9657-cf3706f00409");
        private Guid collectFeedbackWorkflowGuid = new Guid("3bfb07cb-5c6a-4266-849b-8d6711700409");
        private Guid collectSignaturesWorkflowGuid = new Guid("77c71f43-f403-484b-bcb2-303710e00409");
        Reporter report = new Reporter();

        public bool CompareWorkflowCollection(WorkflowAssociationCollection sourceWorkflowCollection, WorkflowAssociationCollection desWorkflowCollection)
        {
            foreach (WorkflowAssociation sourceWorkflow in sourceWorkflowCollection)
            {
                /*=================目的端存在多个重复的Workflow不支持check，待修改=====================*/
                foreach (WorkflowAssociation desWorkflow in desWorkflowCollection)
                {
                    if (sourceWorkflow.Name.Equals(desWorkflow.Name, StringComparison.InvariantCulture))
                    {
                        if (IsSameBaseType(sourceWorkflow, desWorkflow))
                        {
                            if (IsSelectTypeWorkflow(sourceWorkflow, threeStateWorkflowGuid))
                            {
                                CompareThreeStateWorkflow(sourceWorkflow, desWorkflow);
                                break;
                            }
                            else if (IsSelectTypeWorkflow(sourceWorkflow, dispositionWorkflowGuid))
                            {
                                CompareDispositionWorkflow(sourceWorkflow, desWorkflow);
                                break;
                            }
                            else if (IsSelectTypeWorkflow(sourceWorkflow, collectFeedbackWorkflowGuid))
                            {
                                CompareApprovalWorkflow("d:Reviewers", sourceWorkflow, desWorkflow);
                                break;
                            }
                            else if (IsSelectTypeWorkflow(sourceWorkflow, collectSignaturesWorkflowGuid))
                            {
                                CompareCollectSignaturesWorkflow(sourceWorkflow, desWorkflow);
                                break;
                            }
                            else
                            {
                                CompareApprovalWorkflow("d:Approvers", sourceWorkflow, desWorkflow);
                                break;
                            }

                        }
                        else
                        {
                            caseResult = false;
                            compareResult += sourceWorkflow.Name + ":\n" + "Warn：　The workflow named " + sourceWorkflow.Name + " has different base type between the source and destination." +
                                "Can not compare.\n";
                        }
                    }
                    else if (desWorkflow.Name == desWorkflowCollection[desWorkflowCollection.Count - 1].Name)
                    {
                        caseResult = false;
                        compareResult += sourceWorkflow.Name + ":\n" + "Warn:  The source workflow named \"" + sourceWorkflow.Name + "\"can not find in the destination.\n";
                    }
                }
            }
            report.updateXslDetails(compareResult);
            return caseResult;
        }
        private bool IsSameBaseType(WorkflowAssociation w1, WorkflowAssociation w2)
        {
            bool returnKey = false;
            if (w1.BaseId.Equals(w2.BaseId))
            {
                returnKey = true;
            }
            return returnKey;
        }
        private bool IsSelectTypeWorkflow(WorkflowAssociation w1, Guid guid)
        {
            bool returnKey = false;
            if (w1.BaseId.Equals(guid))
            {
                returnKey = true;
            }
            return returnKey;

        }
        /*用于比对Three state类型的workflow*/
        private void CompareThreeStateWorkflow(WorkflowAssociation sourceWorkflow, WorkflowAssociation desWorkflow)
        {

            try
            {
                compareResult += sourceWorkflow.Name + ":\n";
                CompareHistoryAndTaskList(sourceWorkflow, desWorkflow);   //比对第一页的Task list和History list，workflow setting api不支持转移所以目前不支持比对
                //=========================================初始化Xml文档和声明namespace===============================================//
                sourceThreeStateXmlDoc.LoadXml(sourceWorkflow.AssociationData);
                desThreeStateXmlDoc.LoadXml(desWorkflow.AssociationData);
                sourceWorkflowEle = sourceThreeStateXmlDoc.DocumentElement;
                desWorkflowEle = desThreeStateXmlDoc.DocumentElement;
                XmlNamespaceManager sourceManager = new XmlNamespaceManager(sourceThreeStateXmlDoc.NameTable);
                XmlNamespaceManager desManager = new XmlNamespaceManager(desThreeStateXmlDoc.NameTable);
                sourceManager.AddNamespace("my", sourceWorkflowEle.NamespaceURI);
                desManager.AddNamespace("my", desWorkflowEle.NamespaceURI);
                //=========================================Choice Column Setting Check===============================================//
                if (CompareThreeStateWorkflowXmlElement("my:StatusField", "\"Workflow states\"→\"Select a 'Choice' field\""))
                {
                    CompareThreeStateWorkflowXmlElement("my:InitialState", "\"Workflow states\"→\"Initial state\"");
                    CompareThreeStateWorkflowXmlElement("my:MiddleState", "\"Workflow states\"→\"Middle state\"");
                    CompareThreeStateWorkflowXmlElement("my:FinalState", "\"Workflow states\"→\"Final state\"");
                }
                //=========================================First State Setting Check===============================================//
                CompareThreeStateWorkflowXmlElement("my:CustomMessageText", "\"First State\"→\"Task Details\"→\"Task Title\"→\"Custom message\"");
                if (CompareThreeStateWorkflowXmlElement("my:AddLFCustomMessage", "\"First State\"→\"Task Details\"→\"Task Title\"→\"Include list field\""))
                {
                    CompareThreeStateWorkflowXmlElement("my:CustomMessageField", "\"First State\"→\"Task Details\"→\"Task Title\"→\"Include list field\"");
                }
                CompareThreeStateWorkflowXmlElement("my:CustomMessageTextBody", "\"First State\"→\"Task Details\"→\"Task Description\"→\"Custom message\"");
                if (CompareThreeStateWorkflowXmlElement("my:AddLFCustomMessageBody", "\"First State\"→\"Task Details\"→\"Task Description\"→\"Include list field\""))
                {
                    CompareThreeStateWorkflowXmlElement("my:CustomMessageBodyField", "\"First State\"→\"Task Details\"→\"Task Description\"→\"Include list field\"");
                }
                CompareThreeStateWorkflowXmlElement("my:InsertLinkToListItem", "\"First State\"→\"Task Details\"→\"Task Description\"→\"Insert link to List item\"");
                if (CompareThreeStateWorkflowXmlElement("my:AddLFDueDate", "\"First State\"→\"Task Details\"→\"Task Due Date\"→\"Insert list field\""))
                {
                    CompareThreeStateWorkflowXmlElement("my:DueDateField", "\"First State\"→\"Task Details\"→\"Task Due Date\"→\"Insert list field\"");
                }
                if ((sourceWorkflowEle.SelectSingleNode("//my:AddLFAT", sourceManager).InnerText == "UseAT")
                    && (sourceWorkflowEle.SelectSingleNode("//my:AddLFAT", sourceManager).InnerText == desWorkflowEle.SelectSingleNode("//my:AddLFAT", desManager).InnerText))
                {
                    CompareThreeStateWorkflowXmlElement("my:AssignedToField", "\"First State\"→\"Task Details\"→\"Task Assigned to\"→\"Include list field\"");
                }
                else if ((sourceWorkflowEle.SelectSingleNode("//my:AddLFAT", sourceManager).InnerText == "UseCustom")
                    && (sourceWorkflowEle.SelectSingleNode("//my:AddLFAT", sourceManager).InnerText == desWorkflowEle.SelectSingleNode("//my:AddLFAT", desManager).InnerText))
                {
                    CompareThreeStateWorkflowXmlElement("my:CustomAssignedTo", "\"First State\"→\"Task Details\"→\"Task Assigned to\"→\"Custom\"");
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"First State\"→\"Task Details\"→\"Task Assigned to\" does not matched between source and destination.\n ";
                }
                if (CompareThreeStateWorkflowXmlElement("my:SendEmailCheck", "\"First State\"→\"E-mail Message Details\"→\"Send e-mail message\""))
                {
                    if (CompareThreeStateWorkflowXmlElement("my:ToTextCheck", "\"First State\"→\"E-mail Message Details\"→\"Include Task Assigned To\""))
                    {
                        CompareThreeStateWorkflowXmlElement("my:ToText", "\"First State\"→\"E-mail Message Details\"→\"To\"");
                    }
                    if (CompareThreeStateWorkflowXmlElement("my:SubjectTextCheck", "\"First State\"→\"E-mail Message Details\"→\"Use Task Title\""))
                    {
                        CompareThreeStateWorkflowXmlElement("my:SubjectText", "\"First State\"→\"E-mail Message Details\"→\"Subject\"");
                    }
                    if (CompareThreeStateWorkflowXmlElement("my:BodyTextCheck", "\"First State\"→\"E-mail Message Details\"→\"Insert link to List item\""))
                    {
                        CompareThreeStateWorkflowXmlElement("my:BodyText", "\"First State\"→\"E-mail Message Details\"→\"Body\"");
                    }
                }
                //=========================================Middle State Setting Check===============================================//
                CompareThreeStateWorkflowXmlElement("my:CustomMessageText2", "\"Second State\"→\"Task Details\"→\"Task Title\"→\"Custom message\"");
                if (CompareThreeStateWorkflowXmlElement("my:AddLFCustomMessage2", "\"Second State\"→\"Task Details\"→\"Task Title\"→\"Include list field\""))
                {
                    CompareThreeStateWorkflowXmlElement("my:CustomMessageField2", "\"Second State\"→\"Task Details\"→\"Task Title\"→\"Include list field\"");
                }
                CompareThreeStateWorkflowXmlElement("my:CustomMessageTextBody2", "\"Second State\"→\"Task Details\"→\"Task Description\"→\"Custom message\"");
                if (CompareThreeStateWorkflowXmlElement("my:AddLFCustomMessageBody2", "\"Second State\"→\"Task Details\"→\"Task Description\"→\"Include list field\""))
                {
                    CompareThreeStateWorkflowXmlElement("my:CustomMessageBodyField2", "\"Second State\"→\"Task Details\"→\"Task Description\"→\"Include list field\"");
                }
                CompareThreeStateWorkflowXmlElement("my:InsertLinkToListItem2", "\"Second State\"→\"Task Details\"→\"Task Description\"→\"Insert link to List item\"");
                if (CompareThreeStateWorkflowXmlElement("my:AddLFDueDate2", "\"Second State\"→\"Task Details\"→\"Task Due Date\"→\"Insert list field\""))
                {
                    CompareThreeStateWorkflowXmlElement("my:DueDateField2", "\"Second State\"→\"Task Details\"→\"Task Due Date\"→\"Insert list field\"");
                }
                if ((sourceWorkflowEle.SelectSingleNode("//my:AddLFAT2", sourceManager).InnerText == "UseAT")
                    && (sourceWorkflowEle.SelectSingleNode("//my:AddLFAT2", sourceManager).InnerText == desWorkflowEle.SelectSingleNode("//my:AddLFAT2", desManager).InnerText))
                {
                    CompareThreeStateWorkflowXmlElement("my:AssignedToField2", "\"Second State\"→\"Task Details\"→\"Task Assigned to\"→\"Include list field\"");
                }
                else if ((sourceWorkflowEle.SelectSingleNode("//my:AddLFAT2", sourceManager).InnerText == "UseCustom")
                    && (sourceWorkflowEle.SelectSingleNode("//my:AddLFAT2", sourceManager).InnerText == desWorkflowEle.SelectSingleNode("//my:AddLFAT2", desManager).InnerText))
                {
                    CompareThreeStateWorkflowXmlElement("my:CustomAssignedTo2", "\"Second State\"→\"Task Details\"→\"Task Assigned to\"→\"Custom\"");
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"Second State\"→\"Task Details\"→\"Task Assigned to\" does not matched between source and destination.\n ";
                }
                if (CompareThreeStateWorkflowXmlElement("my:SendEmailCheck2", "\"Second State\"→\"E-mail Message Details\"→\"Send e-mail message\""))
                {
                    if (CompareThreeStateWorkflowXmlElement("my:ToTextCheck2", "\"Second State\"→\"E-mail Message Details\"→\"Include Task Assigned To\""))
                    {
                        CompareThreeStateWorkflowXmlElement("my:ToText2", "\"Second State\"→\"E-mail Message Details\"→\"To\"");
                    }
                    if (CompareThreeStateWorkflowXmlElement("my:SubjectTextCheck2", "\"Second State\"→\"E-mail Message Details\"→\"Use Task Title\""))
                    {
                        CompareThreeStateWorkflowXmlElement("my:SubjectText2", "\"Second State\"→\"E-mail Message Details\"→\"Subject\"");
                    }
                    if (CompareThreeStateWorkflowXmlElement("my:BodyTextCheck2", "\"Second State\"→\"E-mail Message Details\"→\"Insert link to List item\""))
                    {
                        CompareThreeStateWorkflowXmlElement("my:BodyText2", "\"Second State\"→\"E-mail Message Details\"→\"Body\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occored while init three state workflow xml, error message :" + ex.Message);
            }

        }
        /*用于检查两个workflow的task list与 history list的title是否一致，只比对title，若不一致会返回结果
         最近更新：加入Description 比对~*/
        private void CompareHistoryAndTaskList(WorkflowAssociation sourceWorkflow, WorkflowAssociation desWorkflow)
        {
            try
            {
                if (sourceWorkflow.Description.Equals(desWorkflow.Description, StringComparison.InvariantCulture))
                {
                    //log
                }
                else
                {
                    caseResult = false;
                    compareResult += "The workflow's description does not matched between source and destination.\n";
                }
                if (sourceWorkflow.TaskListTitle.Equals(desWorkflow.TaskListTitle, StringComparison.InvariantCulture))
                {
                    //log
                }
                else
                {
                    caseResult = false;
                    compareResult += "The task list being used does not matched between source and destination.\n";
                }
                if (sourceWorkflow.HistoryListTitle.Equals(desWorkflow.HistoryListTitle, StringComparison.InvariantCulture))
                {
                    //log
                }
                else
                {
                    caseResult = false;
                    compareResult += "The history list being used does not matched between source and destination.\n";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occored while compare history list and task list setting , workflow named :" + sourceWorkflow.Name + ". Error message:" + e.Message + "\n");
            }

        }
        private void CompareApprovalWorkflow(string Key, WorkflowAssociation sourceWorkflow, WorkflowAssociation desWorkflow)
        {
            string compareKey = "//";
            compareKey += Key;
            compareResult += sourceWorkflow.Name + ":\n";
            CompareHistoryAndTaskList(sourceWorkflow, desWorkflow);
            sourceApprovalXmlDoc.LoadXml(sourceWorkflow.AssociationData);
            desApprovalXmlDoc.LoadXml(desWorkflow.AssociationData);
            sourceWorkflowEle = sourceApprovalXmlDoc.DocumentElement;
            desWorkflowEle = desApprovalXmlDoc.DocumentElement;
            XmlNamespaceManager sourceManager = new XmlNamespaceManager(sourceApprovalXmlDoc.NameTable);
            XmlNamespaceManager desManager = new XmlNamespaceManager(desApprovalXmlDoc.NameTable);
            sourceManager.AddNamespace("d", "http://schemas.microsoft.com/office/infopath/2009/WSSList/dataFields");
            sourceManager.AddNamespace("pc", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
            sourceManager.AddNamespace("dfs", "http://schemas.microsoft.com/office/infopath/2003/dataFormSolution");
            desManager.AddNamespace("d", "http://schemas.microsoft.com/office/infopath/2009/WSSList/dataFields");
            desManager.AddNamespace("dfs", "http://schemas.microsoft.com/office/infopath/2003/dataFormSolution");
            desManager.AddNamespace("pc", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
            try
            {
                if (sourceWorkflowEle.LastChild.FirstChild.FirstChild.ChildNodes.Count == desWorkflowEle.LastChild.FirstChild.FirstChild.ChildNodes.Count)
                {
                    for (int i = 0; i < sourceWorkflowEle.SelectSingleNode("descendant::" + Key, sourceManager).ChildNodes.Count; i++)
                    {
                        XmlNode sourceAssignmentNode = sourceWorkflowEle.SelectSingleNode(compareKey, sourceManager).ChildNodes[i];
                        XmlNode desAssignmentNode = desWorkflowEle.SelectSingleNode(compareKey, desManager).ChildNodes[i];
                        XmlNode sourcePersonNode = sourceAssignmentNode.SelectSingleNode("descendant::pc:Person", sourceManager);
                        XmlNode desPersonNode = desAssignmentNode.SelectSingleNode("descendant::pc:Person", desManager);
                        CompareApprovalWorkflowXmlElement(sourcePersonNode, desPersonNode, "pc:AccountId", "\"" + Key + "\"→\"Assign to\"→\"" + (i + 1) + " level\"");
                        CompareApprovalWorkflowXmlElement(sourceAssignmentNode, desAssignmentNode, "d:AssignmentType", "\"" + Key + "\"→\"Order\"→\"" + (i + 1) + " level\"");
                    }
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"Approvers\" does not matched between source and destination.\n";
                }
                /*比对Expand Groups*/
                CompareApprovalWorkflowXmlElement(sourceWorkflowEle, desWorkflowEle, "d:ExpandGroups", "Expand Groups");
                /*比对Request*/
                CompareApprovalWorkflowXmlElement(sourceWorkflowEle, desWorkflowEle, "d:NotificationMessage", "Request");
                /*比对Due Date for All Tasks*/
                CompareApprovalWorkflowXmlElement(sourceWorkflowEle, desWorkflowEle, "d:DueDateforAllTasks", "Due Date for All Tasks");
                /*比对Duration Per Task*/
                CompareApprovalWorkflowXmlElement(sourceWorkflowEle, desWorkflowEle, "d:DurationforSerialTasks", "Duration Per Task");
                /*比对Duration Units*/
                CompareApprovalWorkflowXmlElement(sourceWorkflowEle, desWorkflowEle, "d:DurationUnits", "Duration Units");
                /*比对CC*/
                XmlNode sourceCCNode = sourceWorkflowEle.SelectSingleNode("//d:CC", sourceManager);
                XmlNode desCCnode = desWorkflowEle.SelectSingleNode("//d:CC", desManager);
                if ((sourceCCNode != null) && (desCCnode != null))
                {
                    CompareApprovalWorkflowXmlElement(sourceCCNode.FirstChild, desCCnode.FirstChild, "pc:AccountId", "CC");
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"CC\" does not matched between source and destination.\n";
                }
                /*End on First Rejection*/
                CompareApprovalWorkflowXmlElement(sourceWorkflowEle, desWorkflowEle, "d:CancelonChange", "End on Document Change");
                if (Key == "d:Approvers")
                {
                    CompareApprovalWorkflowXmlElement(sourceWorkflowEle, desWorkflowEle, "d:CancelonRejection", "End on First Rejection");
                    CompareApprovalWorkflowXmlElement(sourceWorkflowEle, desWorkflowEle, "d:EnableContentApproval", "Enable Content Approval");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An error occored while compare Approval workflow named " + sourceWorkflow.Name + ". Error message: " + e.Message);
            }
        }
        private void CompareDispositionWorkflow(WorkflowAssociation sourceWorkflow, WorkflowAssociation desWorkflow)
        {
            compareResult += sourceWorkflow.Name + ":\n";
            CompareHistoryAndTaskList(sourceWorkflow, desWorkflow);
        }
        private void CompareCollectSignaturesWorkflow(WorkflowAssociation sourceWorkflow, WorkflowAssociation desWorkflow)
        {
            compareResult += sourceWorkflow.Name + ":\n";
            CompareHistoryAndTaskList(sourceWorkflow, desWorkflow);
            XmlDocument sourceDoc = new XmlDocument();
            XmlDocument desDoc = new XmlDocument();
            sourceDoc.LoadXml(sourceWorkflow.AssociationData);
            desDoc.LoadXml(desWorkflow.AssociationData);
            sourceWorkflowEle = sourceDoc.DocumentElement;
            desWorkflowEle = desDoc.DocumentElement;
            XmlNamespaceManager sourceManager = new XmlNamespaceManager(sourceDoc.NameTable);
            XmlNamespaceManager desManager = new XmlNamespaceManager(desDoc.NameTable);
            sourceManager.AddNamespace("d", "http://schemas.microsoft.com/office/infopath/2009/WSSList/dataFields");
            sourceManager.AddNamespace("pc", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
            sourceManager.AddNamespace("dfs", "http://schemas.microsoft.com/office/infopath/2003/dataFormSolution");
            sourceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            desManager.AddNamespace("d", "http://schemas.microsoft.com/office/infopath/2009/WSSList/dataFields");
            desManager.AddNamespace("dfs", "http://schemas.microsoft.com/office/infopath/2003/dataFormSolution");
            desManager.AddNamespace("pc", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
            desManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            try
            {
                XmlNodeList sourceAssignmentNodeList = sourceWorkflowEle.SelectNodes("//d:Assignment", sourceManager);
                XmlNodeList desAssignmentNodeList = desWorkflowEle.SelectNodes("//d:Assignment", desManager);
                if (sourceAssignmentNodeList.Count == desAssignmentNodeList.Count)
                {
                    for (int i = 0; i < sourceAssignmentNodeList.Count; i++)
                    {
                        XmlNode sourcePeopleNode = sourceAssignmentNodeList[i].SelectSingleNode("descendant::pc:Person", sourceManager);
                        XmlNode desPeopleNode = desAssignmentNodeList[i].SelectSingleNode("descendant::pc:Person", desManager);
                        CompareApprovalWorkflowXmlElement(sourcePeopleNode, desPeopleNode, "pc:AccountId", "\"Signers\"→\"Assign To\"→\"" + (i + 1) + " level\"");
                        CompareApprovalWorkflowXmlElement(sourceAssignmentNodeList[i], desAssignmentNodeList[i], "d:AssignmentType", "\"Signers\"→\"Order\"→\"" + (i + 1) + " level\"");
                    }
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"Signers\" does not matched between source and destination.\n";
                }
                XmlNode sourceCCNode = sourceWorkflowEle.SelectSingleNode("//d:CC", sourceManager);
                XmlNode desCCNode = desWorkflowEle.SelectSingleNode("//d:CC", desManager);
                CompareApprovalWorkflowXmlElement(sourceCCNode, desCCNode, "pc:AccountId", "CC");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occored while compare Collect Signatures workflow named :" + sourceWorkflow.Name + ". Error Message: " + ex.Message + "\n");
            }
        }
        /*用来比对Three State workflow的element子节点，传入值为节点name和比对的setting值，如果两端节点值不属于True或False类型，当两节点值相等时，
         返回true，否则返回false。若两节点值属于Tree或False类型，当两节点值均为True时，返回True，否则返回False。方法可重用。*/
        private bool CompareThreeStateWorkflowXmlElement(string xpath, string settingName)
        {
            XmlElement sourceEle = sourceWorkflowEle;
            XmlElement desEle = desWorkflowEle;
            XmlNamespaceManager sourceManager = new XmlNamespaceManager(sourceThreeStateXmlDoc.NameTable);
            XmlNamespaceManager desManager = new XmlNamespaceManager(desThreeStateXmlDoc.NameTable);
            sourceManager.AddNamespace("my", sourceEle.NamespaceURI);
            desManager.AddNamespace("my", desEle.NamespaceURI);
            bool returnKey = true;
            string path = "//";
            path += xpath;
            string my = sourceEle.SelectSingleNode(path, sourceManager).InnerText;
            try
            {
                if ((sourceEle.SelectSingleNode(path, sourceManager).InnerText == "True") || (sourceEle.SelectSingleNode(path, sourceManager).InnerText == "False"))
                {
                    if ((sourceEle.SelectSingleNode(path, sourceManager).InnerText == "True") && (desEle.SelectSingleNode(path, desManager).InnerText == "True"))
                    {
                        //log
                    }
                    else if ((sourceEle.SelectSingleNode(path, sourceManager).InnerText == "False") && (desEle.SelectSingleNode(path, desManager).InnerText == "False"))
                    {
                        returnKey = false;
                    }
                    else
                    {
                        caseResult = false;
                        returnKey = false;
                        compareResult += "The setting " + settingName + " is unchecked at the source or destination.\n";
                    }
                }
                else if (sourceEle.SelectSingleNode(path, sourceManager).InnerText == desEle.SelectSingleNode(path, desManager).InnerText)
                {
                    //log
                }
                else
                {
                    returnKey = false;
                    caseResult = false;
                    compareResult += "The setting " + settingName + " does not matched between source and destination.\n";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An error occored while compare Three State Workflow Setting: " + settingName + ".Error Message: " + e.Message);
            }
            return returnKey;
        }
        /*用于比对两个ApprovalWorkflowXmlElement节点，传入值为源端和目的端的父节点，Xpath和Setting的name*/
        private bool CompareApprovalWorkflowXmlElement(XmlNode sourceNode, XmlNode desNode, string xpath, string settingName)
        {
            bool returnKey = true;
            XmlElement sourceEle = sourceWorkflowEle;
            XmlElement desEle = desWorkflowEle;
            XmlNamespaceManager sourceManager = new XmlNamespaceManager(sourceApprovalXmlDoc.NameTable);
            XmlNamespaceManager desManager = new XmlNamespaceManager(desApprovalXmlDoc.NameTable);
            sourceManager.AddNamespace("d", "http://schemas.microsoft.com/office/infopath/2009/WSSList/dataFields");
            sourceManager.AddNamespace("pc", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
            sourceManager.AddNamespace("dfs", "http://schemas.microsoft.com/office/infopath/2003/dataFormSolution");
            sourceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            desManager.AddNamespace("d", "http://schemas.microsoft.com/office/infopath/2009/WSSList/dataFields");
            desManager.AddNamespace("dfs", "http://schemas.microsoft.com/office/infopath/2003/dataFormSolution");
            desManager.AddNamespace("pc", "http://schemas.microsoft.com/office/infopath/2007/PartnerControls");
            desManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            string path = "descendant::";
            path += xpath;
            try
            {
                if (sourceNode.SelectSingleNode(path, sourceManager).InnerText == desNode.SelectSingleNode(path, desManager).InnerText)
                {
                    //log
                }
                else
                {
                    returnKey = false;
                    caseResult = false;
                    compareResult += "The setting \"" + settingName + "\" does not matched between source and destination.\n"; ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occored while compare Approval Workflow node,node name is: " + sourceNode.Name + ". Error Message: " + ex.Message);
            }
            return returnKey;
        }
    }
}
