namespace WorksheetGenerator.Models.WorksheetModels.TaskModels
{
    public class ZeitformAnhandSatz
    {

        public ZeitformAnhandSatz(string _satz, string _zeitform)
        {
            Satz = _satz;
            Zeitform = _zeitform;
        }
        public string Satz { get; set; }
        public string Zeitform { get; set; }
    }
}
