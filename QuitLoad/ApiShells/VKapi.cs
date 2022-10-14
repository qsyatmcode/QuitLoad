using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace ApiShells
{
	public class VKapi
	{
		private static VkApi api = new VkApi();
		public static VkNet.Model.Attachments.Video Save;

		public VKapi(string accesToken)
		{
			if (accesToken != String.Empty)
			{
				api.Authorize(new ApiAuthParams
				{
					AccessToken = accesToken
				});
			}
		}

		public User GetInfo()
		{
			return api.Users.Get(Array.Empty<long>()).First();
		}

		public void CreateServer()
		{
			Save = api.Video.Save(new VkNet.Model.RequestParams.VideoSaveParams
			{
				""
			});
		}

		public void Ping()
		{
			if (api.UserId != null)
			{
				api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
				{
					ChatId = api.UserId.Value,
					RandomId = 0,
					UserId = api.UserId,
					Message = "Pong"
				});
			}
			else
			{
				throw new NullReferenceException("api.UserId is null! (Ping method loc)");
			}
		}
	}
}