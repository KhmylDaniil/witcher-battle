using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.ModifierRequests.ChangeModifier;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.ModifierRequests.ChangeModifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.ModifierRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeModifierHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeModifierHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Modifier _modifier;
		private readonly ImgFile _imgFile1;
		private readonly ImgFile _imgFile2;
		private readonly CharacterTemplate _characterTemplate1;
		private readonly CharacterTemplate _characterTemplate2;
		private readonly ItemTemplate _itemTemplate1;
		private readonly ItemTemplate _itemTemplate2;
		private readonly Script _script1;
		private readonly Script _script2;
		private readonly Event _event1;
		private readonly Event _event2;
		private readonly ModifierScript _modifierScript;

		/// <summary>
		/// Конструктор для теста <see cref="ChangeModifierHandler"/>
		/// </summary>
		public ChangeModifierHandlerTest(): base()
		{
			_game = Game.CreateForTest(id: GameId);

			_imgFile1 = ImgFile.CreateForTest();
			_imgFile2 = ImgFile.CreateForTest();
			_modifier = Modifier.CreateForTest(game: _game, imgFile: _imgFile1);
			_characterTemplate1 = CharacterTemplate.CreateForTest(game: _game);
			_characterTemplate2 = CharacterTemplate.CreateForTest(game: _game);
			_itemTemplate1 = ItemTemplate.CreateForTest(game: _game);
			_itemTemplate2 = ItemTemplate.CreateForTest(game: _game);
			_script1 = Script.CreateForTest(game: _game);
			_script2 = Script.CreateForTest(game: _game);
			_event1 = Event.CreateForTest(game: _game);
			_event2 = Event.CreateForTest(game: _game);
			_modifierScript = ModifierScript.CreateForTest(
				script: _script1,
				@event: _event1,
				modifier: _modifier);

			_modifier.CharacterTemplateModifiers.Add(
				new CharacterTemplateModifier(_modifier, _characterTemplate1));
			_modifier.ItemTemplateModifiers.Add(
				new ItemTemplateModifier(_modifier, _itemTemplate1));
			_modifier.ModifierScripts.Add(_modifierScript);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_modifier,
				_imgFile1,
				_imgFile2,
				_characterTemplate1,
				_characterTemplate2,
				_itemTemplate1,
				_itemTemplate2,
				_script1,
				_script2,
				_event1,
				_event2
				));
		}

		/// <summary>
		/// Тест метода Handle, должен изменить модификатор и зависимые сущности
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeModifier_ShouldReturnUnit()
		{
			var request = new ChangeModifierCommand()
			{
				ModifierId = _modifier.Id,
				GameId = _game.Id,
				Name = "new name",
				Description = "new description",
				ImgFileId = _imgFile2.Id,
				CharacterTemplates = new List<Guid>() { _characterTemplate2.Id },
				ItemTemplates = new List<Guid> { _itemTemplate2.Id },
				ModifierScripts = new List<ChangeModifierCommandItem>
				{
					new ChangeModifierCommandItem()
					{
						Id = _modifierScript.Id,
						ScriptId = _script2.Id,
						EventId = _event2.Id,
						ActivationTime = DateTimeProvider.Object.TimeProvider,
						PeriodOfActivity = 1,
						PeriodOfInactivity = 1,
						NumberOfRepetitions = 1
					}
				}
			};

			var newHandler = new ChangeModifierHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var modifier = _dbContext.Modifiers
				.FirstOrDefault(x => x.Id == request.ModifierId);
			Assert.IsNotNull(modifier);
			Assert.AreEqual(request.Name, modifier.Name);
			Assert.AreEqual(request.Description, modifier.Description);
			Assert.AreEqual(request.ImgFileId, modifier.ImgFileId);

			var characterTemplateModifier = _dbContext.CharacterTemplateModifiers
				.FirstOrDefault(x => x.ModifierId == modifier.Id);
			Assert.IsNotNull(characterTemplateModifier);
			Assert.IsTrue(characterTemplateModifier.CharacterTemplateId == _characterTemplate2.Id);

			var itemTemplateModifier = _dbContext.ItemTemplateModifiers
				.FirstOrDefault(x => x.ModifierId == modifier.Id);
			Assert.IsNotNull(itemTemplateModifier);
			Assert.IsTrue(itemTemplateModifier.ItemTemplateId == _itemTemplate2.Id);

			var modifierScript = modifier.ModifierScripts.
				FirstOrDefault(x => x.Id == _modifierScript.Id);
			Assert.IsNotNull(modifierScript);
			Assert.IsTrue(modifierScript.ScriptId == _script2.Id);
			Assert.IsTrue(modifierScript.EventId == _event2.Id);

			var activeCycles = modifierScript.ActiveCycles;
			Assert.IsNotNull(activeCycles);
			Assert.AreEqual(2, activeCycles.Count);
			var activeCycle = activeCycles.Last();
			Assert.AreEqual(DateTimeProvider.Object.TimeProvider.AddMinutes(2), activeCycle.ActivationBeginning);
			Assert.AreEqual(DateTimeProvider.Object.TimeProvider.AddMinutes(3), activeCycle.ActivationEnd);
		}
	}
}
