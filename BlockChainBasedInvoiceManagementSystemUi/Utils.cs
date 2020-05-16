using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using Microsoft.Win32;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public static class Utils {
		public static void ShowErrorMBox(string message, Window owner) =>
			MessageBox.Show(
							owner,
							message,
							caption: "Block chain based invoice management system",
							button: MessageBoxButton.OK,
							icon: MessageBoxImage.Error);

		public static void ShowInfoMBox(string message, Window owner) =>
			MessageBox.Show(
							owner,
							message,
							caption: "Block chain based invoice management system",
							button: MessageBoxButton.OK,
							icon: MessageBoxImage.Information);

		public static string ShowFileDialogBox<TFileDialogBox>(
			IEnumerable<(string desc, string ext)> exts,
			Window                                 window
		) where TFileDialogBox: FileDialog, new() {
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

		public static bool IsFilePathValid(string fileName) {
			System.IO.FileInfo fi = null;
			try {
				fi = new System.IO.FileInfo(fileName);
			}
			catch (ArgumentException) { }
			catch (System.IO.PathTooLongException) { }
			catch (NotSupportedException) { }
			return fi != null;
		}
	}
}
