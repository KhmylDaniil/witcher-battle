using Wastelands.Core.Contracts.Contracts;

namespace Wastelands.Service.Infrastructure.IntegrationTest.TestSupport
{
	internal sealed class TestUserContext : IUserContext
	{
		public long CurrentUserId { get; set; }
	}
}
