﻿using System.Diagnostics;
using Maxsys.WorldCupPredictTheScore.Web.Models;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
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

    [Route("Error/{statusCode}")]
    public IActionResult Error(int statusCode)
    {
        return statusCode switch
        {
            404 => View("404"),
            _ => View("Error")
        };
    }
}
