﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace edu.ksu.cis.masaaki
{
    public partial class BookShop : Form
    {
        StaffWindow staffWindow;
        CustomerWindow customerWindow;
        Controller _control = new Controller();
        // XXX You can add more fields
       
        public BookShop()
        {
            InitializeComponent();
        }
        private void BookShop_Load(object sender, EventArgs e)
        {
            

            // XXX You may change the contructors of StaffWindow and CustomerWindow to take
            // some arguments
            staffWindow = new StaffWindow(_control);
            staffWindow.StartPosition = FormStartPosition.Manual;
            staffWindow.Location = new Point(600, 100);
            customerWindow = new CustomerWindow(_control);
            customerWindow.StartPosition = FormStartPosition.Manual;
            customerWindow.Location = new Point(100, 100);    
        }
        private int getInt(string d)
        {
            int i;
            try
            {
                i = int.Parse(d);
            }
            catch (Exception)
            {
                throw new BookShopException("String is not int");
            }
            return i;
        }
        private decimal getDecimal(string d)
        {
            decimal m;
            try
            {
                m = decimal.Parse(d);
            }
            catch (Exception)
            {
                throw new BookShopException("String is not decimal");
            }
            return m;
        }
 
        private void bnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void initial_Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BookStore Data Files|*.bdf";
            openFileDialog.InitialDirectory = Application.StartupPath;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TextReader trs = new StreamReader(openFileDialog.FileName);
                string s;
                List<string> words;
                int stringIndex;

                while (((s = trs.ReadLine()) != null) && (s != ""))
                {
                    words = new List<string>();
                    while (true)
                    {
                        if ((stringIndex = s.IndexOf('#')) == -1) break;
                        words.Add(s.Substring(0, stringIndex).Trim());
                        s = s.Substring(stringIndex + 1);
                    }
                    if (words.Count > 0)
                    {
                        try
                        { // to catprue an exception from getInt( ) getDecimal
                            switch (words[0])
                            {
                                case "AddBook":
                                    decimal price = getDecimal(words[5]);
                                    int stock = getInt(words[7]);
                                    // XXX use words[1]~words[4], price, words[6], and stock to register a book
                                    _control.AddBook(words[1], words[2], words[3], words[4], words[6], price, stock);

                                    break;
                                case "AddCustomer":
                                    // XXX use words[1]~words[7] to register a customer
                                    _control.RegisterCustomer(words[2], words[1], words[3], words[4], words[5], words[6], words[7]);

                                    break;
                                case "Login":
                                    // XXX use words[1] and words[2] to login a customer
                                    _control.LoginCustomer(words[1], words[2]);
                                    customerWindow.UpdateCustomerText();

                                    break;
                                case "AddBookToWishList":
                                    // XXX use words[1] (ISBN) to register the book in the current customer's wishlist
                             
                                   _control.AddBookToWishListByISBN(words[1]);

                                    break;
                                case "AddBookToCart":
                                    // XXX use words[1] (ISBN) to add the book in the current customer's cart
                                    _control.AddBookToCartByISBN(words[1]);

                                    break;
                                case "CheckOut":
                                    // XXX check out the current customer's cart
                                    _control.CheckOutCurrentCustomer();
                                    break;
                                case "ProcessPendingTransaction":
                                    // XXX use words[1] (index of the pending transactions) to identify the pending
                                    // transaction to approve
                                    _control.ProcessPendingTransaction(Convert.ToInt32(words[1]));
                                    break;
                                default:
                                    MessageBox.Show(this, "Unknown Operation : " + words[0]);
                                    break;
                            }
                        }
                        catch (BookShopException ex)
                        {
                            MessageBox.Show(this, ex.ErrorMessage);
                        }
                    }
                }
                
            }
        }

        void dispatchWindows()
        {
            staffWindow.Show();
            customerWindow.Show();
        }
        private void bnLoadAndStart_Click(object sender, EventArgs e)
        {
            initial_Load();
            dispatchWindows();
        }

        private void bnStartWithoutLoad_Click(object sender, EventArgs e)
        {
            dispatchWindows();
        }
    }
}
