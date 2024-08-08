using Microsoft.Extensions.Logging;

namespace MauiApp1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			// Add this section anywhere on the builder:
			.UseSentry(options =>
			{
				// The DSN is the only required setting.
				options.Dsn = "https://1db5e4fd3844285b38fd9c27f2d57dbf@o88872.ingest.us.sentry.io/4507737910673408";

				// By default, we will send the last 100 breadcrumbs with each event.
                // If you want to see everything we can capture from MAUI, you may wish to use a larger value.
                options.MaxBreadcrumbs = 1000;

				// Use debug mode if you want to see what the SDK is doing.
				// Debug messages are written to stdout with Console.Writeline,
				// and are viewable in your IDE's debug console or with 'adb logcat', etc.
				// This option is not recommended when deploying your application.
				options.Debug = true;

				// Set TracesSampleRate to 1.0 to capture 100% of transactions for tracing.
				// We recommend adjusting this value in production.
				options.TracesSampleRate = 1.0;

				// Be aware that screenshots may contain PII
                options.AttachScreenshot = true;

				// Other Sentry options can be set here.
				options.Release = "1.0";
			})

			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
