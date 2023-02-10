namespace WorksheetGenerator.Models.Task.Helper_DetailsTaskModel
{
    public class DetailsTaskModel_Element
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string SubTaskType { get; set; }
        public string? Query { get; set; }
        public bool Activated { get; set; }
    }
}
