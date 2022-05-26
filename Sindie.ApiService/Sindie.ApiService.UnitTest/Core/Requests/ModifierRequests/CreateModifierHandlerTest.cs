using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.ModifierRequests.CreateModifier;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.ModifierRequests.CreateModifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.ModifierRequests
{
	/// <summary>
	/// Тест для <see cref="CreateModifierHandler"/>
	/// </summary>
	[TestClass]
	public class CreateModifierHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;
		private readonly CharacterTemplate _characterTemplate;
		private readonly ItemTemplate _itemTemplate;
		private readonly Script _script;
		private readonly Event _event;

		/// <summary>
		/// Конструктор для теста <see cref="CreateModifierHandler"/>
		/// </summary>
		public CreateModifierHandlerTest(): base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
			_characterTemplate = CharacterTemplate.CreateForTest(game: _game);
			_itemTemplate = ItemTemplate.CreateForTest(game: _game);
			_script = Script.CreateForTest(game: _game);
			_event = Event.CreateForTest(game: _game);
			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _imgFile, _characterTemplate, _itemTemplate, _script, _event));
		}

		/// <summary>
		/// Тест метода Handle - создание модификатора, возвращает юнит
		/// должен создать модификатор и списки зависимых сущностей
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateModifier_ShouldReturnUnit()
		{
			var request = new CreateModifierCommand()
			{
				GameId = _game.Id,
				Name = "name",
				Description = "description",
				ImgFileId = _imgFile.Id,
				CharacterTemplates = new List<Guid> { _characterTemplate.Id },
				ItemTemplates = new List<Guid> { _itemTemplate.Id },
				ModifierScripts = new List<CreateModifierCommandItem>
				{
					new CreateModifierCommandItem()
					{
						ScriptId = _script.Id,
						EventId = _event.Id,
						ActivationTime = DateTimeProvider.Object.TimeProvider,
						PeriodOfActivity = 1,
						PeriodOfInactivity = 1,
						NumberOfRepetitions = 1,
					}
				}
			};

			var newHandler = new CreateModifierHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			Assert.AreEqual(1, _dbContext.Modifiers.Count());
			var modifier = _dbContext.Modifiers.FirstOrDefault();

			Assert.AreEqual(request.GameId, modifier.GameId);
			Assert.IsNotNull(modifier.Name);
			Assert.AreEqual(request.Name, modifier.Name);
			Assert.AreEqual(request.Description, modifier.Description);
			Assert.AreEqual(request.ImgFileId, modifier.ImgFileId);

			var characterTemplateModifier = _dbContext.CharacterTemplateModifiers
				.FirstOrDefault(x => x.ModifierId == modifier.Id);
			Assert.IsNotNull(characterTemplateModifier);
			Assert.IsTrue(characterTemplateModifier.CharacterTemplateId == _characterTemplate.Id);

			var itemTemplateModifier = _dbContext.ItemTemplateModifiers
				.FirstOrDefault(x => x.ModifierId == modifier.Id);
			Assert.IsNotNull(itemTemplateModifier);
			Assert.IsTrue(itemTemplateModifier.ItemTemplateId == _itemTemplate.Id);

			var modifierScript = modifier.ModifierScripts.
				FirstOrDefault();
			Assert.IsNotNull(modifierScript);
			Assert.IsTrue(modifierScript.ScriptId == _script.Id);
			Assert.IsTrue(modifierScript.EventId == _event.Id);

			var activeCycles = modifierScript.ActiveCycles;
			Assert.IsNotNull(activeCycles);
			Assert.AreEqual(2, activeCycles.Count);
			var activeCycle = activeCycles.Last();
			Assert.AreEqual(DateTimeProvider.Object.TimeProvider.AddMinutes(2), activeCycle.ActivationBeginning);
			Assert.AreEqual(DateTimeProvider.Object.TimeProvider.AddMinutes(3), activeCycle.ActivationEnd);
		}
	}
}
