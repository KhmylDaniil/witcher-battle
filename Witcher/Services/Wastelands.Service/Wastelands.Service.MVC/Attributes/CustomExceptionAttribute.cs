using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Wastelands.Service.MVC.Attributes
{
	public class ExceptionAttribute : Attribute, IExceptionFilter
	{
		public ExceptionAttribute(string path = null)
		{
			Path = path;
		}

		public string Path { get; private set; }

		public void OnException(ExceptionContext filterContext)
		{
			Exception ex = filterContext.Exception;
			filterContext.ExceptionHandled = true;

			var action = filterContext.RouteData.Values["action"].ToString();
			var id = filterContext.RouteData.Values["id"]?.ToString();

			var path = string.Join('/', action, id);
			var model = 

			var result = new ViewResult
			{
				ViewName = Path ?? action,
				ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), filterContext.ModelState),
			};

			result.ViewData["ErrorMessage"] = ex.Message;
			filterContext.Result = result;
		}
	}
}
