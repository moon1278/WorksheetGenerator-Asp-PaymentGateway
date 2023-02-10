using WorksheetGenerator.TaskHelper;

namespace WorksheetGenerator.Models.WorksheetModels.TaskModels.General
{
    public class SingleStringReturn_Task : ITask
    {
        public SingleStringReturn SingleStringReturn_Value { get; set; }
        public List<SingleStringReturn> StringList_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<SatzZeitUmschreiben> SatzZeitUmschreiben_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<AddiereZahlen100Bis1000> AddiereZahlen100Bis1000_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ListArrayReturn ListArray_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<ZeitformAnhandSatz> ITask.ZeitformAnhandSatz_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<ZeichnenSilbenBoegen> ITask.ZeichnenSilbenBoegen_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
