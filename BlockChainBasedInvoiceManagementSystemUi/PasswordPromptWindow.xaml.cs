using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	/// Interaction logic for PasswordPromptWindow.xaml
	/// </summary>
	public partial class PasswordPromptWindow: Window {

		public PasswordPromptWindow(string prompt) {
			InitializeComponent();
			closedProperly = false;
			PasswordPrompt.Text = prompt;
		}

		private bool closedProperly;
		public SecureString Password => closedProperly ? PasswordBox.SecurePassword : null;

		private void OkBtn_OnClick(object sender = null, RoutedEventArgs e = null) {
			closedProperly = true;
			Close();
		}

		private void PasswordBox_OnKeyDown(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				OkBtn_OnClick();
			}
		}

		private void PasswordPrompt_OnLoaded(object sender, RoutedEventArgs e) {
			var s = (PasswordBox)sender;
			s.Focusable = true;
			s.Focus();
			Keyboard.ClearFocus();
			Keyboard.Focus(s);
		}
	}
}
