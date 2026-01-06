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
        private string _CustomerSpecificNumber;
        private bool _isConfirmation;
        private string watermark_;


        public PdfHeaderFooter(string logoPath, string reportNo, string footerLogoPath, string CustomerSpecificNumber, bool isConfirmation,string watermarkPath)
        {
            _logoPath = logoPath;
            _reportNo = reportNo;
            _footerLogoPath = footerLogoPath;
            _CustomerSpecificNumber = CustomerSpecificNumber;
            _isConfirmation = isConfirmation;
            watermark_ = watermarkPath;

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


            if (!_isConfirmation && File.Exists(watermark_))
            {
                iTextSharp.text.Image draftImg = iTextSharp.text.Image.GetInstance(watermark_);
                draftImg.ScaleAbsolute(300f, 300f);
                draftImg.SetAbsolutePosition(
                    (document.PageSize.Width - 300) / 2,
                    (document.PageSize.Height - 300) / 2);
                draftImg.Transparency = new int[] { 0x0F, 0x10 };

                PdfContentByte under = writer.DirectContentUnder;
                under.AddImage(draftImg);
            }
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.SetWidths(new float[] { 20f, 80f });

            if (File.Exists(_logoPath))
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(_logoPath);
                //logo.ScaleAbsolute(120f, 50f);
                logo.ScaleAbsolute(120f, 31f);
                PdfPCell logoCell = new PdfPCell(logo, false)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingLeft = 30f,
                    PaddingTop = -5f,

                };
                headerTable.AddCell(logoCell);
            }
            else
            {
                headerTable.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });
            }




            PdfPTable textTable = new PdfPTable(1);
            textTable.WidthPercentage = 100;

            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            Font subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);

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
            textTable.AddCell(new PdfPCell(new Phrase(_CustomerSpecificNumber, smallFont))
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
                PaddingLeft = -50f
            };
            headerTable.AddCell(textCell);

            // Write header

            if (!_isConfirmation)
            {
            }
            else
            {
                headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 10, cb);
            }




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
            p1.Add(new Chunk("This IVR shall not be considered as final acceptance of inspected item(s)." + "The final acceptance will be given through Inspection Release Note.\n" + "The inspection by TUV India Pvt.Ltd., review of Test Certificates / Reports and issue of Inspection Visit Report does not relieve the Client / Supplier / Manufacturer / Stockiest from their responsibility towards the Client/ End User to supply the genuine material / item(s) and document(s) in full compliance with applicable Order, Specification, Technical, Quality, Quantity, Warranty, Guarantee requirements.Supplier / Manufacturer / stockiest is wholly legally responsible for genuineness of the material / item(s) supplied and document(s) submitted.TÜV India’s responsibility is only limited to correctness of inspection results including review of the documents, within its agreed scope against written requirements and neither TUV India nor any of its group companies, associates or employees are in any way / legally responsible for genuineness of the material / item(s) and document(s).If the calibration certificate(s) for the measuring instrument(s) / equipment(s) used during inspection do not have traceability to NABL / Other certifying bodies, then the scope of review is limited only to technical content in the calibration certificate.", font9));

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
            p2.Add(new Chunk("This document is the property of TUV India Pvt. Ltd. and should not be reproduced, except in full without the consent of TUV India Pvt. Ltd.", font9));

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
            p5.Add(new Chunk(" F / INSP / VR / 11 – R13 / TIIMES; Revision Date: 27.10.2023", font11));

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
            if (!_isConfirmation)
            {

            }
            else
            {
                footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin + 90, cb);
            }
        }












    }



    /// <summary>
    /// /Bind pdf
    /// </summary>

    public class HtmlFooterEvent : PdfPageEventHelper
    {
        private PdfTemplate footerTemplate;
        private PdfImportedPage footerPage;
        private string footerPath;

        public HtmlFooterEvent(string footerPdfPath)
        {
            footerPath = footerPdfPath;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);

            if (File.Exists(footerPath))
            {
                // Read footer PDF fragment
                PdfReader reader = new PdfReader(footerPath);
                footerPage = writer.GetImportedPage(reader, 1); // assuming single-page footer template
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (footerPage != null)
            {
                PdfContentByte cb = writer.DirectContentUnder;

                // Position footer (adjust Y as needed)
                float x = document.LeftMargin;
                float y = document.BottomMargin - 5;

                // Scale the imported footer to fit page width
                float pageWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                float scale = pageWidth / footerPage.Width;

                cb.AddTemplate(footerPage, scale, 0, 0, scale, x, y);
            }
        }
    }




}