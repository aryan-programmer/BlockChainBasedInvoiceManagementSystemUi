using System;
using System.Windows;
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
			var commandLineApiFile = CommandLineApiFile_TextBlock.Text;
			var apiPort            = ApiPortTextBox.Text;
			var p2PPort            = P2PPortTextBox.Text;
			var peers              = PeersTextBox.Text;
			var publicKeyFile      = PublicKeyFile_TextBlock.Text;
			var privateKeyFile     = PrivateKeyFile_TextBlock.Text;
			if (ValidateSettings_ShowErrors(
											  commandLineApiFile,
											  apiPort,
											  p2PPort,
											  peers,
											  publicKeyFile,
											  privateKeyFile)
			) return;

			Settings.Default["CommandLineApiFile"] = commandLineApiFile;
			Settings.Default["ApiPort"]            = Convert.ToUInt32(apiPort);
			Settings.Default["P2PPort"]            = Convert.ToUInt32(p2PPort);
			Settings.Default["Peers"]              = peers;
			Settings.Default["PublicKeyFile"]      = publicKeyFile;
			Settings.Default["PrivateKeyFile"]     = privateKeyFile;
			Settings.Default.Save();
			Close();
		}

		private void CancelBtn_OnClick(object sender, RoutedEventArgs e) => Close();
	}
}