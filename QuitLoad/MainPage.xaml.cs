using System.Reflection.Metadata;
using System.Security.Cryptography;
using ApiShells;
using VkNet;

namespace QuitLoad;

public partial class MainPage : ContentPage
{
	private VKapi _vkApi; // Нужна инициализация
	private YTapi _ytapi;

	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnEditorCompleted()
	{
		videoTitle.Text = searchEditor.Text;
	}

	private void OnSearchClicked(object sender, EventArgs e)
	{
		videoTitle.Text = searchEditor.Text;
	}

	private void OnEditorCompleted(object sender, EventArgs e)
	{

	}
}

