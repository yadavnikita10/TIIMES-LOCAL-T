
using System.Drawing;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.IO;

namespace TuvVision.Controllers
{
    public class MRMPPTController : Controller
    {
        // GET: MRMPPT

        DALInspectionVisitReport objdal = new DALInspectionVisitReport();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult MRMReport()
        {
            return View();
        }


        public ActionResult GenerateMRMReport()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetDataMRM();
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string fileName = "MRM_Report.pptx";
                    string filePath = Server.MapPath("~/Reports/" + fileName);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        //public ActionResult GenerateReport()
        //{
        //    DataSet ds = objdal.GetDataMRM();
        //    DataTable dt = ds.Tables[1];

        //    if (dt == null || dt.Rows.Count == 0)
        //        return View();

        //    string fileName = "MRM_Report.pptx";
        //    string filePath = Server.MapPath("~/Reports/" + fileName);

        //    CreatePptWithTable(filePath, dt);

        //    return File(filePath,
        //        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        //        fileName);
        //}



        //private void CreatePptWithTable(string filePath, DataTable dt)
        //{

        //    using (PresentationDocument presentationDoc =
        //     PresentationDocument.Create(filePath, PresentationDocumentType.Presentation))
        //    {
        //        PresentationPart presentationPart = presentationDoc.AddPresentationPart();
        //        presentationPart.Presentation = new Presentation();

        //        SlidePart slidePart = presentationPart.AddNewPart<SlidePart>();
        //        slidePart.Slide = new Slide(new CommonSlideData(new ShapeTree()));

        //        ShapeTree shapeTree = slidePart.Slide.CommonSlideData.ShapeTree;

        //        // Required root
        //        shapeTree.Append(new NonVisualGroupShapeProperties(
        //            new NonVisualDrawingProperties() { Id = 1, Name = "Slide" },
        //            new NonVisualGroupShapeDrawingProperties(),
        //            new ApplicationNonVisualDrawingProperties()));

        //        shapeTree.Append(new GroupShapeProperties(new A.TransformGroup()));

        //        // Slide size
        //        long slideWidth = 9144000;
        //        long slideHeight = 6858000;

        //        long margin = 300000;
        //        long usableWidth = slideWidth - (2 * margin);
        //        long usableHeight = slideHeight - (2 * margin);

        //        int cols = dt.Columns.Count;
        //        int rows = dt.Rows.Count + 1;

        //        long colWidth = usableWidth / cols;
        //        long rowHeight = usableHeight / rows;

        //        int fontSize = rows > 15 ? 900 : 1100;

        //        // ---------- TABLE ----------
        //        A.Table table = new A.Table();

        //        // Grid
        //        A.TableGrid grid = new A.TableGrid();
        //        for (int i = 0; i < cols; i++)
        //            grid.Append(new A.GridColumn() { Width = colWidth });
        //        table.Append(grid);

        //        // Header
        //        A.TableRow headerRow = new A.TableRow() { Height = rowHeight };
        //        foreach (DataColumn col in dt.Columns)
        //            headerRow.Append(CreateCell(col.ColumnName, true, fontSize));
        //        table.Append(headerRow);

        //        // Data
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            A.TableRow row = new A.TableRow() { Height = rowHeight };
        //            foreach (var item in dr.ItemArray)
        //                row.Append(CreateCell(item?.ToString(), false, fontSize));
        //            table.Append(row);
        //        }

        //        // Graphic Frame
        //        GraphicFrame graphicFrame = new GraphicFrame(
        //            new NonVisualGraphicFrameProperties(
        //                new NonVisualDrawingProperties() { Id = 2, Name = "Table" },
        //                new NonVisualGraphicFrameDrawingProperties(),
        //                new ApplicationNonVisualDrawingProperties()),
        //            new Transform(
        //                new A.Offset() { X = margin, Y = margin },
        //                new A.Extents() { Cx = usableWidth, Cy = usableHeight }),
        //            new A.Graphic(
        //                new A.GraphicData(table)
        //                { Uri = "http://schemas.openxmlformats.org/drawingml/2006/table" })
        //        );

        //        shapeTree.Append(graphicFrame);

        //        presentationPart.Presentation.AppendChild(
        //            new SlideIdList(
        //                new SlideId()
        //                {
        //                    Id = 256,
        //                    RelationshipId = presentationPart.GetIdOfPart(slidePart)
        //                })
        //        );

        //        presentationPart.Presentation.Save();
        //    }
        //}

        //private A.TableCell CreateCell(string text, bool isHeader,int  fontSize)
        //{
        //    return new A.TableCell(
        //        new A.TextBody(
        //            new A.BodyProperties(),
        //            new A.ListStyle(),
        //            new A.Paragraph(
        //                new A.Run(
        //                    new A.RunProperties()
        //                    {
        //                        Bold = isHeader,
        //                        FontSize = 1200
        //                    },
        //                    new A.Text(text)
        //                )
        //            )
        //        ),
        //        new A.TableCellProperties()
        //    );
        //}
        public ActionResult GenerateExcelReport()
        {
            try
            {
                DataSet ds = objdal.GetDataMRM();
                if (ds.Tables.Count == 0)
                    return Content("No data found.");
                string fileName = "MRM_Report.xlsx";
                string filePath = Server.MapPath("~/Reports/" + fileName);               
                using (ExcelPackage package = new ExcelPackage())
                {
                    int sheetIndex = 1;
                    foreach (DataTable dt in ds.Tables)
                    {
                        if (dt.Rows.Count == 0)
                            continue; 
                        string sheetName = !string.IsNullOrEmpty(dt.TableName)
                            ? dt.TableName.Length > 31 ? dt.TableName.Substring(0, 31) : dt.TableName
                            : "Table" + sheetIndex;
                        var ws = package.Workbook.Worksheets.Add(sheetName);                        
                        ws.Cells["A1"].LoadFromDataTable(dt, true);
                        using (var range = ws.Cells[1, 1, 1, dt.Columns.Count])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189)); 
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }                        
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        ws.View.FreezePanes(2, 1);                        
                        int targetTatCol = -1;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Columns[i].ColumnName.Equals("Target TAT%", StringComparison.OrdinalIgnoreCase))
                            {
                                targetTatCol = i + 1; 
                                break;
                            }
                        }
                        if (targetTatCol > 0)
                        {
                            var tatRange = ws.Cells[2, targetTatCol, dt.Rows.Count + 1, targetTatCol];
                            var cond1 = tatRange.ConditionalFormatting.AddLessThan();
                            cond1.Formula = "90";
                            cond1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cond1.Style.Fill.BackgroundColor.Color = System.Drawing.Color.LightPink;
                            var cond2 = tatRange.ConditionalFormatting.AddGreaterThanOrEqual();
                            cond2.Formula = "90";
                            cond2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cond2.Style.Fill.BackgroundColor.Color = System.Drawing.Color.LightGreen;
                        }
                        var tblRange = ws.Cells[1, 1, dt.Rows.Count + 1, dt.Columns.Count];
                        var table = ws.Tables.Add(tblRange, "Table" + sheetIndex);
                        table.TableStyle = OfficeOpenXml.Table.TableStyles.Medium9;

                        sheetIndex++;
                    }
                    FileInfo fi = new FileInfo(filePath);
                    package.SaveAs(fi);
                }
                return File(filePath,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName);
            }
            catch (Exception ex)
            {
                return Content("Error generating report: " + ex.Message);
            }
        }

        //public ActionResult GenerateExcelReport()
        //{
        //    try
        //    {
        //        // Get data from database
        //        DataSet ds = objdal.GetDataMRM();
        //        DataTable dt = ds.Tables[1]; // take second table

        //        if (dt.Rows.Count == 0)
        //            return Content("No data found.");

        //        string fileName = "MRM_Report.xlsx";
        //        string filePath = Server.MapPath("~/Reports/" + fileName);



        //        using (ExcelPackage package = new ExcelPackage())
        //        {
        //            var ws = package.Workbook.Worksheets.Add("MRM Report");

        //            // Load DataTable starting from cell A1
        //            ws.Cells["A1"].LoadFromDataTable(dt, true);

        //            // Style header row
        //            using (var range = ws.Cells[1, 1, 1, dt.Columns.Count])
        //            {
        //                range.Style.Font.Bold = true;
        //                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                //range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189)); // Blue header
        //                //range.Style.Font.Color.SetColor(Color.White);
        //                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            }

        //            // Auto-fit columns
        //            ws.Cells[ws.Dimension.Address].AutoFitColumns();

        //            // Freeze header row
        //            ws.View.FreezePanes(2, 1);

        //            // Optional: Conditional formatting for "Target TAT%"
        //            int targetTatCol = -1;
        //            for (int i = 0; i < dt.Columns.Count; i++)
        //            {
        //                if (dt.Columns[i].ColumnName == "Target TAT%")
        //                {
        //                    targetTatCol = i + 1; // EPPlus is 1-based
        //                    break;
        //                }
        //            }

        //            if (targetTatCol > 0)
        //            {
        //                var tatRange = ws.Cells[2, targetTatCol, dt.Rows.Count + 1, targetTatCol];
        //                var cond1 = tatRange.ConditionalFormatting.AddLessThan();
        //                cond1.Formula = "90";
        //                cond1.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                //cond1.Style.Fill.BackgroundColor.Color = Color.LightPink;

        //                var cond2 = tatRange.ConditionalFormatting.AddGreaterThanOrEqual();
        //                cond2.Formula = "90";
        //                cond2.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                //cond2.Style.Fill.BackgroundColor.Color = Color.LightGreen;
        //            }

        //            // Optional: Table style
        //            var tblRange = ws.Cells[1, 1, dt.Rows.Count + 1, dt.Columns.Count];
        //            var table = ws.Tables.Add(tblRange, "MRMTable");
        //            table.TableStyle = OfficeOpenXml.Table.TableStyles.Medium9;

        //            // Save file
        //            FileInfo fi = new FileInfo(filePath);
        //            package.SaveAs(fi);
        //        }

        //        return File(filePath,
        //                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                    fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("Error generating report: " + ex.Message);
        //    }
        //}
    }

}
