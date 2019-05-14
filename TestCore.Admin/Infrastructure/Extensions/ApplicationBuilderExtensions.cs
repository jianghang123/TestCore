using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TestCore.Common.Configuration;
using TestCore.Common.Helper;
using TestCore.Common.Infrastructure;
using TestCore.Common.Routing;

namespace TestCore.Admin.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
  {
    /// <summary>
    /// Configure the application HTTP request pipeline
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void ConfigureRequestPipeline(this IApplicationBuilder application)
    {
      EngineContext.Current.ConfigureRequestPipeline(application);
    }

    /// <summary>
    /// Add exception handling
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseTestCoreExceptionHandler(this IApplicationBuilder application)
    {
      var TestCoreConfig = EngineContext.Current.Resolve<TestCoreConfig>();
      var hostingEnvironment = EngineContext.Current.Resolve<IHostingEnvironment>();
      var useDetailedExceptionPage = TestCoreConfig.DisplayFullErrorStack || hostingEnvironment.IsDevelopment();
      if (useDetailedExceptionPage)
      {
        //get detailed exceptions for developing and testing purposes
        application.UseDeveloperExceptionPage();
      }
      else
      {
        //or use special exception handler
        application.UseExceptionHandler("/error");
      }

      //log errors
      application.UseExceptionHandler(handler =>
      {
        handler.Run(context =>
              {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception == null)
                  return Task.CompletedTask;

                try
                {
                  //log error
                }
                finally
                {
                  //rethrow the exception to show the error page
                  throw exception;
                }
              });
      });
    }

    /// <summary>
    /// Adds a special handler that checks for responses with the 404 status code that do not have a body
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UsePageNotFound(this IApplicationBuilder application)
    {
      application.UseStatusCodePages(async context =>
      {
        //handle 404 Not Found
        if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
        {
          var webHelper = EngineContext.Current.Resolve<IWebHelper>();
          if (!webHelper.IsStaticResource())
          {
            //get original path and query
            var originalPath = context.HttpContext.Request.Path;
            var originalQueryString = context.HttpContext.Request.QueryString;

            //store the original paths in special feature, so we can use it later
            context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
            {
              OriginalPathBase = context.HttpContext.Request.PathBase.Value,
              OriginalPath = originalPath.Value,
              OriginalQueryString = originalQueryString.HasValue ? originalQueryString.Value : null,
            });

            //get new path
            context.HttpContext.Request.Path = "/page-not-found";
            context.HttpContext.Request.QueryString = QueryString.Empty;

            try
            {
              //re-execute request with new path
              await context.Next(context.HttpContext);
            }
            finally
            {
              //return original path to request
              context.HttpContext.Request.QueryString = originalQueryString;
              context.HttpContext.Request.Path = originalPath;
              context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(null);
            }
          }
        }
      });
    }

    /// <summary>
    /// Adds a special handler that checks for responses with the 400 status code (bad request)
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseBadRequestResult(this IApplicationBuilder application)
    {
      application.UseStatusCodePages(context =>
      {
        //handle 404 (Bad request)
        if (context.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
        {
          //log error
        }

        return Task.CompletedTask;
      });
    }

    /// <summary>
    /// Configure MVC routing
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseTestCoreMvc(this IApplicationBuilder application)
    {
      application.UseMvc(routeBuilder =>
      {
        //register all routes
        EngineContext.Current.Resolve<IRoutePublisher>().RegisterRoutes(routeBuilder);
      });
    }
  }
}
