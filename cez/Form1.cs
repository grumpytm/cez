using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace cez
{
    public partial class Form1 : Form
    {
        private FormManager _formManager;
        public Form1()
        {
            InitializeComponent();
            _formManager = new FormManager(this, FormType.MainMenu);
            this.Top = 0;
            this.Left = 0;
        }

        // public static Form1 _Form1;
        private void button1_Click(object sender, EventArgs e)
        {
            _formManager.ShowForm(FormType.Intrare);
            //Intrare f = new Intrare();
            //this.Hide();
            //f.ShowDialog();
            //this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _formManager.ShowForm(FormType.Retur);
        }
    }
}