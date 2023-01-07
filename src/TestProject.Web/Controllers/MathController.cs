using Microsoft.AspNetCore.Mvc;
using TestProject.Web.Infrastucture.Authentication;

namespace TestProject.Web.Controllers
{
  [BasicAuthorization]
  public class MathController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public MathController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      _logger.LogInformation("Test index");
      return View();
    }
  }
}
