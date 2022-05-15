using TasksApi.Application.Mappings.Anchor;
using TasksApi.Application.Services.TaskExecuter;
using TasksApi.Application.Services.Tasks;

namespace TasksApi.WebApi.AppStart
{
    public partial class Startup
    {
        public static void CreateServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(DtosAssemblyAnchor));

            builder.Services.AddSingleton<ITaskExecuterService, TaskExecuterBackgroundService>()
                            .AddHostedService(services => services.GetService<ITaskExecuterService>());

            builder.Services.AddScoped<ITasksService, TasksService>();
        }
    }
}
