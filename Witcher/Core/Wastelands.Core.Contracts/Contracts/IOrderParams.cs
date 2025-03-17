using Wastelands.Core.Contracts.Enums;

namespace Wastelands.Core.Contracts.Contracts
{
	public interface IOrderParams
	{
		string OrderBy { get; set; }

		OrderDirection OrderDirection { get; set; }
	}
}
