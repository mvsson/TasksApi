using System.Linq.Expressions;
using TasksApi.Application.Entities;
using TasksApi.Domain;

namespace TasksApi.Application.Abstract.Data
{
    /// <summary>
    ///     Репозиторий для работы с <see cref="Domain.TaskState"/>
    /// </summary>
    public interface ITaskStatesRepository
    {
        /// <summary>
        ///     Добавить задачу
        /// </summary>
        Task AddTaskStateAsync(TaskState taskState);

        /// <summary>
        ///     Получить первую задачу подходящую под выражение
        /// </summary>
        Task<TaskStateEntity?> GetTaskStateAsync(Expression<Func<TaskStateEntity, bool>> expression);

        /// <summary>
        ///     Получить все задачи подходящие под выражение
        /// </summary>
        Task<ICollection<TaskStateEntity>> GetAllTaskStatesAsync<TOrderBy>(Expression<Func<TaskStateEntity, bool>>? filterBy = null, Expression<Func<TaskStateEntity, TOrderBy>>? orderBy = null);

        /// <summary>
        ///     Обновить задачу
        /// </summary>
        Task UpdateTaskStateAsync(TaskState taskState);

        /// <summary>
        ///     Удалить задачу
        /// </summary>
        Task DeleteTaskStateAsync(TaskState taskState);
    }
}
