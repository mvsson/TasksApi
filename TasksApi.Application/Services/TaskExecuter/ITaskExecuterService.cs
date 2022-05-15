using Microsoft.Extensions.Hosting;
using TasksApi.Application.Abstract.Data;
using TasksApi.Domain;

namespace TasksApi.Application.Services.TaskExecuter
{
    /// <summary>
    ///     Сервис исполнения задач
    /// </summary>
    public interface ITaskExecuterService: IHostedService
    {
        /// <summary>
        ///     Записать задачу в очередь на исполнение
        /// </summary>
        Task EnqueueToExecute(TaskState task, ITaskStatesRepository? taskStatesRepository);
    }
}
