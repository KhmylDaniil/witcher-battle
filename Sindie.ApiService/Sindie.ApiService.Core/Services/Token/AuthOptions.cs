using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Sindie.ApiService.Core.Services.Token
{
	/// <summary>
	/// Параметры для генерации токенов
	/// </summary>
	public class AuthOptions
	{
		/// <summary>
		/// Издатель токена
		/// </summary>
		public string AuthServer { get; set; }

		/// <summary>
		/// Потребитель токена
		/// </summary>
		public string AuthClient { get; set; }

		/// <summary>
		/// Ключ для шифрации
		/// </summary>
		public static string AuthKey { get; set; }

		/// <summary>
		/// Время жизни токена - 10 мин
		/// </summary>
		public const int LIFETIME = 1000;

		/// <summary>
		/// Конструктор параметров генератора токена
		/// </summary>
		/// <param name="authServer">Издатель токена</param>
		/// <param name="authClient">Потребитель токена</param>
		/// <param name="authKey">Ключ шифрации токена</param>
		public AuthOptions(string authServer, string authClient, string authKey)
		{
			AuthServer = authServer;
			AuthClient = authClient;
			AuthKey = authKey;
		}

		/// <summary>
		/// Кодировщик ключа токена
		/// </summary>
		/// <returns>Симметричный ключ безопасности</returns>
		public static SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthKey));
		}
	}
}
