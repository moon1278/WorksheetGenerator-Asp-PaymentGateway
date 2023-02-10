using WorksheetGenerator.Models.WorksheetModels.TaskModels.General;
using WorksheetGenerator.TaskHelper;

namespace WorksheetGenerator.Models.WorksheetModels.TaskModels
{
    public interface ITask
    {
        public List<ZeitformAnhandSatz> ZeitformAnhandSatz_Values { get; set; }
        public List<ZeichnenSilbenBoegen> ZeichnenSilbenBoegen_Values { get; set; }
        public SingleStringReturn SingleStringReturn_Value { get; set; }
        public List<SingleStringReturn> StringList_Values { get; set; }
        public List<SatzZeitUmschreiben> SatzZeitUmschreiben_Values { get; set; }

        public List<AddiereZahlen100Bis1000> AddiereZahlen100Bis1000_Values { get; set; }
         
        public ListArrayReturn ListArray_Values { get; set;}
    }
}
