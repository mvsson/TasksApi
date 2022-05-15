using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasksApi.Application.Entities;

namespace TasksApi.Data.EntityConfiguration
{
    public class TaskStateConfiguration : IEntityTypeConfiguration<TaskStateEntity>
    {
        public void Configure(EntityTypeBuilder<TaskStateEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
