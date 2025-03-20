using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.Extensions.Localization;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using System.Data;
using DocumentFormat.OpenXml.Office2019.Excel.ThreadedComments;
using DocumentFormat.OpenXml.Vml;
using System.Text;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
namespace RAS823_MC_CiudadMunicipal_FrontEnd.Services
{
    public interface IExcelService
    {
        MemoryStream GetExcelStreamOpenXML(DataTable products, out bool resultToNotify, List<Catalog> catalog = null,
            DataTable secondPageData = null, string secondPageName = "",
            DataTable thirdPageData = null, string thirdPageName = "");
        Task<DataTable> ImportExcelFile(InputFileChangeEventArgs e);
    }

    public class ExcelService : IExcelService
    {
        //private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        //public ExcelService(IStringLocalizer<SharedResource> localizerString)
        //{
        //    _stringLocalizer = localizerString;
        //}
        public ExcelService()
        {

        }

        public MemoryStream GetExcelStreamOpenXML(DataTable products, out bool resultToNotify, List<Catalog> catalog = null,
            DataTable secondPageData = null, string secondPageName = "",
            DataTable thirdPageData = null, string thirdPageName = "")
        {

            MemoryStream SpreadsheetStream = new MemoryStream();

            var movementsHeaderNames = products.Columns.Cast<DataColumn>().
                                          Select(column => column.ColumnName).ToList();
            // Get limit row
            //var limitRowsWorkSheet = GetMaxRowsExcel();
            var suppliesRowCount = products.Rows.Count;


            resultToNotify = false;
            // Create spreadSheet
            using (var spreadSheet = SpreadsheetDocument.Create(SpreadsheetStream, SpreadsheetDocumentType.Workbook))
            {
                // create the workbook
                var workbookPart = spreadSheet.AddWorkbookPart();

                // UseCustomStyle document
                var openXmlExportHelper = new OpenXmlWriterHelper();
                openXmlExportHelper.SaveCustomStylesheet(workbookPart);


                var workbook = workbookPart.Workbook = new Workbook();
                var sheets = workbook.AppendChild<Sheets>(new Sheets());

                // create worksheet 1
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();



                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Información a cargar" };
                sheets.Append(sheet);




                using (var writer = OpenXmlWriter.Create(worksheetPart))
                {

                    writer.WriteStartElement(new Worksheet());
                    //Start of Columns tag
                    writer.WriteStartElement(new Columns());
                    //Start of Column tag
                    writer.WriteStartElement(new Column() { Min = (UInt32)1, Max = (UInt32)movementsHeaderNames.Count, Width = 30, CustomWidth = true });
                    writer.WriteEndElement();  //Start of Column tag


                    writer.WriteEndElement(); //end of Columns tag
                    writer.WriteStartElement(new SheetData());

                    //Create header row
                    writer.WriteStartElement(new Row());
                    for (int i = 0; i < movementsHeaderNames.Count; i++)
                    {
                        //header formatting attribute.  This will create a <c> element with s=2 as its attribute
                        //s stands for styleindex
                        var attributes = new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "3") }.ToList();
                        openXmlExportHelper.WriteCellValueSax(writer, movementsHeaderNames[i], CellValues.SharedString, attributes);

                    }
                    writer.WriteEndElement(); //end of Row tag



                    var finalItem = movementsHeaderNames.Last();
                    var columnsCount = 1;
                    var currentRow = 2;
                    bool IsEven = true;
                    for (int i = 0; i < suppliesRowCount; i++)
                    {

                        currentRow++;
                        columnsCount = 1;
                        writer.WriteStartElement(new Row());
                        foreach (var item in movementsHeaderNames)
                        {
                            //Random rnd = new Random();
                            //int month = rnd.Next(1, 13);
                            List<OpenXmlAttribute> attributes;
                            if (item.Contains(finalItem))
                                attributes = IsEven ? new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "6") }.ToList() : new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "7") }.ToList();
                            else
                                attributes = IsEven ? new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "4") }.ToList() : new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "5") }.ToList();

                            if (item.Contains("Día"))
                            {
                                try
                                {
                                    var timeSpaned = (DateTime)(products.Rows[i][item]);
                                    var timeSpanedFormat = new DateTime(timeSpaned.Year, timeSpaned.Month, timeSpaned.Day).ToString("yyyy-MM-dd");
                                    openXmlExportHelper.WriteCellValueSax(writer, timeSpanedFormat, CellValues.SharedString, attributes);
                                }
                                catch
                                {
                                    var result = products.Rows[i][item].ToString();
                                    openXmlExportHelper.WriteCellValueSax(writer, result, CellValues.SharedString, attributes);
                                }
                            }
                            else
                            {
                                var result = products.Rows[i][item];
                                WriteInCell(openXmlExportHelper, writer, attributes, result, catalog);
                            }
                            columnsCount++;

                        }
                        IsEven = !IsEven;
                        writer.WriteEndElement(); //end of Row tag
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();



                    writer.WriteEndElement(); //end of SheetData
                    writer.WriteEndElement(); //end of worksheet
                    writer.Close();
                }





                if (secondPageData != null)
                {
                    // create worksheet 2
                    var worksheetPart2 = workbookPart.AddNewPart<WorksheetPart>();
                    var sheet2 = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart2), SheetId = 2, Name = secondPageName };
                    sheets.Append(sheet2);

                    var totalsHeaderNames = secondPageData.Columns.Cast<DataColumn>().
                                         Select(column => column.ColumnName).ToList();
                    using (var writer = OpenXmlWriter.Create(worksheetPart2))
                    {

                        writer.WriteStartElement(new Worksheet());
                        //Start of Columns tag
                        writer.WriteStartElement(new Columns());

                        //Start of Column tag
                        writer.WriteStartElement(new Column() { Min = (UInt32)1, Max = (UInt32)totalsHeaderNames.Count, Width = 30, CustomWidth = true });
                        writer.WriteEndElement();  //Start of Column tag


                        writer.WriteEndElement(); //end of Columns tag
                        writer.WriteStartElement(new SheetData());

                        //Create header row
                        writer.WriteStartElement(new Row());
                        for (int i = 0; i < totalsHeaderNames.Count; i++)
                        {
                            //header formatting attribute.  This will create a <c> element with s=2 as its attribute
                            //s stands for styleindex

                            var attributes = new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "3") }.ToList();
                            openXmlExportHelper.WriteCellValueSax(writer, totalsHeaderNames[i], CellValues.SharedString, attributes);

                        }
                        writer.WriteEndElement(); //end of Row tag


                        var finalItem = totalsHeaderNames.Last();
                        var columnsCount = 1;
                        var currentRow = 2;
                        bool IsEven = true;
                        for (int i = 0; i < secondPageData.Rows.Count; i++)
                        {

                            currentRow++;
                            columnsCount = 1;
                            writer.WriteStartElement(new Row());
                            foreach (var item in totalsHeaderNames)
                            {
                                List<OpenXmlAttribute> attributes;
                                if (item.Contains(finalItem))
                                    attributes = IsEven ? new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "6") }.ToList() : new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "7") }.ToList();
                                else
                                    attributes = IsEven ? new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "4") }.ToList() : new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "5") }.ToList();

                                var result = secondPageData.Rows[i][item];
                                WriteInCell(openXmlExportHelper, writer, attributes, result);

                                columnsCount++;

                            }
                            IsEven = !IsEven;
                            writer.WriteEndElement(); //end of Row tag
                        }


                        GC.Collect();
                        GC.WaitForPendingFinalizers();


                        writer.WriteEndElement(); //end of SheetData
                        writer.WriteEndElement(); //end of worksheet
                        writer.Close();
                    }
                }



                if (thirdPageData != null)
                {
                    // create worksheet 2
                    var worksheetPart3 = workbookPart.AddNewPart<WorksheetPart>();
                    var sheet3 = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart3), SheetId = 3, Name = thirdPageName };
                    sheets.Append(sheet3);

                    var totalsHeaderNames = thirdPageData.Columns.Cast<DataColumn>().
                                         Select(column => column.ColumnName).ToList();
                    using (var writer = OpenXmlWriter.Create(worksheetPart3))
                    {

                        writer.WriteStartElement(new Worksheet());
                        //Start of Columns tag
                        writer.WriteStartElement(new Columns());

                        //Start of Column tag
                        writer.WriteStartElement(new Column() { Min = (UInt32)1, Max = (UInt32)totalsHeaderNames.Count, Width = 30, CustomWidth = true });
                        writer.WriteEndElement();  //Start of Column tag


                        writer.WriteEndElement(); //end of Columns tag
                        writer.WriteStartElement(new SheetData());

                        //Create header row
                        writer.WriteStartElement(new Row());
                        for (int i = 0; i < totalsHeaderNames.Count; i++)
                        {
                            //header formatting attribute.  This will create a <c> element with s=2 as its attribute
                            //s stands for styleindex

                            var attributes = new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "3") }.ToList();
                            openXmlExportHelper.WriteCellValueSax(writer, totalsHeaderNames[i], CellValues.SharedString, attributes);

                        }
                        writer.WriteEndElement(); //end of Row tag


                        var finalItem = totalsHeaderNames.Last();
                        var columnsCount = 1;
                        var currentRow = 2;
                        bool IsEven = true;
                        for (int i = 0; i < thirdPageData.Rows.Count; i++)
                        {

                            currentRow++;
                            columnsCount = 1;
                            writer.WriteStartElement(new Row());
                            foreach (var item in totalsHeaderNames)
                            {
                                List<OpenXmlAttribute> attributes;
                                if (item.Contains(finalItem))
                                    attributes = IsEven ? new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "6") }.ToList() : new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "7") }.ToList();
                                else
                                    attributes = IsEven ? new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "4") }.ToList() : new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "5") }.ToList();

                                var result = thirdPageData.Rows[i][item];
                                WriteInCell(openXmlExportHelper, writer, attributes, result);

                                columnsCount++;

                            }
                            IsEven = !IsEven;
                            writer.WriteEndElement(); //end of Row tag
                        }


                        GC.Collect();
                        GC.WaitForPendingFinalizers();


                        writer.WriteEndElement(); //end of SheetData
                        writer.WriteEndElement(); //end of worksheet
                        writer.Close();
                    }
                }


                //create the share string part using sax like approach too
                openXmlExportHelper.CreateShareStringPart(workbookPart);
            }
            //return SpreadsheetStream.ToArray();
            return SpreadsheetStream;


        }
        private static void WriteInCell(OpenXmlWriterHelper openXmlExportHelper, OpenXmlWriter writer, List<OpenXmlAttribute> attributes, object result, List<Catalog> catalog = null)
        {
            switch (result)
            {
                case bool boolType:
                    openXmlExportHelper.WriteCellValueSax(writer, result.ToString(), CellValues.Boolean, attributes);
                    break;
                case string stringType:
                    bool isNumeric = result.ToString().All(char.IsDigit);
                    if (isNumeric)
                    {
                        openXmlExportHelper.WriteCellValueSax(writer, result.ToString().Replace(',', '.'), CellValues.Number, attributes);
                    }
                    else
                    {
                        var resultString = result.ToString();

                        if (catalog != null)
                        {
                            var catalogValue = catalog.FirstOrDefault(x => x.Code == stringType);
                            if (catalogValue != null)
                            {
                                resultString = catalogValue.DisplayLabel;
                            }
                        }

                        openXmlExportHelper.WriteCellValueSax(writer, resultString, CellValues.SharedString, attributes);
                    }

                    break;
                default:
                    openXmlExportHelper.WriteCellValueSax(writer, result.ToString().Replace(',', '.'), CellValues.Number, attributes);
                    break;
            }
        }

        public async Task<DataTable> ImportExcelFile(InputFileChangeEventArgs e)
        {
            DataTable dataTable = new DataTable();
            var fileStream = e.File.OpenReadStream();
            var ms = new MemoryStream();
            await fileStream.CopyToAsync(ms);
            fileStream.Close();
            ms.Position = 0;

            ISheet sheet;
            var xsswb = new XSSFWorkbook(ms);

            sheet = xsswb.GetSheetAt(0);
            IRow hr = sheet.GetRow(0);
            var rl = new List<string>();

            int cc = hr.LastCellNum;
            for (var j = 0; j < cc; j++)
            {
                ICell cell = hr.GetCell(j);
                dataTable.Columns.Add(cell.ToString());
            }

            for (var j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
            {
                var r = sheet.GetRow(j);
                for (var i = r.FirstCellNum; i < cc; i++)
                {
                    var cell = r.GetCell(i);

                    if(cell != null)
                    {
                        if (cell.DateOnlyCellValue.HasValue)
                        {
                            var timeParse = cell.DateOnlyCellValue.Value;
                            rl.Add(timeParse.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            rl.Add(cell.ToString());
                        }

                  
                    }
                    else
                    {
                        rl.Add("");
                    }
                }
                if (rl.Count > 0)
                {
                    dataTable.Rows.Add(rl.ToArray());
                }
                rl.Clear();
            }
            return dataTable;
        }
        //Fin de funcion para administrar excel

    }
}
