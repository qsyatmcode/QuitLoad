using System.Security.Cryptography;
using VKdnldapi;
using VkNet;

namespace QuitLoad;

public partial class MainPage : ContentPage
{
	private VKapiShell vkApi;

	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		//count++;

		//if (count == 1)
		//	CounterBtn.Text = $"Clicked {count} time";
		//else
		//	CounterBtn.Text = $"Clicked {count} times";

		//SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private void OnEditorCompleted(object sender, EventArgs e)
	{

	}
}

