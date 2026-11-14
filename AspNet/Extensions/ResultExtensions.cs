
using AspNet.BLL.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Extensions
{

    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result, Func<T, IActionResult>? onSuccess = null)
        {
            if (result.IsFailure)
                return controller.ToErrorResponse(result.Error!);

            return onSuccess is not null
                ? onSuccess(result.Value!)
                : controller.Ok(result.Value);
        }

        public static IActionResult ToActionResult(this ControllerBase controller, Result result)
        {
            if (result.IsFailure)
                return controller.ToErrorResponse(result.Error!);

            return controller.NoContent();
        }

        private static IActionResult ToErrorResponse(this ControllerBase controller, Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var detail = error.Type == ErrorType.Failure
                ? "An unexpected error occurred. Please try again later."
                : error.Message;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = error.Code,
                Detail = detail
            };

            problemDetails.Extensions["code"] = error.Code;

            problemDetails.Extensions["traceId"] =
                controller.HttpContext.TraceIdentifier;

            return controller.StatusCode(statusCode, problemDetails);
        }
    }
}
