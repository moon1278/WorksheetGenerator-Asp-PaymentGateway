using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.Role;
using WorksheetGenerator.Models.User;
using WorksheetGenerator.Models.Token;

namespace WorksheetGenerator.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Option> Options { get; set; } = null!;
        public virtual DbSet<RlTaskSpecification> RlTaskSpecifications { get; set; } = null!;
        public virtual DbSet<RlWorksheetClass> RlWorksheetClasses { get; set; } = null!;
        public virtual DbSet<RlWorksheetTask> RlWorksheetTasks { get; set; } = null!;
        public virtual DbSet<Specification> Specifications { get; set; } = null!;
        public virtual DbSet<SubTaskType> SubTaskTypes { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<TaskType> TaskTypes { get; set; } = null!;
        public virtual DbSet<Worksheet> Worksheets { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<HTML_SpecificationType> HTML_SpecificationTypes { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Data Source=SQL8004.site4now.net;Initial Catalog=db_a8ad21_xpanddb; User Id = xpanddb; Password = LeC8qBynqDAa4F");
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-U4DIOTN;Initial Catalog=db_a8ad21_xpanddb;Integrated Security=True");
            }
        }

        public DbSet<WorksheetGenerator.Models.User.UserViewModel> UserViewModel { get; set; }

        public DbSet<WorksheetGenerator.Models.Token.TokenViewModel> TokenViewModel { get; set; }

     
    }
}