using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TasksApi.Application.Abstract.Data;
using TasksApi.Application.Entities;
using TasksApi.Domain;

namespace TasksApi.Application.Services.TaskExecuter
{
    /// <inheritdoc cref="ITaskExecuterService"/>
    public class TaskExecuterBackgroundService: BackgroundService, ITaskExecuterService
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly Queue<TaskState> _tasksQueue;  // очередь задач на исполнение
        
        private readonly TimeSpan _taskExecutingTime;   // время исполнения задачи //todo вынос в конфиг, например


        /// <summary>
        ///     Ctor
        /// </summary>
        public TaskExecuterBackgroundService(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));

            _tasksQueue = new Queue<TaskState>();

            _taskExecutingTime = TimeSpan.FromMinutes(2);
        }


        /// <summary>
        ///     worker
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Подгружаем задачи из базы во время инициализации сервиса
                await LoadTasksFromDb();

                while (true)
                {
                    stoppingToken.ThrowIfCancellationRequested();

                    if (_tasksQueue.TryPeek(out var peekTask) && CheckTheNeedForExecution(peekTask.Timestamp))
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var taskStatesRepository = scope.ServiceProvider.GetRequiredService<ITaskStatesRepository>();

                        do
                        {
                            var task = _tasksQueue.Dequeue();

                            task.StateStatus = TaskStateStatus.Finished;

                            await taskStatesRepository.UpdateTaskStateAsync(task);
                        }
                        while (_tasksQueue.TryPeek(out peekTask) && CheckTheNeedForExecution(peekTask.Timestamp));
                    }

                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                /*
                 *  логика обработки краша бэкграунд сервиса 
                 */
            }
        }

        /// <inheritdoc />
        public async Task EnqueueToExecute(TaskState task, ITaskStatesRepository? taskStatesRepository = null)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            if (taskStatesRepository == null)
            {
                using var scope = _scopeFactory.CreateScope();
                taskStatesRepository = scope.ServiceProvider.GetRequiredService<ITaskStatesRepository>();
            }

            if (task.StateStatus != TaskStateStatus.Created)
            {
                throw new ArgumentException(
                    $"В очередь на выполнение может быть поставлена только задача со статусом {TaskStateStatus.Created}. " +
                    $"Id задачи: {task.Id}. Текущий статус: {task.StateStatus}.");
            }

            _tasksQueue.Enqueue(task);

            task.StateStatus = TaskStateStatus.Running;

            await taskStatesRepository.UpdateTaskStateAsync(task);
        }
        
        /// <summary>
        ///     Проверка необходимости перевода задачи в статус finished
        /// </summary>
        /// <returns>
        ///     True - если уже пора, False - если ещё рано
        /// </returns>
        private bool CheckTheNeedForExecution(DateTime taskTime)
        {
            var elapsedTime = DateTime.UtcNow - taskTime;

            return elapsedTime >= _taskExecutingTime;
        }

        /// <summary>
        ///     Загрузка задач из бд при запуске
        /// </summary>
        private async Task LoadTasksFromDb()
        {
            using var scope = _scopeFactory.CreateScope();
            var taskStatesRepository = scope.ServiceProvider.GetRequiredService<ITaskStatesRepository>();

            var tasks = await taskStatesRepository.GetAllTaskStatesAsync(task => task.StateStatus < TaskStateStatus.Finished && 
                                                                                  task.StateStatus > TaskStateStatus.None,
                                                                          task => task.Timestamp);
            
            foreach (var taskEntity in tasks)
            {
                var task = _mapper.Map<TaskStateEntity, TaskState>(taskEntity);

                switch (taskEntity.StateStatus)
                {
                    case TaskStateStatus.Created:
                        await EnqueueToExecute(task, taskStatesRepository);
                        break;
                    case TaskStateStatus.Running:
                        _tasksQueue.Enqueue(task);
                        break;
                }
            }
        }
    }
}
