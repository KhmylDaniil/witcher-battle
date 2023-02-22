using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Services.Token;
using System;

namespace Sindie.ApiService.UnitTest.Core.Token
{
	/// <summary>
	/// Тест для <see cref="JwtGenerator" >
	/// </summary>
	[TestClass]
	public class JwtGeneratorTest : UnitTestBase
	{
		/// <summary>
		/// Тест метода CreateToken( - создание токена
		/// - должен создавать токен
		/// </summary>
		/// <param name="id">Айди</param>
		/// <returns></returns>
		[DataRow("6F9619FF-8B86-D011-B42D-00CF4FC964FF")]
		[DataRow("6F9619FF-8B86-D011-B42D-00CF4FC964AA")]
		[TestMethod]
		public void CreateToken_ByJwtGenerator_ShouldCreateToken
			(string id)
		{
			var authOptions = new AuthOptions("123", "123", "12345678901234567890");
			var userId = new Guid(id);

			//Arrange
			var jwtGenerator = new JwtGenerator(authOptions);

			//Act
			var result = jwtGenerator.CreateToken(userId, "UserRole");

			//Assert
			Assert.IsNotNull(result);
		}
	}
}