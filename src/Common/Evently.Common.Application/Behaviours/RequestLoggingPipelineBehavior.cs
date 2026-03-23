using Evently.Common.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Evently.Common.Application.Behaviours;

internal sealed partial class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string moduleName = GetModuleName(typeof(TRequest).FullName!);
        string requestName = typeof(TRequest).Name;

        using (LogContext.PushProperty("Module", moduleName))
        {
            LogProcessingRequestRequestName(logger, requestName);

            TResponse result = await next(cancellationToken);

            if (result.IsSuccess)
            {
                LogCompletedRequestRequestName(logger, requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed request {RequestName} with error", requestName);
                }
            }

            return result;
        }
    }

    private static string GetModuleName(string requestName)
    {
        return requestName.Split('.')[2];
    }

    [LoggerMessage(LogLevel.Information, "Processing request {requestName}")]
    static partial void LogProcessingRequestRequestName(
        ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger, string requestName);

    [LoggerMessage(LogLevel.Information, "Completed request {requestName}")]
    static partial void LogCompletedRequestRequestName(
        ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger, string requestName);
}
