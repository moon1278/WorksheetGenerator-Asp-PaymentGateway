using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using M = DocumentFormat.OpenXml.Math;
using Ovml = DocumentFormat.OpenXml.Vml.Office;
using V = DocumentFormat.OpenXml.Vml;
using W14 = DocumentFormat.OpenXml.Office2010.Word;
using W15 = DocumentFormat.OpenXml.Office2013.Word;
using A = DocumentFormat.OpenXml.Drawing;
using Thm15 = DocumentFormat.OpenXml.Office2013.Theme;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorksheetGenerator.Data;
using WorksheetGenerator.Helper.Word.Element;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using SectionProperties = DocumentFormat.OpenXml.Wordprocessing.SectionProperties;

namespace WorksheetGenerator.Helper.Word
{
    public class WordHelper
    {
        #region Header
        // Generates content of part.
        public void AddWorksheetNameHeader(HeaderPart part, string worksheetName, int classId)
        {
            Header header1 = new Header() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 w16se w16cid w16 w16cex w16sdtdh wp14" } };
            header1.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
            header1.AddNamespaceDeclaration("cx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
            header1.AddNamespaceDeclaration("cx1", "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
            header1.AddNamespaceDeclaration("cx2", "http://schemas.microsoft.com/office/drawing/2015/10/21/chartex");
            header1.AddNamespaceDeclaration("cx3", "http://schemas.microsoft.com/office/drawing/2016/5/9/chartex");
            header1.AddNamespaceDeclaration("cx4", "http://schemas.microsoft.com/office/drawing/2016/5/10/chartex");
            header1.AddNamespaceDeclaration("cx5", "http://schemas.microsoft.com/office/drawing/2016/5/11/chartex");
            header1.AddNamespaceDeclaration("cx6", "http://schemas.microsoft.com/office/drawing/2016/5/12/chartex");
            header1.AddNamespaceDeclaration("cx7", "http://schemas.microsoft.com/office/drawing/2016/5/13/chartex");
            header1.AddNamespaceDeclaration("cx8", "http://schemas.microsoft.com/office/drawing/2016/5/14/chartex");
            header1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            header1.AddNamespaceDeclaration("aink", "http://schemas.microsoft.com/office/drawing/2016/ink");
            header1.AddNamespaceDeclaration("am3d", "http://schemas.microsoft.com/office/drawing/2017/model3d");
            header1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            header1.AddNamespaceDeclaration("oel", "http://schemas.microsoft.com/office/2019/extlst");
            header1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            header1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            header1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            header1.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
            header1.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            header1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
            header1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            header1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            header1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
            header1.AddNamespaceDeclaration("w16cex", "http://schemas.microsoft.com/office/word/2018/wordml/cex");
            header1.AddNamespaceDeclaration("w16cid", "http://schemas.microsoft.com/office/word/2016/wordml/cid");
            header1.AddNamespaceDeclaration("w16", "http://schemas.microsoft.com/office/word/2018/wordml");
            header1.AddNamespaceDeclaration("w16sdtdh", "http://schemas.microsoft.com/office/word/2020/wordml/sdtdatahash");
            header1.AddNamespaceDeclaration("w16se", "http://schemas.microsoft.com/office/word/2015/wordml/symex");
            header1.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            header1.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
            header1.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
            header1.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00384E3B", RsidParagraphAddition = "00384E3B", RsidParagraphProperties = "00772360", RsidRunAdditionDefault = "00772360", ParagraphId = "7F142B81", TextId = "14650D98" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId2 = new ParagraphStyleId() { Val = "Kopfzeile" };

            Tabs tabs1 = new Tabs();
            TabStop tabStop1 = new TabStop() { Val = TabStopValues.Clear, Position = 9072 };
            TabStop tabStop2 = new TabStop() { Val = TabStopValues.Right, Position = 9356 };

            tabs1.Append(tabStop1);
            tabs1.Append(tabStop2);

            paragraphProperties2.Append(paragraphStyleId2);
            paragraphProperties2.Append(tabs1);

            Run run1 = new Run();
            Text text1 = new Text();
            text1.Text = "Arbeitsblatt: " + worksheetName;

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };

            runProperties1.Append(runFonts12);
            run1.Append(runProperties1);


            run1.Append(text1);

            string _className = "Fehler";

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Class c = db.Classes.ToList().Where(x => x.Id == classId).FirstOrDefault();
                if (c != null)
                {
                    _className = c.Name;
                }
            }

            Run run3 = new Run();
            TabChar tabChar2 = new TabChar();
            Text text2 = new Text();
            text2.Text = "Klasse: " + _className;

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts123 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };

            runProperties2.Append(runFonts123);
            run3.Append(runProperties2);

            run3.Append(tabChar2);
            run3.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run1);
            paragraph2.Append(run3);

            header1.Append(paragraph2);

            part.Header = header1;


        }

        // Generates content of part.
        public void AddSolutionHeader(HeaderPart part, string worksheetName, int classId)
        {
            Header header1 = new Header() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 w16se w16cid w16 w16cex w16sdtdh wp14" } };
            header1.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
            header1.AddNamespaceDeclaration("cx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
            header1.AddNamespaceDeclaration("cx1", "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
            header1.AddNamespaceDeclaration("cx2", "http://schemas.microsoft.com/office/drawing/2015/10/21/chartex");
            header1.AddNamespaceDeclaration("cx3", "http://schemas.microsoft.com/office/drawing/2016/5/9/chartex");
            header1.AddNamespaceDeclaration("cx4", "http://schemas.microsoft.com/office/drawing/2016/5/10/chartex");
            header1.AddNamespaceDeclaration("cx5", "http://schemas.microsoft.com/office/drawing/2016/5/11/chartex");
            header1.AddNamespaceDeclaration("cx6", "http://schemas.microsoft.com/office/drawing/2016/5/12/chartex");
            header1.AddNamespaceDeclaration("cx7", "http://schemas.microsoft.com/office/drawing/2016/5/13/chartex");
            header1.AddNamespaceDeclaration("cx8", "http://schemas.microsoft.com/office/drawing/2016/5/14/chartex");
            header1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            header1.AddNamespaceDeclaration("aink", "http://schemas.microsoft.com/office/drawing/2016/ink");
            header1.AddNamespaceDeclaration("am3d", "http://schemas.microsoft.com/office/drawing/2017/model3d");
            header1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            header1.AddNamespaceDeclaration("oel", "http://schemas.microsoft.com/office/2019/extlst");
            header1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            header1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            header1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            header1.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
            header1.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            header1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
            header1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            header1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            header1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
            header1.AddNamespaceDeclaration("w16cex", "http://schemas.microsoft.com/office/word/2018/wordml/cex");
            header1.AddNamespaceDeclaration("w16cid", "http://schemas.microsoft.com/office/word/2016/wordml/cid");
            header1.AddNamespaceDeclaration("w16", "http://schemas.microsoft.com/office/word/2018/wordml");
            header1.AddNamespaceDeclaration("w16sdtdh", "http://schemas.microsoft.com/office/word/2020/wordml/sdtdatahash");
            header1.AddNamespaceDeclaration("w16se", "http://schemas.microsoft.com/office/word/2015/wordml/symex");
            header1.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            header1.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
            header1.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
            header1.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");

            Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00384E3B", RsidParagraphAddition = "00384E3B", RsidParagraphProperties = "00772360", RsidRunAdditionDefault = "00772360", ParagraphId = "7F142B81", TextId = "14650D98" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId2 = new ParagraphStyleId() { Val = "Kopfzeile" };

            Tabs tabs1 = new Tabs();
            TabStop tabStop1 = new TabStop() { Val = TabStopValues.Clear, Position = 9072 };
            TabStop tabStop2 = new TabStop() { Val = TabStopValues.Right, Position = 9356 };

            tabs1.Append(tabStop1);
            tabs1.Append(tabStop2);

            paragraphProperties2.Append(paragraphStyleId2);
            paragraphProperties2.Append(tabs1);

            Run run1 = new Run();
            Text text1 = new Text();
            text1.Text = "Lösungsblatt für: " + worksheetName;

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };

            runProperties1.Append(runFonts12);
            run1.Append(runProperties1);


            run1.Append(text1);

            string _className = "Fehler";

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Class c = db.Classes.ToList().Where(x => x.Id == classId).FirstOrDefault();
                if (c != null)
                {
                    _className = c.Name;
                }
            }

            Run run3 = new Run();
            TabChar tabChar2 = new TabChar();
            Text text2 = new Text();
            text2.Text = "Klasse: " + _className;

            RunProperties runProperties2 = new RunProperties();
            RunFonts runFonts123 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };

            runProperties2.Append(runFonts123);
            run3.Append(runProperties2);

            run3.Append(tabChar2);
            run3.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run1);
            paragraph2.Append(run3);

            header1.Append(paragraph2);

            part.Header = header1;


        }

        #endregion

        public void AddTextParagraph(MainDocumentPart part, Document doc, string taskName, bool bold, string font, string fontSize = "11", bool italic = false)
        {
            int tempFontSize = int.Parse(fontSize)*2;
            fontSize = "" + tempFontSize;

            Body body1 = new Body();

            Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00936DFE", RsidParagraphAddition = "009361AA", RsidRunAdditionDefault = "00936DFE", ParagraphId = "300DEAA2", TextId = "12C5168A" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
             
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            Run run1 = new Run();

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts12 = new RunFonts() { Ascii = font, HighAnsi = font, ComplexScript = font };

            runProperties1.Append(runFonts12);

            if (bold)
            {
                Bold bold2 = new Bold();
                BoldComplexScript boldComplexScript2 = new BoldComplexScript();

                runProperties1.Append(bold2);
                runProperties1.Append(boldComplexScript2);
            }

            FontSize fontSize2 = new FontSize() { Val = fontSize };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = fontSize };
            runProperties1.Append(fontSize2);
            runProperties1.Append(fontSizeComplexScript2);

            if (italic)
            {
                Italic italic2 = new Italic();
                ItalicComplexScript italicComplexScript2 = new ItalicComplexScript();

                runProperties1.Append(italic2);
                runProperties1.Append(italicComplexScript2);
            }
       

            Text text1 = new Text();
            text1.Text = taskName;

            run1.Append(runProperties1);
            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            SectionProperties sectionProperties1 = new SectionProperties() { RsidRPr = "00936DFE", RsidR = "009361AA" };
            PageSize pageSize1 = new PageSize() { Width = (UInt32Value)11906U, Height = (UInt32Value)16838U };
            PageMargin pageMargin1 = new PageMargin() { Top = 1417, Right = (UInt32Value)1417U, Bottom = 1134, Left = (UInt32Value)1417U, Header = (UInt32Value)708U, Footer = (UInt32Value)708U, Gutter = (UInt32Value)0U };
            Columns columns1 = new Columns() { Space = "708" };
            DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

            sectionProperties1.Append(pageSize1);
            sectionProperties1.Append(pageMargin1);
            sectionProperties1.Append(columns1);
            sectionProperties1.Append(docGrid1);

            body1.Append(paragraph1);
            body1.Append(sectionProperties1);

            doc.Append(body1);



        }
        // Generates content of part.
        public void AddBulletList(MainDocumentPart part, Document doc, List<BulletListElement> bulletList)
        {

            Body body1 = new Body();

            foreach (BulletListElement bulletListElement in bulletList)
            {

                #region paragraph1
                Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00736AB3", RsidParagraphAddition = "009361AA", RsidParagraphProperties = "0032308B", RsidRunAdditionDefault = "0032308B", ParagraphId = "0AEF3543", TextId = "157D2681" };

                ParagraphProperties paragraphProperties1 = new ParagraphProperties();
                ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "Listenabsatz" };

                NumberingProperties numberingProperties1 = new NumberingProperties();
                NumberingLevelReference numberingLevelReference1 = new NumberingLevelReference() { Val = 0 };
                NumberingId numberingId1 = new NumberingId() { Val = 1 };

                numberingProperties1.Append(numberingLevelReference1);
                numberingProperties1.Append(numberingId1);

                ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
                RunFonts runFonts11 = new RunFonts() { Ascii = bulletListElement.Font, HighAnsi = bulletListElement.Font, ComplexScript = bulletListElement.Font };

                paragraphMarkRunProperties1.Append(runFonts11);

                paragraphProperties1.Append(paragraphStyleId1);
                paragraphProperties1.Append(numberingProperties1);
                paragraphProperties1.Append(paragraphMarkRunProperties1);

                Run run1 = new Run() { RsidRunProperties = "00736AB3" };

                RunProperties runProperties1 = new RunProperties();
                RunFonts runFonts12 = new RunFonts() { Ascii = bulletListElement.Font, HighAnsi = bulletListElement.Font, ComplexScript = bulletListElement.Font };

                runProperties1.Append(runFonts12);
                Text text1 = new Text();
                text1.Text = bulletListElement.Text;

                run1.Append(runProperties1);
                run1.Append(text1);

                paragraph1.Append(paragraphProperties1);
                paragraph1.Append(run1);

                body1.Append(paragraph1);

                #endregion
            }

            #region section

            SectionProperties sectionProperties1 = new SectionProperties() { RsidR = "0032308B" };
            PageSize pageSize1 = new PageSize() { Width = (UInt32Value)11906U, Height = (UInt32Value)16838U };
            PageMargin pageMargin1 = new PageMargin() { Top = 1417, Right = (UInt32Value)1417U, Bottom = 1134, Left = (UInt32Value)1417U, Header = (UInt32Value)708U, Footer = (UInt32Value)708U, Gutter = (UInt32Value)0U };
            Columns columns1 = new Columns() { Space = "708" };
            DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

            sectionProperties1.Append(pageSize1);
            sectionProperties1.Append(pageMargin1);
            sectionProperties1.Append(columns1);
            sectionProperties1.Append(docGrid1);
            #endregion


            body1.Append(sectionProperties1);

            doc.Append(body1);

        }

        public void AddTableWithText(MainDocumentPart part, Document doc, string text, bool bold, string font)
        {
            Body body1 = new Body();

            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "TabellemithellemGitternetz" };
            TableWidth tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            TableLook tableLook1 = new TableLook() { Val = "04A0" };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "9042" };

            tableGrid1.Append(gridColumn1);

            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "005C4EC9", RsidTableRowProperties = "00EF1B7C", ParagraphId = "7F50E49E", TextId = "77777777" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "9062", Type = TableWidthUnitValues.Dxa };

            TableCellBorders tableCellBorders1 = new TableCellBorders();
            TopBorder topBorder3 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            LeftBorder leftBorder3 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            BottomBorder bottomBorder3 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
            RightBorder rightBorder3 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

            tableCellBorders1.Append(topBorder3);
            tableCellBorders1.Append(leftBorder3);
            tableCellBorders1.Append(bottomBorder3);
            tableCellBorders1.Append(rightBorder3);
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "auto" };

            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(tableCellBorders1);
            tableCellProperties1.Append(shading1);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "00320994", RsidParagraphAddition = "005C4EC9", RsidParagraphProperties = "008206E9", RsidRunAdditionDefault = "000E5034", ParagraphId = "761ADB02", TextId = "0E964C27" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            SpacingBetweenLines spacingBetweenLines10 = new SpacingBetweenLines() { Line = "480", LineRule = LineSpacingRuleValues.Auto };
            Indentation indentation1 = new Indentation() { Start = "166" };

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            RunFonts runFonts2 = new RunFonts() { Ascii = font, HighAnsi = font, ComplexScript = font };
            FontSize fontSize2 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "24" };

            paragraphMarkRunProperties1.Append(runFonts2);
            paragraphMarkRunProperties1.Append(fontSize2);
            paragraphMarkRunProperties1.Append(fontSizeComplexScript2);

            paragraphProperties5.Append(spacingBetweenLines10);
            paragraphProperties5.Append(indentation1);
            paragraphProperties5.Append(paragraphMarkRunProperties1);

            Run run5 = new Run() { RsidRunProperties = "00320994" };

            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts3 = new RunFonts() { Ascii = font, HighAnsi = font, ComplexScript = font };
            FontSize fontSize3 = new FontSize() { Val = "24" };
            FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "24" };

            runProperties1.Append(runFonts3);
            runProperties1.Append(fontSize3);
            runProperties1.Append(fontSizeComplexScript3);
            Text text1 = new Text();
            text1.Text = text;

            run5.Append(runProperties1);
            run5.Append(text1);

            paragraph5.Append(paragraphProperties5);
            paragraph5.Append(run5);

            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph5);

            tableRow1.Append(tableCell1);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            Paragraph paragraph6 = new Paragraph() { RsidParagraphMarkRevision = "00A33602", RsidParagraphAddition = "009361AA", RsidParagraphProperties = "00A33602", RsidRunAdditionDefault = "009361AA", ParagraphId = "52A36AEB", TextId = "26A40FA9" };

            SectionProperties sectionProperties1 = new SectionProperties() { RsidRPr = "00A33602", RsidR = "009361AA" };
            PageSize pageSize1 = new PageSize() { Width = (UInt32Value)11906U, Height = (UInt32Value)16838U };
            PageMargin pageMargin1 = new PageMargin() { Top = 1417, Right = (UInt32Value)1417U, Bottom = 1134, Left = (UInt32Value)1417U, Header = (UInt32Value)708U, Footer = (UInt32Value)708U, Gutter = (UInt32Value)0U };
            Columns columns1 = new Columns() { Space = "708" };
            DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };

            sectionProperties1.Append(pageSize1);
            sectionProperties1.Append(pageMargin1);
            sectionProperties1.Append(columns1);
            sectionProperties1.Append(docGrid1);

            body1.Append(table1);
            body1.Append(paragraph6);
            body1.Append(sectionProperties1);

            doc.Append(body1);


        }

        public void AddTableDynamicllyRowCol(MainDocumentPart part, Document doc, bool fillOut, string[,] data)
        {
            Body body1 = new Body();

            var tbl = new Table();

            var tblProperties = new TableProperties();
            var tblBorders = new TableBorders();

            TopBorder topBorder = new TopBorder();
            topBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            topBorder.Color = "#000000";
            tblBorders.AppendChild(topBorder);

            BottomBorder bottomBorder = new BottomBorder();
            bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            bottomBorder.Color = "#000000";
            tblBorders.AppendChild(bottomBorder);

            RightBorder rightBorder = new RightBorder();
            rightBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            rightBorder.Color = "#000000";
            tblBorders.AppendChild(rightBorder);

            LeftBorder leftBorder = new LeftBorder();
            leftBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            leftBorder.Color = "#000000";
            tblBorders.AppendChild(leftBorder);

            InsideHorizontalBorder insideHBorder = new InsideHorizontalBorder();
            insideHBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            insideHBorder.Color = "#000000";
            tblBorders.AppendChild(insideHBorder);

            InsideVerticalBorder insideVBorder = new InsideVerticalBorder();
            insideVBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            insideVBorder.Color = "#000000";
            tblBorders.AppendChild(insideVBorder);

            //width
            var width = new TableWidth();
            width.Width = "5000";//full
            width.Type = TableWidthUnitValues.Pct;

            if (fillOut)
                tblProperties.AppendChild(width);

            tblProperties.AppendChild(tblBorders);
            tbl.AppendChild(tblProperties);

            TableRow tr;
            TableCell tc;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                tr = new TableRow();
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    tc = new TableCell(new Paragraph(new Run(new Text(data[i, j]))));
                    tr.Append(tc);
                }
                tbl.AppendChild(tr);
            }

            body1.AppendChild(tbl);       

            doc.Append(body1);

        }

        public void AddTableWithNumberCount(MainDocumentPart part, Document doc, List<string> rows, string font)
        {

            Body body1 = new Body();

            Table table1 = new Table();

            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "Tabellenraster" };
            TableWidth tableWidth1 = new TableWidth() { Width = "9057", Type = TableWidthUnitValues.Auto };
            TableLook tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "358" };
            GridColumn gridColumn2 = new GridColumn() { Width = "8699" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);

            table1.Append(tableProperties1);
            table1.Append(tableGrid1);

            AddTableNumberRow(table1, font, rows);

            body1.Append(table1);

            doc.Append(body1);


        }

        private void AddTableNumberRow(Table table, string font, List<string> rowContents)
        {
            for (int i = 0; i < rowContents.Count; i++)
            {
                TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "00F72565", RsidTableRowProperties = "00F72565", ParagraphId = "7B432EFF", TextId = "29C2B28E" };

                TableRowProperties tableRowProperties1 = new TableRowProperties();
                TableRowHeight tableRowHeight1 = new TableRowHeight() { Val = (UInt32Value)416U };

                tableRowProperties1.Append(tableRowHeight1);

                TableCell tableCell1 = new TableCell();

                TableCellProperties tableCellProperties1 = new TableCellProperties();
                TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "358", Type = TableWidthUnitValues.Dxa };

                TableCellBorders tableCellBorders1 = new TableCellBorders();
                TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
                LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
                BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
                RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

                tableCellBorders1.Append(topBorder1);
                tableCellBorders1.Append(leftBorder1);
                tableCellBorders1.Append(bottomBorder1);
                tableCellBorders1.Append(rightBorder1);

                tableCellProperties1.Append(tableCellWidth1);
                tableCellProperties1.Append(tableCellBorders1);

                Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "00CF6226", RsidParagraphAddition = "00F72565", RsidParagraphProperties = "00CF6226", RsidRunAdditionDefault = "00F72565", ParagraphId = "69B8320F", TextId = "6F532FB9" };

                ParagraphProperties paragraphProperties1 = new ParagraphProperties();
                Justification justification1 = new Justification() { Val = JustificationValues.Center };

                ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
                RunFonts runFonts1 = new RunFonts() { Ascii = font, HighAnsi = font, ComplexScript = font };
                FontSize fontSize1 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties1.Append(runFonts1);
                paragraphMarkRunProperties1.Append(fontSize1);
                paragraphMarkRunProperties1.Append(fontSizeComplexScript1);

                paragraphProperties1.Append(justification1);
                paragraphProperties1.Append(paragraphMarkRunProperties1);

                Run run1 = new Run() { RsidRunProperties = "00CF6226" };

                RunProperties runProperties1 = new RunProperties();
                RunFonts runFonts2 = new RunFonts() { Ascii = font, HighAnsi = font, ComplexScript = font };
                FontSize fontSize2 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "24" };

                runProperties1.Append(runFonts2);
                runProperties1.Append(fontSize2);
                runProperties1.Append(fontSizeComplexScript2);
                Text text1 = new Text();
                text1.Text = "" + (i + 1);

                run1.Append(runProperties1);
                run1.Append(text1);

                paragraph1.Append(paragraphProperties1);
                paragraph1.Append(run1);

                tableCell1.Append(tableCellProperties1);
                tableCell1.Append(paragraph1);

                TableCell tableCell2 = new TableCell();

                TableCellProperties tableCellProperties2 = new TableCellProperties();
                TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "8699", Type = TableWidthUnitValues.Dxa };

                TableCellBorders tableCellBorders2 = new TableCellBorders();
                TopBorder topBorder2 = new TopBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
                LeftBorder leftBorder2 = new LeftBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
                BottomBorder bottomBorder2 = new BottomBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };
                RightBorder rightBorder2 = new RightBorder() { Val = BorderValues.Single, Color = "auto", Size = (UInt32Value)12U, Space = (UInt32Value)0U };

                tableCellBorders2.Append(topBorder2);
                tableCellBorders2.Append(leftBorder2);
                tableCellBorders2.Append(bottomBorder2);
                tableCellBorders2.Append(rightBorder2);

                tableCellProperties2.Append(tableCellWidth2);
                tableCellProperties2.Append(tableCellBorders2);

                Paragraph paragraph2 = new Paragraph() { RsidParagraphMarkRevision = "00CF6226", RsidParagraphAddition = "00F72565", RsidParagraphProperties = "000E008B", RsidRunAdditionDefault = "00F72565", ParagraphId = "5A654CE1", TextId = "03F23AFA" };

                ParagraphProperties paragraphProperties2 = new ParagraphProperties();

                ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
                RunFonts runFonts3 = new RunFonts() { Ascii = font, HighAnsi = font, ComplexScript = font };
                FontSize fontSize3 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript3 = new FontSizeComplexScript() { Val = "24" };

                paragraphMarkRunProperties2.Append(runFonts3);
                paragraphMarkRunProperties2.Append(fontSize3);
                paragraphMarkRunProperties2.Append(fontSizeComplexScript3);

                paragraphProperties2.Append(paragraphMarkRunProperties2);

                Run run2 = new Run();

                RunProperties runProperties2 = new RunProperties();
                RunFonts runFonts4 = new RunFonts() { Ascii = font, HighAnsi = font, ComplexScript = font };
                FontSize fontSize4 = new FontSize() { Val = "24" };
                FontSizeComplexScript fontSizeComplexScript4 = new FontSizeComplexScript() { Val = "24" };

                runProperties2.Append(runFonts4);
                runProperties2.Append(fontSize4);
                runProperties2.Append(fontSizeComplexScript4);
                Text text2 = new Text();
                text2.Text = rowContents.ElementAt(i);

                run2.Append(runProperties2);
                run2.Append(text2);

                paragraph2.Append(paragraphProperties2);
                paragraph2.Append(run2);

                tableCell2.Append(tableCellProperties2);
                tableCell2.Append(paragraph2);

                tableRow1.Append(tableRowProperties1);
                tableRow1.Append(tableCell1);
                tableRow1.Append(tableCell2);

                table.Append(tableRow1);
            }

        }


    }
}
