using System;
using System.Collections.Generic;

namespace BlockChainBasedInvoiceManagementSystemUi {
	// ReSharper disable InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles

	#region Received JSON Data Classes

	public class Purchaser {
		public string phoneNumber { get; set; }
		public string name        { get; set; }
		public bool   isVendor    { get; set; }
	}

	public class Product {
		public string name          { get; set; }
		public string quantity      { get; set; }
		public float  cost          { get; set; }
		public float  taxPercentage { get; set; }
		public float  tax           { get; set; }
		public float  totalCost     { get; set; }
	}

	public class Inv {
		public string        invoiceNumber { get; set; }
		public List<Product> products      { get; set; }
		public float         totalCost     { get; set; }
		public Purchaser     purchaser     { get; set; }
	}

	public class Invoice {
		public Inv    invoice   { get; set; }
		public string signature { get; set; }
		public string publicKey { get; set; }
		public string timestamp { get; set; }
	}

	public class Block {
		public string        timestamp  { get; set; }
		public string        lastHash   { get; set; }
		public string        hash       { get; set; }
		public List<Invoice> data       { get; set; }
		public string        nonce      { get; set; }
		public ushort        difficulty { get; set; }
	}

	#endregion

	#region Parsed Data Classes designed to be easy to use in XAML

	public class UiProduct {
		public UiProduct(Product product) {
			name          = product.name;
			quantity      = product.quantity;
			cost          = product.cost.ToString("₹ #.00");
			taxPercentage = product.taxPercentage.ToString("#.00") + " %";
			tax           = product.tax.ToString("₹ #.00");
			totalCost     = product.totalCost.ToString("₹ #.00");
		}

		public string name          { get; set; }
		public string quantity      { get; set; }
		public string cost          { get; set; }
		public string taxPercentage { get; set; }
		public string tax           { get; set; }
		public string totalCost     { get; set; }
	}

	public class UiInvoice {
		public string          invoiceNumber        { get; set; }
		public List<UiProduct> products             { get; set; }
		public float           totalCost            { get; set; }
		public string          signature            { get; set; }
		public string          publicKey            { get; set; }
		public string          date                 { get; set; }
		public string          time                 { get; set; }
		public DateTime        timestamp            { get; set; }
		public string          purchaserPhoneNumber { get; set; }
		public string          purchaserName        { get; set; }
		public bool            isPurchaserVendor    { get; set; }

		public UiInvoice(Invoice inv) {
			timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
					   .AddMilliseconds(Convert.ToDouble(inv.timestamp))
					   .ToLocalTime();
			date                 = timestamp.ToLongDateString();
			time                 = timestamp.ToLongTimeString();
			signature            = inv.signature;
			publicKey            = inv.publicKey;
			invoiceNumber        = inv.invoice.invoiceNumber;
			products             = inv.invoice.products.ConvertAll(product => new UiProduct(product));
			totalCost            = inv.invoice.totalCost;
			purchaserPhoneNumber = inv.invoice.purchaser.phoneNumber;
			purchaserName        = inv.invoice.purchaser.name;
			isPurchaserVendor    = inv.invoice.purchaser.isVendor;
		}
	}

	#endregion

#pragma warning restore IDE1006 // Naming Styles
	// ReSharper restore InconsistentNaming
}