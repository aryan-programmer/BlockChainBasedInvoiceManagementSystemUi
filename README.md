# blockchain-based-invoice-management-system

A blockchain based invoice management system for vendors to track all their customers' invoices.

This project is the GUI for the CLI API provided by the [blockchain-based-invoice-management-syatem](https://github.com/aryan-programmer/blockchain-based-invoice-management-system).

This application is to be used by manufacturers and vendors so as to keep a log of all their customers’ invoices.
Any shopkeeper can add an invoice, which will be signed by the shopkeeper’s private key, so as to prevent tampering by other shopkeepers. This invoice will be broadcasted to all the shopkeeper’s peers. 
Any vendor can mine the set of invoices in the pool of invoices. This will add the set of invoices to the block chain thereby confirming the invoices in the pool.

A vendor will only have to input the invoice details like the invoice number, the purchaser’s details and the product’s details. 
The purchase’s details are phone number, name, and whether or not the purchaser is an another vendor. 
The product details only need to be the name, quantity, cost, and tax percentage. 
An individual product’s tax price and total cost will be calculated by the application. The invoice’s grand total cost will also be automatically calculated.

## Usage

### Initial Setup

Clone the repo from https://github.com/aryan-programmer/blockchain-based-invoice-management-system.git, go into the source and run:

```
yarn global add tsc
yarn install
tsc
yarn package
```

Transfer the now created executable to a convenient place and specify this as the “Command line API file” in the options menu.
Then click File>New>Key Pair in there specify the public and private key files to save as.
Then specify these key files as the public and private key files in the options menu.
In the options menu set the ”Peers” field to all the peers as per the example given below the textbox.
Then just click ”Start Server” in the main window, this will start the server and the ”View” and ”Tools” menu headers will now be clickable.
The UI is pretty intuitive from that point onwards.

###  Normal Usage

Just click ”Start Server” in the main window, this will start the server and the ”View” and ”Tools” menu headers will now be clickable. And the UI is very intuitive after that.