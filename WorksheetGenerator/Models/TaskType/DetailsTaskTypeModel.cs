using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorksheetGenerator.Models.TaskType.Helper_DetailsTaskTypeModel;

namespace WorksheetGenerator.Models.TaskType
{
    public class DetailsTaskTypeModel
    {
        public DetailsTaskTypeModel()
        {
            DetailsTaskTypeModels = new List<DetailsTaskTypeModel_Element>();
        }

        public List<DetailsTaskTypeModel_Element> DetailsTaskTypeModels { get; set; }

        public string JavascriptToRun { get; set; }

    }
}
