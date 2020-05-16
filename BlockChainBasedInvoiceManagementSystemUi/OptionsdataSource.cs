using System.Windows;
using BlockChainBasedInvoiceManagementSystemUi.Properties;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public class OptionsDataSource : DependencyObject {
		public uint   P2PPort { get; set; } = Settings.Default.P2PPort;
		public uint   ApiPort { get; set; } = Settings.Default.ApiPort;
		public string Peers   { get; set; } = Settings.Default.Peers;
	}
}