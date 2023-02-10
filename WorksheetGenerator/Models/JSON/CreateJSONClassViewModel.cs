using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorksheetGenerator.Models.JSON
{
    public class CreateJSONClassViewModel
    {
        public string PropertyType { get; set; }
        public string PropertyName { get; set; }
        public bool IsArray { get; set; }

        public List<SelectListItem> PropertyTypes { get; set; }
    }
}
