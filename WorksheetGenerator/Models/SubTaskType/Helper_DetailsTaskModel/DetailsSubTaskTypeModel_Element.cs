namespace WorksheetGenerator.Models.SubTaskType.Helper_DetailsSubTaskTypeModel
{
    public class DetailsSubTaskTypeModel_Element
    {
        public int Id { get; set; }
        public string TaskType { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public List<WorksheetGenerator.Data.Task> Tasks { get; set; }

    }
}
