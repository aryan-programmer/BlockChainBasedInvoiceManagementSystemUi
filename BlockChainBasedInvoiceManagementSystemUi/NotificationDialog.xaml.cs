using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BlockChainBasedInvoiceManagementSystemUi {
	// From https://stackoverflow.com/a/18325545
	/// <summary>
	/// Interaction logic for NotificationDialog.xaml
	/// </summary>
	public partial class NotificationDialog : Window {
		public static bool? ShowNotification(
			string title,
			string message,
			Icon   icon,
			string positiveButtonText = "Ok",
			string negativeButtonText = null) {
			var dialog = new NotificationDialog(message, GetIcon(icon), positiveButtonText, negativeButtonText) {
				Title = title
			};
			return dialog.ShowDialog();
		}

		private const int GwlStyle  = -16;
		private const int WsSysmenu = 0x80000;

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		private NotificationDialog(
			string       message,
			ImageSource icon,
			string       positiveButtonText,
			string       negativeButtonText
		) {
			DataContext = this;
			InitializeComponent();
			Loaded += (s, e) => {
				IntPtr windowInteropHelper = new WindowInteropHelper(this).Handle;
				SetWindowLong(windowInteropHelper, GwlStyle, GetWindowLong(windowInteropHelper, GwlStyle) & ~WsSysmenu);
			};
			MessageTextBox.Text = message;
			IconImage.Source    = icon;
			PositiveBtn.Content = positiveButtonText;
			if (negativeButtonText == null) return;
			NegativeBtn.Visibility = Visibility.Visible;
			NegativeBtn.Content    = negativeButtonText;
		}

		private static BitmapSource GetIcon(Icon icon) =>
			Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
												Int32Rect.Empty,
												BitmapSizeOptions.FromEmptyOptions());

		private void PositiveBtn_Click(object sender, RoutedEventArgs e) => DialogResult = true;

		private void NegativeBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
	}
}