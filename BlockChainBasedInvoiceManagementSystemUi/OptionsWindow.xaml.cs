using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BlockChainBasedInvoiceManagementSystemUi.Properties;
using Microsoft.Win32;
using static BlockChainBasedInvoiceManagementSystemUi.Utils;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsWindow : Window {
		public OptionsWindow() {
			InitializeComponent();
			CommandLineApiFile_TextBlock.Text = Settings.Default.CommandLineApiFile;
			ApiPortTextBox.Text               = Settings.Default.ApiPort.ToString();
			P2PPortTextBox.Text               = Settings.Default.P2PPort.ToString();
			PeersTextBox.Text                 = Settings.Default.Peers;
			PublicKeyFile_TextBlock.Text      = Settings.Default.PublicKeyFile;
			PrivateKeyFile_TextBlock.Text     = Settings.Default.PrivateKeyFile;
		}

		private void CommandLineApiFile_Browse_Btn_OnClick(object sender, RoutedEventArgs e) =>
			CommandLineApiFile_TextBlock.Text = ShowFileDialogBox<OpenFileDialog>(new[] {
				("Executable files", "*.exe"),
				("Batch/CMD files", "*.bat;*.cmd"),
				("All files", "*.*"),
			}, this);

		private void PublicKeyFile_Browse_Btn_OnClick(object sender, RoutedEventArgs e) =>
			PublicKeyFile_TextBlock.Text = ShowFileDialogBox<OpenFileDialog>(new[] {
				("PEM Encryption key file", "*.pem"),
				("All files", "*.*"),
			}, this);

		private void PrivateKeyFile_Browse_Btn_OnClick(object sender, RoutedEventArgs e) =>
			PrivateKeyFile_TextBlock.Text = ShowFileDialogBox<OpenFileDialog>(new[] {
				("PEM Encryption key file", "*.pem"),
				("All files", "*.*"),
			}, this);

		private void OkBtn_OnClick(object sender, RoutedEventArgs e) {
			var errors = new List<string>();
			if (!File.Exists(CommandLineApiFile_TextBlock.Text)) {
				errors.Add("The command line API file must refer to an executable, a batch file or command prompt file.");
			}

			if (Validation.GetHasError(ApiPortTextBox)) {
				errors.Add("Invalid API port, it must be a valid number more than 1000");
			}

			if (Validation.GetHasError(P2PPortTextBox)) {
				errors.Add("Invalid P2P port, it must be a valid number more than 1000");
			}

			if (ApiPortTextBox.Text == P2PPortTextBox.Text) {
				errors.Add("The P2P and API ports must not be equal");
			}

			if (Validation.GetHasError(PeersTextBox)) {
				errors.Add("Invalid Peers list, it must be of a string of IPs/URLs of peers, with P2P ports, separated by commas");
			}

			if (!File.Exists(PublicKeyFile_TextBlock.Text)) {
				errors.Add("The public key file must refer to a plain text file containing the public key in PEM format (the default format in this application).");
			}

			if (!File.Exists(PrivateKeyFile_TextBlock.Text)) {
				errors.Add("The private key file must refer to a plain text file containing the private key in PEM format (the default format in this application).");
			}

			if (errors.Count != 0) {
				ShowErrorMBox(
							  errors.Aggregate("There are some error(s):", (ret, err) => $"{ret}\n{err}"),
							  this
							 );
				return;
			}

			Settings.Default["CommandLineApiFile"] = CommandLineApiFile_TextBlock.Text;
			Settings.Default["ApiPort"]            = Convert.ToUInt32(ApiPortTextBox.Text);
			Settings.Default["P2PPort"]            = Convert.ToUInt32(P2PPortTextBox.Text);
			Settings.Default["Peers"]              = PeersTextBox.Text;
			Settings.Default["PublicKeyFile"]      = PublicKeyFile_TextBlock.Text;
			Settings.Default["PrivateKeyFile"]     = PrivateKeyFile_TextBlock.Text;
			Settings.Default.Save();
			Close();
		}

		private void CancelBtn_OnClick(object sender, RoutedEventArgs e) => Close();
	}
}