using System;
using System.Collections.Generic;
using System.Linq;
using Witcher.Core.Contracts.GameRequests;

namespace Witcher.MVC.Controllers.Api.Dto
{
	/// <summary>
	/// GetGameByIdResponse.Users — Dictionary&lt;Guid, (string Name, string RoleName)&gt;; та же проблема с
	/// ValueTuple, что и в BattleDtos.cs — маппим в явную структуру с нормальными именами полей.
	/// </summary>
	public sealed class GameMemberDto
	{
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public string RoleName { get; set; }
	}

	public sealed class GameDetailsDto
	{
		public Guid Id { get; set; }
		public string GameMasterName { get; set; }
		public string Name { get; set; }
		public Guid? AvatarId { get; set; }
		public string Description { get; set; }
		public List<GameMemberDto> Members { get; set; }
		public List<Guid> TextFiles { get; set; }
		public List<Guid> ImgFiles { get; set; }

		public static GameDetailsDto From(GetGameByIdResponse response) => new()
		{
			Id = response.Id,
			GameMasterName = response.GameMasterName,
			Name = response.Name,
			AvatarId = response.AvatarId,
			Description = response.Description,
			Members = response.Users?
				.Select(kv => new GameMemberDto { UserId = kv.Key, Name = kv.Value.Item1, RoleName = kv.Value.Item2 })
				.ToList() ?? new List<GameMemberDto>(),
			TextFiles = response.TextFiles ?? new List<Guid>(),
			ImgFiles = response.ImgFiles ?? new List<Guid>()
		};
	}
}
