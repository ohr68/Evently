using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Application.Abstractions.Exceptions;

public sealed class EventlyException : Exception
{
    public EventlyException(string requestName, Error? error = null, Exception? innerException = null)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }
    public Error? Error { get; }
}
