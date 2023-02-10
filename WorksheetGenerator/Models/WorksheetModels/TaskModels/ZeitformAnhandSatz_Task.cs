using WorksheetGenerator.Models.WorksheetModels.TaskModels.General;
using WorksheetGenerator.TaskHelper;

namespace WorksheetGenerator.Models.WorksheetModels.TaskModels
{
    public class ZeitformAnhandSatz_Task : ITask
    {
        public List<ZeitformAnhandSatz> ZeitformAnhandSatz_Values { get; set; }
        public SingleStringReturn SingleStringReturn_Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<SingleStringReturn> StringList_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<SatzZeitUmschreiben> SatzZeitUmschreiben_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<AddiereZahlen100Bis1000> AddiereZahlen100Bis1000_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ListArrayReturn ListArray_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<ZeichnenSilbenBoegen> ITask.ZeichnenSilbenBoegen_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
