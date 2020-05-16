using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Windows;
using BlockChainBasedInvoiceManagementSystemUi.Properties;
using Microsoft.Win32;
using static BlockChainBasedInvoiceManagementSystemUi.Utils;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	/// Interaction logic for GenerateKeysWindow.xaml
	/// </summary>
	public partial class GenerateKeysWindow : Window {
		public GenerateKeysWindow() {
			InitializeComponent();
		}

		private void PublicKeyFile_Browse_Btn_OnClick(object sender, RoutedEventArgs e) =>
			PublicKeyFile_TextBlock.Text = ShowFileDialogBox<SaveFileDialog>(new[] {
				("PEM Encryption key file", "*.pem"),
				("All files", "*.*"),
			}, this);

		private void PrivateKeyFile_Browse_Btn_OnClick(object sender, RoutedEventArgs e) =>
			PrivateKeyFile_TextBlock.Text = ShowFileDialogBox<SaveFileDialog>(new[] {
				("PEM Encryption key file", "*.pem"),
				("All files", "*.*"),
			}, this);

		private void GenKeysBtn_OnClick(object sender, RoutedEventArgs e) {
			var errors = new List<string>();
			if (!IsFilePathValid(PublicKeyFile_TextBlock.Text)) {
				errors.Add("The public key file location specified must be valid.");
			}

			if (!IsFilePathValid(PrivateKeyFile_TextBlock.Text)) {
				errors.Add("The private key file location specified must be valid.");
			}

			if (errors.Count != 0) {
				ShowErrorMBox(
							  errors.Aggregate("There are some error(s):", (ret, err) => $"{ret}\n{err}"),
							  this
							 );
				return;
			}

			SecureString password = PasswordPrompt("Please enter a password to encrypt the private key:");
			while ((password == null) || (password.Length == 0)) {
				ShowErrorMBox("Please enter a password.", this);
				password?.Dispose();
				password = PasswordPrompt("Please enter a password to encrypt the private key:");
			}

			var processStartInfo = new ProcessStartInfo {
				FileName        = Settings.Default.CommandLineApiFile,
				CreateNoWindow  = true,
				UseShellExecute = false,
				ErrorDialog     = true,
				Arguments = $@"gen-keys --public-key-file-path ""{
						PublicKeyFile_TextBlock.Text
					}"" --private-key-file-path ""{
						PrivateKeyFile_TextBlock.Text
					}"" --password ""{
						SecureStringToString(password)
					}""",
				RedirectStandardError = true,
			};
			password.Dispose();
			Process process = Process.Start(processStartInfo);
			if (process == null) {
				ShowErrorMBox($"There was an error in starting the process.", this);
			} else {
				process.WaitForExit();
				if (process.ExitCode != 0) {
					ShowErrorMBox($"There was an error:\n{process.StandardError.ReadToEnd()}", this);
				} else {
					ShowInfoMBox("Success: Public private key pair created successfully.", this);
					Close();
				}
			}
		}

		private void CancelBtn_OnClick(object sender, RoutedEventArgs e) => Close();
	}
}