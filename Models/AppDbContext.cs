using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizplusApi.Models.Menu;
using QuizplusApi.Models.Others;
using QuizplusApi.Models.Question;
using QuizplusApi.Models.Quiz;
using QuizplusApi.Models.User;
using QuizplusApi.ViewModels.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizplusApi.Models
{
    public class AppDbContext:DbContext
    {     
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<LogHistory> LogHistories { get; set; }
        public virtual DbSet<AppMenu> AppMenus { get; set; }
        public virtual DbSet<MenuMapping> MenuMappings { get; set; }
        public virtual DbSet<SiteSettings> SiteSettings { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<QuestionLavel> QuestionLavels { get; set; }
        public virtual DbSet<QuestionCategory> QuestionCategories { get; set; }
        public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }
        public virtual DbSet<QuizMarkOption> QuizMarkOptions { get; set; }
        public virtual DbSet<QuizParticipantOption> QuizParticipantOptions { get; set; }
        public virtual DbSet<QuizParticipant> QuizParticipants { get; set; }
        public virtual DbSet<QuizPayment> QuizPayments { get; set; }
        public virtual DbSet<ReportType> ReportTypes { get; set; }
        public virtual DbSet<QuizResponseDetail> QuizResponseDetails { get; set; }
        public virtual DbSet<QuizResponseInitial> QuizResponseInitials { get; set; }
        public virtual DbSet<QuizTopic> QuizTopics { get; set; }
        public virtual DbSet<CertificateTemplate> CertificateTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Seed();//insert seed data on Sql server,Mysql,Sqlite,PostgreSql and Oracle
        }

    }
}
