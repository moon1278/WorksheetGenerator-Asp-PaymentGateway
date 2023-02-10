
using WorksheetGenerator.Helper.Specification;

namespace WorksheetGenerator.Models.SpecificationModels
{
    public class SpecificationModel
    {
        public SpecificationModel()
        {
            TaskSpecifications = new List<TaskSpecification>();
        }

        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public List<TaskSpecification> TaskSpecifications { get; set; }
        public byte[] PreviewImage { get; set; }
    }
}
