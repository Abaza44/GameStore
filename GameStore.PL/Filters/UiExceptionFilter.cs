using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace GameStore.PL.Filters
{
    public class UiExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<UiExceptionFilter> _logger;
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        private readonly IWebHostEnvironment _env;

        public UiExceptionFilter(
            ILogger<UiExceptionFilter> logger,
            ITempDataDictionaryFactory tempDataFactory,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _tempDataFactory = tempDataFactory;
            _env = env;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;
            var requestId = context.HttpContext.TraceIdentifier;

            // Log
            _logger.LogError(ex, "Unhandled exception. RequestId: {RequestId}", requestId);

            // Friendly message per exception type
            string userMessage = "We hit a snag. Please try again later.";
            if (ex is DbUpdateException)
                userMessage = "A database error occurred while saving your data.";
            else if (ex is UnauthorizedAccessException)
                userMessage = "You don't have permission to perform this action.";

            // TempData to show toast or message in Error view
            var temp = _tempDataFactory.GetTempData(context.HttpContext);
            temp["ErrorTitle"] = "Something went wrong";
            temp["ErrorMessage"] = _env.IsDevelopment() ? ex.Message : userMessage;
            temp["RequestId"] = requestId;

            // Redirect to GUI Error page
            context.Result = new RedirectToActionResult("Error", "Home", null);
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}