using AutoMapper;
using TasksApi.Application.Entities;
using TasksApi.Domain;

namespace TasksApi.Application.Mappings
{
    // ReSharper disable once UnusedMember.Global
    /// <summary>
    ///     Профайл мапинга automapper
    /// </summary>
    public class TaskStateMappingProfile : Profile
    {
        public TaskStateMappingProfile()
        {
            CreateMap<TaskState, TaskStateEntity>();
            CreateMap<TaskStateEntity, TaskState>();
        }
    }
}
