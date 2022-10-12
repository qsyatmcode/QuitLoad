using VideoLibrary;

namespace ApiShells
{
	public class YTapi
	{
		private YouTube _client = new YouTube();

		public VideoInfo GetVideoInfo(string url)
		{
			if (url == null || url == "" || url == String.Empty) throw new ArgumentNullException("Nullable url");

			YouTubeVideo video = _client.GetVideo(url);

			return video.Info;
		}
	}
}