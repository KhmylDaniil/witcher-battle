using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;

namespace Witcher.MVC.ViewModels.BodyTemplate
{
	public class CreateBodyTemplatePartForm : CreateBodyTemplateRequestItem
	{
		public bool IsUsed { get; set; }
	}
}
