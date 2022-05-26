using System;

namespace Sindie.ApiService.Core.Contracts.SlotRequests
{
	/// <summary>
	/// Ответ на команду создания слота
	/// </summary>
	public class CreateSlotResponse
	{
		/// <summary>
		/// Айди слота
		/// </summary>
		public Guid SlotId { get; set; }
	}
}
