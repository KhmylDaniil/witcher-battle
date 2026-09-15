using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Web;
using Wastelands.Service.Domain.Models.Requests;

namespace Wastelands.Service.MVC.Filters
{
	public class ExceptionFilter : IAsyncExceptionFilter
	{
		public async Task OnExceptionAsync(ExceptionContext context)
		{
			var modelType = context.ActionDescriptor.Parameters.Select(x => x.ParameterType).FirstOrDefault(x => x.IsAssignableTo(typeof(BaseRequest)));

			var model = await GetRequestModel(context.HttpContext, modelType) ?? Activator.CreateInstance(modelType);

			var provider = context.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();
			var modelState = context.ModelState;

			var viewData = new ViewDataDictionary(provider, modelState);
			viewData.Model = model;

			var result = new ViewResult
			{
				ViewName = context.RouteData.Values["action"]?.ToString() ?? "Error",
				ViewData = new ViewDataDictionary(provider, modelState),
			};
			result.ViewData = viewData;

			result.ViewData["ErrorMessage"] = context.Exception.Message;
			context.Result = result;

			context.ExceptionHandled = true;
		}

		private async Task<object> GetRequestModel(HttpContext context, Type type)
		{
			var request = context.Request.HttpContext.Request;

			request.Body.Position = 0;
			var bodyAsText = new StreamReader(request.Body).ReadToEndAsync().Result;

			var dict = HttpUtility.ParseQueryString(bodyAsText);
			string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
			var respObj = JsonConvert.DeserializeObject(json, type);

			return respObj;
		}
	}
}

