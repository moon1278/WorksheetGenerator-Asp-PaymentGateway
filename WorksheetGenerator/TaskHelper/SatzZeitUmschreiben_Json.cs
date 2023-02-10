namespace WorksheetGenerator.TaskHelper
{
    public class SatzZeitUmschreiben_Json
    {

        public SatzZeitUmschreiben_Json(string _praesens, string _perfekt, string _präteritum, string _future)
        {
            Praesens = _praesens;
            Perfekt = _perfekt;
            Praeteritum = _präteritum;
            Future = _future;
        }
        public string Praesens { get; set; }
        public string Perfekt { get; set; }
        public string Praeteritum { get; set; }
        public string Future { get; set; }

    }
}
