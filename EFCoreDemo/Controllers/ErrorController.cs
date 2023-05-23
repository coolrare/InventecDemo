﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EFCoreDemo.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        //[Route("/error")]
        //public IActionResult Error() => Problem();

        [Route("/error")]
        public ActionResult Error([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = feature?.Error;
            var isDev = webHostEnvironment.IsDevelopment();
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = feature?.Path,
                Title = isDev ? $"{ex?.GetType().Name}: {ex?.Message}" : "An error occurred.",
                Detail = isDev ? ex?.StackTrace : null,
            };
            return StatusCode(problemDetails.Status.Value, problemDetails);
        }
    }
}
