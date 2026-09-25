using AutoMapper;
using FluentAssertions;
using Moq;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Mapping;
using Wastelands.Service.Application.Services;
using Wastelands.Service.Application.UnitTest.TestSupport;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.Services
{
	/// <summary>
	/// Кто что видит на карте боя. Видимость самого боя (игрок — только идущий бой со своим персонажем)
	/// обеспечивает скоуп BattleRepository — здесь он замокан: "бой не найден" = репозиторий вернул null.
	/// </summary>
	[TestClass]
	public class BattleMapPlacementServiceTests
	{
		private const long BattleId = 1;
		private const long MapId = 5;
		private const long GmUserId = 100;
		private const long PlayerUserId = 200;

		private readonly Mock<IBattleRepository> _battles = new();
		private readonly Mock<IBattleMapRepository> _maps = new();
		private readonly Mock<ICreatureTemplateRepository> _templates = new();
		private readonly Mock<IGameAccessGuard> _guard = new();
		private readonly Mock<IUserContext> _user = new();
		private Battle _battle = null!;
		private BattleMap _map = null!;

		[TestInitialize]
		public void Setup()
		{
			_map = new BattleMap(1, "Map", null, 3, 3, HexTerrainStyle.Grass).WithId(MapId);
			_map.SetMarker(2, 2, "Секретный лаз");
			_map.SetMarker(0, 1, "Колодец", visibleToPlayers: true);

			_battle = new Battle(1, "Battle").WithId(BattleId);
			var wolf = TestBuilders.Creature(BattleId, TestBuilders.CreatureTemplate().WithId(7), "Wolf").WithId(10);
			var heroCharacter = TestBuilders.Character(userId: PlayerUserId).WithId(20);
			// Навигация Character проставляется EF при загрузке боя (Include в BattleRepository), здесь — вручную.
			var hero = TestBuilders.BattleCharacter(BattleId, heroCharacter);
			hero.Character = heroCharacter;
			_battle.Creatures.Add(wolf);
			_battle.Characters.Add(hero);
			_battle.AttachMap(_map);
			_battle.PlaceParticipantOnMap(ParticipantKind.Creature, 10, _map, 0, 0);
			_battle.PlaceParticipantOnMap(ParticipantKind.Character, 20, _map, 1, 0);

			_battles.Setup(r => r.GetByIdAsync(BattleId)).ReturnsAsync(_battle);
			_maps.Setup(r => r.GetByIdAsync(MapId)).ReturnsAsync(_map);
			_maps.Setup(r => r.GetByIdUnscopedAsync(MapId)).ReturnsAsync(_map);
			_templates.Setup(r => r.GetImageKeysUnscopedAsync(It.IsAny<IReadOnlyCollection<long>>()))
				.ReturnsAsync(new Dictionary<long, string?> { [7] = "wolf-key" });
		}

		private BattleMapPlacementService BuildService(long currentUserId)
		{
			_user.Setup(u => u.CurrentUserId).Returns(currentUserId);
			_guard.Setup(g => g.IsOwnerAsync(It.IsAny<long>())).ReturnsAsync(currentUserId == GmUserId);
			_guard.Setup(g => g.GetGameOwnedByCurrentUserAsync(It.IsAny<long>())).Returns(() =>
				currentUserId == GmUserId
					? Task.FromResult(new Game("Game", GmUserId))
					: throw new InvalidArgumentException(Core.Contracts.Enums.ErrorCode.CurrentUserNotAllowedToPerformThisAction, "not gm"));

			var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
			return new BattleMapPlacementService(_battles.Object, _maps.Object, _templates.Object, _guard.Object, Mock.Of<IBattleNotifier>(), _user.Object, mapper);
		}

		[TestMethod]
		public async Task GetMapView_Gm_CanEditAndSeesMarkers()
		{
			var view = await BuildService(GmUserId).GetMapViewAsync(BattleId);

			view.CanEdit.Should().BeTrue();
			view.Map!.Hexes.Single(h => h.Column == 2 && h.Row == 2).MarkerText.Should().Be("Секретный лаз");
			_maps.Verify(r => r.GetByIdUnscopedAsync(It.IsAny<long>()), Times.Never);
		}

		[TestMethod]
		public async Task GetMapView_Player_IsReadOnly_SeesOnlyPlayerVisibleMarkers_PositionsAndAvatars()
		{
			var view = await BuildService(PlayerUserId).GetMapViewAsync(BattleId);

			view.CanEdit.Should().BeFalse();
			view.Map.Should().NotBeNull();
			view.Map!.Hexes.Single(h => h.Column == 2 && h.Row == 2).MarkerText.Should().BeNull("маркер мастера скрыт от игроков");
			view.Map.Hexes.Single(h => h.Column == 0 && h.Row == 1).MarkerText.Should().Be("Колодец");
			view.Map.Hexes.Count(h => h.MarkerText != null).Should().Be(1);
			view.Participants.Should().HaveCount(2);

			var wolf = view.Participants.Single(p => p.Kind == ParticipantKind.Creature);
			(wolf.Column, wolf.Row).Should().Be((0, 0));
			wolf.ImageUrl.Should().Be("/api/images/wolf-key");
			wolf.ControlledByCurrentUser.Should().BeFalse();

			view.Participants.Single(p => p.Kind == ParticipantKind.Character).ControlledByCurrentUser.Should().BeTrue();
		}

		[TestMethod]
		public async Task GetMapView_BattleNotVisibleToUser_ThrowsNotFound()
		{
			_battles.Setup(r => r.GetByIdAsync(BattleId)).ReturnsAsync((Battle?)null);

			var act = () => BuildService(PlayerUserId).GetMapViewAsync(BattleId);

			await act.Should().ThrowAsync<NotFoundException>();
		}

		[TestMethod]
		public async Task PlaceParticipant_Player_IsForbidden()
		{
			var act = () => BuildService(PlayerUserId).PlaceParticipantAsync(new Models.Requests.PlaceParticipantOnMapRequest
			{
				BattleId = BattleId, Kind = ParticipantKind.Character, ParticipantId = 20, Column = 2, Row = 1,
			});

			await act.Should().ThrowAsync<InvalidArgumentException>();
			_battle.Characters[0].MapColumn.Should().Be(1);
		}
	}
}
