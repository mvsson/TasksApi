using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksApi.Application.Abstract.Data;
using TasksApi.Application.Common.Exceptions;
using TasksApi.Application.Entities;
using TasksApi.Domain;

namespace TasksApi.Data.Repositories
{
    /// <inheritdoc cref="ITaskStatesRepository"/>
    public class EfTaskStatesRepository: ITaskStatesRepository
    {
        private readonly ITaskStatesDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Ctor
        /// </summary>
        public EfTaskStatesRepository(ITaskStatesDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc />
        public async Task<Guid> AddTaskStateAsync(TaskState taskState)
        {
            var taskStateEntity = _mapper.Map<TaskState, TaskStateEntity>(taskState);

            await _dbContext.TaskStates.AddAsync(taskStateEntity);

            await _dbContext.SaveChangesAsync();

            return taskStateEntity.Id;
        }

        /// <inheritdoc />
        public async Task<TaskStateEntity?> GetTaskStateAsync(Expression<Func<TaskStateEntity, bool>> expression)
        {
            return await _dbContext.TaskStates.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        /// <inheritdoc />
        public async Task<ICollection<TaskStateEntity>> GetAllTaskStatesAsync<TOrderBy>(Expression<Func<TaskStateEntity, bool>>? filterBy = null, Expression<Func<TaskStateEntity, TOrderBy>>? orderBy = null)
        {
            var allTasks = _dbContext.TaskStates.AsNoTracking();

            if (filterBy != null) allTasks = allTasks.Where(filterBy);

            if (orderBy != null) allTasks = allTasks.OrderBy(orderBy);

            return await allTasks.ToListAsync();
        }

        /// <inheritdoc />
        public async Task UpdateTaskStateAsync(TaskState taskState)
        {
            var entity = await _dbContext.TaskStates.FirstOrDefaultAsync(x => x.Id == taskState.Id);

            if (entity == null) throw new NotFoundException(nameof(TaskStateEntity), taskState.Id);

            entity.StateStatus = taskState.StateStatus; //todo вынеести в маппер
            entity.Timestamp = taskState.Timestamp;

            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteTaskStateAsync(TaskState taskState)
        {
            var taskStateEntity = _mapper.Map<TaskState, TaskStateEntity>(taskState);

            _dbContext.TaskStates.Remove(taskStateEntity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
