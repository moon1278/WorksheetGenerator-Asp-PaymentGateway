using WorksheetGenerator.Data;

namespace WorksheetGenerator.Helper.Logging
{
    public class LogHelper
    {
        public void Log(string toLog)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                Log log = new Log();
                log.Level = "Info";
                log.Message = toLog;
                log.Application = "Website";
                log.Callsite = "Website";
                log.Exception = "Website";
                log.Logger = "Website";
                log.Logged = DateTime.UtcNow;
                db.Logs.Add(log);
                db.SaveChanges();
            }
        }
    }
}
