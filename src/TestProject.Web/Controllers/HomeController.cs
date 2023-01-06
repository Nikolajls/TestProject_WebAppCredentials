using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestProject.Web.Infrastucture.Authentication;
using TestProject.Web.Models;

namespace TestProject.Web.Controllers
{
  [BasicAuthorization]
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      _logger.LogInformation("Test index");
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}