using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System.Text;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Infrastructure.Options;

namespace Wastelands.Service.Infrastructure.Services
{
	public class PasswordService : IPasswordService
	{
		private readonly HasherOptions _options;

		public PasswordService(IOptions<HasherOptions> options)
		{
			_options = options.Value;				
		}

		public string GetPasswordHash(string password)
		{
			ArgumentNullException.ThrowIfNull(password);

			byte[] Salt = Encoding.ASCII.GetBytes(_options.Salt);

			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: Salt,
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 10000,
				numBytesRequested: 256 / 8));

			return hashed;
		}

		/// <summary>
		/// Верификация пароля
		/// </summary>
		/// <param name="password">Пароль</param>
		/// <param name="hash">Хешированый пароль</param>
		/// <returns>Истинность</returns>
		public bool VerifyHash(string password, string hash)
		{
			ArgumentNullException.ThrowIfNull(password);
			ArgumentNullException.ThrowIfNull(hash);

			return string.Equals(hash, GetPasswordHash(password), StringComparison.Ordinal);
		}
	}
}
