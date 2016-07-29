using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace CompareDataTool
{
    public class Reporter
    {
        private HSSFWorkbook hssfworkbook;
        private ISheet s1;

        public void WriteToFile()
        {
            //Write the stream data of workbook to the directory
            string PathFile = @"test.xls";
            if (!File.Exists(PathFile))
            {
                FileStream file = new FileStream(@"test.xls", FileMode.Create);
                hssfworkbook.Write(file);
                file.Close();
            }
            else
            {
                FileStream file = new FileStream(@"test.xls", FileMode.Open);
                hssfworkbook.Write(file);
                file.Close();
            }
        }

        public void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();

            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "DAO Team";
            hssfworkbook.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "DAO Test Result";
            hssfworkbook.SummaryInformation = si;
        }
        //创建xls文件
        public void NewXsl()
        {
            InitializeWorkbook();

            //here, we must insert at least one sheet to the workbook. otherwise, Excel will say 'data lost in file'
            //So we insert three sheet just like what Excel does
            s1 = hssfworkbook.CreateSheet("TestResult");
            IRow row = s1.CreateRow(0);
            row.CreateCell(0).SetCellValue("TestResult");
            row.CreateCell(1).SetCellValue("Details");

            //Write the stream data of workbook to the root directory
            WriteToFile();
        }

        public void updateXsl(string caseID, string TestResult)
        {
            FileStream file = new FileStream(@"test.xls", FileMode.Open);
            hssfworkbook = new HSSFWorkbook(file);
            s1 = hssfworkbook.GetSheetAt(0);
            int rowid = s1.LastRowNum;
            IRow row = s1.CreateRow(rowid + 1);
            row.CreateCell(0).SetCellValue(caseID);
            row.CreateCell(1).SetCellValue(TestResult);

            WriteToFile();
        }

        public void updateXslResult(string result)
        {
            FileStream file = new FileStream(@"test.xls", FileMode.Open);
            hssfworkbook = new HSSFWorkbook(file);
            s1 = hssfworkbook.GetSheetAt(0);
            int rowid = s1.LastRowNum;           
            IRow row = s1.GetRow(rowid);
            row.CreateCell(0).SetCellValue(result);
            WriteToFile();
        }

        public void updateXslDetails(string details)
        {
            FileStream file = new FileStream(@"test.xls", FileMode.Open);
            hssfworkbook = new HSSFWorkbook(file);
            s1 = hssfworkbook.GetSheetAt(0);
            int rowid = s1.LastRowNum;
            IRow row = s1.CreateRow(rowid + 1);
            row.CreateCell(1).SetCellValue(details);
            WriteToFile();
        }
    }
}
