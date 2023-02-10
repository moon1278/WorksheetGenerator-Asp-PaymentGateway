using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorksheetGenerator.Models.SubTaskType.Helper_DetailsSubTaskTypeModel;

namespace WorksheetGenerator.Models.SubTaskType
{
    public class DetailsSubTaskTypeModel
    {
        public DetailsSubTaskTypeModel()
        {
            DetailsSubTaskTypeModels = new List<DetailsSubTaskTypeModel_Element>();
        }

        public List<DetailsSubTaskTypeModel_Element> DetailsSubTaskTypeModels { get; set; }

        public string JavascriptToRun { get; set; }

    }
}
