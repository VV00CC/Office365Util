using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Xml;

namespace CompareDataTool.CompareCore
{
    class CompareViews
    {
        private string compareResult = "";
        private bool caseResult = true;
        //private XmlNode compareDesKey;

        Reporter report = new Reporter();

        public bool CompareViewCollection(ViewCollection view1, ViewCollection view2)
        {
            foreach (View v in view1)
            {
                int i = 1;
                foreach (View vie in view2)
                {

                    if (IsEqualOrHidden(v, vie))     // compare view name and whether the view is hidden, return true if anyone view is hidden.
                    {
                        if (i == 1)
                        {
                            CompareView(v, vie);
                        }
                        else
                        {
                            Console.WriteLine("The View: " + vie.Title + " has great than 1 in the destination. Can not compare!");
                            break;
                        }
                        i++;
                    }
                    else if (!v.Hidden && !vie.Hidden && vie.Equals(view2[view2.Count - 1]))
                    {
                        compareResult += "The view: " + v.Title + " cannot find in destination!\n";
                    }
                }

            }
            
            report.updateXslDetails(compareResult);
            return caseResult;
            //Console.WriteLine("Compare result is :\n " + compareResult);
        }
        private void CompareView(View sourceView, View desView)
        {
            if (IsSameBaseType(sourceView, desView))
            {
                switch (sourceView.ViewType)
                {
                    case "HTML":
                        CompareStandardView(sourceView, desView);
                        break;
                    case "GRID":
                        CompareStandardView(sourceView, desView);
                        break;
                    case "CALENDAR":
                        CompareCalendarView(sourceView, desView);
                        break;
                    case "GANTT":
                        CompareGanttView(sourceView, desView);
                        break;
                }

            }

        }
        private void CompareStandardView(View v1, View v2)              //compare for standard view
        {
            try
            {
                compareResult += v1.Title + ": \n";
                var sourceViewDoc = new XmlDocument();
                var desViewDoc = new XmlDocument();
                sourceViewDoc.LoadXml(v1.ListViewXml);
                desViewDoc.LoadXml(v2.ListViewXml);
                var sourceViewEle = sourceViewDoc.DocumentElement;
                var desViewEle = desViewDoc.DocumentElement;
                /*XmlAttributeCollection sourceNodeAtt = sourceViewEle.Attributes;
                XmlAttributeCollection desNodeAtt = desViewEle.Attributes;*/
                if (v1.DefaultView == v2.DefaultView)          //  compare default view
                {
                    //log
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting Default View does not matched between source and destination.\n";
                }
                if (v1.MobileView == v2.MobileView)
                {
                    //log
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"Mobile\"→\"Enable this view for mobile access\"does not matched between source and destination.\n";
                }
                if (v1.MobileDefaultView == v2.MobileDefaultView)
                {
                    //log
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"Mobile\"→\"Make this view the default view for mobile access\"does not matched between source and destination.\n";
                }
                XmlAttribute sourceTabularViewAtt = sourceViewEle.SelectSingleNode("/View").Attributes["TabularView"];
                XmlAttribute desTabularViewAtt = desViewEle.SelectSingleNode("/View").Attributes["TabularView"];
                if (sourceTabularViewAtt == desTabularViewAtt)
                {
                    //log
                }
                else
                {
                    compareResult += "The setting \"Tabular View\" does not matched between source and destination.\n";
                }
                foreach (XmlNode sourceNode in sourceViewEle.ChildNodes)
                {
                    if (sourceNode.Name.Equals("Query", StringComparison.InvariantCulture))
                    {
                        XmlNode sourceGroupByNode = sourceViewDoc.SelectSingleNode("/View/Query/GroupBy");
                        XmlNode desGroupByNode = desViewDoc.SelectSingleNode("/View/Query/GroupBy");
                        if ((sourceGroupByNode != null) && (desGroupByNode != null))
                        {
                            XmlAttribute a = sourceGroupByNode.Attributes["Collapse"];
                            XmlAttribute b = desGroupByNode.Attributes["Collapse"];
                            if ((a != null) && (b != null))
                            {
                                //log
                                XmlAttribute c = sourceGroupByNode.Attributes["GroupLimit"];
                                XmlAttribute d = desGroupByNode.Attributes["GroupLimit"];
                                if (c.Value.Equals(d.Value))
                                {
                                    //log
                                }
                                else
                                {
                                    caseResult = false;
                                    compareResult += "The setting \"Group By\"→\"Number of groups to display per page\" "
                                + "does not match in the source or destination.\n";
                                }
                            }
                            else
                            {
                                caseResult = false;
                                compareResult += "The setting \"Group By\"→\"By default, show groupings\" "
                                + "does not match between  source and destination.\n";
                            }
                            if (sourceGroupByNode.HasChildNodes)                //compare Group By -----------need test
                            {
                                bool groupByKey = true;
                                foreach (XmlNode sourceFieldRef in sourceGroupByNode.ChildNodes)
                                {
                                    foreach (XmlAttribute att in sourceFieldRef.Attributes)
                                    {
                                        string f = att.Value;
                                        //string f = sourceFieldRef.Attributes["Name"].Value;
                                        string key = "/View/Query/GroupBy/FieldRef[@";
                                        key += att.Name + "='" + att.Value + "']";
                                        //Console.WriteLine(key);
                                        if (desViewEle.SelectSingleNode(key) != null)
                                        {
                                            //log
                                        }
                                        else
                                        {
                                            groupByKey = false;
                                            caseResult = false;
                                            compareResult += "The setting \"Group By\"→\"Group By the Column\" "
                                            + "does not match between source and destination. Source Setting or Column named: " + att.Value + ".\n";
                                        }
                                    }
                                }
                                if ((sourceGroupByNode.ChildNodes.Count != desGroupByNode.ChildNodes.Count) && groupByKey)  //只能判断出目的端比远端多，但不能返回Column值
                                {
                                    caseResult = false;
                                    compareResult += "The setting \"Group By\"→\"Group By the Column\" ,destination's column is great than source.\n";
                                }

                            }
                        }
                        else if ((sourceGroupByNode == null) && (desGroupByNode == null))
                        {
                            //log
                        }
                        else
                        {
                            caseResult = false;
                            compareResult += "The setting \"Group By\" does not configure in the source or destination.\n";
                        }
                        XmlNode sourceOrderByNode = sourceViewDoc.SelectSingleNode("/View/Query/OrderBy");
                        XmlNode desOrderByNode = desViewDoc.SelectSingleNode("/View/Query/OrderBy");
                        if ((sourceOrderByNode != null) && (desOrderByNode != null))
                        {
                            XmlAttribute a = sourceOrderByNode.Attributes["Override"];
                            XmlAttribute b = desOrderByNode.Attributes["Override"];
                            if ((a != null) && (b != null))
                            {
                                //log
                            }
                            else if ((a == null) && (b == null))
                            {
                                //log
                            }
                            else
                            {
                                caseResult = false;
                                compareResult += "The setting \"Sort\"→\"Sort only by specified criteria\" "
                                + "does not match between  source and destination.\n";
                            }
                            bool orderByKey = true;
                            if (sourceOrderByNode.HasChildNodes)                //sort column --------------need test
                            {
                                foreach (XmlNode sourceFieldRef in sourceOrderByNode.ChildNodes)
                                {
                                    foreach (XmlAttribute att in sourceFieldRef.Attributes)
                                    {
                                        string f = att.Value;
                                        //string f = sourceFieldRef.Attributes["Name"].Value;
                                        string key = "/View/Query/OrderBy/FieldRef[@";
                                        key += att.Name + "='" + att.Value + "']";
                                        //Console.WriteLine(key);
                                        if (desViewEle.SelectSingleNode(key) != null)
                                        {
                                            //log
                                        }
                                        else
                                        {
                                            orderByKey = false;
                                            caseResult = false;
                                            compareResult += "The setting \"Sort\"→\"sort by the column\" "
                                            + "does not matched between source and destination. Source Setting or Column named: " + att.Value + ".\n";
                                        }
                                    }
                                    if ((sourceOrderByNode.ChildNodes.Count != desOrderByNode.ChildNodes.Count) && orderByKey)
                                    {
                                        caseResult = false;
                                        compareResult += "The setting \"Sort\"→\"sort by the column\" , destination's column is great than source.\n";
                                    }

                                }
                                //Console.WriteLine("Compare Result is : "+compareResult);
                            }
                        }
                        else if ((sourceOrderByNode == null) && (desOrderByNode == null))
                        {
                            //log
                        }
                        else
                        {
                            caseResult = false;
                            compareResult += "The setting \"Sort\" does not config in the source or destination.\n";
                        }
                        try
                        {
                            XmlNode sourceWhereNode = sourceViewEle.SelectSingleNode("/View/Query/Where");
                            if (sourceWhereNode != null)
                            {
                                if (desViewEle.SelectSingleNode("/View/Query/Where") != null)
                                {
                                    XmlNode sourceLastChildNode = sourceWhereNode.LastChild;
                                    XmlNode desLastChildNode = desViewEle.SelectSingleNode("/View/Query/Where").LastChild;
                                    XmlNodeList sourceFieldRefNodeList = sourceLastChildNode.SelectNodes("descendant::FieldRef");
                                    XmlNodeList desFieldRefNodeList = desViewEle.SelectSingleNode("/View/Query/Where").LastChild.SelectNodes("descendant::FieldRef");
                                    if (sourceFieldRefNodeList.Count == desFieldRefNodeList.Count)
                                    {
                                        if (sourceFieldRefNodeList.Count == 1)
                                        {
                                            if ((sourceLastChildNode.Name == desLastChildNode.Name) && (sourceFieldRefNodeList[0].Attributes["Name"].Value == desFieldRefNodeList[0].Attributes["Name"].Value)
                                                && (sourceLastChildNode.LastChild.InnerText == desLastChildNode.LastChild.InnerText))
                                            {
                                                //log
                                            }
                                            else
                                            {
                                                caseResult = false;
                                                compareResult += "The setting \"Filter\" does not matched between source and destination.\n";
                                            }
                                        }
                                        else if (sourceFieldRefNodeList.Count == 2)
                                        {
                                            if (sourceLastChildNode.Name == desLastChildNode.Name)
                                            {
                                                int i = 0;
                                                foreach (XmlNode node1 in sourceLastChildNode.ChildNodes)
                                                {
                                                    if ((node1.Name == desLastChildNode.ChildNodes[i].Name) && (sourceFieldRefNodeList[i].Attributes["Name"].Value == desFieldRefNodeList[i].Attributes["Name"].Value)
                                                        && (node1.LastChild.InnerText == desLastChildNode.ChildNodes[i].LastChild.InnerText))
                                                    {
                                                        //log
                                                    }
                                                    else
                                                    {
                                                        caseResult = false;
                                                        compareResult += "The setting \"Filter\" does not matched between source and destination.Column level is " + (i + 1) + ".\n";
                                                        break;
                                                    }
                                                    i++;
                                                }
                                            }
                                        }
                                        else if (sourceFieldRefNodeList.Count == 3)
                                        {
                                            if ((sourceLastChildNode.Name == desLastChildNode.Name) && (sourceLastChildNode.FirstChild.Name == desLastChildNode.FirstChild.Name))
                                            {
                                                int i = 0;
                                                foreach (XmlNode node in sourceFieldRefNodeList)
                                                {
                                                    if ((node.Attributes["Name"].Value == desFieldRefNodeList[i].Attributes["Name"].Value)
                                                        && (node.ParentNode.Name == desFieldRefNodeList[i].ParentNode.Name)
                                                        && (node.NextSibling.InnerText == desFieldRefNodeList[i].NextSibling.InnerText))
                                                    {
                                                        //log
                                                    }
                                                    else
                                                    {
                                                        caseResult = false;
                                                        compareResult += "The setting \"Filter\" does not matched between source and destination.Column level is " + (i + 1) + ".\n";
                                                        break;
                                                    }
                                                    i++;
                                                }

                                            }
                                            else
                                            {
                                                caseResult = false;
                                                compareResult += "The setting \"Filter\" does not matched between source and destination.\n";
                                            }

                                        }
                                        else
                                        {
                                            compareResult += "The source Filter Column Count is great than 3, can not compare.\n";
                                        }
                                    }
                                    else
                                    {
                                        caseResult = false;
                                        compareResult += "The setting \"Filter\"→Column's count does not matched between source and destination.\n";
                                    }
                                }
                                else
                                {
                                    caseResult = false;
                                    compareResult += "The setting \"Filter\" does not config in the destination.\n";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occored while compare Filter setting between " + v1.Title + " and " + v2.Title + ".Error message: " + ex.Message + ".\n");
                        }
                        //=======================================================
                    }

                    else if (sourceNode.Name.Equals("ViewFields", StringComparison.InvariantCulture))        //Column Compare
                    {
                        XmlNode sourceViewFieldsNode = sourceViewEle.SelectSingleNode("/View/ViewFields");
                        XmlNode desViewFieldsNode = desViewEle.SelectSingleNode("/View/ViewFields");
                        bool viewFieldsKey = true;
                        foreach (XmlNode sourceFieldRef in sourceViewFieldsNode.ChildNodes)
                        {
                            XmlAttribute att = sourceFieldRef.Attributes["Name"];
                            //string f = sourceFieldRef.Attributes["Name"].Value;
                            string key = "/View/ViewFields/FieldRef[@";
                            key += att.Name + "='" + att.Value + "']";        // "/View/.../FieldRef[@Name='value']"
                            //Console.WriteLine(key);
                            if (desViewEle.SelectSingleNode(key) != null)
                            {
                                //log
                            }
                            else
                            {
                                viewFieldsKey = false;
                                caseResult = false;
                                compareResult += "One column in setting \"Column\" "
                                    + "is unchecked at the destination. Source Column named: " + att.Value + ".\n";
                            }
                        }
                        if ((desViewFieldsNode.ChildNodes.Count != sourceViewFieldsNode.ChildNodes.Count) && (viewFieldsKey))
                        {
                            caseResult = false;
                            compareResult += "The setting  \"Column\",destination's column is greater than source.\n";
                        }
                    }
                    else if (sourceNode.Name.Equals("RowLimit", StringComparison.InvariantCulture))       //  RowLimit compare
                    {
                        XmlNode sourceRowLimitNode = sourceViewEle.SelectSingleNode("/View/RowLimit");
                        XmlNode desRowLimitNode = desViewEle.SelectSingleNode("/View/RowLimit");
                        XmlAttribute sourcePageAtt = sourceRowLimitNode.Attributes["Paged"];
                        XmlAttribute desPageAtt = desRowLimitNode.Attributes["Paged"];
                        if ((sourcePageAtt != null) && (desPageAtt != null))
                        {
                            //log...
                        }
                        else
                        {
                            caseResult = false;
                            compareResult += "The setting \"Item Limit\""
                                + "is unchecked between source and destination.\n";
                        }
                        if (sourceRowLimitNode.InnerText.Equals(desRowLimitNode.InnerText))
                        {
                            //log
                        }
                        else
                        {
                            caseResult = false;
                            compareResult += "The setting \"Item Limit\"→\"Number of items to display\" "
                                + "does not match between source and destination.Source is " + sourceRowLimitNode.InnerText + ".Destination is " + desRowLimitNode.InnerText + ".\n";
                        }
                    }
                    else if (sourceNode.Name.Equals("Aggregations", StringComparison.InvariantCulture))   //Total compare
                    {
                        XmlNode sourceAggregationsNode = sourceNode;
                        XmlNode desAggregationsNode = desViewEle.SelectSingleNode("/View/Aggregations");
                        if (sourceAggregationsNode.Attributes["Value"].Value.Equals("On", StringComparison.InvariantCulture)
                            && (desAggregationsNode.Attributes["Value"].Value.Equals("On", StringComparison.InvariantCulture)))
                        {
                            foreach (XmlNode sourceFieldRef in sourceAggregationsNode.ChildNodes)
                            {
                                XmlAttribute att = sourceFieldRef.Attributes["Name"];
                                //string f = sourceFieldRef.Attributes["Name"].Value;
                                string key = "/View/Aggregations/FieldRef[@";
                                key += att.Name + "='" + att.Value + "']";
                                //Console.WriteLine(key);
                                if (desViewEle.SelectSingleNode(key) != null)
                                {
                                    if (sourceFieldRef.Attributes["Type"].Value == desViewEle.SelectSingleNode(key).Attributes["Type"].Value)
                                    {
                                        //log
                                    }
                                    else
                                    {
                                        caseResult = false;
                                        compareResult += "The setting \"Totals\"→\"Total\" does not matched between source and detination."
                                        + " Source Column named: " + att.Value + ".\n";
                                    }
                                }
                                else
                                {
                                    caseResult = false;
                                    compareResult += "The setting \"Totals\""
                                        + "is unchecked at destination. Source Column named: " + att.Value + ".\n";
                                }

                            }
                        }
                        else if (sourceAggregationsNode.Attributes["Value"].Value == desAggregationsNode.Attributes["Value"].Value)
                        {
                            //log  均为Off
                        }
                        else
                        {
                            caseResult = false;
                            compareResult += "The Setting \"Totals\" does not config in the source or destination.\n";
                        }
                    }
                    else if (sourceNode.Name.Equals("ViewStyle"))   //View Style compare
                    {
                        XmlNode desViewStyleNode = desViewEle.SelectSingleNode("/View/ViewStyle");
                        if (desViewStyleNode != null)
                        {
                            if (sourceNode.Attributes["ID"].Value == desViewStyleNode.Attributes["ID"].Value)
                            {
                                //log
                            }
                            else
                            {
                                caseResult = false;
                                compareResult += "The setting \"Stytle\" does not matched between source and destination.\n";
                            }
                        }
                        else
                        {
                            caseResult = false;
                            compareResult += "The setting \"Stytle\" does not matched between source and destination.\n";
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An error occored when try to compare Standard View: " + v1.Title + " and " + v2.Title + ".Error Message: " + e.Message);
            }

        }
        private void CompareCalendarView(View v1, View v2)             //compare for calendar view
        {
            try
            {
                compareResult += v1.Title + ": \n";
                var sourceViewDoc = new XmlDocument();
                var desViewDoc = new XmlDocument();
                sourceViewDoc.LoadXml(v1.HtmlSchemaXml);
                desViewDoc.LoadXml(v2.HtmlSchemaXml);
                var sourceViewEle = sourceViewDoc.DocumentElement;
                var desViewEle = desViewDoc.DocumentElement;
                if (v1.DefaultView == v2.DefaultView)             //  compare default view
                {
                    //log
                }
                else
                {
                    compareResult += "The setting Default View does not matched between source and destination.\n";
                }
                if (v1.MobileView == v2.MobileView)
                {
                    //log
                }
                else
                {
                    compareResult += "The setting \"Mobile\"→\"Enable this view for mobile access\"does not matched between source and destination.\n";
                }
                if (v1.MobileDefaultView == v2.MobileDefaultView)
                {
                    //log
                }
                else
                {
                    compareResult += "The setting \"Mobile\"→\"Make this view the default view for mobile access\"does not matched between source and destination.\n";
                }
                XmlNode sourceViewFieldsNode = sourceViewEle.SelectSingleNode("/View/ViewFields");
                XmlNode desViewFieldsNode = desViewEle.SelectSingleNode("/View/ViewFields");
                if (sourceViewFieldsNode.FirstChild.Attributes["Name"].Value == desViewFieldsNode.FirstChild.Attributes["Name"].Value)
                {
                    //log
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"Time Interval\"→\"Begin\"is not matched between source and destination. Source column named "
                        + sourceViewFieldsNode.FirstChild.Attributes["Name"].Value + "\n";
                }
                if (sourceViewFieldsNode.FirstChild.NextSibling.Attributes["Name"].Value == desViewFieldsNode.FirstChild.NextSibling.Attributes["Name"].Value)
                {
                    //log
                }
                else
                {
                    caseResult = false;
                    compareResult += "The setting \"Time Interval\"→\"End\"is not matched between source and destination. Source column named "
                        + sourceViewFieldsNode.FirstChild.NextSibling.Attributes["Name"].Value + ".\n";
                }
                try
                {
                    XmlNode sourceViewDataNode = sourceViewEle.SelectSingleNode("/View/ViewData");
                    XmlNode desViewDataNode = desViewEle.SelectSingleNode("/View/ViewData");
                    int i = 0;
                    foreach (XmlNode node in sourceViewDataNode.ChildNodes)
                    {
                        if (node.Attributes["Name"].Value == desViewDataNode.ChildNodes[i].Attributes["Name"].Value)
                        {
                            //log
                        }
                        else
                        {
                            caseResult = false;
                            string viewDataKey = "";
                            switch (node.Attributes["Type"].Value)
                            {
                                case "CalendarMonthTitle":
                                    viewDataKey = "Month View Title";
                                    break;
                                case "CalendarWeekTitle":
                                    viewDataKey = "Week View Title";
                                    break;
                                case "CalendarWeekLocation":
                                    viewDataKey = "Week View Sub Heading";
                                    break;
                                case "CalendarDayTitle":
                                    viewDataKey = "Day View Title";
                                    break;
                                case "CalendarDayLocation":
                                    viewDataKey = "Day View Sub Heading";
                                    break;
                            }
                            compareResult += "The setting \"Calendar Columns\"→\"" + viewDataKey + "\"is not" +
                            " matched between source and destination.\n";
                        }
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occored while compare /View/ViewData between Calendar View:" + v1.Title + " and " + v2.Title + ".Error Message:\n" + ex.Message);
                }
                try
                {
                    XmlNode sourceAndNode = sourceViewEle.SelectSingleNode("/View/Query/Where/And");          //  Filter 比对目前只支持最多三层Column，并且只有一项出错的时候就会返回大Setting不同，不会返回细致的比对结果
                    if (sourceAndNode != null)
                    {
                        if (desViewEle.SelectSingleNode("/View/Query/Where/And") != null)
                        {
                            XmlNode sourceLastChildNode = sourceAndNode.LastChild;
                            XmlNode desLastChildNode = desViewEle.SelectSingleNode("/View/Query/Where/And").LastChild;
                            XmlNodeList sourceFieldRefNodeList = sourceLastChildNode.SelectNodes("descendant::FieldRef");
                            XmlNodeList desFieldRefNodeList = desViewEle.SelectSingleNode("/View/Query/Where/And").LastChild.SelectNodes("descendant::FieldRef");
                            if (sourceFieldRefNodeList.Count == desFieldRefNodeList.Count)
                            {
                                if (sourceFieldRefNodeList.Count == 1)
                                {
                                    if ((sourceLastChildNode.Name == desLastChildNode.Name) && (sourceFieldRefNodeList[0].Attributes["Name"].Value == desFieldRefNodeList[0].Attributes["Name"].Value)
                                        && (sourceLastChildNode.LastChild.InnerText == desLastChildNode.LastChild.InnerText))
                                    {
                                        //log
                                    }
                                    else
                                    {
                                        caseResult = false;
                                        compareResult += "The setting \"Filter\" does not matched between source and destination.\n";
                                    }
                                }
                                else if (sourceFieldRefNodeList.Count == 2)
                                {
                                    if (sourceLastChildNode.Name == desLastChildNode.Name)
                                    {
                                        int i = 0;
                                        foreach (XmlNode node1 in sourceLastChildNode.ChildNodes)
                                        {
                                            if ((node1.Name == desLastChildNode.ChildNodes[i].Name) && (sourceFieldRefNodeList[i].Attributes["Name"].Value == desFieldRefNodeList[i].Attributes["Name"].Value)
                                                && (node1.LastChild.InnerText == desLastChildNode.ChildNodes[i].LastChild.InnerText))
                                            {
                                                //log
                                            }
                                            else
                                            {
                                                caseResult = false;
                                                compareResult += "The setting \"Filter\" does not matched between source and destination.Column level is " + (i + 1) + ".\n";
                                                break;
                                            }
                                            i++;
                                        }
                                    }
                                }
                                else if (sourceFieldRefNodeList.Count == 3)
                                {
                                    if ((sourceLastChildNode.Name == desLastChildNode.Name) && (sourceLastChildNode.FirstChild.Name == desLastChildNode.FirstChild.Name))
                                    {
                                        int i = 0;
                                        foreach (XmlNode node in sourceFieldRefNodeList)
                                        {
                                            if ((node.Attributes["Name"].Value == desFieldRefNodeList[i].Attributes["Name"].Value)
                                                && (node.ParentNode.Name == desFieldRefNodeList[i].ParentNode.Name)
                                                && (node.NextSibling.InnerText == desFieldRefNodeList[i].NextSibling.InnerText))
                                            {
                                                //log
                                            }
                                            else
                                            {
                                                caseResult = false;
                                                compareResult += "The setting \"Filter\" does not matched between source and destination.Column level is " + (i + 1) + ".\n";
                                                break;
                                            }
                                            i++;
                                        }

                                    }
                                    else
                                    {
                                        caseResult = false;
                                        compareResult += "The setting \"Filter\" does not matched between source and destination.\n";
                                    }

                                }
                                else
                                {
                                    compareResult += "The source Filter Column Count is great than 3, can not compare.\n";
                                }
                            }
                            else
                            {
                                caseResult = false;
                                compareResult += "The setting \"Filter\"→column's count does not matched between source and destination.\n";
                            }
                        }
                        else
                        {
                            caseResult = false;
                            compareResult += "The setting \"Filter\" does not config in the destination.\n";
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occored while compare Filter Setting between Calendar View:" + v1.Title + " and " + v2.Title + ".Error Message:\n" + e.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occored while compare Calendar View:" + v1.Title + " and " + v2.Title + ".Error Message:\n" + e.Message);
            }


        }

        private void CompareGanttView(View v1, View v2)                //Compare for gantt view
        {
            CompareStandardView(v1, v2);
            try
            {
                var sourceViewDoc = new XmlDocument();
                var desViewDoc = new XmlDocument();
                sourceViewDoc.LoadXml(v1.HtmlSchemaXml);
                desViewDoc.LoadXml(v2.HtmlSchemaXml);
                var sourceViewEle = sourceViewDoc.DocumentElement;
                var desViewEle = desViewDoc.DocumentElement;
                XmlNode sourceViewDataNode = sourceViewEle.SelectSingleNode("/View/ViewData");
                XmlNode desViewDataNode = desViewEle.SelectSingleNode("/View/ViewData");
                foreach (XmlNode sourceFieldRef in sourceViewEle.ChildNodes)
                {
                    XmlAttribute att = sourceFieldRef.Attributes["Type"];
                    string key = "/View/ViewData/FieldRef[@";
                    key += att.Name + "='" + att.Value + "']";        // "/View/.../FieldRef[@Name='value']"
                    //Console.WriteLine(key);
                    if (sourceFieldRef.Attributes["Name"].Value == desViewEle.SelectSingleNode(key).Attributes["Name"].Value)
                    {
                        //log
                    }
                    else
                    {
                        string resultKey = "";
                        caseResult = false;
                        switch (att.Value)
                        {
                            case "GanttStartDate":
                                resultKey = "Start Date";
                                break;
                            case "GanttEndDate":
                                resultKey = "Due Date";
                                break;
                            case "GanttTitle":
                                resultKey = "Title";
                                break;
                        }
                        compareResult += "The setting \"Gantt Columns\"→ \"" + resultKey + "\" "
                            + "does not matched between the source and destination.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occored while compare Gannt View " + v1.Title + " and " + v2.Title + ".Error Message: " + ex.Message + ".\n");
            }


        }

        private bool IsEqualOrHidden(View v1, View v2)       //判断两个View的Title是否相同，并且均不为Hidden View。
        {
            bool equalKey = false;
            if (v1.Title.Equals(v2.Title, StringComparison.InvariantCultureIgnoreCase))    //比对Title时忽略大小写
            {
                if (!(v1.Hidden) && !(v2.Hidden))
                {
                    equalKey = true;
                }
            }
            return equalKey;
        }
        private bool IsSameBaseType(View v1, View v2)      // 用来比对View1和View2的Base type是否相同
        {
            bool sameBaseKey = false;
            if (v1.ViewType.Equals(v2.ViewType, StringComparison.InvariantCultureIgnoreCase))
            {
                sameBaseKey = true;
            }
            else
            {
                // log
            }
            return sameBaseKey;
        }
    }
}
