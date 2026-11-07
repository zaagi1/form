using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe
{
    public partial class Form1 : Form
    {
        const double TEA_PRICE = 1.00;
        const double COFFEE_PRICE = 2.00;
        const double JUICE_PRICE = 3.00;

        double total;
        double discount;
        int luckyNumber;
        string luckyMessage;
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // Get customer name
                string customerName = label.Text;

                if (customerName == "")
                {
                    MessageBox.Show("Please enter customer name.");
                    return;
                }

                // Get quantity
                int quantity = Convert.ToInt32(txtQuantity.Text);

                if (quantity < 1)
                {
                    MessageBox.Show("Quantity must be at least 1.");
                    return;
                }

                // Select drink and price
                double price = 0;
                string drink = "";

                if (rdoTea.Checked)
                {
                    price = TEA_PRICE;
                    drink = "Tea";
                }
                else if (rdoCoffee.Checked)
                {
                    price = COFFEE_PRICE;
                    drink = "Coffee";
                }
                else if (rdoJuice.Checked)
                {
                    price = JUICE_PRICE;
                    drink = "Juice";
                }
                else
                {
                    MessageBox.Show("Please select a drink.");
                    return;
                }

                // Clear previous orders
                lstOrderList.Items.Clear();

                // Generate lucky numbers
                Random random = new Random();

                bool gotPrize = false;

                for (int i = 1; i <= quantity; i++)
                {
                    int luckyNumber = random.Next(1, 51);

                    if (luckyNumber == 7 || luckyNumber == 17)
                    {
                        gotPrize = true;
                    }

                    lstOrderList.Items.Add(i + ") " + customerName + " - " + drink +
                        " - Lucky: " + luckyNumber + " - $" + price.ToString("F2"));
                }

                // Calculate total price
                total = price * quantity;

                // Apply student discount
                if (chkStudentMember.Checked)
                {
                    total = total - (total * 0.10);
                }

                // Display total price
                lblTotalPrice.Text = "Total Price: $" + total.ToString("F2");

                // Display lucky message
                if (gotPrize == true)
                {
                    luckyMessage = "You got a Free Cookie!";
                }
                else
                {
                    luckyMessage = "No prize this time.";
                }

                lblLuckyMessage.Text = luckyMessage;
            }
            catch
            {
                MessageBox.Show("Please enter a valid quantity.");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if the ListBox is empty
            if (lstOrderList.Items.Count == 0)
            {
                MessageBox.Show("No order to save.");
                return;
            }

            try
            {
                // Open the file for adding information
                StreamWriter outputFile = File.AppendText("orders.txt");

                // Write every order into the file
                foreach (object item in lstOrderList.Items)
                {
                    outputFile.WriteLine(item.ToString());
                }

                // Write total price and lucky message
                outputFile.WriteLine(lblTotalPrice.Text);
                outputFile.WriteLine(lblLuckyMessage.Text);

                // Close the file
                outputFile.Close();

                MessageBox.Show("Order saved.");
            }
            catch
            {
                MessageBox.Show("Error saving the file.");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the file exists
                if (File.Exists("orders.txt") == false)
                {
                    MessageBox.Show("No file found.");
                    return;
                }

                // Clear the ListBox
                lstOrderList.Items.Clear();

                // Open the file
                StreamReader inputFile = File.OpenText("orders.txt");

                // Read the file line by line
                while (inputFile.EndOfStream == false)
                {
                    string line = inputFile.ReadLine();

                    lstOrderList.Items.Add(line);
                }

                // Close the file
                inputFile.Close();
            }
            catch
            {
                MessageBox.Show("Error loading the file.");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear customer name
            txtCustomerName.Clear();

            // Clear quantity
            txtQuantity.Clear();

            // Clear ListBox
            lstOrderList.Items.Clear();

            // Clear total price
            lblTotalPrice.Text = "";

            // Clear lucky message
            lblLuckyMessage.Text = "";

            // Uncheck Student Member
            chkStudentMember.Checked = false;

            // Reset total variable
            total = 0;

            // Reset lucky message variable
            luckyMessage = "";
        }
    }
}
