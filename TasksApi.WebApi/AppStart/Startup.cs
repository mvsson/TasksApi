namespace TasksApi.WebApi.AppStart
{
    public partial class Startup
    {
        public static void Initialize(WebApplicationBuilder builder)
        {
            DbInitialize(builder);

            CreateServices(builder);
        }
    }
}
