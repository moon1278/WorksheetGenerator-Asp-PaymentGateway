namespace WorksheetGenerator.Models.WorksheetModels.TaskModels
{
    public class AddiereZahlen100Bis1000
    {
        public AddiereZahlen100Bis1000()
        {
        }

        public AddiereZahlen100Bis1000(string _aufgabe, string _loesung)
        {
            Aufgabe = _aufgabe;
            Loesung = _loesung;
        }
        public string Aufgabe { get; set; }
        public string Loesung { get; set; }
    }
}
