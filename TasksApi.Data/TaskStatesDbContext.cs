using Microsoft.EntityFrameworkCore;
using TasksApi.Application.Abstract.Data;
using TasksApi.Application.Entities;
using TasksApi.Data.EntityConfiguration;

namespace TasksApi.Data
{
    public sealed class TaskStatesDbContext: DbContext, ITaskStatesDbContext
    {
        public DbSet<TaskStateEntity> TaskStates { get; set; }

        public TaskStatesDbContext(DbContextOptions<TaskStatesDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskStateConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
