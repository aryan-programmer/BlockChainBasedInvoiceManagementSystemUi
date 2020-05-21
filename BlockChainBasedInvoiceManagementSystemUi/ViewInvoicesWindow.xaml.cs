using System.Collections.Generic;
using System.Windows;

namespace BlockChainBasedInvoiceManagementSystemUi {
	/// <summary>
	/// Interaction logic for ViewInvoicesWindow.xaml
	/// </summary>
	public partial class ViewInvoicesWindow: Window {
		public ViewInvoicesWindow(List<UiInvoice> invoices) {
			InitializeComponent();
			Invoices = invoices;
			DataContext = this;
			Grid.DataContext = this;
		}

		public UiInvoice Invoice {
			get => (UiInvoice)GetValue(InvoiceProperty);
			set => SetValue(InvoiceProperty, value);
		}

		public static readonly DependencyProperty InvoiceProperty =
			DependencyProperty.Register("Invoice", typeof(UiInvoice), typeof(ViewInvoicesWindow), new PropertyMetadata(null));

		public List<UiInvoice> Invoices {
			get => (List<UiInvoice>)GetValue(InvoicesProperty);
			set => SetValue(InvoicesProperty, value);
		}

		public static readonly DependencyProperty InvoicesProperty =
			DependencyProperty.Register("Invoices", typeof(List<UiInvoice>), typeof(ViewInvoicesWindow), new PropertyMetadata(defaultValue: null));
	}
}
