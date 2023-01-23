using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace X_labs_BeerQuest.Domain
{
	public class ValidateRequestHandlerDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		readonly private IRequestHandler<TRequest, TResponse> _decoratedRequest;
		readonly private IValidator<TRequest>? _validator;

		public ValidateRequestHandlerDecorator(IValidator<TRequest>? validator,
			IRequestHandler<TRequest, TResponse> decoratedRequest
		)
		{
			_validator = validator;
			_decoratedRequest = decoratedRequest;
		}

		#region Implementation of IRequestHandler<in TRequest,TResponse>

		/// <summary>
		///     Validates the request before passing down the request pipeline
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ValidationException"></exception>
		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
		{
			if (_validator is null)
			{
				return await _decoratedRequest.Handle(request, cancellationToken);
			}

			var validationResult = await _validator.ValidateAsync(request, cancellationToken);
			if (validationResult.IsValid)
			{
				return await _decoratedRequest.Handle(request, cancellationToken);
			}

			throw new ValidationException("invalid request", validationResult.Errors);
		}

		#endregion
	}
}