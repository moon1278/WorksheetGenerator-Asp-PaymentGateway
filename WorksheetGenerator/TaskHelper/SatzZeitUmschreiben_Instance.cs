namespace WorksheetGenerator.TaskHelper
{
    public class SatzZeitUmschreiben_Instance
    {
        public SatzZeitUmschreiben_Instance(string _sentence, bool _isSolution, ZeitFormen zeitForm)
        {
            Sentence = _sentence;
            IsSolution = _isSolution;
            ZeitForm = zeitForm;
        }
        public string Sentence { get; set; }
        public bool IsSolution { get; set; }
        public ZeitFormen ZeitForm { get; set; }
    }
}
