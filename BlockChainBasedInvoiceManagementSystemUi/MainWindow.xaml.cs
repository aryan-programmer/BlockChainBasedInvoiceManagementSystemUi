using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using BlockChainBasedInvoiceManagementSystemUi.Properties;
using static BlockChainBasedInvoiceManagementSystemUi.Utils;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			if (ServerProcess.IsStarted) {
				ShowWhenServerOff.Visibility          = Visibility.Collapsed;
				ShowWhenServerOn.Visibility = Visibility.Visible;
			} else {
				ShowWhenServerOff.Visibility          = Visibility.Visible;
				ShowWhenServerOn.Visibility = Visibility.Collapsed;
			}
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

		private void StartCommandLineApiBtn_OnClick(object sender, RoutedEventArgs e) {
			ServerProcess.Exit += OnProcessExit;
			if (!ServerProcess.StartProcess_ShowErrors()) {
				ServerProcess.Exit -= OnProcessExit;
				return;
			}

			ShowWhenServerOff.Visibility          = Visibility.Collapsed;
			ShowWhenServerOn.Visibility = Visibility.Visible;
		}

		private void OnProcessExit(Process process) {
			Application.Current.Dispatcher.Invoke(() => {
				ShowWhenServerOff.Visibility          = Visibility.Visible;
				ShowWhenServerOn.Visibility = Visibility.Collapsed;
			});
			if (process.ExitCode != 0) {
				var err = process.StandardError.ReadToEnd();
				if (err != "") ShowErrorMBox($"There was an error:\n{err}");
			}

			ServerProcess.Exit -= OnProcessExit;
		}

		private void StopCommandLineApiServerBtn_OnClick(object sender, RoutedEventArgs e) {
			if (ShowConfirmationMBox("Are you sure that you want to stop the server?")) {
				ServerProcess.StopProcess();
			}
		}
	}
}