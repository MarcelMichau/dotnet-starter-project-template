using CSharpFunctionalExtensions;

namespace DotNetStarterProjectTemplate.Application.Shared.Utils;

public interface ICommandHandler<in TRequest>
{
    Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TRequest, TResponse>
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}