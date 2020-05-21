using System.Windows;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private void App_OnExit(object sender, ExitEventArgs e) => ServerProcess.I.StopProcess();
	}
}