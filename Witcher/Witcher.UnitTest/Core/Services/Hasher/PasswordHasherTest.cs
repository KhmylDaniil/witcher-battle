using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Exceptions;
using Witcher.Core.Services.Hasher;
using System;

namespace Witcher.UnitTest.Core.Hasher
{
	/// <summary>
	/// Тест для <see cref="PasswordHasher" >
	/// </summary>
	[TestClass]
	public class PasswordHasherTest : UnitTestBase
	{
		/// <summary>
		/// Тест метода Hash( - хеширование пароля
		/// - хешированный пароль должен совпадать с ожидаемым паролем
		/// </summary>
		/// <param name="salt">Соль</param>
		/// <param name="password">Пароль</param>
		/// <param name="expectedPassword">Ожидаемый пароль</param>
		/// <returns></returns>
		[DataRow("1", "2", "pMfNI9obZJmKEi8loD0tFxZcYFu4hoNyLwhmMSGKy3I=")]
		[DataRow("2", "2", "cTlVOEINQZa1IqglSZUxNzVMtGNjeqiPNa11x35gKY4=")]
		[TestMethod]
		public void Hash_ByPasswordHasher_ShouldFindHashedPassword
			(string salt, string password, string expectedPassword)
		{
			var hasherOptions = new HasherOptions()
			{
				Salt = salt
			};

			//Arrange
			var passwordHasher = new PasswordHasher(hasherOptions);

			//Act
			var result = passwordHasher.Hash(password);

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(expectedPassword, result);
		}

		/// <summary>
		/// Тест метода Hash( - хеширование пароля
		/// - должен генерировать исключение в случае если пришел пустой пароль
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public void Hash_ByPasswordHasher_ShouldThrowArgumentNullException()
		{
			var hasherOptions = new HasherOptions()
			{
				Salt = "a"
			};

			//Arrange
			var passwordHasher = new PasswordHasher(hasherOptions);

			//Assert
			Assert.ThrowsException<ArgumentException>(() =>
				passwordHasher.Hash(null));
		}

		/// <summary>
		/// Тест метода VerifyHash( - верификация пароля
		/// - должен проверять истинность в зависимости от совпадения паролей
		/// </summary>
		/// <param name="password">Пароль</param>
		/// <param name="hash">Хеш</param>
		/// <param name="expectedBool">Ожидаемый логический результат</param>
		/// <returns></returns>
		[DataRow("2", "pMfNI9obZJmKEi8loD0tFxZcYFu4hoNyLwhmMSGKy3I=", true)]
		[DataRow("1", "1", false)]
		[TestMethod]
		public void VerifyHash_ByPasswordHasher_ShouldFindBoole
			(string password, string hash, bool expectedBool)
		{
			var hasherOptions = new HasherOptions()
			{
				Salt = "1"
			};

			//Arrange
			var passwordHasher = new PasswordHasher(hasherOptions);

			//Act
			var result = passwordHasher.VerifyHash(password, hash);

			//Assert
			Assert.AreEqual(expectedBool, result);
		}

		/// <summary>
		/// Тест метода VerifyHash( - верификация пароля
		/// - должен генерировать исключение в случае если пришел пустой пароль или хеш
		/// </summary>
		/// <param name="password">Пароль</param>
		/// <param name="hash">Ожидаемый пароль</param>
		/// <returns></returns>
		[DataRow(null, "1")]
		[DataRow("1", null)]
		[TestMethod]
		public void VerifyHash_ByPasswordHasher_ShouldThrowArgumentNullException
			(string password, string hash)
		{
			var hasherOptions = new HasherOptions()
			{
				Salt = "1"
			};

			//Arrange
			var passwordHasher = new PasswordHasher(hasherOptions);

			//Assert
			Assert.ThrowsException<ArgumentException>(() =>
				passwordHasher.VerifyHash(password, hash));
		}
	}
}