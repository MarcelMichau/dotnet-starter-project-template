using CSharpFunctionalExtensions;

namespace DotNetStarterProjectTemplate.Application.Shared.Utils;

public interface IQueryHandler<in TRequest, TResponse>
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}