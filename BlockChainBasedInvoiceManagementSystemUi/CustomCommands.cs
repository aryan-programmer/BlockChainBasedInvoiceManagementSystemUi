using System.Windows.Input;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public static class CustomCommands {
		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
			 "Exit",
			 "Exit",
			 typeof(CustomCommands),
			 new InputGestureCollection {
				 new KeyGesture(Key.F4, ModifierKeys.Alt)
			 }
			);

		public static readonly RoutedUICommand Options = new RoutedUICommand
			(
			 "Options",
			 "Options",
			 typeof(CustomCommands),
			 new InputGestureCollection {
				 new KeyGesture(Key.O, ModifierKeys.Control)
			 }
			);

		public static readonly RoutedUICommand GenerateKeys = new RoutedUICommand
			(
			 "Generate Keys",
			 "GenerateKeys",
			 typeof(CustomCommands),
			 new InputGestureCollection {
				 new KeyGesture(Key.G, ModifierKeys.Control)
			 }
			);
	}
}