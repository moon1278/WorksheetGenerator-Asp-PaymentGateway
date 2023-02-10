using Microsoft.AspNetCore.Mvc;

namespace WorksheetGenerator.Data
{
    public class Role
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string ConcurrencyStamp { get; set; }

    }
}
