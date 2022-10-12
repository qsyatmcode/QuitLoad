using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace ApiShells
{
	public class VKapi
	{
		private static VkApi api = new VkApi();

		public VKapi(string accesToken)
		{
			api.Authorize(new ApiAuthParams
			{
				AccessToken = accesToken
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