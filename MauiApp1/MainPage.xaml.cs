namespace MauiApp1;
using Sentry;

public partial class MainPage : ContentPage
{
	int count = 0;

    // NOTE: You can only inject an ILogger<T>, not a plain ILogger
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1) {
			CounterBtn.Text = $"Clicked {count} time";
			SentrySdk.ConfigureScope(scope =>
			{
				scope.User = new SentryUser
				{
					Username = "jdoe"
				};
				SentrySdk.CaptureMessage("Hello Sentry with user username");
			});
		} else {
			CounterBtn.Text = $"Clicked {count} times";
		}

		if (count == 3) 
		{
			try
			{
				throw null;
			}
			catch (Exception ex)
			{
				SentrySdk.CaptureException(ex);
			}

			SentrySdk.ConfigureScope(scope =>
			{
				scope.User = new SentryUser
				{
					Id = "12345"
				};
				SentrySdk.CaptureMessage("Hello Sentry with user id");
			});
		}

		if (count == 4) 
		{
			getProducts();

			SentrySdk.ConfigureScope(scope =>
			{
				scope.User = new SentryUser
				{
					Email = "john.doe@example.com"
				};
				SentrySdk.CaptureMessage("Hello Sentry with user email");
			});
		}

		SemanticScreenReader.Announce(CounterBtn.Text);		

		// SentrySdk.CaptureMessage("Hello Sentry");

	}

	private void OnUnhandledExceptionClicked(object sender, EventArgs e)
    {
#pragma warning disable CS0618
        SentrySdk.CauseCrash(CrashType.Managed);
#pragma warning restore CS0618
    }

    private void OnBackgroundThreadUnhandledExceptionClicked(object sender, EventArgs e)
    {
#pragma warning disable CS0618
        SentrySdk.CauseCrash(CrashType.ManagedBackgroundThread);
#pragma warning restore CS0618
    }

    private void OnCapturedExceptionClicked(object sender, EventArgs e)
    {
        try
        {
            throw new ApplicationException("This exception was thrown and captured manually, without crashing the app.");
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
        }
    }
    private void OnJavaCrashClicked(object sender, EventArgs e)
    {
#if ANDROID
#pragma warning disable CS0618
        SentrySdk.CauseCrash(CrashType.Java);
#pragma warning restore CS0618
#endif
    }

    private void OnNativeCrashClicked(object sender, EventArgs e)
    {
#if __MOBILE__
#pragma warning disable CS0618
        SentrySdk.CauseCrash(CrashType.Native);
#pragma warning restore CS0618
#endif
    }

    private class FlakyMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => throw new Exception();
    }

	static async void getProducts()
	{
		var transaction = SentrySdk.StartTransaction("tutorial", "ui.load");
		SentrySdk.ConfigureScope(scope => scope.Transaction = transaction);
		var httpHandler = new SentryHttpMessageHandler();
		var httpClient = new HttpClient(httpHandler);
		var response = await httpClient.GetStringAsync("https://application-monitoring-flask-dot-sales-engineering-sf.appspot.com/products");
		transaction.Finish();
	}

}

