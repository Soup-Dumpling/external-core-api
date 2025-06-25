using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace External.Product.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        }
    }
}
