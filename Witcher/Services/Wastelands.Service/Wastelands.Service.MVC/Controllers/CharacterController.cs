using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Models.Filters;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Controllers
{
	public class CharacterController : BaseController
	{
		private readonly ICharacterService _characterService;

		public CharacterController(ICharacterService characterService)
		{
			_characterService = characterService;
		}

		public async Task<IActionResult> Index(CharacterFilter filter, CancellationToken cancellationToken)
		{
			var result = await _characterService.GetCharactersAsync(filter);

			return View(result);
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
		{
			var result = await _characterService.GetCharacterByIdAsync(id);
			return View(result);
		}

		[HttpGet]
		[Route("[controller]/[action]")]
		public ActionResult Create(CreateCharacterRequest request)
		{
			return View(request);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateCharacterRequest request, CancellationToken cancellationToken)
		{
			var result = await _characterService.CreateCharacterAsync(request);
			return RedirectToAction(nameof(Details), routeValues: new { id = result.Id });
		}

		[HttpGet]
		[Route("[controller]/[action]/{id}")]
		public ActionResult Update(UpdateCharacterRequest request)
		{
			return View(request);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Update(UpdateCharacterRequest request, CancellationToken cancellationToken)
		{
			var result = await _characterService.UpdateCharacterAsync(request);
			return RedirectToAction(nameof(Details), routeValues: new { id = result.Id });
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(BaseDeleteRequest request) => View(request);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(BaseDeleteRequest request, CancellationToken cancellationToken)
		{
			await _characterService.DeleteCharacterAsync(request.Id);
			return RedirectToAction(nameof(Index), new CharacterFilter());
		}
	}
}
