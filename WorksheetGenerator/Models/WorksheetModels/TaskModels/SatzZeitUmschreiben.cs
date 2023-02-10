using WorksheetGenerator.TaskHelper;

namespace WorksheetGenerator.Models.WorksheetModels.TaskModels
{
    public class SatzZeitUmschreiben
    {
        public SatzZeitUmschreiben()
        {
            SentenceList = new List<SatzZeitUmschreiben_Instance>();
        }

        public List<SatzZeitUmschreiben_Instance> SentenceList { get; set; }
    }
}
