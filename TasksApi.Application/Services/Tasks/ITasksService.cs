using TasksApi.Domain;

namespace TasksApi.Application.Services.Tasks
{
    /// <summary>
    ///     Сервис для создания и обновления задач <see cref="Domain.TaskState"/>
    /// </summary>
    public interface ITasksService
    {
        /// <summary>
        ///     Создать новую дефолтную задачу со статусом "Created" и отметкой UtcNow
        /// </summary>
        Task<Guid> CreateAndRunNewTask();

        /// <summary>
        ///     Получить задачу по айди
        /// </summary>
        Task<TaskState?> GetTaskById(Guid taskId);
    }
}
