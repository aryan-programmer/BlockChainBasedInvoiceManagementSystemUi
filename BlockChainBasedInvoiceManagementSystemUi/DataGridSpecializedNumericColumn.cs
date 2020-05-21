using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public class DataGridSpecializedNumericColumn : DataGridTextColumn {

		public bool IsTaxColumn { get; set; }

		protected override object
			PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs) {
			var edit = (TextBox) editingElement;
			edit.PreviewTextInput += Edit_PreviewTextInput;
			DataObject.AddPastingHandler(edit, OnPaste);
			return base.PrepareCellForEdit(editingElement, editingEventArgs);
		}

		private void OnPaste(object sender, DataObjectPastingEventArgs e) {
			var str = (e.Source as TextBox)?.Text + e.SourceDataObject.GetData(DataFormats.Text);
			if (!IsDataValid(str))
				e.CancelCommand();
		}

		private void Edit_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			var str = (e.Source as TextBox)?.Text + e.Text;
			e.Handled = !IsDataValid(str);
		}

		private bool IsDataValid(string str) {
			try {
				var val = Convert.ToDouble(str);
				if (IsTaxColumn && (val > 100)) {
					return false;
				}

				var dot = str.IndexOf('.');
				if (dot != -1) {
					if (dot < (str.Length - 3)) {
						return false;
					}
				}

				return true;
			} catch {
				return false;
			}
		}
	}
}