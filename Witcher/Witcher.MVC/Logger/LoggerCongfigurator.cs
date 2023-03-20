using Serilog.Events;
using Serilog;

namespace Witcher.MVC.Logger
{
	public static class LoggerCongfigurator
	{
		public static void ConfigureSerilog(this WebApplicationBuilder builder, LogLevel minimumLogLevel)
		{
			var outputTemplate =
				$"{{Timestamp:HH:mm}} [l:{{Level}}] (th:{{ThreadId}}) " +
				$"Message: {{Message}}{{NewLine}}Esception: {{Exception}}";

			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(outputTemplate: outputTemplate)
				.MinimumLevel.Is((LogEventLevel)minimumLogLevel)
				.CreateLogger();

			builder.Host.UseSerilog();
		}
	}
}
