using System.Collections.Generic;
using System.Linq;
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

		public static string OpenFileDialogBox(
			IEnumerable<(string desc, string ext)> exts,
			Window                                 window
		) {
			var dialog = new OpenFileDialog {
				Filter = exts.Aggregate("",
										(val, ext) => $"{val}{ext.desc} ({ext.ext})|{ext.ext}|",
										val => val.Substring(0, val.Length - 1)),
				FilterIndex = 0,
				Multiselect = false
			};
			return dialog.ShowDialog(window) == true ? dialog.FileName : null;
		}
	}
}
