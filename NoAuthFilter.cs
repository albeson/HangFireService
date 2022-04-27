
using Hangfire.Dashboard;
using Hangfire.Annotations;
 
public class NoAuthFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}