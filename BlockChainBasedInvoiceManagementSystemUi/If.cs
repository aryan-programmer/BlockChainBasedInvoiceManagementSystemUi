using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public class IfElse : UserControl, INotifyPropertyChanged {

		public static readonly DependencyProperty VProperty =
			DependencyProperty.Register(
										"V",
										typeof(bool),
										typeof(IfElse),
										new PropertyMetadata(
															 false,
															 OnVPropertyChanged));

		public IfElse() {
			Loaded          += OnLoaded;
			PropertyChanged += OnPropertyChanged;
		}

		public object Then { get; set; }
		public object Else { get; set; }

		public bool V {
			get => (bool) GetValue(VProperty);
			set => SetValue(VProperty, value);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnLoaded(object sender, RoutedEventArgs e) => Content = V ? Then : Else;

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == "V") Content = V ? Then : Else;
		}

		private static void OnVPropertyChanged(
			DependencyObject sender, DependencyPropertyChangedEventArgs e) =>
			(sender as IfElse)?.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("V"));
	}
}