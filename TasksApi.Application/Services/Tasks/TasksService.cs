using AutoMapper;
using TasksApi.Application.Abstract.Data;
using TasksApi.Application.Common.Exceptions;
using TasksApi.Application.Entities;
using TasksApi.Application.Services.TaskExecuter;
using TasksApi.Domain;

namespace TasksApi.Application.Services.Tasks
{
    /// <inheritdoc cref="ITasksService"/>
    public class TasksService : ITasksService
    {
        private readonly ITaskStatesRepository _taskStatesRepository;
        private readonly ITaskExecuterService _taskExecuterService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Ctor    
        /// </summary>
        public TasksService(ITaskStatesRepository taskStatesRepository, ITaskExecuterService taskExecuterService, IMapper mapper)
        {
            _taskStatesRepository = taskStatesRepository ?? throw new ArgumentNullException(nameof(taskStatesRepository));
            _taskExecuterService = taskExecuterService ?? throw new ArgumentNullException(nameof(taskExecuterService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc />
        public async Task<Guid> CreateAndRunNewTask()
        {
            var createdTask = TaskState.CreateDefault();

            await _taskStatesRepository.AddTaskStateAsync(createdTask);

            await _taskExecuterService.EnqueueToExecute(createdTask, _taskStatesRepository);

            return createdTask.Id;
        }

        /// <inheritdoc />
        public async Task<TaskState?> GetTaskById(Guid taskId)
        {
            try
            {
                var task = await _taskStatesRepository.GetTaskStateAsync(x => x.Id == taskId);

                if (task == null) throw new NotFoundException(nameof(TaskStateEntity), taskId);

                return _mapper.Map<TaskStateEntity, TaskState>(task);
            }
            catch (NotFoundException)
            {
                return await Task.FromResult((TaskState)null);
            }
        }
    }
}
