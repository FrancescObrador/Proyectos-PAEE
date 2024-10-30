using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using DataAccess;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Snippets.Font;
using MigraDoc.Rendering;

namespace ExportPDF
{
    public class ProvinciasPDF
    {
        public bool SavePDF(List<Provincia> provincias, string path)
        {
			try
			{
                if (PdfSharp.Capabilities.Build.IsCoreBuild)
                {
                    GlobalFontSettings.FontResolver = new FailsafeFontResolver();
                }

                Document doc = new Document();
                Section section = doc.AddSection();
                Paragraph paragraph = section.AddParagraph();
                paragraph.AddText("Provincias");
                paragraph.Format.Font.Size = 20;
                paragraph.Format.Font.Bold = true;
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                section.AddParagraph();

                Table table = section.AddTable();
                table.Style = "table";

                Column column = table.AddColumn("2cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column = table.AddColumn("10cm");
                column.Format.Alignment = ParagraphAlignment.Left;

                Row row = table.AddRow();
                row.HeadingFormat = true;

                row.Format.Font.Bold = true;
                row.Shading.Color = Colors.LightCoral;
                row.Cells[0].AddParagraph("ID").Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].AddParagraph("Provincias");

                foreach(Provincia p in provincias)
                {
                    row = table.AddRow();
                    row.Cells[0].AddParagraph(p.ID.ToString()).Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[1].AddParagraph(p.Nombre);
                }

                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer
                {
                    Document = doc,
                    PdfDocument = new PdfDocument()
                };

                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(path);
                return true;
            }
            catch (Exception)
			{
                return false;
			}
        }
    }

}
