using WorksheetGenerator.Data;

namespace WorksheetGenerator.Helper.Specification
{
    public class TaskSpecification
    {
        public WorksheetGenerator.Data.Specification Specification { get; set; }
        public List<Option> Options { get; set; }
        public HTML_SpecificationType HTML_SpecificationType { get; set; }
    }
}
