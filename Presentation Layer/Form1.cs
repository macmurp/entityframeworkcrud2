using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using entityframeworkcrud2;
using System.Data.SqlClient;
using System.Configuration;

namespace Presentation_Layer
{
    public partial class Form1 : Form
    {
        bool changed = false;
        SalesContext ctx = new SalesContext();
        
        public Form1()
        {
            InitializeComponent();
        }
        void getData()
        {
            var c = ctx.Customers.OrderBy(x => x.LastName).ToList();
            dataGridView1.DataSource = c;
        }

        private void updatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (changed)
                ctx.SaveChanges();
                MessageBox.Show("Updated customers.");
                getData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            changed = true;

        }

        private void searchbtn_Click(object sender, EventArgs e)
        {
            string searchbox = txtsearch.Text.ToString().Trim();
            string searchboxcity = txtcitysearch.Text.ToString().Trim();
            if((searchbox == null || searchbox == "") && (searchboxcity == null || searchboxcity == ""))
            {
                getData();               
            }
            else if ((searchbox != null && searchbox != "") && (searchboxcity != null && searchboxcity != ""))
            {
                //if both last name search box and the city search box have text to search, then attempt to use both
                var search = ctx.Customers.Where(c => c.LastName == searchbox)
                    .Where(c => c.City == searchboxcity);
                dataGridView1.DataSource = search.ToList();
            }
            else if ((searchbox != null && searchbox != "") && (searchboxcity == null || searchboxcity == ""))
            { 
                //if only the last name search has text, only use that
                var search = ctx.Customers.Where(c => c.LastName == searchbox);
                dataGridView1.DataSource = search.ToList();
            }
            else if ((searchbox == null || searchbox == "") && (searchboxcity != null && searchboxcity != ""))
            {
                //if only the city search has text, only use that
                var search = ctx.Customers.Where(c => c.City == searchboxcity);
                dataGridView1.DataSource = search.ToList();
            }
            else
            {
                getData();
                MessageBox.Show("Invalid search.");
            }
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {
            getData();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtfirst.Text.ToString().Trim() == null || txtfirst.Text.ToString().Trim() == "")
                    || (txtlast.Text.ToString().Trim() == null || txtlast.Text.ToString().Trim() == ""))
                {
                    //if the required name fields are empty strings or null, do not add customer
                    MessageBox.Show("First and Last Name are required fields.");
                }
                else
                {
                    Customer customer = new Customer();
                    customer.FirstName = txtfirst.Text.ToString().Trim();
                    customer.LastName = txtlast.Text.ToString().Trim();
                    customer.City = txtcity.Text.ToString().Trim();
                    customer.Country = txtcountry.Text.ToString().Trim();
                    customer.Phone = txtphone.Text.ToString().Trim();
                    //take info in text boxes and put into a customer object to add
                    ctx.Add(customer);
                    ctx.SaveChanges();
                    MessageBox.Show("Added new customer.");
                    txtfirst.Text = txtlast.Text = txtcity.Text = txtcountry.Text = txtphone.Text = "";
                    //clears out text boxes after entry
                    getData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm deletion of row?", "DataGridView", MessageBoxButtons.YesNo) == DialogResult.Yes)
                try
            {
                    Customer todelete = new Customer();
                    todelete = ctx.Customers.SingleOrDefault(x => x.Id == Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value));
                    //search for matching customer based on the id of the selected row
                    //use to delete
                    ctx.Customers.Remove(todelete);
                    ctx.SaveChanges();
                    MessageBox.Show("Deleted customer.");
                    getData();
            }
                catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            else
            {
                //don't delete if they say no to the pop out dialogue
            }
        }

    }
}
