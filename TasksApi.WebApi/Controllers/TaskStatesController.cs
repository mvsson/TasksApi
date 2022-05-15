using Microsoft.AspNetCore.Mvc;
using TasksApi.Application.Services.Tasks;
using TasksApi.Application.Utils;

namespace TasksApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskStatesController : ControllerBase
    {
        private readonly ITasksService _tasksService;
        private readonly ILogger<TaskStatesController> _logger;

        /// <summary>
        ///     Ctor
        /// </summary>
        public TaskStatesController(ITasksService tasksService, ILogger<TaskStatesController> logger)
        {
            _tasksService = tasksService ?? throw new ArgumentNullException(nameof(tasksService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Создать задачу
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTask()
        {
            try
            {
                var newTaskId = await _tasksService.CreateAndRunNewTask();

                return StatusCode(202, newTaskId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Возникла ошибка при создании и запуске новой задачи");

                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Получить статус задачи по её идентификатору
        /// </summary>
        [HttpGet("{taskIdString}")]
        public async Task<IActionResult> GetTask(string taskIdString)
        {
            if (!Guid.TryParse(taskIdString, out var taskId)) return StatusCode(400);

            try
            {
                var requestedTask = await _tasksService.GetTaskById(taskId);

                if (requestedTask == null) return NotFound();

                return Ok(new {status = requestedTask.StateStatus.ToStringCached(), timestamp = requestedTask.Timestamp.ToString("O")});
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Возникла ошибка при гет-запросе задачи по айди");

                return StatusCode(500);
            }
        }
    }
}
