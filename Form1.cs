using System;
using System.IO;
using System.Windows.Forms;

namespace LuckyCafeOrder
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        decimal total = 0m;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            string name = txtCustomerName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Enter customer name.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty < 1)
            {
                MessageBox.Show("Enter a valid quantity.");
                return;
            }

            string drink = "";
            decimal price = 0m;

            if (rdoTea.Checked)
            {
                drink = "Tea";
                price = 1m;
            }
            else if (rdoCoffee.Checked)
            {
                drink = "Coffee";
                price = 2m;
            }
            else if (rdoJuice.Checked)
            {
                drink = "Juice";
                price = 3m;
            }
            else
            {
                MessageBox.Show("Select a drink.");
                return;
            }

            lstOrder.Items.Clear();

            bool prize = false;
            int win = 0;

            for (int i = 1; i <= qty; i++)
            {
                int lucky = rand.Next(1, 51);

                lstOrder.Items.Add(
                    $"{i}. {name} - {drink} - Lucky: {lucky} - ${price:F2}");

                if (lucky == 7 || lucky == 17)
                {
                    prize = true;
                    win = lucky;
                }
            }

            decimal before = price * qty;
            decimal after = before;

            lstOrder.Items.Add("-------------------------");

            if (chkStudentMember.Checked)
            {
                after = before * 0.90m;
                lstOrder.Items.Add("Member Discount: 10%");
            }
            else
            {
                lstOrder.Items.Add("Member Discount: 0%");
            }

            lstOrder.Items.Add($"Total Before Discount: ${before:F2}");
            lstOrder.Items.Add($"Total After Discount: ${after:F2}");

            if (prize)
            {
                lstOrder.Items.Add(
                    $"Lucky Message: You got a Free Cookie! (Lucky {win})");
            }
            else
            {
                lstOrder.Items.Add("Lucky Message: No prize this time.");
            }

            total = after;

            lblTotalPrice.Text = $"Total Price: ${total:F2}";
            lblLuckyMessage.Text = prize
                ? $"You got a Free Cookie! (Lucky {win})"
                : "No prize this time.";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstOrder.Items.Count == 0)
            {
                MessageBox.Show("No order to save.");
                return;
            }

            using (StreamWriter writer = File.AppendText("orders.txt"))
            {
                foreach (var item in lstOrder.Items)
                {
                    writer.WriteLine(item.ToString());
                }

                writer.WriteLine("-------------------------");
            }

            MessageBox.Show("Order saved.");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists("orders.txt"))
            {
                MessageBox.Show("No file found.");
                return;
            }

            lstOrder.Items.Clear();

            using (StreamReader reader = File.OpenText("orders.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    lstOrder.Items.Add(line);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();
            txtQuantity.Clear();

            lstOrder.Items.Clear();

            lblTotalPrice.Text = "Total Price:";
            lblLuckyMessage.Text = "Lucky Message:";

            chkStudentMember.Checked = false;

            rdoTea.Checked = false;
            rdoCoffee.Checked = false;
            rdoJuice.Checked = false;

            total = 0m;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}