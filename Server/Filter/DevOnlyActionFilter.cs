using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace ApiMedicalClinicEx.Server.Filter;

public class DevOnlyActionFilter : ActionFilterAttribute
{
    private IHostEnvironment HostingEnv { get; }
    public DevOnlyActionFilter(IHostEnvironment hostingEnv)
    {
        HostingEnv = hostingEnv;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if(!HostingEnv.IsDevelopment())
        {
            context.Result = new NotFoundResult();
            return;
        }    

        base.OnActionExecuting(context);
    }
}