using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Witcher.MVC.ViewModels.BodyTemplate;

namespace Witcher.MVC.Controllers
{
	public class BodyTemplateController : Controller
	{
		private readonly IMediator _mediator;

		public BodyTemplateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Route("[controller]/{gameId}")]
		public async Task<IActionResult> Index(Guid gameId, string name, string authorName, string bodyPartName, CancellationToken cancellationToken)
		{
			TempData["GameId"] = gameId;

			var request = new GetBodyTemplateQuery() { GameId = gameId, Name = name, UserName = authorName, BodyPartName = bodyPartName};

			var response = await _mediator.Send(request, cancellationToken);

			return View(response.BodyTemplatesList);
		}

		[Route("[controller]/{gameId}/{id}")]
		public async Task<IActionResult> Details(GetBodyTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(query, cancellationToken);

			return View(response);
		}

		[Route("[controller]/[action]")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public ActionResult Create(CreateBodyTemplatePartForm form)
		{
			try
			{
				if (form.IsUsed)
				{
					var completedForm = new List<CreateBodyTemplatePartForm> { form };

					if (TempData.TryGetValue("form", out object getForm) && getForm is List<CreateBodyTemplatePartForm> list && list.Count > 0)
						completedForm.AddRange(list);
					
					TempData["form"] = completedForm;

					return View();
				}

				return RedirectToAction(nameof(Create2));
			}
			catch
			{
				return View();
			}
		}

		[Route("[controller]/[action]")]
		public ActionResult Create2()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create2(CreateBodyTemplateRequest request, CancellationToken cancellationToken)
		{
			try
			{
				request.BodyTemplateParts = (List<CreateBodyTemplateRequestItem>)TempData["form"];
				request.GameId = (Guid)TempData["GameId"];

				await _mediator.Send(request, cancellationToken);

				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: BodyTemplateController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: BodyTemplateController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: BodyTemplateController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: BodyTemplateController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
