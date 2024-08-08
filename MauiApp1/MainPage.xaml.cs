namespace MauiApp1;
using Sentry;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

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
		}

		if (count == 4) 
		{
			getProducts();
		}

		SemanticScreenReader.Announce(CounterBtn.Text);
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

