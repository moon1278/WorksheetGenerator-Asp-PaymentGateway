using WorksheetGenerator.Models.SpecificationModels;

namespace WorksheetGenerator.Models.WorksheetModels
{
    public class JsonTasksOfSubType
    {
        public string[] TasksOfSubType { get; set; }
        public string WorksheetName { get; set; }
        public string ClassId { get; set; }
        public List<TaskJSONModel> Tasks { get; set; }
    }
}
