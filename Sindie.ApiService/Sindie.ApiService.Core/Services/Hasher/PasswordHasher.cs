using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions;
using System;
using System.Text;

namespace Sindie.ApiService.Core.Services.Hasher
{
	/// <summary>
	/// хеширование пароля
	/// </summary>
	public class PasswordHasher : IPasswordHasher
	{
		/// <summary>
		/// Соль
		/// </summary>
		private readonly string _salt;

		/// <summary>
		/// Конструктор класса хеширование пароля
		/// </summary>
		/// <param name="options">Параметры хеширования</param>
		public PasswordHasher(HasherOptions options)
		{
			_salt = options.Salt;
		}

		/// <summary>
		/// Хеш функция
		/// </summary>
		/// <param name="password">Пароль</param>
		/// <returns>Хешированый пароль</returns>
		public string Hash(string password)
		{
			if (password == null)
				throw new ArgumentException("Пустое поле пароль.");

			byte[] Salt = Encoding.ASCII.GetBytes(_salt);

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
			if (password == null)
				throw new ArgumentException("Пустое поле пароль.");
			if (hash == null)
				throw new ArgumentException("Пустое поле хешированый пароль.");

			return string.Equals(hash, Hash(password), StringComparison.Ordinal);
		}
	}
}
