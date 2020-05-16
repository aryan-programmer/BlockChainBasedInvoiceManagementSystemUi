using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using BlockChainBasedInvoiceManagementSystemUi.Properties;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void ExitCmd_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			Close();
			Environment.Exit(0);
		}

		private void OptionsCmd_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			var optionsWindow = new OptionsWindow();
			optionsWindow.ShowDialog();
		}

		private void GenerateKeysCmd_OnClick(object sender, ExecutedRoutedEventArgs e) {
			var optionsWindow = new GenerateKeysWindow();
			optionsWindow.ShowDialog();
		}

		private void GenerateKeysCmd_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) => 
			e.CanExecute = File.Exists(Settings.Default.CommandLineApiFile);
	}
}