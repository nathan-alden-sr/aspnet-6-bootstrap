namespace Company.Product.WebApi.ScheduledTasks;

public static class HostEnvironmentExtensions
{
    public static bool IsDeveloper(this IHostEnvironment hostEnvironment)
    {
        ThrowIfNull(hostEnvironment);

        return hostEnvironment.EnvironmentName is "DeveloperVisualStudio" or "DeveloperDocker";
    }
}
