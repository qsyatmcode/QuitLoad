using VideoLibrary;

namespace ApiShells
{
	public class YTapi
	{
		private static YouTube _client = new YouTube();

		static public VideoInfo GetVideoInfo(string url)
		{
			YouTubeVideo video = _client.GetVideo(url);

			return video.Info;
		}
	}
}