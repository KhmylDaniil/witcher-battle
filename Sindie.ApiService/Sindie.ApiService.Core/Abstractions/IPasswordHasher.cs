namespace Witcher.Core.Abstractions
{
	/// <summary>
	/// Интерфейс хеширования пароля
	/// </summary>
	public interface IPasswordHasher
	{
		/// <summary>
		/// Хеш функция
		/// </summary>
		/// <param name="password">Пароль</param>
		/// <returns>Хешированный пароль</returns>
		string Hash(string password);

		/// <summary>
		/// Верификация пароля
		/// </summary>
		/// <param name="password">Пароль</param>
		/// <param name="hash">Хешированный пароль</param>
		/// <returns>Истинность</returns>
		bool VerifyHash(string password, string hash);
	}
}
