namespace WorksheetGenerator.Models.WorksheetModels.TaskModels
{
    public class ZeichnenSilbenBoegen
    {

        public ZeichnenSilbenBoegen()
        {
         
        }

        public ZeichnenSilbenBoegen(string _begriff)
        {
            Begriff = _begriff;
            SilbenPosition = new List<int>();
        }

        public ZeichnenSilbenBoegen(string _begriff, List<int> _silbenPosition)
        {
            Begriff = _begriff;
            SilbenPosition = _silbenPosition;
        }
        public string Begriff { get; set; }
        public List<int> SilbenPosition { get; set; }
    }
}
