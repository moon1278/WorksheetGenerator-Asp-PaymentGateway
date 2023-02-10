using WorksheetGenerator.Models.WorksheetManagement.Helper_DetailsWorksheetManagementModel;


namespace WorksheetGenerator.Models.WorksheetManagement
{
    public class DetailsWorksheetManagementModel
    {
        public DetailsWorksheetManagementModel()
        {
            DetailsTaskModels = new List<DetailsWorksheetManagementModel_Element>();
        }

        public List<DetailsWorksheetManagementModel_Element> DetailsTaskModels { get; set; }

        public string JavascriptToRun { get; set; }

    }
}
