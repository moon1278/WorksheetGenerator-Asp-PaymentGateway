namespace WorksheetGenerator.Models.WorksheetModels
{
    public class TasksOfSubType
    {
        public string Id { get; set; }
        public string SubTaskTypeName { get; set; }
        public string Color { get; set; }
        public List<WorksheetGenerator.Data.Task> Tasks { get; set; }

    }
}
