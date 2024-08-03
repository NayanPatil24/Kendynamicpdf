using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Kendynamicpdf.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using iText.Layout.Borders;
using iText.Layout.Properties;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Reflection.Metadata;
using System.IO;
using Microsoft.Extensions.Logging;

using Document = iTextSharp.text.Document;
using KenTextileERP.Application.Business.Controllers;
using iText.IO.Font.Otf;


namespace Kendynamicpdf.Controllers
{
    public class TaxInvoiceController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TaxPDFGenerator()
        {
            // Create a memory stream to hold the PDF
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Create a new PDF document
                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 15, 15, 15, 15);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                // Open the document

                document.Open();


                // Create a rectangle for the border
                Rectangle border = new Rectangle(document.PageSize);
                border.Left += document.LeftMargin;
                border.Right -= document.RightMargin;
                border.Top -= document.TopMargin;
                border.Bottom += document.BottomMargin;
                border.BorderColor = BaseColor.BLACK; // Set the border color and width
                border.BorderWidth = 1f;
                border.Border = Rectangle.BOX;

                // Add the border to the document
                PdfContentByte content = writer.DirectContent;
                content.Rectangle(border);
                content.Stroke();


                // Path to the logo

                // string imagePath = Server.MapPath("~/Content/Images/logo.png");

                // string imagePath = 
                //Image logo = Image.GetInstance(imagePath);
                //logo.ScaleToFit(100f, 100f); // Adjust the size of the logo
                //logo.PaddingTop = -10;
                //logo.SpacingBefore = -50;
                // Load the image
                Image logo = Image.GetInstance("wwwroot/image/ken.png");
                float imageWidth = 100f; // New width of the image
                float imageHeight = 100f; // New height of the image
                logo.ScaleToFit(imageWidth, imageHeight);

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100; // Width of the table

                // Set the relative column widths
                float[] widths = new float[] { 1f, 3f };
                table.SetWidths(widths);

                // Get the PdfContentByte object
                PdfContentByte cb = writer.DirectContent;
                Paragraph p = new Paragraph();
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase("||Shri Hari||", FontFactory.GetFont("Arial", 12, Font.BOLD)), 297.5f, 810, 0); // (x, y) position and rotation angle
                p.SpacingAfter = 30f; // Adjust the spacing as needed
                p.SpacingBefore = -40f;
                document.Add(p);

                //Company Name
                Chunk boldText = new Chunk("KEN ENTERPRISES PRIVATE LIMITED", FontFactory.GetFont("Arial", 14, Font.BOLD));
                p.SpacingBefore = 7f;
                p.SpacingAfter = 5f;
                p.Alignment = (Element.ALIGN_CENTER);
                p.Add(boldText);
                document.Add(new Paragraph("\n"));
                document.Add(p);

                // Create a Phrase for the address text
                Phrase addressPhrase;
                addressPhrase = new Phrase("9/621, Industrial Estate, near Kalyan Kendra,Ichalkaranji-416 115.Dist.Kolhapur, INDIA" + "\n" + " Tel:+91 230 2437249,2438538 | E-mail:office@kenindia.in | Web:www.kenindia.in", FontFactory.GetFont("Arial", 10, Font.NORMAL));

                // Create a cell to hold both the logo and the address text
                PdfPCell compositeCell = new PdfPCell();
                compositeCell.Border = Rectangle.NO_BORDER;

                // Create a PdfPTable to hold the logo and the address side by side
                PdfPTable innerTable = new PdfPTable(2);
                innerTable.SetWidths(new float[] { 1f, 3f });
                innerTable.WidthPercentage = 100;

                // Add the logo to the inner table
                PdfPCell logoCell = new PdfPCell(logo);
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                logoCell.PaddingTop = -50;
                logoCell.PaddingLeft = 10;
                logoCell.BorderWidthBottom = 1f;
                logoCell.BorderColor = BaseColor.BLUE;
                innerTable.AddCell(logoCell);

                // Add the address to the inner table
                PdfPCell addressCell = new PdfPCell(addressPhrase);
                addressCell.PaddingTop = -10f;
                addressCell.PaddingRight = -3f;
                addressCell.PaddingLeft = 11f;
                addressCell.PaddingBottom = 6f;
                addressCell.Border = Rectangle.NO_BORDER;
                addressCell.VerticalAlignment = Element.ALIGN_LEFT;
                addressCell.BorderWidthBottom = 1f;
                addressCell.BorderColor = BaseColor.GREEN;
                addressCell.AddElement(new Paragraph(""));
                addressCell.AddElement(addressPhrase);
                innerTable.AddCell(addressCell);

                // Add the inner table to the composite cell
                compositeCell.AddElement(innerTable);

                // Add the composite cell to the main table with colspan of 2
                PdfPCell mainCell = new PdfPCell(compositeCell);
                mainCell.Colspan = 2;
                mainCell.Border = Rectangle.NO_BORDER;
                table.AddCell(mainCell);

                // Add the table to the document
                document.Add(table);

                //Another Table
                PdfPTable table1 = new PdfPTable(3);
                table1.WidthPercentage = 100;
                table1.SetWidths(new float[] { 2.5f, 1.5f, 1.0f });

                //// Add cells without borders
                PdfPCell cell1 = new PdfPCell(new Phrase($"CORPORATE IDENTITY NUMBER:        ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                PdfPCell cell2 = new PdfPCell(new Phrase($"GST NO:          ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                PdfPCell cell3 = new PdfPCell(new Phrase($"PAN NO:          ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));

                //// Remove borders
                cell1.Border = Rectangle.NO_BORDER;
                cell2.Border = Rectangle.NO_BORDER;
                cell3.Border = Rectangle.NO_BORDER;

                //// Add cells to the table
                table1.AddCell(cell1);
                table1.AddCell(cell2);
                table1.AddCell(cell3);

                //// Add the table to the document
                document.Add(table1);

                ////Horizontal Line
                var canvas = writer.DirectContent;
                var lineStartX = document.LeftMargin;
                var lineEndX = document.PageSize.Width - document.RightMargin;
                var lineY = document.PageSize.Height - 110; // Example Y position
                var lineWidth = 1.0f; // Line width

                //// Draw the line
                canvas.SetLineWidth(lineWidth);
                canvas.MoveTo(lineStartX, lineY);
                canvas.LineTo(lineEndX, lineY);
                canvas.Stroke();

                ////Heading Sales Order
                Paragraph para = new Paragraph("TAX INVOICE", FontFactory.GetFont("Arial", 14, Font.BOLD));
                para.SpacingBefore = 5f;
                para.SpacingAfter = 5f;
                
                para.Alignment = Element.ALIGN_CENTER;
                document.Add(para);

                //Another Table
                PdfPTable table2 = new PdfPTable(2);
                table2.WidthPercentage = 100;
          
                table2.SetWidths(new float[] { 2f, 1f });
                //table2.
               // table2.SpacingAfter = 0f;
                //table2.SpacingBefore = 0f;

                PdfPCell InvoiceCell = new PdfPCell(new Phrase($"Invoice No.       :\n", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                InvoiceCell.Border = Rectangle.NO_BORDER;
                //InvoiceCell.BorderWidthTop = 1f;
                table2.AddCell(InvoiceCell);

              
                PdfPCell DateCell1 = new PdfPCell(new Phrase($"Date  : \n", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                DateCell1.Border = Rectangle.NO_BORDER;
                //DateCell1.BorderWidthTop = 1;
               // DateCell1.BorderWidthLeft = 1;
                //DateCell1.BorderWidthRight = 1;
                table2.AddCell(DateCell1);

                PdfPCell emptycell = new PdfPCell(new Phrase("   ", FontFactory.GetFont("Arial", 11, Font.NORMAL)));
                emptycell.Border = Rectangle.NO_BORDER;
                table2.AddCell(emptycell);
                PdfPCell emptycell2 = new PdfPCell(new Phrase("   ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                emptycell2.Border = Rectangle.NO_BORDER;
                table2.AddCell(emptycell2);

                PdfPCell sonocell = new PdfPCell(new Phrase($"SO No.                   :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                sonocell.Border = Rectangle.NO_BORDER;
                table2.AddCell(sonocell);

                PdfPCell DateCell2 = new PdfPCell(new Phrase($"Date   : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                DateCell2.Border = Rectangle.NO_BORDER;
                // DateCell2.BorderWidthTop = 1;
                //DateCell2.BorderWidthLeft = 1;
                // DateCell2.BorderWidthRight = 1;
                table2.AddCell(DateCell2);

                PdfPCell confcell = new PdfPCell(new Phrase($"PO No.                   : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                confcell.Border = Rectangle.NO_BORDER;
                table2.AddCell(confcell);

                PdfPCell DateCell3 = new PdfPCell(new Phrase($"Date   : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                DateCell3.Border = Rectangle.NO_BORDER;
                // DateCell3.BorderWidthTop = 1;
                // DateCell3.BorderWidthLeft = 1;
                // DateCell3.BorderWidthRight = 1;
                table2.AddCell(DateCell3);

                PdfPCell paymentCell = new PdfPCell(new Phrase($"Payment Terms      :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                paymentCell.Border = Rectangle.NO_BORDER;
                // paymentCell.BorderWidthTop = 1;
                table2.AddCell(paymentCell);

                PdfPCell emptycell5 = new PdfPCell(new Phrase("   ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                emptycell5.Border = Rectangle.NO_BORDER;
                table2.AddCell(emptycell5);

                //PdfPCell dnoteCell = new PdfPCell(new Phrase("Dilivery Note            :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
               // dnoteCell.Border = Rectangle.NO_BORDER;
                //dnoteCell.BorderWidthLeft = 1;
                //table2.AddCell(dnoteCell);

                PdfPCell DeliveryCell = new PdfPCell(new Phrase($"Delivery Terms         :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                DeliveryCell.Border = Rectangle.NO_BORDER;
                ///DeliveryCell.BorderWidthTop = 1;
                table2.AddCell(DeliveryCell);

                PdfPCell emptycell4 = new PdfPCell(new Phrase("   ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                emptycell4.Border = Rectangle.NO_BORDER;
                table2.AddCell(emptycell4);

                // PdfPCell emptycell6 = new PdfPCell(new Phrase("   ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                // emptycell6.Border = Rectangle.NO_BORDER;
                // table2.AddCell(emptycell6);

                // PdfPCell emptycell7 = new PdfPCell(new Phrase("   ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                // emptycell7.Border = Rectangle.NO_BORDER;
                // table2.AddCell(emptycell7);


               // document.Add(table2);

                //Another Table
                PdfPTable table3 = new PdfPTable(1);
                table3.WidthPercentage = 100;

                //table3.SetWidths(new float[] { 2f, 1f });

                PdfPCell distCell = new PdfPCell(new Phrase("Destination          :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                distCell.Border = Rectangle.NO_BORDER;
                //distCell.BorderWidthTop = 1;
               // distCell.BorderWidthLeft = 1;
                table3.AddCell(distCell);

                PdfPCell transCell = new PdfPCell(new Phrase("Transport           :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                transCell.Border = Rectangle.NO_BORDER;
                //transCell.BorderWidthLeft = 1;
                table3.AddCell(transCell);

                PdfPCell lrCell = new PdfPCell(new Phrase("LR.No.                  :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                lrCell.Border = Rectangle.NO_BORDER;
               // lrCell.BorderWidthLeft = 1;
                table3.AddCell(lrCell);


                PdfPCell lrdateCell = new PdfPCell(new Phrase("LR.Date.            :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                lrdateCell.Border = Rectangle.NO_BORDER;
              //  lrdateCell.BorderWidthLeft = 1;
                table3.AddCell(lrdateCell);


                PdfPCell tdCell = new PdfPCell(new Phrase("Terms of Delivery        :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tdCell.Border = Rectangle.NO_BORDER;
               // tdCell.BorderWidthLeft = 1;
                table3.AddCell(tdCell);

                PdfPCell ewayCell = new PdfPCell(new Phrase("E-way Bill No.         :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ewayCell.Border = Rectangle.NO_BORDER;
              //  ewayCell.BorderWidthLeft = 1;
                table3.AddCell(ewayCell);

              //  document.Add(table3);


                PdfPTable CombinedPurVenTbl = new PdfPTable(2);
                CombinedPurVenTbl.WidthPercentage = 100;
                CombinedPurVenTbl.SpacingBefore = 3;
                CombinedPurVenTbl.DefaultCell.Border = Rectangle.NO_BORDER; // No border for cells by default
                CombinedPurVenTbl.SpacingAfter = 0; // Add some spacing after the table

                CombinedPurVenTbl.SetWidths(new float[] { 2.5f, 2.5f });
                CombinedPurVenTbl.DefaultCell.Border = Rectangle.NO_BORDER; // Border for the entire table
                //                                                            //CombinedPurVenTbl.DefaultCell.BorderWidth = 0.5f; // Border width
                //                                                            //AmountTbl.AddCell(tableMiddle);
                PdfPCell InvoiceCell1 = new PdfPCell();
                InvoiceCell1.Border = Rectangle.NO_BORDER;
                InvoiceCell1.BorderWidthRight = 0.5f;
                InvoiceCell1.BorderWidthTop = 0.5f;
               //InvoiceCell.Colspan = 1; // Span across the number of columns in table1
                InvoiceCell1.AddElement(table2);
                CombinedPurVenTbl.AddCell(InvoiceCell1);

                PdfPCell distCell1 = new PdfPCell();
                distCell1.Border = Rectangle.NO_BORDER;
                distCell1.BorderWidthTop = 0.5f;
                distCell1.BorderWidthRight = 0.5f;
                ////ConsigneeTblCell.Colspan = 1; // Span across the number of columns in table1
                distCell1.AddElement(table3);
                CombinedPurVenTbl.AddCell(distCell1);

                document.Add(CombinedPurVenTbl);


                PdfPTable table4 = new PdfPTable(2);
                table4.WidthPercentage = 100;
                
                table4.SpacingAfter = 0f;
                table4.SpacingBefore = 3f;

                PdfPCell buyerCell = new PdfPCell(new Phrase($"Buyer  Details : \n\n", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                buyerCell.Border = Rectangle.NO_BORDER;
                buyerCell.BorderWidthTop = 0.5f;
                table4.AddCell(buyerCell);

                PdfPCell consiCell = new PdfPCell(new Phrase($"Consignee Details : \n\n", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                consiCell.Border = Rectangle.NO_BORDER;
                consiCell.BorderWidthTop = 0.5f;
                consiCell.BorderWidthLeft = 0.5f;
                consiCell.BorderWidthRight = 0.5f;
                table4.AddCell(consiCell);

                PdfPCell gstcell1 = new PdfPCell(new Phrase($"GST No : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                gstcell1.Border = Rectangle.NO_BORDER;
                table4.AddCell(gstcell1);

                PdfPCell gstcell2 = new PdfPCell(new Phrase($"GST No : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                gstcell2.Border = Rectangle.NO_BORDER;
                gstcell2.BorderWidthLeft = 0.5f;
                gstcell2.BorderWidthRight = 0.5f;
                table4.AddCell(gstcell2);


                PdfPCell statecell1 = new PdfPCell(new Phrase($"State : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                statecell1.Border = Rectangle.NO_BORDER;
                statecell1.PaddingBottom = 5;
                statecell1.BorderWidthBottom = 0.5f;
                table4.AddCell(statecell1);

                PdfPCell statecell2 = new PdfPCell(new Phrase($"State : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                statecell2.Border = Rectangle.NO_BORDER;
                statecell2.PaddingBottom = 5;
                statecell2.BorderWidthBottom = 0.5f;
                statecell2.BorderWidthLeft = 0.5f;
                statecell2.BorderWidthRight = 0.5f;
                table4.AddCell(statecell2);

                PdfPCell emptycell8 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                emptycell8.Border = Rectangle.NO_BORDER;
                emptycell8.BorderWidthBottom = 0.5f;
                emptycell8.PaddingBottom = 5;
                table4.AddCell(emptycell8);

                
                document.Add(table4);

        

                //Agent Name 
                Paragraph para1 = new Paragraph($"Agent Name :  ", FontFactory.GetFont("Arial", 10, Font.NORMAL));
                para1.PaddingTop = 0;
                para1.SpacingBefore = -5;
                para1.Alignment = Element.ALIGN_MIDDLE;
                para1.SpacingAfter = 5;
                document.Add(para1);

                //Adding new table 
                PdfPTable table5 = new PdfPTable(10);
               table5.SetWidths(new float[] { 0.5f, 4f, 1f, 2f, 0.8f, 0.6f, 1f, 1f, 1f, 2f });
                table5.WidthPercentage = 100;

                PdfPCell tcell1 = new PdfPCell(new Phrase($"Sr.", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell1.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell1);

                PdfPCell tcell2 = new PdfPCell(new Phrase("Description of Goods", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell2.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell2);

                PdfPCell tcell3 = new PdfPCell(new Phrase("HSN", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell3.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell3);

                PdfPCell tcell4 = new PdfPCell(new Phrase("Roll No", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell4.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell4);

                PdfPCell tcell5 = new PdfPCell(new Phrase("Roll", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell5.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell5);

                PdfPCell tcell6 = new PdfPCell(new Phrase("Pcs", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell6.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell6);

                PdfPCell tcell7 = new PdfPCell(new Phrase("Quantity", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell7.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell7);

                PdfPCell tcell8 = new PdfPCell(new Phrase("UOM", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell8.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell8);

                PdfPCell tcell9 = new PdfPCell(new Phrase("Rate", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell9.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell9);

                PdfPCell tcell10 = new PdfPCell(new Phrase("Amount", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell10.HorizontalAlignment = Element.ALIGN_CENTER;
                table5.AddCell(tcell10);

                document.Add(table5);
               // int rowCount = 1;
                //int recordLimit = 5;

                //List<> list = new List<>();
                //foreach (var item in list)
                //{
                //    if (rowCount >= recordLimit)
                //    {
                //        document.Add(table3);
                //        document.NewPage();
                //        Rectangle border1 = new Rectangle(document.PageSize);
                //        border1.Left += document.LeftMargin;
                //        border1.Right -= document.RightMargin;
                //        border1.Top -= document.TopMargin;
                //        border1.Bottom += document.BottomMargin;
                //        border1.BorderColor = BaseColor.BLACK; // Set the border color and width
                //        border1.BorderWidth = 1f;
                //        border1.Border = Rectangle.BOX;

                //        // Add the border to the document
                //        PdfContentByte content1 = writer.DirectContent;
                //        content1.Rectangle(border1);
                //        content1.Stroke();

                //        table.DeleteBodyRows();
                //        rowCount = 1;
                //    }
                   PdfPTable table6 = new PdfPTable(10);
                   table6.SetWidths(new float[] { 0.5f, 4f, 1f, 2f, 0.8f, 0.6f, 1f, 1f, 1f, 2f });
                   table6.WidthPercentage = 100;

                    PdfPCell tcell11 = new PdfPCell(new Phrase($"  1  ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell11.Border = Rectangle.NO_BORDER;
                    tcell11.HorizontalAlignment = Element.ALIGN_CENTER;
                   
                    table6.AddCell(tcell11);

                    PdfPCell tcell12 = new PdfPCell(new Phrase($"DG00432P - GREY FABRIC - 100% COTTON   ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell12.Border = Rectangle.NO_BORDER;
                    tcell12.HorizontalAlignment = Element.ALIGN_CENTER;
                   
                    table6.AddCell(tcell12);

                    PdfPCell tcell13 = new PdfPCell(new Phrase($"52081190 ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell13.Border = Rectangle.NO_BORDER;
                    tcell13.HorizontalAlignment = Element.ALIGN_CENTER;
                    //tcell11.BorderWidthBottom = 1;
                    tcell13.FixedHeight = 40f;
                    //tcell11.PaddingBottom = 375;
                    table6.AddCell(tcell13);

                    PdfPCell tcell14 = new PdfPCell(new Phrase($"BB-3708 TO BB-3708 ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell14.Border = Rectangle.NO_BORDER;
                    tcell14.HorizontalAlignment = Element.ALIGN_CENTER;
                    // tcell12.BorderWidthBottom = 1;
                    //tcell14.FixedHeight = 40f;
                    table6.AddCell(tcell14);

                    PdfPCell tcell15 = new PdfPCell(new Phrase($" 1 ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell15.Border = Rectangle.NO_BORDER;
                    tcell15.HorizontalAlignment = Element.ALIGN_CENTER;
                    // tcell13.BorderWidthBottom = 1;
                    tcell15.FixedHeight = 40f;
                    table6.AddCell(tcell15);

                    PdfPCell tcell16 = new PdfPCell(new Phrase($" 1 ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                   // tcell16.Border = Rectangle.NO_BORDER;
                    tcell16.HorizontalAlignment = Element.ALIGN_CENTER;
                    //tcell14.BorderWidthBottom = 1;
                    tcell14.FixedHeight = 40f;
                    table6.AddCell(tcell16);

                    PdfPCell tcell17 = new PdfPCell(new Phrase($"10,064.20 ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell17.Border = Rectangle.NO_BORDER;
                    tcell17.HorizontalAlignment = Element.ALIGN_CENTER;
                    //tcell15.BorderWidthBottom = 1;
                    tcell17.FixedHeight = 40f;
                    table6.AddCell(tcell17);

                    PdfPCell tcell18 = new PdfPCell(new Phrase($"MTRS ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell18.Border = Rectangle.NO_BORDER;
                    tcell18.HorizontalAlignment = Element.ALIGN_CENTER;
                    //tcell16.BorderWidthBottom = 1;
                    tcell18.FixedHeight = 40f;
                    table6.AddCell(tcell18);

                    PdfPCell tcell19 = new PdfPCell(new Phrase($"58.00 ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell19.Border = Rectangle.NO_BORDER;
                    tcell19.HorizontalAlignment = Element.ALIGN_CENTER;
                    //tcell15.BorderWidthBottom = 1;
                    tcell19.FixedHeight = 40f;
                    table6.AddCell(tcell19);

                    PdfPCell tcell20 = new PdfPCell(new Phrase($"5,83,723.60 ", FontFactory.GetFont("Arial", 8, Font.NORMAL)));
                    //tcell20.Border = Rectangle.NO_BORDER;
                    tcell20.HorizontalAlignment = Element.ALIGN_CENTER;
                    //tcell16.BorderWidthBottom = 1;
                    tcell20.FixedHeight = 40f;
                    table6.AddCell(tcell20);

                    //rowCount++;
                

                    document.Add(table6);

                PdfPTable table7 = new PdfPTable(7);
                table7.SetWidths(new float[] {7.5f, 0.8f, 0.6f, 1f, 1f, 1f, 2f });
                table7.WidthPercentage = 100;

                PdfPCell tcell21 = new PdfPCell(new Phrase($"Total : ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell21.HorizontalAlignment = Element.ALIGN_RIGHT;
                table7.AddCell(tcell21);

                PdfPCell tcell22 = new PdfPCell(new Phrase("1", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell22.HorizontalAlignment = Element.ALIGN_CENTER;
                table7.AddCell(tcell22);

                PdfPCell tcell23 = new PdfPCell(new Phrase("1", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell23.HorizontalAlignment = Element.ALIGN_CENTER;
                table7.AddCell(tcell23);

                PdfPCell tcell24 = new PdfPCell(new Phrase(" 10,064.20", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell24.HorizontalAlignment = Element.ALIGN_CENTER;
                table7.AddCell(tcell24);

                PdfPCell tcell25 = new PdfPCell(new Phrase("   ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell25.HorizontalAlignment = Element.ALIGN_CENTER;
                table7.AddCell(tcell25);

                PdfPCell tcell26 = new PdfPCell(new Phrase("  ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell26.HorizontalAlignment = Element.ALIGN_CENTER;
                table7.AddCell(tcell26);

                PdfPCell tcell27 = new PdfPCell(new Phrase("5,83,723.60 ", FontFactory.GetFont("Arial", 9, Font.BOLD)));
                tcell27.HorizontalAlignment = Element.ALIGN_CENTER;
                table7.AddCell(tcell27);

                document.Add(table7);

                //Add Table
                PdfPTable table8 = new PdfPTable(1);
                table8.WidthPercentage = 100;

                PdfPCell addchangCell = new PdfPCell(new Phrase($"Additional Changes : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                addchangCell.Border = Rectangle.NO_BORDER;
                //addchangCell.BorderWidthBottom = 0.5f;
                table8.AddCell(addchangCell);

                //document.Add(table8);

                PdfPTable table9 = new PdfPTable(1);
                table9.WidthPercentage = 100;
                
                PdfPCell lesschangCell = new PdfPCell(new Phrase($"Less Changes : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                lesschangCell.Border = Rectangle.NO_BORDER;
                table9.AddCell(lesschangCell);

                PdfPTable table10 = new PdfPTable(2);
                table10.WidthPercentage = 100;

                table10.SetWidths(new float[] { 2f, 2f });
              
                PdfPCell addCell = new PdfPCell(new Phrase($"Add                    :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                addCell.Border = Rectangle.NO_BORDER;
                //addCell.BorderWidthLeft = 1f;
                //addCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table10.AddCell(addCell);


                PdfPCell ACell1 = new PdfPCell(new Phrase($" - ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell1.Border = Rectangle.NO_BORDER;
                ACell1.BorderWidthLeft = 0.5f;
                ACell1.HorizontalAlignment = Element.ALIGN_RIGHT; 
                table10.AddCell(ACell1);

                PdfPCell LessCell = new PdfPCell(new Phrase($"Less                    :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                LessCell.Border = Rectangle.NO_BORDER;
                //LessCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table10.AddCell(LessCell);

                PdfPCell ACell2 = new PdfPCell(new Phrase(" -  ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell2.Border = Rectangle.NO_BORDER;
                ACell2.BorderWidthLeft = 0.5f;
                ACell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                table10.AddCell(ACell2);

                PdfPCell Totalcell = new PdfPCell(new Phrase($"Total                  :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                Totalcell.Border = Rectangle.NO_BORDER;
                table10.AddCell(Totalcell);

                PdfPCell ACell3 = new PdfPCell(new Phrase("5,83,723.60 ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell3.Border = Rectangle.NO_BORDER;
                ACell3.BorderWidthLeft = 0.5f;
                ACell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                table10.AddCell(ACell3);

                PdfPCell igstcell = new PdfPCell(new Phrase($"IGST                   : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                igstcell.Border = Rectangle.NO_BORDER;
                table10.AddCell(igstcell);

                PdfPCell ACell4 = new PdfPCell(new Phrase(" -  ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell4.Border = Rectangle.NO_BORDER;
                ACell4.BorderWidthLeft = 0.5f;
                ACell4.HorizontalAlignment = Element.ALIGN_RIGHT; 
                table10.AddCell(ACell4);

                PdfPCell cgstcell = new PdfPCell(new Phrase($"CGST 2.5%         : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                cgstcell.Border = Rectangle.NO_BORDER;
                table10.AddCell(cgstcell);

                PdfPCell ACell5 = new PdfPCell(new Phrase("14,593.9 ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell5.Border = Rectangle.NO_BORDER;
                ACell5.BorderWidthLeft = 0.5f;
                ACell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                table10.AddCell(ACell5);

                PdfPCell sgstcell = new PdfPCell(new Phrase($"SGST 2.5%         : ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                sgstcell.Border = Rectangle.NO_BORDER;
                table10.AddCell(sgstcell);

                PdfPCell ACell6 = new PdfPCell(new Phrase("14,593.9 ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell6.Border = Rectangle.NO_BORDER;
                ACell6.BorderWidthLeft = 0.5f;
                ACell6.HorizontalAlignment = Element.ALIGN_RIGHT; 
                table10.AddCell(ACell6);

                PdfPCell TCScell = new PdfPCell(new Phrase($"TCS Amount       :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                TCScell.Border = Rectangle.NO_BORDER;
                table10.AddCell(TCScell);

                PdfPCell ACell7 = new PdfPCell(new Phrase("- ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell7.Border = Rectangle.NO_BORDER;
                ACell7.BorderWidthLeft = 0.5f;
                ACell7.HorizontalAlignment = Element.ALIGN_RIGHT;
                table10.AddCell(ACell7);

                PdfPCell Rocell = new PdfPCell(new Phrase($"R/O                      :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                Rocell.Border = Rectangle.NO_BORDER;
                table10.AddCell(Rocell);

                PdfPCell ACell8 = new PdfPCell(new Phrase("0.22 ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell8.Border = Rectangle.NO_BORDER;
                ACell8.BorderWidthLeft = 0.5f;
                ACell8.HorizontalAlignment = Element.ALIGN_RIGHT;
                table10.AddCell(ACell8);

                PdfPCell Gtotalcell = new PdfPCell(new Phrase($"Grand Total         :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                Gtotalcell.Border = Rectangle.NO_BORDER;
                table10.AddCell(Gtotalcell);

                PdfPCell ACell9 = new PdfPCell(new Phrase("6,12,910.0 ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                ACell9.Border = Rectangle.NO_BORDER;
                ACell9.BorderWidthLeft = 0.5f;
                ACell9.HorizontalAlignment = Element.ALIGN_RIGHT;
                table10.AddCell(ACell9);

               
                // document.Add(table10);

                PdfPTable CombinedPurVenTbl2 = new PdfPTable(3);
                CombinedPurVenTbl2.WidthPercentage = 100;
                //CombinedPurVenTbl2.SpacingBefore = 3;
                CombinedPurVenTbl2.DefaultCell.Border = Rectangle.NO_BORDER; // No border for cells by default
                CombinedPurVenTbl2.SpacingAfter = 0; // Add some spacing after the table
                CombinedPurVenTbl2.DefaultCell.BorderWidthBottom = 0.5f;

                CombinedPurVenTbl2.SetWidths(new float[] { 5.5f, 4.4f ,4f });
                CombinedPurVenTbl2.DefaultCell.Border = Rectangle.NO_BORDER; // Border for the entire table
                //                                                            //CombinedPurVenTbl.DefaultCell.BorderWidth = 0.5f; // Border width
                //                                                            //AmountTbl.AddCell(tableMiddle);
                PdfPCell addchangCell1 = new PdfPCell();
               // addchangCell1.Border = Rectangle.NO_BORDER;
                addchangCell1.BorderWidthRight = 0.5f;
               // addchangCell1.BorderWidthTop = 0.5f;
                //InvoiceCell.Colspan = 1; // Span across the number of columns in table1
                addchangCell1.AddElement(table8);
                CombinedPurVenTbl2.AddCell(addchangCell1);

                PdfPCell lesschangCell1 = new PdfPCell();
               // lesschangCell1.Border = Rectangle.NO_BORDER;
                lesschangCell1.BorderWidthRight = 0.5f;
               // lesschangCell1.BorderWidthTop = 0.5f;
                //InvoiceCell.Colspan = 1; // Span across the number of columns in table1
                lesschangCell1.AddElement(table9);
                CombinedPurVenTbl2.AddCell(lesschangCell1);

                PdfPCell totalcell = new PdfPCell();
                totalcell.BorderWidthRight = 0.5f;
                ////ConsigneeTblCell.Colspan = 1; // Span across the number of columns in table1
                totalcell.AddElement(table10);
                CombinedPurVenTbl2.AddCell(totalcell);

                document.Add(CombinedPurVenTbl2);

                Paragraph para2 = new Paragraph($"Amount Chargeable (in words) :  ", FontFactory.GetFont("Arial", 10, Font.BOLD));
                para2.PaddingTop = 0;
                para2.SpacingBefore = -5;
                para2.Alignment = Element.ALIGN_MIDDLE;
                para2.SpacingAfter = 5;
                document.Add(para2);


                // Calculate the position for the footer

                PdfContentByte cb5 = writer.DirectContent;

                Rectangle pageSize5 = document.PageSize;

                float footerY5 = pageSize5.GetBottom(10);

                float footerX5 = pageSize5.GetLeft(15);

                float footerWidth5 = pageSize5.Width - 30; // 50 margins on both sides

                float footerHeight5 = 200; // Adjust based on the height of your footer content

                // Draw a horizontal line above the footer content

                cb5.MoveTo(footerX5, footerY5 + footerHeight5 - 10); // Adjust the height accordingly

                //cb.LineTo(footerX + footerWidth, footerY + 100 + 10);

                cb5.Stroke();

                PdfPTable table11 = new PdfPTable(1);
                table11.WidthPercentage = 100;
                table11.SpacingBefore = 0;

                PdfPCell tcell01 = new PdfPCell(new Phrase($"Terms and Conditions :\n\n  ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                tcell01.Border = Rectangle.NO_BORDER;
                //tcell01.BorderWidthTop = 1;
                table11.AddCell(tcell01);

                PdfPCell tcell02 = new PdfPCell(new Phrase($"1) Transit Insurance: TATA Aig Insurance Policy No # 2001/269236/01/000 # Period 15/11/2023 to 14/11/2024. \n ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tcell02.Border = Rectangle.NO_BORDER;
                table11.AddCell(tcell02);

                PdfPCell tcell03 = new PdfPCell(new Phrase($"2) Payment must be made by A/C Payee Cheque/RTGS Only.\n ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tcell03.Border = Rectangle.NO_BORDER;
                table11.AddCell(tcell03);

                PdfPCell tcell04 = new PdfPCell(new Phrase($"3) Overdue interest will be charged @24% per annum. \n ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tcell04.Border = Rectangle.NO_BORDER;
                table11.AddCell(tcell04);

                PdfPCell tcell05 = new PdfPCell(new Phrase($"4) Our responsibility ceases once goods leave our godowns. \n ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tcell05.Border = Rectangle.NO_BORDER;
                table11.AddCell(tcell05);

                PdfPCell tcell06 = new PdfPCell(new Phrase($"5) Subject to ICHALKARANJI Jurisdiction only. \n ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tcell06.Border = Rectangle.NO_BORDER;
                table11.AddCell(tcell06);
                // document.Add(table11);

                //Another table 
                PdfPTable table12 = new PdfPTable(2);
                table12.SetWidths(new float[] {1.3f,2f });
                
                table12.WidthPercentage = 100;
                table12.SpacingBefore = 0;

                PdfPCell tcell07 = new PdfPCell(new Phrase($"Company's Bank Details : \n\n  ", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                tcell07.Border = Rectangle.NO_BORDER;
               // tcell07.BorderWidthTop = 1;
                table12.AddCell(tcell07);

                PdfPCell emptycell9 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                emptycell9.Border = Rectangle.NO_BORDER;
                //emptycell9.BorderWidthTop = 0.5f;
                //emptycell9.BorderWidthBottom = 0.5f;
                emptycell9.PaddingBottom = 5;
                table12.AddCell(emptycell9);

                PdfPCell tcell08 = new PdfPCell(new Phrase($"Bank Name                      :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tcell08.Border = Rectangle.NO_BORDER;
                table12.AddCell(tcell08);

                PdfPCell Bcell1 = new PdfPCell(new Phrase($"CANARA BANK - ODCC ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                Bcell1.Border = Rectangle.NO_BORDER;
                table12.AddCell(Bcell1);

                PdfPCell tcell09 = new PdfPCell(new Phrase($"A/c No                           :   ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tcell09.Border = Rectangle.NO_BORDER;
                table12.AddCell(tcell09);

                PdfPCell Bcell2 = new PdfPCell(new Phrase($"8529261000010 ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                Bcell2.Border = Rectangle.NO_BORDER; 
                table12.AddCell(Bcell2);

                PdfPCell tcell010 = new PdfPCell(new Phrase($"Branch & IFSC Code      :", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                tcell010.Border = Rectangle.NO_BORDER;
                table12.AddCell(tcell010);

                PdfPCell Bcell3 = new PdfPCell(new Phrase($"ICHALKARANJIN & CNRB0015231 ", FontFactory.GetFont("Arial", 9, Font.NORMAL)));
                Bcell3.Border = Rectangle.NO_BORDER;
                table12.AddCell(Bcell3);
                //document.Add(table12);

                PdfPTable table13 = new PdfPTable(1);
                table13.WidthPercentage = 100;

                PdfPCell kencell = new PdfPCell(new Phrase(" For Ken Enterprises Pvt.Ltd.", FontFactory.GetFont("Arial", 10, Font.NORMAL)));
                //Authcell.Border = Rectangle.NO_BORDER;
                kencell.Border = Rectangle.NO_BORDER;
                kencell.HorizontalAlignment = Element.ALIGN_RIGHT; 
                kencell.PaddingBottom = 8;
                table13.AddCell(kencell);


                PdfPCell Authcell = new PdfPCell(new Phrase(" Authorized Signatory  ", FontFactory.GetFont("Arial", 10, Font.NORMAL)));
                Authcell.Border = Rectangle.NO_BORDER;
                Authcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                Authcell.PaddingTop = 30;
                table13.AddCell(Authcell);

                // document.Add(table13);

                // PdfPTable CombinedPurVenTb3 = new PdfPTable(2);
                // CombinedPurVenTb3.WidthPercentage = 100;
                // CombinedPurVenTb3.SpacingBefore = 3;
                // CombinedPurVenTb3.DefaultCell.Border = Rectangle.NO_BORDER; // No border for cells by default
                // CombinedPurVenTb3.SpacingAfter = 0; // Add some spacing after the table

                // CombinedPurVenTb3.SetWidths(new float[] { 2.5f, 2.5f });
                // CombinedPurVenTb3.DefaultCell.Border = Rectangle.NO_BORDER; // Border for the entire table
                //                                                             //CombinedPurVenTbl.DefaultCell.BorderWidth = 0.5f; // Border width
                //                                                             //AmountTbl.AddCell(tableMiddle);
                // PdfPCell leftTblCell = new PdfPCell();
                // leftTblCell.Border = Rectangle.NO_BORDER;
                // leftTblCell.BorderWidthRight = 0.5f;
                // leftTblCell.BorderWidthTop = 0.5f;
                // //buyerTblCell.Colspan = 1; // Span across the number of columns in table1
                // leftTblCell.AddElement(table11);
                // CombinedPurVenTb3.AddCell(leftTblCell);

                // PdfPTable rightTblTable = new PdfPTable(1);

                // //add 2 table
                // PdfPCell rightTblCell1 = new PdfPCell();
                // rightTblCell1.Border = Rectangle.NO_BORDER;
                // rightTblCell1.BorderWidthTop = 0.5f;
                // rightTblCell1.BorderWidthRight = 0.5f;
                // //rightTblCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                // //ConsigneeTblCell.Colspan = 1; // Span across the number of columns in table1
                // rightTblCell1.AddElement(table12);
                // CombinedPurVenTb3.AddCell(rightTblCell1);

                // PdfPCell rightTblCell2 = new PdfPCell();
                // rightTblCell2.Border = Rectangle.NO_BORDER;
                // //rightTblCell2.BorderWidthTop = 0.5f;
                // rightTblCell2.BorderWidthRight = 0.5f;
                //// rightTblCell2.HorizontalAlignment = Element.ALIGN_LEFT;
                // // orderDetailsTblCell.Colspan = 1; // Span across the number of columns in table1
                // rightTblCell2.AddElement(table13);
                // CombinedPurVenTb3.AddCell(rightTblCell2);


                // // Add the nested table to the second column of the main table
                // PdfPCell rightCell = new PdfPCell(rightTblTable);
                // rightCell.Border = Rectangle.NO_BORDER;
                // CombinedPurVenTb3.AddCell(rightCell);

                // // Add the main table to the document
                // document.Add(CombinedPurVenTb3);

                // Create the main table with 2 columns
                PdfPTable mainTable = new PdfPTable(2);
                mainTable.WidthPercentage = 100;
                mainTable.SpacingBefore = 3;
                mainTable.SetWidths(new float[] { 5f, 5f });
                mainTable.DefaultCell.Border = Rectangle.NO_BORDER;

                // Add table11 to the first column
                PdfPCell leftCell = new PdfPCell();
                leftCell.Border = Rectangle.NO_BORDER;
               // leftCell.BorderWidthRight = 0.5f;
                leftCell.BorderWidthTop = 0.5f;
                leftCell.AddElement(table11);
                mainTable.AddCell(leftCell);

                // Create a nested table to hold table12 and table13
                PdfPTable rightNestedTable = new PdfPTable(1);
                rightNestedTable.WidthPercentage = 100;
                rightNestedTable.DefaultCell.Border = Rectangle.NO_BORDER;

                // Add table12 to the nested table
                PdfPCell rightNestedCell1 = new PdfPCell();
                rightNestedCell1.Border = Rectangle.NO_BORDER;
                rightNestedCell1.BorderWidthTop = 0.5f;
               // rightNestedCell1.BorderWidthRight = 0.5f;
                rightNestedCell1.AddElement(table12);
                rightNestedTable.AddCell(rightNestedCell1);

                // Add table13 to the nested table
                PdfPCell rightNestedCell2 = new PdfPCell();
                rightNestedCell2.Border = Rectangle.NO_BORDER;
                rightNestedCell2.BorderWidthRight = 0.5f;
                rightNestedCell2.BorderWidthLeft = 0.5f;
                rightNestedCell2.BorderWidthTop = 0.5f;
                rightNestedCell2.AddElement(table13);
                rightNestedTable.AddCell(rightNestedCell2);

                // Add the nested table to the second column of the main table
                PdfPCell rightCell = new PdfPCell(rightNestedTable);
                rightCell.Border = Rectangle.NO_BORDER;
                mainTable.AddCell(rightCell);

                mainTable.TotalWidth = footerWidth5;

                mainTable.WriteSelectedRows(0, -1, footerX5, footerY5 + 166, cb5);


                // Add the main table to the document
               // document.Add(mainTable);


                document.Close();
                writer.Close();
                // Return the PDF as a byte array
                byte[] pdfBytes = memoryStream.ToArray();

                // Return the PDF as a file
               // return File(pdfBytes, "application/pdf", "TaxInvicePDFGenerator.pdf");  // Return the PDF as a file
                return File(pdfBytes, "application/pdf", "GeneratedDocument.pdf");

            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
