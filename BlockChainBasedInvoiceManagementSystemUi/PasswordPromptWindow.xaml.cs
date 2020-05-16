using System.Security;
using System.Windows;

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

		private void OkBtn_OnClick(object sender, RoutedEventArgs e) {
			closedProperly = true;
			Close();
		}
	}
}
