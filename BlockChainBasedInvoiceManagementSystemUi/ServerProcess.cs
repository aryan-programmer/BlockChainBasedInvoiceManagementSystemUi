using System;
using System.Diagnostics;
using System.Security;
using BlockChainBasedInvoiceManagementSystemUi.Properties;
using static BlockChainBasedInvoiceManagementSystemUi.Utils;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public static class ServerProcess {

		private static Process process;
		public static  bool    IsStarted => process != null;

		public static event Action<Process> Exit;

		public static bool StartProcess_ShowErrors() {
			if (process != null) {
				ShowErrorMBox("Error: Server process has already been started");
				return false;
			}

			if (ValidateSettings_ShowErrors(Settings.Default.CommandLineApiFile,
											Settings.Default.ApiPort,
											Settings.Default.P2PPort,
											Settings.Default.Peers,
											Settings.Default.PublicKeyFile,
											Settings.Default.PrivateKeyFile))
				return false;

			SecureString password = PasswordPrompt("Please enter a password to encrypt the private key:");
			if ((password == null) || (password.Length == 0)) {
				ShowErrorMBox("Please enter a password.");
				password?.Dispose();
				return false;
			}

			var peersInput = Settings.Default.Peers == ""
								 ? ""
								 : " --peers \"ws://" + Settings.Default.Peers.Replace(",", ",ws://") + "\"";

			var processStartInfo = new ProcessStartInfo {
				FileName        = Settings.Default.CommandLineApiFile,
				CreateNoWindow  = true,
				UseShellExecute = false,
				ErrorDialog     = true,
				Arguments = $@"p2p --port ""{
						Settings.Default.ApiPort
					}"" --p2p-port ""{
						Settings.Default.P2PPort
					}""{
						peersInput
					} --public-key-file-path ""{
						Settings.Default.PublicKeyFile
					}"" --private-key-file-path ""{
						Settings.Default.PrivateKeyFile
					}"" --password ""{
						SecureStringToString(password)
					}""",
				RedirectStandardError  = true,
				RedirectStandardOutput = true
			};
			password.Dispose();
			Process newProcess = Process.Start(processStartInfo);
			if (newProcess == null) {
				ShowErrorMBox("There was an error in starting the process.");
				return false;
			}

			newProcess.EnableRaisingEvents =  true;
			newProcess.Exited              += (sender, args) => Exit?.Invoke(newProcess);

			process = newProcess;
			return true;
		}

		public static void StopProcess() {
			if (process == null) return;
			try {
				process.Kill();
			} catch {
				// ignored
			}

			Exit?.Invoke(process);
			process.Close();
			process = null;
		}
	}
}