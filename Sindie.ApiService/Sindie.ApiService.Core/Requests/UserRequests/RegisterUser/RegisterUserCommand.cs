using MediatR;
using Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.UserRequests.RegisterUser
{
	/// <summary>
	/// Регистрация
	/// </summary>
	public class RegisterUserCommand
		: RegisterUserRequest, IRequest<RegisterUserCommandResponse>
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public RegisterUserCommand()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="registerUserRequest">Запрос регистрации пользователя</param>
		public RegisterUserCommand(RegisterUserRequest registerUserRequest)
		{
			Name = registerUserRequest.Name;
			Email = registerUserRequest.Email;
			Login = registerUserRequest.Login;
			Phone = registerUserRequest.Phone;
			Password = registerUserRequest.Password;
		}
	}
}
