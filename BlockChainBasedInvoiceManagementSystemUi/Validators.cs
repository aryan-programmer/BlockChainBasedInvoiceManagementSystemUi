using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public class PortInputValidator : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			if (!(value is string)) {
				return new ValidationResult(false,
											"Value must a string representation of an integer larger than 1000 (w/o leading zeros) not null.");
			}

			var reg = new Regex("[1-9][0-9]{3,}");
			if (!reg.IsMatch((string) value)) {
				return new ValidationResult(false,
											$"Value must a string representation of an integer larger than 1000 (w/o leading zeros) not {value}.");
			}

			return ValidationResult.ValidResult;
		}
	}

	public class PeersInputValidator : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			if (!(value is string)) {
				return new ValidationResult(false,
											"Value must a string of IPs of peers, with P2P ports, separated by commas, not null.");
			}

			var val = (string) value;

			if (val == "") return ValidationResult.ValidResult;
			var reg = new Regex(@"^([\w-]+\.)+[\w-]+:[1-9][0-9]{3,}(,([\w-]+\.)+[\w-]+:[1-9][0-9]{3,})*$");
			if (!reg.IsMatch(val)) {
				return new ValidationResult(false,
											$"Value must a string of IPs of peers, with P2P ports, separated by commas, not {value}.");
			}

			return ValidationResult.ValidResult;
		}
	}
}