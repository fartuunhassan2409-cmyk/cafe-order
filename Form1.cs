using System;
using System.IO;
using System.Windows.Forms;

namespace LuckyCafeOrder;

public partial class Form1 : Form
{

    public Form1()
    {
        InitializeComponent();
    }
    private void btnCalculate_Click(object sender, EventArgs e)
    {
        string customerName = txtCustomerName.Text;

        if (customerName == "")
        {
            MessageBox.Show("Please enter the customer name.");
            return;
        }

        if (int.TryParse(txtQuantity.Text, out int quantity))
        {
            if (quantity < 1)
            {
                MessageBox.Show("Quwantatiga kow makayaraan karo");
                return;
            }
        }
        else
        {
            MessageBox.Show("quwantiti eber manoqon karo");
            return;
        }

        string drinkName;
        int drinkPrice;

        if (rdoTea.Checked)
        {
            drinkName = "Tea";
            drinkPrice = 1;
        }
        else if (rdoCoffee.Checked)
        {
            drinkName = "Coffee";
            drinkPrice = 2;
        }
        else if (rdoJuice.Checked)
        {
            drinkName = "Juice";
            drinkPrice = 3;
        }
        else
        {
            MessageBox.Show("fadlan dooro magaca cabitaanka.");
            return;
        }

        lstOrder.Items.Clear();
        bool wonPrize = false;
        Random random = new Random();
        int lucky = 0;
        for (int i = 1; i <= quantity; i++)
        {
            int luckyNumber = random.Next(1, 51);
            lstOrder.Items.Add(i + ")" + customerName + "-" + drinkName + "-" + "Lucky" + luckyNumber + "-" + drinkPrice.ToString("c2"));


            if (luckyNumber == 7 || luckyNumber == 17)
            {
                wonPrize = true;
                lucky = luckyNumber;
            }
        }

        int totalPrice = drinkPrice * quantity;

        if (chkStudentMember.Checked)
        {
            lstOrder.Items.Add("Member Discount: 10%");
            lstOrder.Items.Add("Total Before Discount: " + totalPrice.ToString("C2"));
            double discount = totalPrice * 0.1;
            double resalt = totalPrice - discount;
            lstOrder.Items.Add("Total After Discount: " + resalt.ToString("C2"));
            lblTotalPrice.Text = "Total Price " + resalt.ToString("c2");
        }
        else
        {
            lstOrder.Items.Add("Total: " + totalPrice.ToString("C2"));
            lblTotalPrice.Text = "Total Price " + totalPrice.ToString("c2");
        }
        if (wonPrize)
        {
            lstOrder.Items.Add("Lucky Message: You got a Free Cookie!(" + "lucky " + lucky + ")");
            lblLuckyMessage.Text = "Lucky Message: You got a Free Cookie!";
        }
        else
        {
            lstOrder.Items.Add("No prize this time");
            lblLuckyMessage.Text = " No prize this time";
        }

    }


    private void btnSave_Click(object sender, EventArgs e)

    {
        try
        {
            if (lstOrder.Items.Count == 0)
            {
                MessageBox.Show("No order to save.");
                return;
            }
            StreamWriter writer = File.AppendText("orders.txt");

            for (int i = 0; i < lstOrder.Items.Count; i++)
            {
                writer.WriteLine(lstOrder.Items[i]);
            }

            writer.WriteLine(lblTotalPrice.Text);
            writer.WriteLine(lblLuckyMessage.Text);

            writer.Close();

            MessageBox.Show("Order saved.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists("orders.txt"))
            {
                lstOrder.Items.Clear();

                StreamReader reader = File.OpenText("orders.txt");
                while (!reader.EndOfStream)
                {
                    lstOrder.Items.Add(reader.ReadLine());

                }
                reader.Close();


            }
            else
            {
                MessageBox.Show("No file found.");
                return;

            }


        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        txtCustomerName.Clear();
        txtQuantity.Clear();
        lstOrder.Items.Clear();
        lblTotalPrice.Text = "";
        lblLuckyMessage.Text = "";
        chkStudentMember.Checked = false;
        rdoTea.Checked = false;
        rdoCoffee.Checked = false;
        rdoJuice.Checked = false;
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
}
