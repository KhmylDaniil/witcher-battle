namespace Witcher.MVC.ViewModels
{
	public class ErrorViewModel
	{
		public string Message { get; set; }

		public ErrorViewModel(Exception ex)
		{
			Message	= ex.Message;
		}
	}
}
