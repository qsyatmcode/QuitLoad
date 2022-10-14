using System.Reflection;

namespace QuitLoad;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		shellContent.Title = "QuitLoad v" + Assembly.GetEntryAssembly().GetName().Version.Major.ToString() + '.' + Assembly.GetExecutingAssembly().GetName().Version.Minor;
	}
}
