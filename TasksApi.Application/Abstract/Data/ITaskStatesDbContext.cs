using Microsoft.EntityFrameworkCore;
using TasksApi.Application.Entities;

namespace TasksApi.Application.Abstract.Data
{
    /// <summary>
    ///     Контекст базы данных
    /// </summary>
    public interface ITaskStatesDbContext
    {
        DbSet<TaskStateEntity> TaskStates { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
