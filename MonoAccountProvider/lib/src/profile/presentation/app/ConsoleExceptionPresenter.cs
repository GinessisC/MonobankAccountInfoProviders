using MonoAccountProvider.lib.src.profile.presentation.app.Interfaces;

namespace MonoAccountProvider.lib.src.profile.presentation.app;

public class ConsoleExceptionPresenter : IExceptionPresenter
{
	public void Present(Exception e)
	{
		Console.Error.WriteLine($"Error occured: {e.Message}");
	}
}