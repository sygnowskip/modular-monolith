using Hexure.Results;
using MediatR;

namespace Hexure.MediatR
{
    public interface ICommandRequest : IRequest<Result> { }
    public interface ICommandRequest<T> : IRequest<Result<T>> { }
}