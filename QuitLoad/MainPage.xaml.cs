using ApiShells;
using YoutubeExplode;
using Microsoft.Maui.Storage;
using VkNet;
namespace QuitLoad;

public partial class MainPage : ContentPage
{
	private VKapi _vkApi;
	private static YoutubeClient s_youtube;

	public static string savePath { get; private set; } // ВОТ ТУТ ВАЖНО
	public static string dataPath { get; private set; } = FileSystem.AppDataDirectory;
	private string _accessToken = string.Empty;

	private bool _isLogged = false;

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
		if (!_isLogged)
		{
			avatarImage.IsVisible = false;
			accountName.IsVisible = false;
			logginButton.Text = "Log-in";
		}
		else
		{
			avatarImage.IsVisible = true;
			accountName.IsVisible = true;
			logginButton.Text = "Log-out";
		}

		savePathLabel.Text = savePath;
		videoFrame.IsVisible = false;
	}

	private async void ReadLastLogin()
	{
		if (File.Exists(dataPath + "at.txt"))
		{
			try
			{
				using (var streamReader = new StreamReader(dataPath + "at.txt"))
				{
					_accessToken = streamReader.ReadToEnd();
				}
				login();
			}
			catch
			{
				await DisplayAlert("Ошибка", "Не удалось войти в аккаунт", "OK");
			}
		}
	}
	private async void WriteLogin()
	{
		try
		{
			using (var streamWriter = new StreamWriter(dataPath + "at.txt", false)) //отказано в доступе
			{
				await streamWriter.WriteLineAsync(_accessToken);
			}
		}
		catch
		{
			await DisplayAlert("Ошибка", "Не удалось сохранить AccessToken", "OK");
		}
	}
	private async void login()
	{
		if(_accessToken == String.Empty)
		{
			await DisplayAlert("Вход", "Не удалось выполнить вход. Возможно вы ввели неверный AccessToken", "OK");
			return;
		}
		_vkApi = new VKapi(_accessToken);
		_isLogged = true;
		var info = _vkApi.GetInfo();

		avatarImage.Source = info.Photo100;
		accountName.Text = info.FirstName + '-' + info.LastName;

	}
	private async void OnLoginButtonClick(object sender, EventArgs e)
	{
		if (!_isLogged) //log-in
		{
			bool result = await DisplayAlert("Вход", "Вам необходимо получить свой AccessToken, вы хотите открыть открыть страницу для его получения?", "Да", "Нет, у меня он уже есть");
			if (result)
			{
				try
				{
					Uri uri = new Uri("https://vkhost.github.io/");
					await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
				}
				catch
				{
					await DisplayAlert("Ошибка", "Не удалось открыть браузер", "OK");
				}
			}
			else
			{
				_accessToken = await DisplayPromptAsync("Вход", "Введите свой AccessToken");
				login();
				WriteLogin();

				avatarImage.IsVisible = true;
				logginButton.Text = "Log-out";
				accountName.IsVisible = true;

				return;
			}
		}
		else //log-out
		{
			bool result = await DisplayAlert("Выход", "Вы действительно хотите выйти?", "Да", "Нет");
			if (result)
			{
				_isLogged = false;

				avatarImage.IsVisible = false;
				logginButton.Text = "Log-in";
				accountName.IsVisible = false;
			}
		}
	}

	private async void OnSearchClicked(object sender, EventArgs e) // Нужно написать систему сервисов чтобы ифы не писать
	{
		ServiceDefinition();

		if (_selected != Service.undefined)
		{
			serviceLabel.Text = "_selected = " + _selected.ToString();

			if(_selected == Service.vk)
			{
				if (!_isLogged)
				{
					await DisplayAlert("Ошибка", "Для того чтобы скачивать видео с сервиса ВКонтакте, нужно войти в аккаунт", "OK");
				}
				else
				{
					//действия TODO TODO TODO TODO TODO TODO TODO TODO TODO
					videoFrame.IsVisible = true;
				}

			}else if (_selected == Service.youtube)
			{

			}
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

	private async void VKlogin()
	{
		try
		{

		}
		catch
		{
			await DisplayAlert("Ошибка", "Не удалось войти в аккаунт", "OK");
		}
	}
}

