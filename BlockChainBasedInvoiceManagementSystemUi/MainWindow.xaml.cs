using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BlockChainBasedInvoiceManagementSystemUi.Properties;
using RestSharp;
using RestSharp.Serialization.Json;
using static BlockChainBasedInvoiceManagementSystemUi.Utils;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private string publicKey;

		public MainWindow() {
			InitializeComponent();
			var publicKeyFile = Settings.Default.PublicKeyFile;
			if (File.Exists(publicKeyFile))
				publicKey = File.ReadAllText(publicKeyFile);
		}

		private void ExitCmd_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			Close();
			Environment.Exit(0);
		}

		private void OptionsCmd_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
			var prevPublicKey = Settings.Default.PublicKeyFile;
			var optionsWindow = new OptionsWindow();
			optionsWindow.ShowDialog();
			var newPublicKey                             = Settings.Default.PublicKeyFile;
			if (prevPublicKey != newPublicKey) publicKey = File.ReadAllText(newPublicKey);
		}

		private void GenerateKeysCmd_OnClick(object sender, ExecutedRoutedEventArgs e) {
			var genKeysWindow = new GenerateKeysWindow();
			genKeysWindow.ShowDialog();
			var publicKeyFile = Settings.Default.PublicKeyFile;
			if(File.Exists(publicKeyFile)) publicKey = File.ReadAllText(publicKeyFile);
		}

		private void GenerateKeysCmd_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
			e.CanExecute = File.Exists(Settings.Default.CommandLineApiFile);

		private void StartCommandLineApiBtn_OnClick(object sender, RoutedEventArgs e) {
			ServerProcess.I.Exit += OnProcessExit;
			if (ServerProcess.I.StartProcess_ShowErrors()) return;
			ServerProcess.I.Exit -= OnProcessExit;
		}

		private static void OnProcessExit(int errorCode, StreamReader stdout, StreamReader stderr) {
			if (errorCode != 0) {
				var err = stderr.ReadToEnd();
				if (err != "") ShowErrorMBox($"There was an error:\n{err}");
			}

			ServerProcess.I.Exit -= OnProcessExit;
		}

		private void StopCommandLineApiServerBtn_OnClick(object sender, RoutedEventArgs e) {
			if (ShowConfirmationMBox("Are you sure that you want to stop the server?"))
				ServerProcess.I.StopProcess();
		}

		private void AddNewInvoiceMenu_OnClick(object sender, RoutedEventArgs e) {
			try {
				var window = new NewInvoiceWindow();
				if (window.ShowDialog() != true)
					return;
				InpInv inpInv = window.GetInpInv();
				var    invStr = new JsonSerializer().Serialize(new AddInvoice_SendData {data = inpInv});
				var client = new RestClient($"http://localhost:{Settings.Default.ApiPort}/addInvoice") {
					Timeout = -1
				};
				var request = new RestRequest(Method.POST);
				request.AddHeader("Content-Type", "application/json");
				request.AddParameter("application/json", invStr, ParameterType.RequestBody);
				IRestResponse response = client.Execute(request);
				if (!string.IsNullOrEmpty(response.ErrorMessage))
					ShowErrorMBox(@"There was an error in adding the new invoice.");
			} catch (Exception) {
				ShowErrorMBox(@"There was an error in adding the new invoice.");
			}
		}

		private void MyInvoicesMenu_OnClick(object sender, RoutedEventArgs e) {
			try {
				RestClient client =
					new RestClient(
								   $"http://localhost:{Settings.Default.ApiPort}/blocks"
								  ) {
						Timeout = -1
					}.UseJson();
				var           request  = new RestRequest(Method.GET);
				IRestResponse response = client.Execute(request);
				if (!string.IsNullOrEmpty(response.ErrorMessage)) {
					ShowErrorMBox(@"There was an error in getting the mined invoices.");
					return;
				}

				var blocks = new JsonSerializer().Deserialize<List<Block>>(response);
				List<UiInvoice> invoices =
					(from block in blocks
					 from invoice in block.data
					 where invoice.publicKey == publicKey
					 let inv = new UiInvoice(invoice)
					 orderby inv.timestamp descending
					 select inv)
				   .ToList();
				if (invoices.Count == 0) {
					ShowWarningMBox(@"You don't have any mined invoices.
Please add an invoice, mine it,
or wait for someone else to mine it.
Then open this dialog.");
				} else {
					var window = new ViewInvoicesWindow(invoices) {
						Title = "My mined invoices"
					};
					window.ShowDialog();
				}
			} catch (Exception) {
				ShowErrorMBox(@"There was an error in parsing/showing the mined invoices");
			}
		}

		private void MyPendingInvoices_OnClick(object sender, RoutedEventArgs e) {
			try {
				RestClient client =
					new RestClient(
								   $"http://localhost:{Settings.Default.ApiPort}/pendingInvoices"
								  ) {
						Timeout = -1
					}.UseJson();
				var           request  = new RestRequest(Method.GET);
				IRestResponse response = client.Execute(request);
				if (!string.IsNullOrEmpty(response.ErrorMessage)) {
					ShowErrorMBox(@"There was an error in getting the pending invoices.");
					return;
				}

				List<UiInvoice> invoices =
					(from invoice in new JsonSerializer().Deserialize<List<Invoice>>(response)
					 where invoice.publicKey == publicKey
					 let inv = new UiInvoice(invoice)
					 orderby inv.timestamp descending
					 select inv)
				   .ToList();
				if (invoices.Count == 0) {
					ShowWarningMBox(@"You don't have any pending invoices.
This may mean that all your invoices are already
been mined or that you didn't add any invoices at all.");
				} else {
					var window = new ViewInvoicesWindow(invoices) {
						Title = "My pending invoices"
					};
					window.ShowDialog();
				}
			} catch (Exception) {
				ShowErrorMBox(@"There was an error in parsing/showing the pending invoices");
			}
		}

		private void MineAllInPoolMenu_OnClick(object sender, RoutedEventArgs e) {
			try {
				var client = new RestClient($"http://localhost:{Settings.Default.ApiPort}/mine") {
					Timeout = -1
				};
				var           request  = new RestRequest(Method.POST);
				IRestResponse response = client.Execute(request);
				if (!string.IsNullOrEmpty(response.ErrorMessage))
					ShowErrorMBox(@"There was an error in mining the invoices.");
				ShowInfoMBox("Done!");
			} catch (Exception) {
				ShowErrorMBox(@"There was an error in mining the invoices.");
			}
		}
	}
}