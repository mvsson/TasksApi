namespace TasksApi.Domain
{
    /// <summary>
    ///     Доменная сущность {sampleName} задачи
    /// </summary>
    public class TaskState
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


        /// <summary>
        ///     Ctor
        /// </summary>
        public TaskState()
        { }

        /// <summary>
        ///     Создать задачу со статусом Created
        /// </summary>
        public static TaskState CreateDefault()
        {
            return new TaskState()
            {
                StateStatus = TaskStateStatus.Created,
                Timestamp = DateTime.UtcNow,
            };
        }
    }
}