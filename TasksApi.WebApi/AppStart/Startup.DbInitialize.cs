using Microsoft.EntityFrameworkCore;
using TasksApi.Application.Abstract.Data;
using TasksApi.Data;
using TasksApi.Data.Repositories;

namespace TasksApi.WebApi.AppStart
{
    public partial class Startup
    {
        private static void DbInitialize(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<TaskStatesDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<ITaskStatesDbContext, TaskStatesDbContext>();
            builder.Services.AddScoped<ITaskStatesRepository, EfTaskStatesRepository>();
        }
    }
}
