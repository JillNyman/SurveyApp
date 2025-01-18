using System;
using Microsoft.EntityFrameworkCore;
using surveyapi.Models;

namespace surveyapi.Data
{
    public class SurveyContext: DbContext
    {
        //Konstruktor för att konfigurera DBcontext-alternativ
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options) { }

        public DbSet<Survey> Surveys { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;

        public DbSet<Option> Options { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Konfiguration av relationen mellan Question och Option
            modelBuilder.Entity<Option>()
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
