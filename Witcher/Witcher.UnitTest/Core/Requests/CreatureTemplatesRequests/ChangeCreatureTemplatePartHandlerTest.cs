using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Drafts.BodyTemplateDrafts;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CreatureTemplateRequests;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	[TestClass]
	public class ChangeCreatureTemplatePartHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplatePart _head;
		private readonly CreatureTemplatePart _torso;

		/// <summary>
		/// Конструктор для теста <see cref="ChangeCreatureTemplatePartHandler"/>
		/// </summary>
		public ChangeCreatureTemplatePartHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_bodyTemplate.CreateBodyTemplateParts(CreateBodyTemplatePartsDraft.CreateBodyPartsDraft());

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: _bodyTemplate,
				creatureType: CreatureType.Beast);

			_torso = CreatureTemplatePart.CreateForTest(
				creatureTemplate: _creatureTemplate,
				bodyPartType: BodyPartType.Torso,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 3,
				maxToHit: 10,
				armor: 0);

			_head = CreatureTemplatePart.CreateForTest(
				creatureTemplate: _creatureTemplate,
				bodyPartType: BodyPartType.Head,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 2,
				armor: 0);

			_creatureTemplate.CreatureTemplateParts.Add(_head);
			_creatureTemplate.CreatureTemplateParts.Add(_torso);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_bodyTemplate,
				_creatureTemplate));
		}

		/// <summary>
		/// Тест метода Handle - изменение части шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeCreatureTemplatePart_OneCreatureTemplatePartIsChanged()
		{
			var request = new ChangeCreatureTemplatePartCommand()
			{
				CreatureTemplateId = _creatureTemplate.Id,
				Id = _head.Id,
				ArmorValue = 5
			};

			var newHandler = new ChangeCreatureTemplatePartHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.CreatureTemplateParts);

			var head = creatureTemplate.CreatureTemplateParts.FirstOrDefault(x => x.Id == _head.Id);
			Assert.IsNotNull(head);
			Assert.AreEqual(5, head.Armor);
		}

		/// <summary>
		/// Тест метода Handle - изменение всех частей шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeCreatureTemplatePart_AllCreatureTemplatePartsAreChanged()
		{
			var request = new ChangeCreatureTemplatePartCommand()
			{
				CreatureTemplateId = _creatureTemplate.Id,
				Id = null,
				ArmorValue = 3
			};

			var newHandler = new ChangeCreatureTemplatePartHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.CreatureTemplateParts);

			var head = creatureTemplate.CreatureTemplateParts.FirstOrDefault(x => x.Id == _head.Id);
			Assert.IsNotNull(head);
			Assert.AreEqual(3, head.Armor);

			var torso = creatureTemplate.CreatureTemplateParts.FirstOrDefault(x => x.Id == _torso.Id);
			Assert.IsNotNull(torso);
			Assert.AreEqual(3, torso.Armor);
		}
	}
}
