using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SobelFilterAppCSharp.Models;
using SobelFilterAppCSharp.Services;

namespace SobelFilterAppCSharp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<string> SendImage([FromForm] IFormFile? formImage)
    {
        var sobelImage = await SobelService.SobelEdgeDetection(formImage);
        return Convert.ToBase64String(sobelImage);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}