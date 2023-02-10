using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorksheetGenerator.Models.Task.Helper_DetailsTaskModel;

namespace WorksheetGenerator.Models.Task
{
    public class DetailsTaskModel
    {
        public DetailsTaskModel()
        {
            DetailsTaskModels = new List<DetailsTaskModel_Element>();
        }

        public List<DetailsTaskModel_Element> DetailsTaskModels { get; set; }

        public string JavascriptToRun { get; set; }

    }
}
