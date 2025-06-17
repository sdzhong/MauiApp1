using Microsoft.Extensions.Logging;
using Sentry;

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
				// A Sentry Data Source Name (DSN) is required.
				// See https://docs.sentry.io/product/sentry-basics/dsn-explainer/
				// You can set it in the SENTRY_DSN environment variable, or you can set it in code here.
				options.Dsn = "https://f6da04e8f919d0653336aca12e04eecd@o88872.ingest.us.sentry.io/4507737910673408";
				options.AttachScreenshot = true;
				// When debug is enabled, the Sentry client will emit detailed debugging information to the console.
				// This might be helpful, or might interfere with the normal operation of your application.
				// We enable it here for demonstration purposes when first trying Sentry.
				// You shouldn't do this in your applications unless you're troubleshooting issues with Sentry.
				options.Debug = true;

				// This option is recommended. It enables Sentry's "Release Health" feature.
				options.AutoSessionTracking = true;

				// Enabling this option is recommended for client applications only. It ensures all threads use the same global scope.
				options.IsGlobalModeEnabled = false;

				// Example sample rate for your transactions: captures 100% of transactions
				options.TracesSampleRate = 1.0;

				options.Release = "2.0";
				options.SetBeforeSend((sentryEvent, hint) =>
				{
					if (sentryEvent.Tags.ContainsKey("mytag")) {
						string mytagValue = sentryEvent.Tags["mytag"];
						if (mytagValue == "abc")
						{
							sentryEvent.SetTag("mytag", "abc2");
						}
					}
					return sentryEvent;
				});


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
