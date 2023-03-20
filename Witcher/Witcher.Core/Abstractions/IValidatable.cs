using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher.Core.Abstractions
{
	public interface IValidatableCommand<T> : IRequest<T>
	{
		void Validate();
	}

	public interface IValidatableCommand : IRequest
	{
		void Validate();
	}
}
