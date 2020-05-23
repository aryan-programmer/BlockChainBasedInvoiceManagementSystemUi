using System.Windows;
using System.Windows.Controls;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public class IfElse : UserControl {

		public static readonly DependencyProperty VProperty =
			DependencyProperty.Register(
										"V",
										typeof(bool),
										typeof(IfElse),
										new PropertyMetadata(
															 false,
															 OnVChanged));

		public IfElse() => Loaded += OnLoaded;

		public object Then { get; set; }
		public object Else { get; set; }

		public bool V {
			get => (bool) GetValue(VProperty);
			set => SetValue(VProperty, value);
		}

		private void OnLoaded(object sender, RoutedEventArgs e) => Content = V ? Then : Else;

		private void OnVChanged() => Content = V ? Then : Else;

		private static void OnVChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) =>
			(sender as IfElse)?.OnVChanged();
	}
}