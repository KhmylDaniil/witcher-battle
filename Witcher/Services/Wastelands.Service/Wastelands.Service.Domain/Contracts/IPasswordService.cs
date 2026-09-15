namespace Wastelands.Service.Domain.Contracts
{
	public interface IPasswordService
	{
		string GetPasswordHash(string password);

		bool VerifyHash(string password, string hash);
	}
}
