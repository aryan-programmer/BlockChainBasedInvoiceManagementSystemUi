using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public class IfElse : UserControl, INotifyPropertyChanged {
		public IfElse() {
			Loaded          += OnLoaded;
			PropertyChanged += OnPropertyChanged;
		}

		private void OnLoaded(object sender, RoutedEventArgs e) => Content = V ? Then : Else;

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == "V") {
				Content = V ? Then : Else;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public object Then { get; set; }
		public object Else { get; set; }

		public bool V {
			get => (bool) GetValue(VProperty);
			set => SetValue(VProperty, value);
		}

		// Using a DependencyProperty as the backing store for V.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty VProperty =
			DependencyProperty.Register(
										"V",
										typeof(bool),
										typeof(IfElse),
										new PropertyMetadata(
															 false,
															 OnVPropertyChanged));

		private static void OnVPropertyChanged(
			DependencyObject sender, DependencyPropertyChangedEventArgs e) =>
			(sender as IfElse)?.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("V"));
	}
}