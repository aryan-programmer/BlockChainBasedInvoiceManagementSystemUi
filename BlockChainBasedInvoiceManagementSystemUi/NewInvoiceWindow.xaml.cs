using System;
using System.Collections.Generic;
using System.Windows;
using static BlockChainBasedInvoiceManagementSystemUi.Utils;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	/// Interaction logic for NewInvoiceWindow.xaml
	/// </summary>
	public partial class NewInvoiceWindow : Window {
		public NewInvoiceWindow() {
			InitializeComponent();
			DataGrid.ItemsSource = products;
			Grid.DataContext     = this;
		}

		public List<InpProduct> products = new List<InpProduct>();

		public string InvoiceNumber        { get; set; } = Guid.NewGuid().ToString();
		public string PurchaserName        { get; set; } = "";
		public string PurchaserPhoneNumber { get; set; } = "";
		public bool   IsPurchaserVendor    { get; set; }

		public InpInv GetInpInv() => new InpInv {
			invoiceNumber = InvoiceNumber,
			products      = products,
			purchaser = new Purchaser {
				name        = PurchaserName,
				phoneNumber = PurchaserPhoneNumber,
				isVendor    = IsPurchaserVendor,
			}
		};

		private void OkBtn_OnClick(object sender, RoutedEventArgs e) {
			if (!ValidateInpInv_ShowError(
										  InvoiceNumber,
										  PurchaserName,
										  PhoneNumberTextBox.Text,
										  products)) return;
			if (!ShowConfirmationMBox(@"Are you sure you want to send this invoice to pool for mining?
Once you do this it can't be undone.")) return;
			DialogResult = true;
		}

		private void CancelBtn_OnClick(object sender, RoutedEventArgs e) {
			if (ShowConfirmationMBox(@"Are you sure you want close the dialog and lose all data entered on this form?
Once you do this it can't be undone."))
				DialogResult = false;
		}
	}
}