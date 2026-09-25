using FluentAssertions;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Domain.UnitTest.TestSupport;

namespace Wastelands.Service.Domain.UnitTest.Entities
{
	[TestClass]
	public class BattleMapPlacementTests
	{
		private const long MapId = 7;

		private static BattleMap CreateMap(long gameId = 1) => new BattleMap(gameId, "Map", null, 5, 5, HexTerrainStyle.Grass).WithId(MapId);

		private static (Battle Battle, Creature Wolf, BattleCharacter Hero) CreateBattle(BattleMap? map)
		{
			var battle = new Battle(gameId: 1, name: "Battle").WithId(1);
			var wolf = TestBuilders.Creature(battle.Id, TestBuilders.CreatureTemplate(TestBuilders.BodyTemplate())).WithId(10);
			var hero = TestBuilders.BattleCharacter(battle.Id, TestBuilders.Character().WithId(20));
			battle.Creatures.Add(wolf);
			battle.Characters.Add(hero);
			if (map is not null)
			{
				battle.AttachMap(map);
			}

			return (battle, wolf, hero);
		}

		[TestMethod]
		public void PlaceParticipantOnMap_SetsPositionOfCreatureAndCharacter()
		{
			var map = CreateMap();
			var (battle, wolf, hero) = CreateBattle(map);

			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 1, 2);
			battle.PlaceParticipantOnMap(ParticipantKind.Character, hero.CharacterId, map, 3, 4);

			(wolf.MapColumn, wolf.MapRow).Should().Be((1, 2));
			(hero.MapColumn, hero.MapRow).Should().Be((3, 4));
			battle.FindParticipantAt(3, 4).Should().Be((ParticipantKind.Character, hero.CharacterId));
		}

		[TestMethod]
		public void PlaceParticipantOnMap_HexOccupiedByAnotherParticipant_Throws()
		{
			var map = CreateMap();
			var (battle, wolf, hero) = CreateBattle(map);
			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 1, 1);

			var act = () => battle.PlaceParticipantOnMap(ParticipantKind.Character, hero.CharacterId, map, 1, 1);

			act.Should().Throw<InvalidArgumentException>();
			hero.MapColumn.Should().BeNull();
		}

		[TestMethod]
		public void PlaceParticipantOnMap_SameParticipantAgain_MovesIt()
		{
			var map = CreateMap();
			var (battle, wolf, _) = CreateBattle(map);
			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 1, 1);

			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 1, 1);
			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 2, 2);

			(wolf.MapColumn, wolf.MapRow).Should().Be((2, 2));
			battle.FindParticipantAt(1, 1).Should().BeNull();
		}

		[TestMethod]
		[DataRow(HexTerrainType.Impassable)]
		[DataRow(HexTerrainType.Wall)]
		[DataRow(HexTerrainType.Void)]
		public void PlaceParticipantOnMap_NotPassableHex_Throws(HexTerrainType terrainType)
		{
			var map = CreateMap();
			map.PaintHexes([new BattleMapHexPaintDraft(0, 0, terrainType, HexTerrainStyle.Grass)]);
			var (battle, wolf, _) = CreateBattle(map);

			var act = () => battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 0, 0);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void PlaceParticipantOnMap_OutOfBounds_Throws()
		{
			var map = CreateMap();
			var (battle, wolf, _) = CreateBattle(map);

			var act = () => battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 5, 0);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void PlaceParticipantOnMap_MapNotAttached_Throws()
		{
			var (battle, wolf, _) = CreateBattle(map: null);

			var act = () => battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, CreateMap(), 0, 0);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void PlaceParticipantOnMap_AfterBattleStarted_IsAllowed()
		{
			var map = CreateMap();
			var (battle, wolf, _) = CreateBattle(map);
			battle.MarkStarted();

			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 0, 0);

			wolf.MapColumn.Should().Be(0);
		}

		[TestMethod]
		public void PlaceParticipantOnMap_UnknownParticipant_Throws()
		{
			var map = CreateMap();
			var (battle, _, _) = CreateBattle(map);

			var act = () => battle.PlaceParticipantOnMap(ParticipantKind.Creature, 999, map, 0, 0);

			act.Should().Throw<NotFoundException>();
		}

		[TestMethod]
		public void AttachMap_AnotherMap_ClearsAllPositions()
		{
			var map = CreateMap();
			var (battle, wolf, hero) = CreateBattle(map);
			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 0, 0);
			battle.PlaceParticipantOnMap(ParticipantKind.Character, hero.CharacterId, map, 1, 0);

			battle.AttachMap(new BattleMap(1, "Other", null, 3, 3, HexTerrainStyle.Sand).WithId(MapId + 1));

			battle.BattleMapId.Should().Be(MapId + 1);
			wolf.MapColumn.Should().BeNull();
			hero.MapColumn.Should().BeNull();
		}

		[TestMethod]
		public void AttachMap_SameMapAgain_KeepsPositions()
		{
			var map = CreateMap();
			var (battle, wolf, _) = CreateBattle(map);
			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 0, 0);

			battle.AttachMap(map);

			wolf.MapColumn.Should().Be(0);
		}

		[TestMethod]
		public void AttachMap_MapOfAnotherGame_Throws()
		{
			var (battle, _, _) = CreateBattle(map: null);

			var act = () => battle.AttachMap(CreateMap(gameId: 2));

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void AttachMap_Null_DetachesAndClearsPositions()
		{
			var map = CreateMap();
			var (battle, wolf, _) = CreateBattle(map);
			battle.PlaceParticipantOnMap(ParticipantKind.Creature, wolf.Id, map, 0, 0);

			battle.AttachMap(null);

			battle.BattleMapId.Should().BeNull();
			wolf.MapColumn.Should().BeNull();
		}

		[TestMethod]
		public void RemoveParticipantFromMap_ClearsPosition()
		{
			var map = CreateMap();
			var (battle, _, hero) = CreateBattle(map);
			battle.PlaceParticipantOnMap(ParticipantKind.Character, hero.CharacterId, map, 2, 2);

			battle.RemoveParticipantFromMap(ParticipantKind.Character, hero.CharacterId);

			hero.MapColumn.Should().BeNull();
			battle.FindParticipantAt(2, 2).Should().BeNull();
		}
	}
}
