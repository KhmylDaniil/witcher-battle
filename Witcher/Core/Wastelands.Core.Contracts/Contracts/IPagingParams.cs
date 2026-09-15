namespace Wastelands.Core.Contracts.Contracts
{
	public interface IPagingParams
	{
		int PageSize { get; set; }

		int PageNumber { get; set; }
	}
}
