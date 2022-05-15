using TasksApi.Domain;

namespace TasksApi.Application.Entities
{
    /// <summary>
    ///     Сущность {sampleName} задачи
    /// </summary>
    public class TaskStateEntity
    {
        /// <summary>
        ///     Идентификатор задачи
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        ///     Состояние задачи
        /// </summary>
        public TaskStateStatus StateStatus { get; set; }

        /// <summary>
        ///     Дата обновления состояния задачи
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}