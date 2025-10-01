using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;
using System.Globalization;
using System.Configuration;
using TuvVision.Models;
using TuvVision.DataAccessLayer;

using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using System.Text;

namespace TuvVision.Controllers
{
    public class TestImageController : Controller
    {
        // GET: TestImage
        // TestImage/UpdateReports_2
        InspectionvisitReportModel ObjModelVisitReport = new InspectionvisitReportModel();
        ReportModel RMData = new ReportModel();
        DALInspectionVisitReport objDalVisitReport = new DALInspectionVisitReport();
        DataTable ReportDashBoard = new DataTable();
        DataSet UpdateReport = new DataSet();
        DataSet dtSrNo = new DataSet();
        string SrNo = "";
        int intSrNo = 0;
        ReportModel RM = new ReportModel();
        int count = 0;


        #region BAK
        public ActionResult UpdateReports(int? PK_CALL_ID, string flag)
        {
            RMData.PK_CALL_ID = 1393361;//Convert.ToInt32(PK_CALL_ID);
            PK_CALL_ID = 1393361;
            DataTable ImageReportDashBoard = new DataTable();

            try
            {
                if (PK_CALL_ID != 0 && PK_CALL_ID != null)
                {
                    ImageReportDashBoard = objDalVisitReport.GetReportImageByCall_Id(PK_CALL_ID);

                    if (ImageReportDashBoard.Rows.Count > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // Create PDF document
                            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
                            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                            doc.Open();

                            int imageCount = ImageReportDashBoard.Rows.Count;

                            for (int i = 0; i < imageCount; i++)
                            {
                                PdfPTable table = new PdfPTable(2);  // 2 images per row
                                table.WidthPercentage = 100;

                                // First image
                                string imageUrl = BuildImagePath(ImageReportDashBoard.Rows[i]);
                                iTextSharp.text.Image img1 = GetScaledImage(imageUrl);
                                PdfPCell cell1 = new PdfPCell(img1, true);
                                cell1.Border = Rectangle.BOX;
                                cell1.Padding = 5;
                                table.AddCell(cell1);

                                // Second image (if exists)
                                if (i + 1 < imageCount)
                                {
                                    string imageUrl2 = BuildImagePath(ImageReportDashBoard.Rows[i + 1]);
                                    iTextSharp.text.Image img2 = GetScaledImage(imageUrl2);
                                    PdfPCell cell2 = new PdfPCell(img2, true);
                                    cell2.Border = Rectangle.BOX;
                                    cell2.Padding = 5;
                                    table.AddCell(cell2);
                                    i++; // Skip next because already added
                                }
                                else
                                {
                                    // Empty cell if odd number of images
                                    table.AddCell(new PdfPCell(new Phrase("")));
                                }

                                doc.Add(table);
                                doc.NewPage(); // Each row of 2 images → new page (optional)
                            }

                            doc.Close();

                            return File(ms.ToArray(), "application/pdf", "Report.pdf");
                        }
                    }
                    else
                    {
                        return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = "No images found" });
                    }
                }
                else
                {
                    return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = "Invalid Call ID" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = ex.Message });
            }
        }

        /// <summary>
        /// Build full URL for image
        /// </summary>
        private string BuildImagePath(DataRow row)
        {
            DateTime CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
            int year = CreatedDate.Year;
            string currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(CreatedDate.Month);
            string imagePath = $"/Content/Uploads/Images/{year}/{currentMonth}";
            return ConfigurationManager.AppSettings["Web"].ToString() + "/" +
                   imagePath + "/" + row["Image"].ToString();
        }

        /// <summary>
        /// Load image & scale to fit in cell
        /// </summary>
        private iTextSharp.text.Image GetScaledImage(string imageUrl)
        {
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(new Uri(imageUrl));
            img.ScaleToFit(250f, 200f); // fit in cell
            img.Alignment = Element.ALIGN_CENTER;
            return img;
        }

        public ActionResult UpdateReports_1()
        {
            int? PK_CALL_ID = 1393361;
            RMData.PK_CALL_ID = Convert.ToInt32(PK_CALL_ID);
            DataTable ImageReportDashBoard = new DataTable();

            try
            {
                if (PK_CALL_ID != 0 && PK_CALL_ID != null)
                {
                    ImageReportDashBoard = objDalVisitReport.GetReportImageByCall_Id(PK_CALL_ID);

                    if (ImageReportDashBoard.Rows.Count > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // Create PDF document
                            Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                            PdfWriter.GetInstance(pdfDoc, ms);
                            pdfDoc.Open();

                            PdfPTable table = null;
                            int imageCounter = 0;

                            for (int i = 0; i < ImageReportDashBoard.Rows.Count; i++)
                            {
                                if (imageCounter % 6 == 0) // New page after 6 images
                                {
                                    if (table != null)
                                    {
                                        pdfDoc.Add(table);
                                        pdfDoc.NewPage();
                                    }

                                    // 2 columns per row, total 3 rows → 6 images
                                    table = new PdfPTable(2);
                                    table.WidthPercentage = 100;
                                }

                                DateTime CreatedDate = Convert.ToDateTime(ImageReportDashBoard.Rows[i]["CreatedDate"]);
                                int year = CreatedDate.Year;
                                string currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(CreatedDate.Month);
                                string imagePath = $"/Content/Uploads/Images/{year}/{currentMonth}";
                                string imageUrl = ConfigurationManager.AppSettings["Web"].ToString() + "/" +
                                                  imagePath + "/" + ImageReportDashBoard.Rows[i]["Image"].ToString();

                                try
                                {
                                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(new Uri(imageUrl));

                                    // Scale to fit nicely
                                    img.ScaleToFit(250f, 180f);
                                    img.Alignment = Element.ALIGN_CENTER;

                                    PdfPCell cell = new PdfPCell(img, true);
                                    cell.Border = Rectangle.BOX;
                                    cell.Padding = 5;
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                                    table.AddCell(cell);
                                }
                                catch (Exception)
                                {
                                    // Handle missing/invalid image
                                    PdfPCell errorCell = new PdfPCell(new Phrase("Image not available"));
                                    errorCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    errorCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    table.AddCell(errorCell);
                                }

                                imageCounter++;
                            }

                            // Add last table if exists
                            if (table != null)
                                pdfDoc.Add(table);

                            pdfDoc.Close();

                            return File(ms.ToArray(), "application/pdf", "Report.pdf");
                        }
                    }
                    else
                    {
                        return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = "No images found" });
                    }
                }
                else
                {
                    return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = "Invalid Call ID" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = ex.Message });
            }
        }
        #endregion


        #region BAK Working
        //public ActionResult GenerateReport()
        //{
        //    int? PK_CALL_ID = 1393361;
        //    RMData.PK_CALL_ID = Convert.ToInt32(PK_CALL_ID);

        //    DataTable ImageReportDashBoard = objDalVisitReport.GetReportImageByCall_Id(PK_CALL_ID);
        //    List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();

        //    if (ImageReportDashBoard.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in ImageReportDashBoard.Rows)
        //        {
        //            DateTime CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
        //            int year = CreatedDate.Year;
        //            string currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(CreatedDate.Month);

        //            ImageDashBoard.Add(new ReportImageModel
        //            {
        //                Path = $"/Content/Uploads/Images/{year}/{currentMonth}",
        //                Image = Convert.ToString(dr["Image"]),
        //                Heading = Convert.ToString(dr["Heading"])
        //            });
        //        }
        //    }

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        Document doc = new Document(PageSize.A4, 20f, 20f, 70f, 30f); // extra top margin for header
        //        PdfWriter writer = PdfWriter.GetInstance(doc, ms);

        //        // Attach header/footer event
        //        string webRoot = ConfigurationManager.AppSettings["Web"].ToString();
        //        //string relativePath = "/AllJsAndCss/images/logo.svg";
        //        string relativePath = "/AllJsAndCss/images/logo.png";
        //        string logoPath = Server.MapPath(relativePath);

        //        //string logoPath = Server.MapPath("~/" + ConfigurationManager.AppSettings["Web"].Trim('/') + "/AllJsAndCss/images/logo.svg");
        //        string reportNo = "Report No: " + PK_CALL_ID; // Example
        //        writer.PageEvent = new PdfHeaderFooter(logoPath, reportNo);

        //        doc.Open();

        //        PdfPTable table = new PdfPTable(2);
        //        table.WidthPercentage = 100;
        //        int imgCount = 0;

        //        foreach (var item in ImageDashBoard)
        //        {
        //            string fullImagePath = Server.MapPath(item.Path + "/" + item.Image);

        //            if (System.IO.File.Exists(fullImagePath))
        //            {
        //                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(fullImagePath);
        //                img.ScaleAbsolute(250f, 180f);
        //                img.Alignment = Element.ALIGN_CENTER;

        //                PdfPCell headingCell = new PdfPCell(new Phrase(item.Heading, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)))
        //                {
        //                    Border = Rectangle.BOX,
        //                    HorizontalAlignment = Element.ALIGN_CENTER,
        //                    VerticalAlignment = Element.ALIGN_MIDDLE,
        //                    Padding = 4,
        //                    BackgroundColor = new BaseColor(220, 220, 220)
        //                };

        //                PdfPCell imgCell = new PdfPCell(img, true)
        //                {
        //                    Border = Rectangle.BOX,
        //                    HorizontalAlignment = Element.ALIGN_CENTER,
        //                    VerticalAlignment = Element.ALIGN_MIDDLE,
        //                    Padding = 5,
        //                    FixedHeight = 190f
        //                };

        //                PdfPTable nested = new PdfPTable(1);
        //                nested.WidthPercentage = 100;
        //                nested.AddCell(headingCell);
        //                nested.AddCell(imgCell);

        //                PdfPCell outerCell = new PdfPCell(nested)
        //                {
        //                    Border = Rectangle.BOX,
        //                    Padding = 5,
        //                    HorizontalAlignment = Element.ALIGN_CENTER,
        //                    VerticalAlignment = Element.ALIGN_MIDDLE
        //                };

        //                table.AddCell(outerCell);
        //                imgCount++;
        //            }

        //            if (imgCount % 6 == 0)
        //            {
        //                doc.Add(table);
        //                doc.NewPage();
        //                table = new PdfPTable(2);
        //                table.WidthPercentage = 100;
        //            }
        //        }

        //        if (imgCount % 6 != 0)
        //        {
        //            while (imgCount % 2 != 0)
        //            {
        //                table.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.BOX });
        //                imgCount++;
        //            }
        //            doc.Add(table);
        //        }

        //        doc.Close();
        //        byte[] fileBytes = ms.ToArray();
        //        return File(fileBytes, "application/pdf", "ImagesWithCaptions.pdf");
        //    }
        //}

        #region Header Footer

        //public class PdfHeaderFooterEvent : PdfPageEventHelper
        //{
        //    private string _headerHtml;
        //    private string _footerHtml;

        //    public PdfHeaderFooterEvent(string headerHtml, string footerHtml)
        //    {
        //        _headerHtml = headerHtml;
        //        _footerHtml = footerHtml;
        //    }

        //    public override void OnEndPage(PdfWriter writer, Document document)
        //    {
        //        // HEADER
        //        if (!string.IsNullOrEmpty(_headerHtml))
        //        {
        //            using (StringReader sr = new StringReader(_headerHtml))
        //            {
        //                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
        //            }
        //        }

        //        // FOOTER
        //        if (!string.IsNullOrEmpty(_footerHtml))
        //        {
        //            using (StringReader sr = new StringReader(_footerHtml))
        //            {
        //                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
        //            }
        //        }
        //    }
        //}

        #endregion

        //public ActionResult UpdateReports_2()
        //{
        //    int? PK_CALL_ID = 1393361;
        //    RMData.PK_CALL_ID = Convert.ToInt32(PK_CALL_ID);
        //    DataTable ImageReportDashBoard = new DataTable();
        //    List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();

        //    try
        //    {
        //        if (PK_CALL_ID != 0 && PK_CALL_ID != null)
        //        {
        //            string Imagepath = "";
        //            ImageReportDashBoard = objDalVisitReport.GetReportImageByCall_Id(PK_CALL_ID);

        //            if (ImageReportDashBoard.Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in ImageReportDashBoard.Rows)
        //                {
        //                    DateTime CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
        //                    int year = CreatedDate.Year;
        //                    int month = CreatedDate.Month;
        //                    string currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);

        //                    Imagepath = $"/Content/Uploads/Images/{year}/{currentMonth}";

        //                    ImageDashBoard.Add(
        //                        new ReportImageModel
        //                        {
        //                            Path = Convert.ToString(Imagepath),
        //                            Image = Convert.ToString(dr["Image"]),
        //                            Heading = Convert.ToString(dr["Heading"]),
        //                        }
        //                    );
        //                }

        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    // Create PDF document
        //                    Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
        //                    PdfWriter.GetInstance(pdfDoc, ms);
        //                    pdfDoc.Open();

        //                    // Add TUV India header
        //                    Font headerFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
        //                    Font subHeaderFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
        //                    Font normalFont = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL);

        //                    // Main header
        //                    Paragraph header = new Paragraph("TUV INDIA", headerFont);
        //                    header.Alignment = Element.ALIGN_CENTER;
        //                    pdfDoc.Add(header);

        //                    // Sub header
        //                    Paragraph subHeader = new Paragraph("TUV INDIA PRIVATE LIMITED", subHeaderFont);
        //                    subHeader.Alignment = Element.ALIGN_CENTER;
        //                    pdfDoc.Add(subHeader);

        //                    // Report title
        //                    Paragraph reportTitle = new Paragraph("INSPECTION VISIT REPORT", subHeaderFont);
        //                    reportTitle.Alignment = Element.ALIGN_CENTER;
        //                    reportTitle.SpacingAfter = 10f;
        //                    pdfDoc.Add(reportTitle);

        //                    // Report number
        //                    Paragraph reportNumber = new Paragraph("IVR-010101530324/5-A10-Rev.0", normalFont);
        //                    reportNumber.Alignment = Element.ALIGN_CENTER;
        //                    reportNumber.SpacingAfter = 20f;
        //                    pdfDoc.Add(reportNumber);

        //                    // Add divider line
        //                    Paragraph divider = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));
        //                    divider.SpacingAfter = 20f;
        //                    pdfDoc.Add(divider);

        //                    // Add Inspection Pictures section
        //                    Paragraph picturesHeader = new Paragraph("Inspection Pictures:", subHeaderFont);
        //                    picturesHeader.SpacingAfter = 15f;
        //                    pdfDoc.Add(picturesHeader);

        //                    // Create table for images (2 columns)
        //                    PdfPTable table = new PdfPTable(2);
        //                    table.WidthPercentage = 100;
        //                    table.SpacingAfter = 15f;

        //                    foreach (var imageModel in ImageDashBoard)
        //                    {
        //                        try
        //                        {
        //                            string imageUrl = ConfigurationManager.AppSettings["Web"].ToString() +
        //                                            imageModel.Path + "/" + imageModel.Image;

        //                            // Create container cell
        //                            PdfPCell containerCell = new PdfPCell();
        //                            containerCell.Border = Rectangle.BOX;
        //                            containerCell.Padding = 5;
        //                            containerCell.HorizontalAlignment = Element.ALIGN_CENTER;

        //                            // Add image
        //                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(new Uri(imageUrl));
        //                            img.ScaleToFit(250f, 180f);
        //                            img.Alignment = Element.ALIGN_CENTER;

        //                            // Create paragraph for image
        //                            Paragraph imageParagraph = new Paragraph();
        //                            imageParagraph.Add(img);
        //                            imageParagraph.Alignment = Element.ALIGN_CENTER;
        //                            containerCell.AddElement(imageParagraph);

        //                            // Add image heading
        //                            Paragraph headingParagraph = new Paragraph(imageModel.Heading, normalFont);
        //                            headingParagraph.Alignment = Element.ALIGN_CENTER;
        //                            headingParagraph.SpacingBefore = 5f;
        //                            containerCell.AddElement(headingParagraph);

        //                            table.AddCell(containerCell);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            // Handle missing/invalid image
        //                            PdfPCell errorCell = new PdfPCell(new Phrase("Image not available", normalFont));
        //                            errorCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            errorCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                            table.AddCell(errorCell);
        //                        }

        //                        // Add new page after every 4 images (2 rows)
        //                        if ((ImageDashBoard.IndexOf(imageModel) + 1) % 4 == 0 &&
        //                            (ImageDashBoard.IndexOf(imageModel) + 1) < ImageDashBoard.Count)
        //                        {
        //                            pdfDoc.Add(table);
        //                            pdfDoc.NewPage();
        //                            table = new PdfPTable(2);
        //                            table.WidthPercentage = 100;
        //                            table.SpacingAfter = 15f;
        //                        }
        //                    }

        //                    // Add the last table if it has content
        //                    if (table.Rows.Count > 0)
        //                    {
        //                        pdfDoc.Add(table);
        //                    }

        //                    pdfDoc.Close();

        //                    return File(ms.ToArray(), "application/pdf", "Inspection_Visit_Report.pdf");
        //                }
        //            }
        //            else
        //            {
        //                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = "No images found" });
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = "Invalid Call ID" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = ex.Message });
        //    }
        //}

        //public ActionResult GenerateReport_()
        //{
        //    int? PK_CALL_ID = 1393361;
        //    RMData.PK_CALL_ID = Convert.ToInt32(PK_CALL_ID);

        //    DataTable ImageReportDashBoard = objDalVisitReport.GetReportImageByCall_Id(PK_CALL_ID);
        //    List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();

        //    if (ImageReportDashBoard.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in ImageReportDashBoard.Rows)
        //        {
        //            DateTime CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
        //            int year = CreatedDate.Year;
        //            string currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(CreatedDate.Month);

        //            ImageDashBoard.Add(new ReportImageModel
        //            {
        //                Path = $"/Content/Uploads/Images/{year}/{currentMonth}",
        //                Image = Convert.ToString(dr["Image"]),
        //                Heading = Convert.ToString(dr["Heading"])
        //            });
        //        }
        //    }

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        Document doc = new Document(PageSize.A4, 20f, 20f, 70f, 30f); // extra top margin for header
        //        PdfWriter writer = PdfWriter.GetInstance(doc, ms);

        //        // Attach header/footer event
        //        string webRoot = ConfigurationManager.AppSettings["Web"].ToString();
        //        //string relativePath = "/AllJsAndCss/images/logo.svg";
        //        string relativePath = "/AllJsAndCss/images/logo.png";
        //        string logoPath = Server.MapPath(relativePath);

        //        //string logoPath = Server.MapPath("~/" + ConfigurationManager.AppSettings["Web"].Trim('/') + "/AllJsAndCss/images/logo.svg");
        //        string reportNo = "Report No: " + PK_CALL_ID; // Example
        //        string footerLogoPath = Server.MapPath("/AllJsAndCss/images/footerlogo.png");
        //        writer.PageEvent = new PdfHeaderFooter(logoPath, reportNo, footerLogoPath);


        //        doc.Open();

        //        PdfPTable table = new PdfPTable(2);
        //        table.WidthPercentage = 100;
        //        int imgCount = 0;

        //        foreach (var item in ImageDashBoard)
        //        {
        //            string fullImagePath = Server.MapPath(item.Path + "/" + item.Image);

        //            if (System.IO.File.Exists(fullImagePath))
        //            {
        //                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(fullImagePath);
        //                img.ScaleAbsolute(250f, 180f);
        //                img.Alignment = Element.ALIGN_CENTER;

        //                PdfPCell headingCell = new PdfPCell(new Phrase(item.Heading, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)))
        //                {
        //                    Border = Rectangle.BOX,
        //                    HorizontalAlignment = Element.ALIGN_CENTER,
        //                    VerticalAlignment = Element.ALIGN_MIDDLE,
        //                    Padding = 4,
        //                    BackgroundColor = new BaseColor(220, 220, 220)
        //                };

        //                PdfPCell imgCell = new PdfPCell(img, true)
        //                {
        //                    Border = Rectangle.BOX,
        //                    HorizontalAlignment = Element.ALIGN_CENTER,
        //                    VerticalAlignment = Element.ALIGN_MIDDLE,
        //                    Padding = 5,
        //                    FixedHeight = 190f
        //                };

        //                PdfPTable nested = new PdfPTable(1);
        //                nested.WidthPercentage = 100;
        //                nested.AddCell(headingCell);
        //                nested.AddCell(imgCell);

        //                PdfPCell outerCell = new PdfPCell(nested)
        //                {
        //                    Border = Rectangle.BOX,
        //                    Padding = 5,
        //                    HorizontalAlignment = Element.ALIGN_CENTER,
        //                    VerticalAlignment = Element.ALIGN_MIDDLE
        //                };

        //                table.AddCell(outerCell);
        //                imgCount++;
        //            }

        //            if (imgCount % 6 == 0)
        //            {
        //                doc.Add(table);
        //                doc.NewPage();
        //                table = new PdfPTable(2);
        //                table.WidthPercentage = 100;
        //            }
        //        }

        //        if (imgCount % 6 != 0)
        //        {
        //            while (imgCount % 2 != 0)
        //            {
        //                table.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.BOX });
        //                imgCount++;
        //            }
        //            doc.Add(table);
        //        }

        //        doc.Close();
        //        byte[] fileBytes = ms.ToArray();
        //        return File(fileBytes, "application/pdf", "ImagesWithCaptions.pdf");
        //    }
        //}

        #endregion


        #region Image download 


        public void GenerateReport(int? PK_CALL_ID, string ReportNo, string Call_No)
        {
            //int? PK_CALL_ID = 1393361;
            RMData.PK_CALL_ID = 1413123;

            DataTable ImageReportDashBoard = objDalVisitReport.GetReportImageByCall_Id(PK_CALL_ID);
            List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();

            if (ImageReportDashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in ImageReportDashBoard.Rows)
                {
                    DateTime CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    int year = CreatedDate.Year;
                    string currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(CreatedDate.Month);

                    ImageDashBoard.Add(new ReportImageModel
                    {
                        Path = $"/Content/Uploads/Images/{year}/{currentMonth}",
                        Image = Convert.ToString(dr["Image"]),
                        Heading = Convert.ToString(dr["Heading"])
                    });
                }
            }

            string pdfPath = Server.MapPath($"~/Content/" + Call_No + "/" + "Image_" + ".pdf");

            using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (Document doc = new Document(PageSize.A4, 20f, 20f, 70f, 50f))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                // Attach header/footer
                string relativePath = "/AllJsAndCss/images/logo.png";
                string logoPath = Server.MapPath(relativePath);
                string reportNo =  ReportNo;

                string relativePathFooter = "/AllJsAndCss/images/footerlogo.png";
                string footerLogoPath = Server.MapPath(relativePathFooter); 
                writer.PageEvent = new PdfHeaderFooter(logoPath, reportNo, footerLogoPath);

                doc.Open();

                PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
                int imgCount = 0;

                foreach (var item in ImageDashBoard)
                {
                    string fullImagePath = Server.MapPath(item.Path + "/" + item.Image);

                    if (System.IO.File.Exists(fullImagePath))
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(fullImagePath);
                        img.ScaleAbsolute(250f, 180f);
                        img.Alignment = Element.ALIGN_CENTER;

                        PdfPCell headingCell = new PdfPCell(new Phrase(item.Heading, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11)))
                        {
                            Border = Rectangle.BOTTOM_BORDER,  // ❌ No border here
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 4
                        };

                        PdfPCell imgCell = new PdfPCell(img, true)
                        {
                            Border = Rectangle.NO_BORDER,  // ❌ No border here
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 5,
                            FixedHeight = 190f
                        };

                        PdfPTable nested = new PdfPTable(1) { WidthPercentage = 100 };
                        nested.AddCell(headingCell);
                        nested.AddCell(imgCell);

                        PdfPCell outerCell = new PdfPCell(nested)
                        {
                            Border = Rectangle.BOX,   // ✅ Only one border around the block
                            Padding = 5,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };

                        table.AddCell(outerCell);
                        imgCount++;
                    }

                    if (imgCount % 6 == 0)
                    {
                        doc.Add(table);
                        doc.NewPage();
                        table = new PdfPTable(2) { WidthPercentage = 100 };
                    }
                }

                if (imgCount % 6 != 0)
                {
                    while (imgCount % 2 != 0)
                    {
                        table.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.BOX });
                        imgCount++;
                    }
                    doc.Add(table);
                }

                doc.Close();
            }
        }


        #endregion





    }
}