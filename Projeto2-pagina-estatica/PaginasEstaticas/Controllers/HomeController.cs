using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PaginasEstaticas.Models;

namespace PaginasEstaticas.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}