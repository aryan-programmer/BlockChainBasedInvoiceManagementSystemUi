using System.Globalization;
using System.Windows.Controls;
using static BlockChainBasedInvoiceManagementSystemUi.Utils;

namespace BlockChainBasedInvoiceManagementSystemUi {
	public class PortInputValidator : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			if (!(value is string))
				return new ValidationResult(false,
											"Value must a string representation of an integer larger than 1000 (w/o leading zeros) not null.");

			if (!ValidatePort((string) value))
				return new ValidationResult(false,
											$"Value must a string representation of an integer larger than 1000 (w/o leading zeros) not {value}.");

			return ValidationResult.ValidResult;
		}
	}

	public class PeersInputValidator : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			if (!(value is string))
				return new ValidationResult(false,
											"Value must a string of IPs of peers, with P2P ports, separated by commas, not null.");

			if (!ValidatePeers((string) value))
				return new ValidationResult(false,
											$"Value must a string of IPs of peers, with P2P ports, separated by commas, not {value}.");

			return ValidationResult.ValidResult;
		}
	}

	public class PhoneNumberValidator : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			if (!(value is string))
				return new ValidationResult(false,
											"Value must a valid phone number, not null.");

			if (!ValidatePhoneNumber((string) value))
				return new ValidationResult(false,
											$"Value must a valid phone number, not {value}.");

			return ValidationResult.ValidResult;
		}
	}
}