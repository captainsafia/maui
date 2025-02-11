using System;
using System.Threading.Tasks;
using Tizen.Applications;

namespace Microsoft.Maui.Essentials.Implementations
{
	public class ShareImplementation : IShare
	{
		public Task RequestAsync(ShareTextRequest request)
		{
			Permissions.EnsureDeclared<Permissions.LaunchApp>();

			var appControl = new AppControl
			{
				Operation = AppControlOperations.ShareText,
			};

			if (!string.IsNullOrEmpty(request.Text))
				appControl.ExtraData.Add(AppControlData.Text, request.Text);
			if (!string.IsNullOrEmpty(request.Uri))
				appControl.ExtraData.Add(AppControlData.Url, request.Uri);
			if (!string.IsNullOrEmpty(request.Subject))
				appControl.ExtraData.Add(AppControlData.Subject, request.Subject);
			if (!string.IsNullOrEmpty(request.Title))
				appControl.ExtraData.Add(AppControlData.Title, request.Title);

			AppControl.SendLaunchRequest(appControl);

			return Task.CompletedTask;
		}

		public Task RequestAsync(ShareMultipleFilesRequest request)
		{
			Permissions.EnsureDeclared<Permissions.LaunchApp>();

			var appControl = new AppControl
			{
				Operation = AppControlOperations.Share,
			};

			if (!string.IsNullOrEmpty(request.Title))
				appControl.ExtraData.Add(AppControlData.Title, request.Title);

			foreach (var file in request.Files)
				appControl.ExtraData.Add(AppControlData.Path, file.FullPath);

			AppControl.SendLaunchRequest(appControl);

			return Task.CompletedTask;
		}
	}
}
