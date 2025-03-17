using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Wastelands.Service.MVC.Attributes
{
	public class ExceptionAttribute : Attribute, IExceptionFilter
	{
		private readonly string? _path;

		public ExceptionAttribute(string? path = null)
		{
			_path = path;
		}

		public void OnException(ExceptionContext filterContext)
		{
			Exception ex = filterContext.Exception;
			filterContext.ExceptionHandled = true;

			var result = new ViewResult
			{
				ViewName = _path ?? "Error",
				ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), filterContext.ModelState)
			};

			result.ViewData["ErrorMessage"] = ex.Message;
			filterContext.Result = result;
		}
	}
}
