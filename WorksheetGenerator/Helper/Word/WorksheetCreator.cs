using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using WorksheetGenerator.Data;
using WorksheetGenerator.Helper.Logging;
using WorksheetGenerator.Helper.String;
using WorksheetGenerator.Helper.Task;
using WorksheetGenerator.Helper.Word;
using WorksheetGenerator.Helper.Word.Element;
using WorksheetGenerator.Models.WorksheetModels.TaskModels;
using WorksheetGenerator.Models.WorksheetModels.TaskModels.General;
using WorksheetGenerator.TaskHelper;

namespace WorksheetGenerator.Helper.Word
{
    public class WorksheetCreator
    {
        private int _taskNumber { get; set; }
        private WordHelper _word { get; set; }
        private readonly LogHelper _logger = new LogHelper();

        public WorksheetCreator()
        {
            _taskNumber = 0;
            _word = new WordHelper();  
        }

        private string getCurrentTaskNumber()
        {
            _taskNumber = _taskNumber + 1;
            return _taskNumber.ToString();
        }

        public void Create_ZeitformAnhandSatz(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, List<ZeitformAnhandSatz> zeitenDesVerbens)
        {
            List<BulletListElement> bulletList = new List<BulletListElement>();

            foreach (ZeitformAnhandSatz _zeitenDesVerbens in zeitenDesVerbens)
            {
                BulletListElement bulletListElement = new BulletListElement();
                bulletListElement.Text = _zeitenDesVerbens.Satz;
                bulletListElement.Font = "Arial";
                bulletList.Add(bulletListElement);
            }

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Zeiten des Verbs", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "Unterstreiche erst das Verb oder die Verben. Bestimme dann, in welcher Zeit die nächsten Sätze stehen. (Präsens, Perfekt, Präteritum, Plusquamperfekt oder Futur).", false, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddBulletList(mainPart, document1, bulletList);
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");

        }

        public void Create_ZeichnenVonSilbenBoegen(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, List<ZeichnenSilbenBoegen> zeichnenSilbenBoegen)
        {
            List<BulletListElement> bulletList = new List<BulletListElement>();

            foreach (ZeichnenSilbenBoegen _zeichnenSilbenBoegen in zeichnenSilbenBoegen)
            {
                BulletListElement bulletListElement = new BulletListElement();
                
                bulletListElement.Text = _zeichnenSilbenBoegen.Begriff.Replace("-","");
                bulletListElement.Font = "Arial";
                bulletList.Add(bulletListElement);
            }

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Silbenbögen", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "Zeichne Silbenbögen ein!", false, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddBulletList(mainPart, document1, bulletList);

         
        }

        public void Create_SatzAnfangAustauschen(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, string text)
        {
            List<BulletListElement> bulletList = new List<BulletListElement>();

            List<string> taskDesc = new List<string>();
            taskDesc.Add("Lies den Text aufmerksam und unterstreiche das Wort „dann“ farbig.");
            taskDesc.Add("Schreibe über jedes unterstrichene „dann“ einen abwechslungsreicheren Satzanfang (Du kannst die Satzanfänge in deinem Heft als Hilfe benutzen!).");
            taskDesc.Add("Schreibe die Geschichte mit den neuen Satzanfängen sauber und ohne Fehler in dein Heft.");
            taskDesc.Add("Lies die Geschichte noch einmal durch – was fällt dir auf?");

            foreach (string desc in taskDesc)
            {
                BulletListElement bulletListElement = new BulletListElement();

                bulletListElement.Text = desc;
                bulletListElement.Font = "Arial";
                bulletList.Add(bulletListElement);
            }

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Abwechslungsreiche Satzanfänge schreiben", true, "Arial");
            _word.AddBulletList(mainPart, document1, bulletList);
            _word.AddTableWithText(mainPart, document1, text, true, "Arial");

        }

        public void Create_PassendeSatzanfaengefinden(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, List<SingleStringReturn> stringList)
        {
            List<BulletListElement> bulletList = new List<BulletListElement>();

            foreach (SingleStringReturn _singleString in stringList)
            {
                BulletListElement bulletListElement = new BulletListElement();
                bulletListElement.Text = _singleString.Text;
                bulletListElement.Font = "Arial";
                bulletList.Add(bulletListElement);
            }

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Satz Anfang Austauschen", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "Satz Anfang Austauschen", false, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddBulletList(mainPart, document1, bulletList);
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
        }

        public void Create_SatzZeitUmschreiben(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, List<SatzZeitUmschreiben> sentences)
        {
        

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Satz Zeit Umschreiben", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "Schreibe die Sätze jeweils in den fehlenden Zeitformen (Präsens, Präteritum, Future und Perfekt) auf und unterstreiche die Verbformen.", false, "Arial");
            foreach(SatzZeitUmschreiben sentence in sentences)
            {
                SatzZeitUmschreiben_Instance praesens = sentence.SentenceList.Where(x => x.ZeitForm == ZeitFormen.Praesens).FirstOrDefault();
                SatzZeitUmschreiben_Instance perfekt = sentence.SentenceList.Where(x => x.ZeitForm == ZeitFormen.Perfekt).FirstOrDefault();
                SatzZeitUmschreiben_Instance praeteritum = sentence.SentenceList.Where(x => x.ZeitForm == ZeitFormen.Praeteritum).FirstOrDefault();
                SatzZeitUmschreiben_Instance future = sentence.SentenceList.Where(x => x.ZeitForm == ZeitFormen.Future).FirstOrDefault();

                if (praesens.IsSolution)
                {
                    _word.AddTextParagraph(mainPart, document1, "Präsens:\t\t" + praesens.Sentence, false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Perfekt:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Präteritum:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Future:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "", false, "Arial");

                }
                else if (perfekt.IsSolution)
                {
                    _word.AddTextParagraph(mainPart, document1, "Präsens:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Perfekt:\t\t " + perfekt.Sentence, false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Präteritum:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Future:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "", false, "Arial");

                }
                else if (praeteritum.IsSolution)
                {
                    _word.AddTextParagraph(mainPart, document1, "Präsens:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Perfekt:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Präteritum:\t\t" + praeteritum.Sentence, false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Future:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "", false, "Arial");

                }
                else if (future.IsSolution)
                {
                    _word.AddTextParagraph(mainPart, document1, "Präsens:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Perfekt:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Präteritum:\t\t ______________________________", false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "Future:\t\t " + future.Sentence, false, "Arial");
                    _word.AddTextParagraph(mainPart, document1, "", false, "Arial");

                }
            }
        }

        public void Create_ArtikelEinsaetzen(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, string text)
        {

            text = text.Replace("der ", "____ ");
            text = text.Replace("die ", "____ ");
            text = text.Replace("das ", "____ ");
            

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Artikel Einsätzen", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, $"Aufgabenstellung", true, "Arial");
            _word.AddTableWithText(mainPart, document1, text, true, "Arial");

        }

        public void Create_ArtikelVorNomen(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, List<SingleStringReturn> stringList)
        {
            List<string> rowsDer = new List<string>();
            List<string> rowsDie = new List<string>();
            List<string> rowsDas = new List<string>();
            List<string> nomen = new List<string>();

            foreach (SingleStringReturn _singleString in stringList)
            {
                string first3 = _singleString.Text.Substring(0, 3);
                if(first3 == "der" || first3 == "Der")
                {
                    rowsDer.Add("");
                }else if (first3 == "die" || first3 == "Die")
                {
                    rowsDie.Add("");
                }else if (first3 == "das" || first3 == "Das")
                {
                    rowsDas.Add("");
                }

                string last = _singleString.Text.Substring(3, _singleString.Text.Length-3);
                last = last.Replace(" ", "");
                nomen.Add(last);
            }

            nomen.Shuffle();

            string nomenString = "";

            for(int i = 0; i< nomen.Count; i++)
            {
                if(i != nomen.Count - 1)
                {
                    nomenString += nomen.ElementAt(i) + ", ";
                }
                else
                {
                    nomenString += nomen.ElementAt(i);
                }
            }

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Artikel vor Nomen", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, $"Wenn du die folgenden {stringList.Count} Nomen richtig verteilst, bleibt keines mehr übrig.", false, "Arial");
            _word.AddTextParagraph(mainPart, document1, nomenString, false, "Arial", "14", true);

            if (rowsDer.Count() > 0)
            {
                _word.AddTextParagraph(mainPart, document1, "DER", false, "Arial", "20");
                _word.AddTableWithNumberCount(mainPart, document1, rowsDer, "Arial");
            }

            if (rowsDie.Count() > 0)
            {
                _word.AddTextParagraph(mainPart, document1, "DIE", false, "Arial", "20");
                _word.AddTableWithNumberCount(mainPart, document1, rowsDie, "Arial");
            }

            if (rowsDas.Count() > 0)
            {
                _word.AddTextParagraph(mainPart, document1, "DAS", false, "Arial", "20");
                _word.AddTableWithNumberCount(mainPart, document1, rowsDas, "Arial");
            }
        }

        public void Create_AddiereZahlen100Bis1000(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, List<AddiereZahlen100Bis1000> stringList)
        {
            List<BulletListElement> bulletList = new List<BulletListElement>();

            foreach (AddiereZahlen100Bis1000 _addiereZahlen100Bis1000 in stringList)
            {
                BulletListElement bulletListElement = new BulletListElement();
                bulletListElement.Text = _addiereZahlen100Bis1000.Aufgabe;
                bulletListElement.Font = "Arial";
                bulletList.Add(bulletListElement);
            }

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Addieren von Zahlen", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "Addiere die Zahlen zwischen 100 und 1000. Ein Beispiel wäre: 138 + 459 = 597", false, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddBulletList(mainPart, document1, bulletList);
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");

        }

        public void Create_GegenteileFinden(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, ListArrayReturn listArrayReturnValue)
        {
            List<BulletListElement> bulletList = new List<BulletListElement>();

            foreach (string[] desc in listArrayReturnValue.ListArrayReturnValue)
            {
                BulletListElement bulletListElement = new BulletListElement();

                bulletListElement.Text = desc[0] + " - " + desc[1];
                bulletListElement.Font = "Arial";
                bulletList.Add(bulletListElement);
            }
        
            _word.AddTextParagraph(mainPart, document1, $"Aufgabe {getCurrentTaskNumber()}: Gegenteile finden", true, "Arial");
            _word.AddBulletList(mainPart, document1, bulletList);

        }

    }
}
