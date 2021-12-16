using Microsoft.AspNetCore.Mvc;

namespace Belugaming.Controllers
{
    [ApiController]
    [Route("/")]
    public class IndexController
    {
        public ContentResult Get()
        {
            var fileContents = File.ReadAllText("./Pages/Swagger.html");
            return new ContentResult
            {
                Content = fileContents,
                ContentType = "text/html"
            };
        }
    }
}
