using ApiShells;
using YoutubeExplode;
using Microsoft.Maui.Storage;
using YoutubeExplode.Videos;
using VkNet;

namespace QuitLoad;

public partial class MainPage : ContentPage
{
	private VKapi _vkApi; // Нужна инициализация
	private static YoutubeClient s_youtube;

	public static string savePath { get; private set; } = FileSystem.AppDataDirectory; // ВОТ ТУТ ВАЖНО

	public MainPage()
	{
		InitializeComponent();
		s_youtube = new YoutubeClient();
		//_vkApi = new VKapi("");

		savePathLabel.Text = savePath;
		videoFrame.IsVisible = false;
	}

	private void OnEditorCompleted()
	{
		
	}

	private async void OnSearchClicked(object sender, EventArgs e) // Нужно написать определение сервиса введеной ссылки
	{
		if (searchEditor.Text == null || searchEditor.Text == "")
		{
			await DisplayAlert("Ошибка", "Поле URL пусто", "OK");
			return;
		}
		try
		{
			var video = await s_youtube.Videos.GetAsync(searchEditor.Text);

			videoTitle.Text = video.Title;
			videoInfo.Text = video.Duration.Value.Minutes + " min " + video.Duration.Value.Seconds + " sec" + " - " + video.Author + " - " + video.UploadDate.DateTime.ToString().Split(' ')[0];
			videoDescription.Text = video.Description;
			var thumbnail = video.Thumbnails[0];
			videoImage.Source = thumbnail.Url;

			videoFrame.IsVisible = true;
		}
		catch
		{
			await DisplayAlert("Ошибка", "Не удалось получить информацию о видео", "OK");
		}
	}

	private void OnEditorCompleted(object sender, EventArgs e)
	{

	}
}

