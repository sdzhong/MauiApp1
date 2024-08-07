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

		if (count > 3) 
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

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

