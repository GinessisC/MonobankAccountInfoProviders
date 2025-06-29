using ConsoleApp.App.Interfaces;

namespace ConsoleApp.App;

public class ConsoleExceptionPresenter : IExceptionPresenter
{
	public void Present(Exception e)
	{
		Console.Error.WriteLine($"""
								Error occured: {e.Message}
								Stack trace: {e.StackTrace}
								Stack source: {e.Source}
								""");
	}
}