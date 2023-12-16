using Blog.BL.Exception_Handling;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Blog.API.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    //private readonly ILogger _logger;

    public GlobalExceptionHandlingMiddleware()
    {
        //_logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ReferenceConstraintException)
        {
            context.Response.StatusCode = 400;

            ProblemDetails problem = new()
            {
                Status = 400,
                Type = "ReferenceConstraintException",
                Detail = "The operation could not be completed due to a reference constraint violation. " +
                "This might be due to providing a record id that doesn't exist in the database." +
                "(EX. make POST request to post and the provided auther doesn't exist in the database.)"
            };

            string json = JsonSerializer.Serialize(problem);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
        catch (BusinessException ex)
        {
            context.Response.StatusCode = ex.StatusCode;

            ProblemDetails problem = new ()
            {
                Status = ex.StatusCode, 
                Detail = ex.Message
            };

            string json = JsonSerializer.Serialize(problem);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
        catch (Exception)
        {
            //_logger.LogError(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
