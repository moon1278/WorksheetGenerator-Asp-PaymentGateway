using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using WorksheetGenerator.Data;
using WorksheetGenerator.Helper.Logging;
using WorksheetGenerator.Helper.Task;
using WorksheetGenerator.Helper.Word;
using WorksheetGenerator.Helper.Word.Element;
using WorksheetGenerator.Models.WorksheetModels.TaskModels;
using WorksheetGenerator.Models.WorksheetModels.TaskModels.General;

namespace WorksheetGenerator.Helper.Word
{
    public class TaskCreationWordHelper
    {
        private WordHelper _word { get; set; }
    
        public TaskCreationWordHelper()
        {
            _word = new WordHelper();          
        }

        private bool checkUserInput(UserInput userInput)
        {
            /*
            if(userInput.ZeitenDesVerbens == false)
            {
                return false;
            }

            */

            return true;

        }

        public MemoryStream CreateTask(UserInput userInput, WorksheetGenerator.Data.Worksheet worksheet)
        {
            if (!checkUserInput(userInput))
            {
                //Logger Überarbeiten
                Console.WriteLine("Fehler beim Input des Nutzers.");
                return null;
            }

            var stream = new MemoryStream();
            using (WordprocessingDocument document = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                // Change the document type to Document
                document.ChangeDocumentType(WordprocessingDocumentType.Document);
                //Get the Main Part of the document
                MainDocumentPart mainPart = document.AddMainDocumentPart();

                #region ka was das macht
                Document document1 = new Document() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 w16se w16cid w16 w16cex w16sdtdh wp14" } };
                document1.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
                document1.AddNamespaceDeclaration("cx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
                document1.AddNamespaceDeclaration("cx1", "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
                document1.AddNamespaceDeclaration("cx2", "http://schemas.microsoft.com/office/drawing/2015/10/21/chartex");
                document1.AddNamespaceDeclaration("cx3", "http://schemas.microsoft.com/office/drawing/2016/5/9/chartex");
                document1.AddNamespaceDeclaration("cx4", "http://schemas.microsoft.com/office/drawing/2016/5/10/chartex");
                document1.AddNamespaceDeclaration("cx5", "http://schemas.microsoft.com/office/drawing/2016/5/11/chartex");
                document1.AddNamespaceDeclaration("cx6", "http://schemas.microsoft.com/office/drawing/2016/5/12/chartex");
                document1.AddNamespaceDeclaration("cx7", "http://schemas.microsoft.com/office/drawing/2016/5/13/chartex");
                document1.AddNamespaceDeclaration("cx8", "http://schemas.microsoft.com/office/drawing/2016/5/14/chartex");
                document1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
                document1.AddNamespaceDeclaration("aink", "http://schemas.microsoft.com/office/drawing/2016/ink");
                document1.AddNamespaceDeclaration("am3d", "http://schemas.microsoft.com/office/drawing/2017/model3d");
                document1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
                document1.AddNamespaceDeclaration("oel", "http://schemas.microsoft.com/office/2019/extlst");
                document1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                document1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
                document1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
                document1.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
                document1.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
                document1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
                document1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                document1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
                document1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
                document1.AddNamespaceDeclaration("w16cex", "http://schemas.microsoft.com/office/word/2018/wordml/cex");
                document1.AddNamespaceDeclaration("w16cid", "http://schemas.microsoft.com/office/word/2016/wordml/cid");
                document1.AddNamespaceDeclaration("w16", "http://schemas.microsoft.com/office/word/2018/wordml");
                document1.AddNamespaceDeclaration("w16sdtdh", "http://schemas.microsoft.com/office/word/2020/wordml/sdtdatahash");
                document1.AddNamespaceDeclaration("w16se", "http://schemas.microsoft.com/office/word/2015/wordml/symex");
                document1.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
                document1.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
                document1.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
                document1.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");
                #endregion

                // Create a new header and footer part
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                int classId;

                using(ApplicationDbContext db = new ApplicationDbContext())
                {
                    classId = db.RlWorksheetClasses.Where(x => x.WorksheetId == worksheet.Id).FirstOrDefault().ClassId;
                }

                _word.AddWorksheetNameHeader(headerPart, worksheet.Name, classId);

                WorksheetCreator worksheetCreator = new WorksheetCreator();

                foreach (var task in userInput.Tasks)
                {
                    if (task.GetType() == typeof(ZeitformAnhandSatz_Task))
                    {
                        worksheetCreator.Create_ZeitformAnhandSatz(headerPart, mainPart, document1, task.ZeitformAnhandSatz_Values);
                    }
                    else if (task.GetType() == typeof(ZeichnenSilbenBoegen_Task))
                    {
                        worksheetCreator.Create_ZeichnenVonSilbenBoegen(headerPart, mainPart, document1, task.ZeichnenSilbenBoegen_Values);
                    }
                    else if (task.GetType() == typeof(SingleStringReturn_Task))
                    {

                        switch (task.SingleStringReturn_Value.TaskId)
                        {
                            case (int)EnumTasks.SatzAnfangAustauschen:
                                worksheetCreator.Create_SatzAnfangAustauschen(headerPart, mainPart, document1, task.SingleStringReturn_Value.Text);

                                break;
                            case (int)EnumTasks.ArtikelEinsaetzen:
                                worksheetCreator.Create_ArtikelEinsaetzen(headerPart, mainPart, document1, task.SingleStringReturn_Value.Text);

                                break;
                        }
                    }
                    else if (task.GetType() == typeof(ListStringReturn_Task))
                    {
                        switch (task.StringList_Values.First().TaskId)
                        {
                            case (int)EnumTasks.PassendeSatzanfaengefinden:
                                worksheetCreator.Create_PassendeSatzanfaengefinden(headerPart, mainPart, document1, task.StringList_Values);

                                break;
                            case (int)EnumTasks.ArtikelVorNomen:
                                worksheetCreator.Create_ArtikelVorNomen(headerPart, mainPart, document1, task.StringList_Values);

                                break;
                        }
                    }
                    else if (task.GetType() == typeof(SatzZeitUmschreiben_Task))
                    {
                        worksheetCreator.Create_SatzZeitUmschreiben(headerPart, mainPart, document1, task.SatzZeitUmschreiben_Values);
                    }
                    else if (task.GetType() == typeof(ListArrayReturn_Task))
                    {

                        switch (task.ListArray_Values.TaskId)
                        {
                            case (int)EnumTasks.GegenteileFinde:
                                worksheetCreator.Create_GegenteileFinden(headerPart, mainPart, document1, task.ListArray_Values);

                                break;
                        }
                    }
                
                    else if (task.GetType() == typeof(AddiereZahlen100Bis1000_Task))
                    {
                        worksheetCreator.Create_AddiereZahlen100Bis1000(headerPart, mainPart, document1, task.AddiereZahlen100Bis1000_Values);
                    }
                }      

                mainPart.Document = document1;

                // Get Id of the headerPart and footer parts
                string headerPartId = mainPart.GetIdOfPart(headerPart);
                //string footerPartId = mainDocumentPart.GetIdOfPart(footerPart);

                // Get SectionProperties and Replace HeaderReference and FooterRefernce with new Id
                IEnumerable<SectionProperties> sections = mainPart.Document.Body.Elements<SectionProperties>();

                foreach (var section in sections)
                {
                    // Delete existing references to headers and footers
                    section.RemoveAllChildren<HeaderReference>();
                    //section.RemoveAllChildren<FooterReference>();

                    // Create the new header and footer reference node
                    section.PrependChild<HeaderReference>(new HeaderReference() { Id = headerPartId });
                    //section.PrependChild<FooterReference>(new FooterReference() { Id = footerPartId });
                }
                mainPart.Document.Save();
            }

            return stream;


        }

        public MemoryStream CreateSolution(UserInput userInput, WorksheetGenerator.Data.Worksheet worksheet)
        {
            if (!checkUserInput(userInput))
            {
                //Logger Überarbeiten
                Console.WriteLine("Fehler beim Input des Nutzers.");
                return null;
            }

            var stream = new MemoryStream();
            using (WordprocessingDocument document = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                // Change the document type to Document
                document.ChangeDocumentType(WordprocessingDocumentType.Document);
                //Get the Main Part of the document
                MainDocumentPart mainPart = document.AddMainDocumentPart();

                #region ka was das macht
                Document document1 = new Document() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15 w16se w16cid w16 w16cex w16sdtdh wp14" } };
                document1.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
                document1.AddNamespaceDeclaration("cx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
                document1.AddNamespaceDeclaration("cx1", "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
                document1.AddNamespaceDeclaration("cx2", "http://schemas.microsoft.com/office/drawing/2015/10/21/chartex");
                document1.AddNamespaceDeclaration("cx3", "http://schemas.microsoft.com/office/drawing/2016/5/9/chartex");
                document1.AddNamespaceDeclaration("cx4", "http://schemas.microsoft.com/office/drawing/2016/5/10/chartex");
                document1.AddNamespaceDeclaration("cx5", "http://schemas.microsoft.com/office/drawing/2016/5/11/chartex");
                document1.AddNamespaceDeclaration("cx6", "http://schemas.microsoft.com/office/drawing/2016/5/12/chartex");
                document1.AddNamespaceDeclaration("cx7", "http://schemas.microsoft.com/office/drawing/2016/5/13/chartex");
                document1.AddNamespaceDeclaration("cx8", "http://schemas.microsoft.com/office/drawing/2016/5/14/chartex");
                document1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
                document1.AddNamespaceDeclaration("aink", "http://schemas.microsoft.com/office/drawing/2016/ink");
                document1.AddNamespaceDeclaration("am3d", "http://schemas.microsoft.com/office/drawing/2017/model3d");
                document1.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
                document1.AddNamespaceDeclaration("oel", "http://schemas.microsoft.com/office/2019/extlst");
                document1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                document1.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
                document1.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
                document1.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
                document1.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
                document1.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
                document1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                document1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
                document1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");
                document1.AddNamespaceDeclaration("w16cex", "http://schemas.microsoft.com/office/word/2018/wordml/cex");
                document1.AddNamespaceDeclaration("w16cid", "http://schemas.microsoft.com/office/word/2016/wordml/cid");
                document1.AddNamespaceDeclaration("w16", "http://schemas.microsoft.com/office/word/2018/wordml");
                document1.AddNamespaceDeclaration("w16sdtdh", "http://schemas.microsoft.com/office/word/2020/wordml/sdtdatahash");
                document1.AddNamespaceDeclaration("w16se", "http://schemas.microsoft.com/office/word/2015/wordml/symex");
                document1.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
                document1.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
                document1.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
                document1.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");
                #endregion

                // Create a new header and footer part
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                int classId;

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    classId = db.RlWorksheetClasses.Where(x => x.WorksheetId == worksheet.Id).FirstOrDefault().ClassId;
                }

                _word.AddSolutionHeader(headerPart, worksheet.Name, classId);

                SolutionCreator solutionCreator = new SolutionCreator();

                foreach (var task in userInput.Tasks)
                {
                    if (task.GetType() == typeof(ZeitformAnhandSatz_Task))
                    {
                        solutionCreator.Create_ZeitformAnhandSatz(headerPart, mainPart, document1, task.ZeitformAnhandSatz_Values);
                    }
                    else if (task.GetType() == typeof(ZeichnenSilbenBoegen_Task))
                    {
                        solutionCreator.Create_ZeichnenVonSilbenBoegen(headerPart, mainPart, document1, task.ZeichnenSilbenBoegen_Values);
                    }
                    else if (task.GetType() == typeof(SingleStringReturn_Task))
                    {

                        switch (task.SingleStringReturn_Value.TaskId)
                        {
                            case (int)EnumTasks.SatzAnfangAustauschen:
                                string taskName = "";

                                using (ApplicationDbContext db = new ApplicationDbContext())
                                {
                                    taskName = db.Tasks.Where(x => x.Id == task.SingleStringReturn_Value.TaskId).FirstOrDefault().Name;
                                }

                                if (taskName != "")
                                {
                                    solutionCreator.Create_NoSolution(headerPart, mainPart, document1, taskName);
                                }
                                else
                                {
                                    solutionCreator.Create_NoSolution(headerPart, mainPart, document1, "Aufgabe " + task.SingleStringReturn_Value.Text);
                                }


                                break;
                            case (int)EnumTasks.ArtikelEinsaetzen:

                                solutionCreator.Create_ArtikelEinsaetzen(headerPart, mainPart, document1, task.SingleStringReturn_Value.Text);
                                break;
                        }
                    }
                    else if (task.GetType() == typeof(ListStringReturn_Task))
                    {
                        switch (task.StringList_Values.First().TaskId)
                        {
                            case (int)EnumTasks.PassendeSatzanfaengefinden:
                                solutionCreator.Create_PassendeSatzanfaengefinden(headerPart, mainPart, document1, task.StringList_Values);

                                break;
                            case (int)EnumTasks.ArtikelVorNomen:
                                solutionCreator.Create_ArtikelVorNomen(headerPart, mainPart, document1, task.StringList_Values);

                                break;

                        }
                    }
                    else if (task.GetType() == typeof(SatzZeitUmschreiben_Task))
                    {
                        solutionCreator.Create_SatzZeitUmschreiben(headerPart, mainPart, document1, task.SatzZeitUmschreiben_Values);
                    }
                    else if (task.GetType() == typeof(ListArrayReturn_Task))
                    {

                        switch (task.ListArray_Values.TaskId)
                        {
                            case (int)EnumTasks.GegenteileFinde:
                                solutionCreator.Create_GegenteileFinden(headerPart, mainPart, document1, task.ListArray_Values);

                                break;
                        }
                    }
                    else if (task.GetType() == typeof(AddiereZahlen100Bis1000_Task))
                    {
                        solutionCreator.Create_AddiereZahlen100Bis1000(headerPart, mainPart, document1, task.AddiereZahlen100Bis1000_Values);
                    }
                }

                mainPart.Document = document1;

                // Get Id of the headerPart and footer parts
                string headerPartId = mainPart.GetIdOfPart(headerPart);
                //string footerPartId = mainDocumentPart.GetIdOfPart(footerPart);

                // Get SectionProperties and Replace HeaderReference and FooterRefernce with new Id
                IEnumerable<SectionProperties> sections = mainPart.Document.Body.Elements<SectionProperties>();

                foreach (var section in sections)
                {
                    // Delete existing references to headers and footers
                    section.RemoveAllChildren<HeaderReference>();
                    //section.RemoveAllChildren<FooterReference>();

                    // Create the new header and footer reference node
                    section.PrependChild<HeaderReference>(new HeaderReference() { Id = headerPartId });
                    //section.PrependChild<FooterReference>(new FooterReference() { Id = footerPartId });
                }
                mainPart.Document.Save();
            }

            return stream;


        }

    }
}
