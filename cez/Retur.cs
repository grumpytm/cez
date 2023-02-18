using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;

using RestSharp;

namespace cez
{

    public partial class Retur : Form
    {
        private FormManager _formManager;
        private int _selected;

        public Retur(FormManager formManager)
        {
            InitializeComponent();
            LoadOrders();
            _formManager = formManager;

            /* Submit on Enter key press */
            this.textBox1.KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
            this.textBox4.KeyPress += new KeyPressEventHandler(textBox4_KeyPress);
        }

        private void LoadOrders()
        {
            var myPara = new Barcode.myParameters();
            myPara.items = new List<Barcode.Item>
                {
                    new Barcode.Item { name = "file", value = "json/get/comenzi.json" }
                };

            var response = Barcode.fetchJson(myPara.items);
            var dict = Barcode.TestResponse(response) as Dictionary<string, string>;
            if (dict["error"].Equals("0"))
            {
                RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
                List<Barcode.Orders> x = deserial.Deserialize<List<Barcode.Orders>>(response);

                comboBox1.Items.Clear();
                for (int i = 0; i < x.Count; i++)
                {
                    comboBox1.Items.Add(x[i].Client + " - " + x[i].Produs);
                    SeriiSingleton.Instance[i] = x[i].Serie;
                }
                statusBar1.Text = string.Format("Incarcat {0} comenzi..", comboBox1.Items.Count);
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
                statusBar1.Text = dict["message"];
            }
        }

        /* Serii Singleton */
        public class SeriiSingleton
        {
            public static readonly Dictionary<int, string> Instance = new Dictionary<int, string>();
        }

        public static int emptyFields(Dictionary<string, string> dict)
        {
            int empty = 0;
            foreach (var item in dict.Values)
            {
                if (item.Equals(string.Empty))
                {
                    empty++;
                }
            }
            return empty;
        }

        /* Reset form */
        private void reset()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
        }

        /* Magazie */
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _selected = -1;
            statusBar1.Text = string.Empty;

            panel1.Visible = false;
            // panel2.Location = new Point(0,38);
            panel2.Show();
            reset();
            textBox1.Focus();
        }

        /* Return */
        private void button2_Click(object sender, EventArgs e)
        {
            _formManager.HideForm();
        }

        /* Selectie produs finit */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //panel2.Location = new Point(0, 70);
            panel2.Show();
            textBox1.Focus();
            reset();
            _selected = comboBox1.SelectedIndex;
        }

        /* One time barcode check */
        private void checkBarcode(string barcode)
        {
            var myPara = new Barcode.myParameters();
            myPara.items = new List<Barcode.Item>
                {
                    new Barcode.Item { name = "file", value = "scan/check/{barcode}" },
                    new Barcode.Item { name = "barcode", value = barcode }
                };
            
            statusBar1.Text = string.Empty;
            var response = Barcode.fetchJson(myPara.items);
            var dict = Barcode.TestResponse(response) as Dictionary<string, string>;
            if (dict["error"].Equals("0"))
            {

                RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
                Barcode.Check x = deserial.Deserialize<Barcode.Check>(response);

                /* show results */
                textBox2.Text = x.details.FirstOrDefault().articol;
                textBox3.Text = x.details.FirstOrDefault().producator;

                textBox4.Focus();
                // statusBar1.Text = "All good?";
            }
            else
            {
                textBox1.SelectAll();
                statusBar1.Text = dict["message"];
            }
        }

        /* Salvare scan */
        private void saveData(string barcode, string cant, string locatie)
        {
            var myPara = new Barcode.myParameters();
            myPara.items = new List<Barcode.Item>
                {
                    new Barcode.Item { name = "file", value = "scan/add/{barcode}/{cant}/{locatie}/{actiune}" },
                    new Barcode.Item { name = "barcode", value = barcode },
                    new Barcode.Item { name = "cant", value = cant },
                    new Barcode.Item { name = "locatie", value = locatie},
                    new Barcode.Item { name = "actiune", value = "2" }
                };

            statusBar1.Text = string.Empty;
            var response = Barcode.fetchJson(myPara.items);
            var dict = Barcode.TestResponse(response) as Dictionary<string, string>;
            if (dict["error"].Equals("0"))
            {
                RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
                Barcode.Save x = deserial.Deserialize<Barcode.Save>(response);

                /* show results */
                reset();
                textBox1.Focus();
                statusBar1.Text = "Salvat..";
            }
            else
            {
                textBox1.SelectAll();
                statusBar1.Text = dict["message"];
            }
        }

        /* Scanare barcode */
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string barcode = textBox1.Text.ToString();
                if (barcode.Length >= 3)
                    checkBarcode(barcode);
                else
                    return;
                e.Handled = true;
            }
        }

        /* Salvare scanare */
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                // string barcode = textBox1.Text.ToString();

                var dict = new Dictionary<string, string>();
                dict.Add("1", textBox2.Text.ToString());
                dict.Add("2", textBox3.Text.ToString());
                dict.Add("3", textBox4.Text.ToString());

                if (emptyFields(dict) == 0)
                {
                    string barcode = textBox1.Text;
                    string cant = textBox4.Text;

                    if (_selected > -1)
                    {
                        statusBar1.Text = "Salvez " + SeriiSingleton.Instance[_selected] + "...";
                        saveData(barcode, cant, SeriiSingleton.Instance[_selected]) ;
                    }
                    else
                    {
                        statusBar1.Text = "Salvez spre magazie..";
                        saveData(barcode, cant, "0");
                    }
                }
                else
                {
                    statusBar1.Text = string.Format("Ai {0} campuri goale..", emptyFields(dict));
                }
            }

            if (e.KeyChar == (char)Keys.Escape)
            {
                // statusBar1.Text = "escape?";
                reset();
                textBox1.Focus();
                // SoundPlayer sound = new SoundPlayer(@"Woosh.wav");
                // sound.Play();
            }
        }
    }
}