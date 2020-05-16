using System.Windows;
using System.Windows.Input;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void ExitCmd_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
		}

		private void OptionsCmd_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			var optionsWindow = new OptionsWindow();
			optionsWindow.ShowDialog();
		}
	}
}