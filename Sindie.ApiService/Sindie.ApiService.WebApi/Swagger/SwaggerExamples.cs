using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using Sindie.ApiService.WebApi.Controllers;
using Swashbuckle.AspNetCore.Filters;

namespace Sindie.ApiService.WebApi.Swagger
{
	/// <summary>
	/// Примеры запросов для эндпоинтов swagger
	/// </summary>
	public class SwaggerExamples : IExamplesProvider<LoginUserCommand>
	{
		/// <summary>
		/// Примеры запроса <see cref="UserController.Login"/>
		/// </summary>
		/// <returns>Примеры запроса</returns>
		public LoginUserCommand GetExamples() => new() { Login = "Sindie", Password = "123456" };
	}
}
