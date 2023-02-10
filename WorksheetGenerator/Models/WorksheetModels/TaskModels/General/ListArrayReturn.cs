using WorksheetGenerator.TaskHelper;

namespace WorksheetGenerator.Models.WorksheetModels.TaskModels.General { 
	public class ListArrayReturn
	{
        public int TaskId { get; set; }
        public List<string[]> ListArrayReturnValue { get; set; }

    }
}
