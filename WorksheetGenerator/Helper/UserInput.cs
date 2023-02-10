using WorksheetGenerator.Models.WorksheetModels.TaskModels;

namespace WorksheetGenerator.Helper
{
    public class UserInput
    {
        public UserInput()
        {
            Tasks = new List<ITask>();
        }
     
        public List<ITask> Tasks { get; set; }
    }
}
