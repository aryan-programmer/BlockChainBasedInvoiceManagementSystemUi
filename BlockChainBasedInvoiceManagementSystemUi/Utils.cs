using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public static class Utils {
		public static void ShowErrorMBox(string message) =>
			Application
			   .Current
			   .Dispatcher
			   .Invoke(() =>
						   NotificationDialog
							  .ShowNotification(
												"Block chain based invoice management system",
												message,
												SystemIcons.Error));

		public static void ShowWarningMBox(string message) =>
			Application
			   .Current
			   .Dispatcher
			   .Invoke(() =>
						   NotificationDialog
							  .ShowNotification(
												"Block chain based invoice management system",
												message,
												SystemIcons.Information));

		public static void ShowInfoMBox(string message) =>
			Application
			   .Current
			   .Dispatcher
			   .Invoke(() =>
						   NotificationDialog
							  .ShowNotification(
												"Block chain based invoice management system",
												message,
												SystemIcons.Information));

		public static bool ShowConfirmationMBox(string message) =>
			NotificationDialog
			   .ShowNotification(
								 "Block chain based invoice management system",
								 message,
								 SystemIcons.Exclamation,
								 "Yes",
								 "No") == true;

		public static string ShowFileDialogBox<TFileDialogBox>(
			IEnumerable<(string desc, string ext)> exts,
			Window                                 window
		) where TFileDialogBox : FileDialog, new() {
			var dialog = new TFileDialogBox {
				Filter = exts.Aggregate("",
										(val, ext) => $"{val}{ext.desc} ({ext.ext})|{ext.ext}|",
										val => val.Substring(0, val.Length - 1)),
				FilterIndex = 0
			};
			return dialog.ShowDialog(window) == true ? dialog.FileName : null;
		}

		public static SecureString PasswordPrompt(string prompt) {
			var promptWindow = new PasswordPromptWindow(prompt);
			promptWindow.ShowDialog();
			return promptWindow.Password;
		}

		public static string SecureStringToString(SecureString value) {
			IntPtr valuePtr = IntPtr.Zero;
			try {
				valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
				return Marshal.PtrToStringUni(valuePtr);
			} finally {
				Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
			}
		}

		public static bool ValidatePath(string fileName) {
			FileInfo fi = null;
			try {
				fi = new FileInfo(fileName);
			} catch (ArgumentException) { } catch (PathTooLongException) { } catch (NotSupportedException) { }

			return fi != null;
		}

		public static bool ValidatePort(string port) =>
			new Regex(@"^[1-9][0-9]{3,}$").IsMatch(port);

		public static bool ValidatePeers(string peers) =>
			(peers == "") ||
			new Regex(@"^([\w-]+\.)+[\w-]+:[1-9][0-9]{3,}(,([\w-]+\.)+[\w-]+:[1-9][0-9]{3,})*$").IsMatch(peers);

		public static bool ValidatePhoneNumber(string phoneNumber) =>
			new Regex(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$").IsMatch(phoneNumber);

		public static bool ValidateInpInv_ShowError(
			string           invoiceNumber,
			string           purchaserName,
			string           phoneNumber,
			List<InpProduct> products) {
			var errors = new List<string>();

			if (invoiceNumber == "")
				errors.Add("Invoice number must not be empty.");

			if (purchaserName == "")
				errors.Add("The purchaser's name must not be empty.");

			if (!ValidatePhoneNumber(phoneNumber))
				errors.Add("Invalid phone number.");

			if (products.Count == 0) {
				errors.Add("The number of products must be at least be 1.");
			} else {
				if (products.Any(product => string.IsNullOrEmpty(product.name)))
					errors.Add("A product's name can't be empty.");

				if (products.Any(product => string.IsNullOrEmpty(product.quantity)))
					errors.Add("A product's quantity can't be empty.");
			}

			if (errors.Count != 0) {
				ShowErrorMBox(errors.Aggregate("There are some error(s):", (ret, err) => $"{ret}\n{err}"));
				return false;
			}

			return true;
		}

		public static bool ValidateSettings_ShowErrors(
			string commandLineApiFile,
			string apiPort,
			string p2PPort,
			string peers,
			string publicKeyFile,
			string privateKeyFile
		) {
			var errors = new List<string>();

			if (!ValidatePort(apiPort))
				errors.Add("Invalid API port, it must be a valid number more than 1000");

			if (!ValidatePort(p2PPort))
				errors.Add("Invalid P2P port, it must be a valid number more than 1000");

			if (apiPort == p2PPort)
				errors.Add("The P2P and API ports must not be equal");

			return ValidateSettings_Rest_ShowErrors(
													commandLineApiFile,
													peers,
													publicKeyFile,
													privateKeyFile,
													errors);
		}

		public static bool ValidateSettings_ShowErrors(
			string commandLineApiFile,
			uint   apiPort,
			uint   p2PPort,
			string peers,
			string publicKeyFile,
			string privateKeyFile
		) {
			var errors = new List<string>();

			if (apiPort < 1000)
				errors.Add("Invalid API port, it must be a more than 1000");

			if (p2PPort < 1000)
				errors.Add("Invalid P2P port, it must be a more than 1000");

			if (apiPort == p2PPort)
				errors.Add("The P2P and API ports must not be equal");

			return ValidateSettings_Rest_ShowErrors(
													commandLineApiFile,
													peers,
													publicKeyFile,
													privateKeyFile,
													errors);
		}

		private static bool ValidateSettings_Rest_ShowErrors(
			string              commandLineApiFile,
			string              peers,
			string              publicKeyFile,
			string              privateKeyFile,
			ICollection<string> errors
		) {
			if (!File.Exists(commandLineApiFile))
				errors.Add("The command line API file must refer to an executable, a batch file or command prompt file.");

			if (!ValidatePeers(peers))
				errors.Add("Invalid Peers list, it must be of a string of IPs/URLs of peers, with P2P ports, separated by commas");

			if (!File.Exists(publicKeyFile))
				errors.Add("The public key file must refer to a plain text file containing the public key in PEM format (the default format in this application).");

			if (!File.Exists(privateKeyFile))
				errors.Add("The private key file must refer to a plain text file containing the private key in PEM format (the default format in this application).");

			if (errors.Count != 0) {
				ShowErrorMBox(errors.Aggregate("There are some error(s):", (ret, err) => $"{ret}\n{err}"));
				return false;
			}

			return true;
		}
	}
}