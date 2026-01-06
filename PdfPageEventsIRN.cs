using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TuvVision
{
    public class PdfPageEventsIRN : PdfPageEventHelper
    {
    }

    public class PdfHeaderFooterIRN : PdfPageEventHelper
    {
        private string _logoPath;
        private string _reportNo;
        private string _footerLogoPath;
        private iTextSharp.text.Image footerLogo;
        private string _CustomerSpecificNumber;
        private bool _isConfirmation;
        private string _watermarkPath;

        public PdfHeaderFooterIRN(string logoPath, string reportNo, string footerLogoPath, string CustomerSpecificNumber, bool isConfirmation, string watermarkPath)
        {
            _logoPath = logoPath;
            _reportNo = reportNo;
            _footerLogoPath = footerLogoPath;
            _CustomerSpecificNumber = CustomerSpecificNumber;
            _isConfirmation = isConfirmation;
            _watermarkPath = watermarkPath;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (!_isConfirmation)
            {
                string watermarkPath = HttpContext.Current.Server.MapPath("~/invalid.jpg");
                if (File.Exists(watermarkPath))
                {
                    PdfContentByte canvas = writer.DirectContentUnder;
                    iTextSharp.text.Image draftImg = iTextSharp.text.Image.GetInstance(watermarkPath);

                    // SET SIZE (Adjust according to need)
                    float width = document.PageSize.Width * 0.75f;
                    float height = document.PageSize.Height * 0.75f;
                    draftImg.ScaleAbsolute(width, height);

                    // CENTER POSITION
                    draftImg.SetAbsolutePosition(
                        (document.PageSize.Width - width) / 2,
                        (document.PageSize.Height - height) / 2);

                    // OPACITY (0.15 = same as SelectPDF approx)
                    PdfGState gs = new PdfGState();
                    gs.FillOpacity = 0.15f;     // <=== lower = lighter
                    gs.StrokeOpacity = 0.15f;
                    canvas.SaveState();
                    canvas.SetGState(gs);

                    canvas.AddImage(draftImg);
                    canvas.RestoreState();
                }
            }

            PdfContentByte cb = writer.DirectContent;

            if (!_isConfirmation && File.Exists(_watermarkPath))
            {
                iTextSharp.text.Image draftImg = iTextSharp.text.Image.GetInstance(_watermarkPath);
                draftImg.ScaleAbsolute(300f, 300f);
                draftImg.SetAbsolutePosition(
                    (document.PageSize.Width - 300) / 2,
                    (document.PageSize.Height - 300) / 2);
                draftImg.Transparency = new int[] { 0x0F, 0x10 };

                PdfContentByte under = writer.DirectContentUnder;
                under.AddImage(draftImg);
            }

            // ================= HEADER =================
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.SetWidths(new float[] { 20f, 80f });

            if (_isConfirmation && File.Exists(_logoPath))
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(_logoPath);
                logo.ScaleAbsolute(115f, 30f);
                PdfPCell logoCell = new PdfPCell(logo, false)
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 0,
                    //PaddingBottom = 0,
                    PaddingLeft = 28
                    //HorizontalAlignment = Element.ALIGN_LEFT,
                    //VerticalAlignment = Element.ALIGN_MIDDLE,
                    //PaddingLeft = 15f
                };
                headerTable.AddCell(logoCell);
            }
            else
            {
                headerTable.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });
            }

            PdfPTable textTable = new PdfPTable(1);
            textTable.WidthPercentage = 100;

            Font titleFont = FontFactory.GetFont("TNG PRO", 14, Font.BOLD);
            Font subFont = FontFactory.GetFont("TNG PRO", 10, Font.BOLD);
            Font smallFont = FontFactory.GetFont("TNG PRO", 8);

            textTable.AddCell(new PdfPCell(new Phrase("TUV INDIA PRIVATE LIMITED", titleFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            textTable.AddCell(new PdfPCell(new Phrase("INSPECTION RELEASE NOTE / CERTIFICATE", subFont))
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
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingLeft = -50,
                PaddingTop = -10
            };
            headerTable.AddCell(textCell);

            // Write header
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, cb);

            // ================= FOOTER =================
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.LockedWidth = true;

            // Define fonts
            Font font9 = FontFactory.GetFont("TNG PRO", 6, Font.ITALIC);
            Font font9Bold = FontFactory.GetFont("TNG PRO", 6, Font.BOLD);
            Font font10 = FontFactory.GetFont("TNG PRO", 6);
            Font font10Bold = FontFactory.GetFont("TNG PRO", 6, Font.BOLD);
            Font font11 = FontFactory.GetFont("TNG PRO", 6);

            // === Disclaimer Paragraph ===
            Paragraph p1 = new Paragraph();
            p1.SetLeading(0, 0.9f);
            p1.Alignment = Element.ALIGN_JUSTIFIED;
            p1.Add(new Chunk("Disclaimer: ", font9Bold));
            p1.Add(new Chunk("The inspection by TUV India Pvt. Ltd., review of Test Certificates / Reports and issue of Inspection Release Note / Certificate does not relieve Client / Supplier / Manufacturer / Stockiest from their responsibility towards the Client / End User to supply the genuine material / item(s) and document(s) in full compliance with applicable Order, Specification, Technical, Quality, Quantity, Warranty, Guarantee requirements. Supplier / Manufacturer / Stockiest is wholly legally responsible for genuineness of the material / item(s) supplied and document(s) submitted. TÜV India’s responsibility is only limited to correctness of inspection results including review of the documents, within its agreed scope against written requirements and neither TUV India nor any of its group companies, associates or employees are in any way / legally responsible for genuineness of the material / item(s) and document(s). If the calibration certificate(s) for the measuring instrument(s) / equipment(s) used during inspection do not have traceability to NABL / Other certifying bodies, then the scope of review is limited only to technical content in the calibration certificate.", font9));

            PdfPCell c1 = new PdfPCell();
            c1.AddElement(p1);
            c1.Border = Rectangle.NO_BORDER;
            c1.PaddingTop = 45;
            c1.PaddingBottom = 0;
            c1.PaddingLeft = 12;
            c1.PaddingRight = 12;

            // CRITICAL LINES — REQUIRED FOR JUSTIFICATION
            c1.UseAscender = true;
            c1.UseDescender = true;
            c1.SetLeading(0, 1f);

            // This enables full justification INSIDE TABLE CELL
            c1.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;

            //PdfPCell c1 = new PdfPCell(p1) { Border = Rectangle.NO_BORDER, PaddingTop = 45, PaddingBottom = 0, PaddingLeft = 12, PaddingRight = 12};
            footerTable.AddCell(c1);

            // === Copyright ===
            Paragraph p2 = new Paragraph();
            p2.SetLeading(0, 1.4f);
            p2.Add(new Chunk("Copyright: ", font9Bold));
            p2.Add(new Chunk("This document is the property of TUV India Private Limited and should not be reproduced, except in full without the consent of TUV India Pvt. Ltd.", font9));

            PdfPCell c2 = new PdfPCell(p2) { Border = Rectangle.NO_BORDER, PaddingTop = 2, PaddingBottom = 0, PaddingLeft = 12, PaddingRight = 12 };
            footerTable.AddCell(c2);

            // === Address Line ===
            Paragraph p3 = new Paragraph();
            p3.SetLeading(0, 0.8f);
            p3.Add(new Chunk("TUV India Pvt. Ltd. (TÜV NORD GROUP): ", font10Bold));
            p3.Add(new Chunk("(REGD. & HEAD OFFICE)\n801, Raheja Plaza - I, LBS Marg, Ghatkopar (West), Mumbai – 400086, Maharashtra, India.", font10));

            PdfPCell c3 = new PdfPCell(p3) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, PaddingLeft = 12, PaddingRight = 12 };
            footerTable.AddCell(c3);

            // === Contact ===
            Paragraph p4 = new Paragraph();
            p4.SetLeading(0, 1.8f);

            // Phone Label
            p4.Add(new Chunk("Tel: ", font10));

            // Phone hyperlink clickable (tel:)
            Chunk phoneLink = new Chunk("+91 22 66477000", FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.UNDERLINE, BaseColor.BLUE));
            phoneLink.SetAnchor("tel:+912266477000");
            p4.Add(phoneLink);

            p4.Add(new Chunk(", Email: ", font10));

            // Email hyperlink clickable (mailto:)
            Chunk emailLink = new Chunk("inspection@tuv-nord.com", FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.UNDERLINE, BaseColor.BLUE));
            emailLink.SetAnchor("mailto:inspection@tuv-nord.com");
            p4.Add(emailLink);

            p4.Add(new Chunk(", Website: ", font10));

            // Website hyperlink
            Chunk websiteLink = new Chunk("www.tuv-nord.com/in", FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.UNDERLINE, BaseColor.BLUE));
            websiteLink.SetAnchor("https://www.tuv-nord.com/in");
            p4.Add(websiteLink);
            //p4.Add(new Chunk("Tel: +91 22 66477000, Email: inspection@tuv-nord.com, Website: ", font10));

            //// Hyperlink chunk
            //Chunk websiteLink = new Chunk("www.tuv-nord.com/in", FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.UNDERLINE, BaseColor.BLUE));
            //websiteLink.SetAnchor("https://www.tuv-nord.com/in"); // Actual hyperlink URL

            //p4.Add(websiteLink);

            PdfPCell c4 = new PdfPCell(p4) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, PaddingLeft = 12, PaddingRight = 12 };
            footerTable.AddCell(c4);

            // === Form No + Logo Right ===
            PdfPTable bottomRow = new PdfPTable(2);
            bottomRow.SetWidths(new float[] { 80f, 20f });
            bottomRow.TotalWidth = footerTable.TotalWidth;

            Paragraph p5 = new Paragraph();
            p5.SetLeading(0, 0.9f);
            p5.Add(new Chunk("Form No.: ", font9Bold));
            p5.Add(new Chunk("F / INSP / IRN / 03 – R08 / TIIMES; Revision Date: 27.10.2023", font11));

            PdfPCell leftCell = new PdfPCell(p5)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingLeft = 12,
                PaddingRight = 12
            };
            bottomRow.AddCell(leftCell);

            // Logo cell
            if (_isConfirmation && !string.IsNullOrEmpty(_footerLogoPath) && File.Exists(_footerLogoPath))
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(_footerLogoPath);
                logo.ScaleAbsolute(100f, 15f);

                PdfPCell logoCell = new PdfPCell(logo, false)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingTop = -10f
                };
                bottomRow.AddCell(logoCell);
            }
            else
            {
                bottomRow.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });
            }

            PdfPCell bottomRowContainer = new PdfPCell(bottomRow) { Border = Rectangle.NO_BORDER };
            footerTable.AddCell(bottomRowContainer);

            // Finally write footer
            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin + 90, cb);


            //PdfPTable footerTable = new PdfPTable(2);
            //footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            //footerTable.LockedWidth = true; // important: lock full width
            //footerTable.SetWidths(new float[] { 80f, 20f });

            ////Font footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
            //Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
            //Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);

            //Paragraph disclaimerPara = new Paragraph() { Alignment = Element.ALIGN_JUSTIFIED };
            //disclaimerPara.SetLeading(0, 1.0f);

            //disclaimerPara.Add(new Chunk("Disclaimer: ", boldFont));
            //disclaimerPara.Add(new Chunk("The inspection by TUV India Pvt. Ltd., review of Test Certificates / Reports and issue of Inspection Release Note / Certificate does not relieve Client / Supplier / Manufacturer / Stockiest from their responsibility towards the Client / End User to supply the genuine material / item(s) and document(s) in full compliance with applicable Order, Specification, Technical, Quality, Quantity, Warranty, Guarantee requirements.Supplier / Manufacturer / stockiest is wholly legally responsible for genuineness of the material / item(s) supplied and document(s) submitted.TÜV India’s responsibility is only limited to correctness of inspection results including review of the documents, within its agreed scope against written requirements and neither TUV India nor any of its group companies, associates or employees are in any way / legally responsible for genuineness of the material / item(s) and document(s).If the calibration certificate(s) for the measuring instrument(s) / equipment(s) used during inspection do not have traceability to NABL / Other certifying bodies, then the scope of review is limited only to technical content in the calibration certificate.", normalFont));

            //disclaimerPara.Add(new Chunk("\nCopyright: ", boldFont));
            //disclaimerPara.Add(new Chunk("This document is the property of TUV India Pvt. Ltd. and should not be reproduced, except in full without the consent of TUV India Pvt. Ltd.", normalFont));

            //disclaimerPara.Add(new Chunk("\nTUV India Pvt. Ltd. (TÜV NORD GROUP): ", boldFont));
            //disclaimerPara.Add(new Chunk("(REGD. & HEAD OFFICE) \n801, Raheja Plaza - I, LBS Marg, Ghatkopar (West), Mumbai – 400086, Maharashtra, India.\nTel: +91 22 66477000, Email: inspection@tuv-nord.com, Website: www.tuv-nord.com/in", normalFont));

            //disclaimerPara.Add(new Chunk("\nForm No.: ", boldFont));
            //disclaimerPara.Add(new Chunk("F / INSP / IRN / 03 – R08 / TIIMES; Revision Date: 27.10.2023", normalFont));

            //PdfPCell disclaimerCell = new PdfPCell(disclaimerPara)
            //{
            //    Border = Rectangle.NO_BORDER,
            //    Colspan = 2,
            //    Padding = 5f,
            //    PaddingTop = 8f,
            //    HorizontalAlignment = Element.ALIGN_JUSTIFIED
            //};
            //footerTable.AddCell(disclaimerCell);

            //PdfPCell emptyCell = new PdfPCell(new Phrase(" ", normalFont))
            //{
            //    Border = Rectangle.NO_BORDER,
            //    HorizontalAlignment = Element.ALIGN_LEFT
            //};
            //footerTable.AddCell(emptyCell);

            //// FOOTER LOGO
            //if (_isConfirmation && !string.IsNullOrEmpty(_footerLogoPath) && File.Exists(_footerLogoPath))
            //{
            //    iTextSharp.text.Image footerLogo = iTextSharp.text.Image.GetInstance(_footerLogoPath);
            //    footerLogo.ScaleAbsolute(100f, 25f);

            //    PdfPCell logoCellFooter = new PdfPCell(footerLogo, false)
            //    {
            //        Border = Rectangle.NO_BORDER,
            //        HorizontalAlignment = Element.ALIGN_RIGHT,
            //        VerticalAlignment = Element.ALIGN_BOTTOM,
            //        PaddingRight = 10f,
            //        PaddingBottom = 5f
            //    };
            //    footerTable.AddCell(logoCellFooter);
            //}
            //else
            //{
            //    footerTable.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });
            //}

            //footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin + 90, cb);            
        }
    }
}