using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml.pipeline;

namespace TuvVision
{
    //public class PdfPageEvents
    //{
    //}


    public class PdfPageEvents : PdfPageEventHelper
    {
        #region BAK
        //public class PdfHeaderFooter : PdfPageEventHelper
        //{
        //    private string _logoPath;
        //    private string _reportNo;

        //    public PdfHeaderFooter(string logoPath, string reportNo)
        //    {
        //        _logoPath = logoPath;
        //        _reportNo = reportNo;
        //    }

        //    public override void OnEndPage(PdfWriter writer, Document document)
        //    {
        //        PdfPTable headerTable = new PdfPTable(2);
        //        headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        //        headerTable.SetWidths(new float[] { 20f, 80f });

        //        // Left cell: Logo
        //        if (File.Exists(_logoPath))
        //        {
        //            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(_logoPath);
        //            logo.ScaleAbsolute(120f, 50f);
        //            PdfPCell logoCell = new PdfPCell(logo, false)
        //            {
        //                Border = Rectangle.NO_BORDER,
        //                HorizontalAlignment = Element.ALIGN_LEFT,
        //                VerticalAlignment = Element.ALIGN_MIDDLE,
        //                PaddingLeft = 10f
        //            };
        //            headerTable.AddCell(logoCell);
        //        }
        //        else
        //        {
        //            headerTable.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });
        //        }

        //        // Right cell: Text content
        //        PdfPTable textTable = new PdfPTable(1);
        //        textTable.WidthPercentage = 100;

        //        Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
        //        Font subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
        //        Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

        //        textTable.AddCell(new PdfPCell(new Phrase("TUV INDIA PRIVATE LIMITED", titleFont))
        //        {
        //            Border = Rectangle.NO_BORDER,
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            PaddingBottom = 2
        //        });
        //        textTable.AddCell(new PdfPCell(new Phrase("INSPECTION VISIT REPORT", subFont))
        //        {
        //            Border = Rectangle.NO_BORDER,
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            PaddingBottom = 2
        //        });
        //        textTable.AddCell(new PdfPCell(new Phrase(_reportNo, smallFont))
        //        {
        //            Border = Rectangle.NO_BORDER,
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            PaddingBottom = 2
        //        });

        //        PdfPCell textCell = new PdfPCell(textTable)
        //        {
        //            Border = Rectangle.NO_BORDER,
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            VerticalAlignment = Element.ALIGN_MIDDLE,
        //            PaddingTop = 5
        //        };

        //        headerTable.AddCell(textCell);

        //        // Write header
        //        headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, writer.DirectContent);
        //    }
        //}
        #endregion



    }





    public class PdfHeaderFooter : PdfPageEventHelper
    {
        private string _logoPath;
        private string _reportNo;
        private string _footerLogoPath;
        private iTextSharp.text.Image footerLogo;

        public PdfHeaderFooter(string logoPath, string reportNo, string footerLogoPath)
        {
            _logoPath = logoPath;
            _reportNo = reportNo;
            _footerLogoPath = footerLogoPath;


            //if (System.IO.File.Exists(footerLogoPath))
            //{
            //    footerLogo = iTextSharp.text.Image.GetInstance(footerLogoPath); // ✅ assign to field
            //    footerLogo.ScaleToFit(60f, 40f); // resize footer image
            //}


        }


        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;

            // ================= HEADER =================
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.SetWidths(new float[] { 20f, 80f });

            if (File.Exists(_logoPath))
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(_logoPath);
                logo.ScaleAbsolute(120f, 50f);
                PdfPCell logoCell = new PdfPCell(logo, false)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingLeft = 10f
                };
                headerTable.AddCell(logoCell);
            }
            else
            {
                headerTable.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });
            }

            PdfPTable textTable = new PdfPTable(1);
            textTable.WidthPercentage = 100;

            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Font subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            textTable.AddCell(new PdfPCell(new Phrase("TUV INDIA PRIVATE LIMITED", titleFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            textTable.AddCell(new PdfPCell(new Phrase("INSPECTION VISIT REPORT", subFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            textTable.AddCell(new PdfPCell(new Phrase(_reportNo, smallFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            PdfPCell textCell = new PdfPCell(textTable)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            headerTable.AddCell(textCell);

            // Write header
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, cb);

            // ================= FOOTER =================
            // One-column table spanning full width
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.LockedWidth = true;

            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);

            PdfPCell footerCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                PaddingTop = 5f,
                PaddingBottom = 0f,
                PaddingLeft = 5f,
                PaddingRight = 5f
            };

            // Build all footer content as one Paragraph
            Paragraph footerContent = new Paragraph
            {
                Alignment = Element.ALIGN_JUSTIFIED,
                Leading = 9f // line spacing
            };
          
            footerContent.Add(new Chunk("Disclaimer: ", boldFont));
            footerContent.Add(new Chunk("The inspection by TUV India Pvt. Ltd., review of Test Certificates / Reports and issue of Inspection Release Note / Certificate does not relieve Client / Supplier / Manufacturer / Stockiest from their responsibility towards the Client / End User to supply the genuine material / item(s) and document(s) in full compliance with applicable Order, Specification, Technical, Quality, Quantity, Warranty, Guarantee requirements. Supplier / Manufacturer / Stockiest is wholly legally responsible for genuineness of the material / item(s) supplied and document(s) submitted. TÜV India’s responsibility is only limited to correctness of inspection results including review of the documents within its agreed scope against written requirements and neither TUV India nor any of its group companies, associates or employees are in any way / legally responsible for genuineness of the material / item(s) and document(s). If the calibration certificate(s) for the measuring instrument(s) / equipment(s) used during inspection do not have traceability to NABL / Other certifying bodies, then the scope of review is limited only to technical content in the calibration certificate.", normalFont));

            footerContent.Add(Chunk.NEWLINE);
            footerContent.Add(Chunk.NEWLINE);
            footerContent.Add(new Chunk("Copyright: ", boldFont));
            footerContent.Add(new Chunk("This document is the property of TUV India Private Limited and should not be reproduced, except in full without the consent of TUV India Pvt. Ltd.", normalFont));

            footerContent.Add(Chunk.NEWLINE);
            footerContent.Add(new Chunk("TUV India Pvt. Ltd. (TÜV NORD GROUP): ", boldFont));
            footerContent.Add(new Chunk("(REGD. & HEAD OFFICE) 801, Raheja Plaza - I, LBS Marg, Ghatkopar (West), Mumbai – 400086, Maharashtra, India.", normalFont));

            footerContent.Add(Chunk.NEWLINE);
            footerContent.Add(new Chunk("Tel: ", boldFont));
            footerContent.Add(new Chunk("+91 22 66477000, ", normalFont));
            footerContent.Add(new Chunk("Email: ", boldFont));
            footerContent.Add(new Chunk("inspection@tuv-nord.com, ", normalFont));
            footerContent.Add(new Chunk("Website: ", boldFont));
            footerContent.Add(new Chunk("www.tuv-nord.com/in", normalFont));

            footerContent.Add(Chunk.NEWLINE);
            footerContent.Add(new Chunk("Form No.: ", boldFont));
            footerContent.Add(new Chunk("F / INSP / IRN / 03 – R08 / TIIMES; Revision Date: 27.10.2023", normalFont));

            // Add logo if exists
            if (!string.IsNullOrEmpty(_footerLogoPath) && File.Exists(_footerLogoPath))
            {
                iTextSharp.text.Image footerLogo = iTextSharp.text.Image.GetInstance(_footerLogoPath);
                footerLogo.ScaleAbsolute(80f, 25f);
                footerLogo.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                footerContent.Add(Chunk.NEWLINE);
                footerContent.Add(new Chunk(footerLogo, 0, 0, true)); // logo floated right
            }

            // Add content to cell and table
            footerCell.AddElement(footerContent);
            footerTable.AddCell(footerCell);

            // Write footer to PDF
            footerTable.WriteSelectedRows(
                0, -1,
                document.LeftMargin,
                document.BottomMargin + 65,
                writer.DirectContent
            );
        }
      


    }


}