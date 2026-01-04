using Com.ByteAnalysis.IFacilita.eCartorio.Application.Errors;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Método construtor da classe
        /// </summary>
        /// <param name="next">Objeto de controle das requisições</param>
        /// <param name="logger">Objeto de log</param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Método de flow da aplicação
        /// </summary>
        /// <param name="httpContext">Contexto http</param>
        /// <returns>Indicativo de processo async</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (IsInternalServerError(ex))
                {
                    //Add Logs
                }

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static bool IsInternalServerError(Exception exception)
        {
            return GetStatusCode(exception) == 500;
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception is BaseException
                ? ((BaseException)exception).StatusCodeException
                : (int)HttpStatusCode.InternalServerError;
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetStatusCode(exception);
            var problemDetails = new ProblemDetailsFields
            {
                Status = context.Response.StatusCode,
                Title = "Error",
                Type = exception.GetType().Name,
                Detail = exception.Message,
                Instance = exception?.TargetSite?.Name ?? string.Empty,
                FieldErrors = exception.Data.Contains("FieldErrors") ? (List<FieldError>)exception.Data["FieldErrors"] : null
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
        }
    }
}
