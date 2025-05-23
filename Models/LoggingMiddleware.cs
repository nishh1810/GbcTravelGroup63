﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
namespace Gbc_Travel_Group63.Models
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log incoming request
            _logger.LogInformation($"Incoming Request: {context.Request.Path}");

            // Call the next middleware in the pipeline
            await _next(context);

            // Log response status
            _logger.LogInformation($"Response Status: {context.Response.StatusCode}");
        }
    }
}

