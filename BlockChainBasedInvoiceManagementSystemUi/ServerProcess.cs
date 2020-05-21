using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using BlockChainBasedInvoiceManagementSystemUi.Properties;
using static BlockChainBasedInvoiceManagementSystemUi.Utils;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public delegate void ServerProcessExited(int errorCode, StreamReader stdout, StreamReader stderr);

	public class ServerProcess : INotifyPropertyChanged {
		private bool isStarted //*
				= true
			//*///
			;

		private Process process;

		private ServerProcess() { }

		public static ServerProcess I { get; } = new ServerProcess();

		public bool IsStarted {
			get => isStarted;
			private set {
				isStarted = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		public event ServerProcessExited Exit;

		public bool StartProcess_ShowErrors() {
			if (process != null) {
				ShowErrorMBox("Error: Server process has already been started");
				return false;
			}

			if (!ValidateSettings_ShowErrors(Settings.Default.CommandLineApiFile,
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

			newProcess.EnableRaisingEvents = true;
			newProcess.Exited += (sender, args) => {
				Exit?.Invoke(newProcess.ExitCode, newProcess.StandardOutput, newProcess.StandardError);
				process.Close();
				process   = null;
				IsStarted = false;
			};

			process   = newProcess;
			IsStarted = true;
			return true;
		}

		public void StopProcess() {
			if (process == null) return;
			try {
				process.Kill();
			} catch {
				// ignored
			}
		}
	}
}