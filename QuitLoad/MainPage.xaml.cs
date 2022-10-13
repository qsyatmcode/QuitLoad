using ApiShells;
using YoutubeExplode;
using Microsoft.Maui.Storage;
using YoutubeExplode.Videos;
using VkNet;

namespace QuitLoad;

public partial class MainPage : ContentPage
{
	//private VKapi _vkApi; // Нужна инициализация
	private static YoutubeClient s_youtube;

	public static string savePath { get; private set; } = FileSystem.AppDataDirectory; // ВОТ ТУТ ВАЖНО

	private Service _selected;
	
	enum Service
	{
		undefined = -1,
		vk,
		youtube,
		pornhub
	}

	public MainPage()
	{
		InitializeComponent();
		s_youtube = new YoutubeClient();
		//_vkApi = new VKapi(""); // AccesToken надо хранить в каком-то файле чтобы при выходе сохранялся и входе загружался
		_selected = new Service();

		savePathLabel.Text = savePath;
		videoFrame.IsVisible = false;
	}

	private void OnEditorCompleted()
	{
		
	}

	private async void OnSearchClicked(object sender, EventArgs e) // Нужно написать определение сервиса введеной ссылки
	{
		ServiceDefinition();

		if (_selected != Service.undefined)
		{
			serviceLabel.Text = "_selected = " + _selected.ToString();
			videoFrame.IsVisible = true;
		}
		else
		{
			videoFrame.IsVisible = false;
			await DisplayAlert("Ошибка", "Не удалось определить сервис", "OK");
		}
	}

	private void OnEditorCompleted(object sender, EventArgs e)
	{

	}

	private async void ServiceDefinition()
	{
		if (searchEditor.Text == null || searchEditor.Text == "")
		{
			await DisplayAlert("Ошибка", "Поле URL пусто", "OK");
			return;
		}
		
		for (int i = 0; i < Enum.GetNames(typeof(Service)).Length; i++)
		{
			if (searchEditor.Text.Contains(Enum.GetNames(typeof(Service))[i]))
			{
				_selected = (Service)i;
				return;
			}
		}
		_selected = Service.undefined;
		
	}

	private async void YTSearch()
	{
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
}

