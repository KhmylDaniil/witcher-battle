using MediatR;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.ExtensionMethods
{
	public static class Handler
	{
		public static Task<TResponse> SendValidated<TResponse>(this IMediator mediator, IValidatableCommand<TResponse> request, CancellationToken cancellationToken = default)
		{
			if (request is null)
				throw new RequestNullException<IValidatableCommand<TResponse>>();

			request.Validate();
			return mediator.Send(request, cancellationToken);
		}

		public static Task SendValidated(this IMediator mediator, IValidatableCommand request, CancellationToken cancellationToken = default)
		{
			if (request is null)
				throw new RequestNullException<IValidatableCommand>();

			request.Validate();
			return mediator.Send(request, cancellationToken);
		}
	}
}
