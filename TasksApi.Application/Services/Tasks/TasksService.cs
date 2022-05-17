using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            //todo если выполнять досконально по пайплайну из тз, тогда задача будет отправляться не сразу в репозиторий, а на очередь для сохранения, а идентификатор записи будет создаваться основываясь на кеше
            //     тогда ответ 202 с айди задачи будет получен до присвоения задаче статуса running
            //     но, так как сущность не велика, а обработка её сохранения не занимает столь много времени, решил отправлять её сразу в репозиторий

            var createdTask = TaskState.CreateDefault();
            createdTask.Id = Guid.Empty;
            
            var taskId = await _taskStatesRepository.AddTaskStateAsync(createdTask);

            createdTask.Id = taskId;

            await _taskExecuterService.EnqueueToExecute(createdTask);

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
