using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class WebHostEnviromentHelper
    {
        public static string GetWebRootPath() {
            var accessor = new HttpContextAccessor();
            return accessor.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootPath;
        }
    }
}