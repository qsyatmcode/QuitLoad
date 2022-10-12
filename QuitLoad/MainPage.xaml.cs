using System.Security.Cryptography;
using ApiShells;
using VkNet;

namespace QuitLoad;

public partial class MainPage : ContentPage
{
	private VKapi _vkApi;
	private YTapi _ytapi;

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

