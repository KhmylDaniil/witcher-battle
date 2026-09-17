namespace Wastelands.Service.Application.Models.Dto
{
	/// <summary>Единый формат ответа для всех пагинируемых списков (шаблоны существ, шаблоны тела, игры, персонажи).</summary>
	public sealed class PagedResultDto<T>
	{
		public List<T> Items { get; set; } = [];

		public long TotalCount { get; set; }

		public int PageNumber { get; set; }

		public int PageSize { get; set; }
	}
}
