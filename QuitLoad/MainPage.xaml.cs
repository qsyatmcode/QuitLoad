using YoutubeExplode;
using Microsoft.Maui.Storage;
using VkNet;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;
using System.Security.Cryptography;

namespace QuitLoad;

public partial class MainPage : ContentPage
{
	private static YoutubeClient s_youtube;

	private static string s_savePath =  FileSystem.Current.AppDataDirectory;
	private YoutubeExplode.Videos.Video _selectedYTvideo;
	private double _downloadProgress = 0d;

	private Service _selected;
	
	enum Service
	{
		undefined = -1,
		vk,
		youtube
	}

	public MainPage()
	{
		InitializeComponent();
		s_youtube = new YoutubeClient();
		//_vkApi = new VKapi(""); // AccesToken надо хранить в каком-то файле чтобы при выходе сохранялся и входе загружался
		_selected = new Service();

		if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
		{
			s_savePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyVideos);
		}

		savePathLabel.Text = s_savePath;
		videoFrame.IsVisible = false;
		progressBar.IsVisible = false;
	}

	private async void OnSearchClicked(object sender, EventArgs e) // Нужно написать систему сервисов чтобы ифы не писать
	{
		HapticFeedback.Default.Perform(HapticFeedbackType.Click);

		ServiceDefinition();

		if (_selected != Service.undefined)
		{
			serviceLabel.Text = "_selected = " + _selected.ToString();

			if(_selected == Service.vk)
			{

			}else if (_selected == Service.youtube)
			{
				YTSearch();
			}
		}
		else
		{
			videoFrame.IsVisible = false;
			await DisplayAlert("Ошибка", "Не удалось определить сервис", "OK");
		}
	}

	private async void OnDownloadButtonClicked(object sender, EventArgs e)
	{
		HapticFeedback.Default.Perform(HapticFeedbackType.Click);
		await Download();
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
		videoFrame.IsVisible = false;
		try
		{
			var video = await s_youtube.Videos.GetAsync(searchEditor.Text);

			videoTitle.Text = video.Title;
			videoInfo.Text = video.Duration.Value.Minutes + " min " + video.Duration.Value.Seconds + " sec" + " - " + video.Author + " - " + video.UploadDate.DateTime.ToString().Split(' ')[0];
			videoDescription.Text = video.Description;
			var thumbnail = video.Thumbnails[0];
			videoImage.Source = thumbnail.Url;
			_selectedYTvideo = video;
			HapticFeedback.Default.Perform(HapticFeedbackType.Click);
		}
		catch
		{
			await DisplayAlert("Ошибка", "Не удалось получить информацию о видео", "OK");
			return;
		}
		videoFrame.IsVisible = true;
	}

	private async Task Download()
	{
		if (_selected == Service.youtube) {
			var	streamManifest = await s_youtube.Videos.Streams.GetManifestAsync(_selectedYTvideo.Id);

			IStreamInfo streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
			//var stream = await s_youtube.Videos.Streams.GetAsync(streamInfo);

			bool result = await DisplayAlert("Download", $"Размер файла составит {streamInfo.Size} Mb ({streamManifest.GetMuxedStreams().GetWithHighestVideoQuality().VideoQuality.ToString()}), вы хотите продолжить?" + System.Environment.NewLine + "По пути: " + s_savePath + "\\" + GenerateSavename() + ".mp4", "Да", "Нет");
			if (result)
			{
				progressBar.IsVisible = true;
				Progress<double> progress = new Progress<double>(p => _downloadProgress = p);
				progress.ProgressChanged += Progress_ProgressChanged;
				
				await s_youtube.Videos.Streams.DownloadAsync(streamInfo, s_savePath + $"\\{GenerateSavename()}" + "." + streamInfo.Container, progress);

				progressBar.IsVisible = false;
				_downloadProgress = 0d;
				await DisplayAlert("Download", "Скачивание завершено!", "OK");
			}
			else
			{
				return;
			}
		}else if (_selected == Service.vk) // TODO
		{

		}
	}

	private void Progress_ProgressChanged(object sender, double e)
	{
		progressBar.ProgressTo(e - progressBar.Progress, 500, Easing.Linear);
	}

	private static string GenerateSavename()
	{
		string output = String.Empty;
		int seed = 0;
		using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
		{
			var buffer = new byte[4];
			rng.GetBytes(buffer);
			seed = BitConverter.ToInt32(buffer, 0);
		}
		Random rnd = new Random(seed);
		for (int i = 0; i <= 12; i++)
		{
			output += Convert.ToString(rnd.Next());
		}
		return output.Substring(output.Length - 12);
	}
}

