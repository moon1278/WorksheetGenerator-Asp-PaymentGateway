using WorksheetGenerator.TaskHelper;

namespace WorksheetGenerator.Models.WorksheetModels.TaskModels.General
{
    public class ListArrayReturn_Task : ITask
    {
        public List<SingleStringReturn> StringList_Values { get; set; }
        public List<SatzZeitUmschreiben> SatzZeitUmschreiben_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<AddiereZahlen100Bis1000> AddiereZahlen100Bis1000_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ListArrayReturn ListArray_Values { get; set; }
        List<ZeitformAnhandSatz> ITask.ZeitformAnhandSatz_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<ZeichnenSilbenBoegen> ITask.ZeichnenSilbenBoegen_Values { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        SingleStringReturn ITask.SingleStringReturn_Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
