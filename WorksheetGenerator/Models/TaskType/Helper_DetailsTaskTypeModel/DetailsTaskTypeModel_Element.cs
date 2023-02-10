namespace WorksheetGenerator.Models.TaskType.Helper_DetailsTaskTypeModel
{
    public class DetailsTaskTypeModel_Element
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? HexColor { get; set; }


        public List<WorksheetGenerator.Data.SubTaskType> SubTaskTypes { get; set; }
        public List<WorksheetGenerator.Data.Task> Tasks { get; set; }

    }
}
