using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using Entity_Framework.Database_Model;

namespace Entity_Framework
{
    public partial class Main_Form : Form
    {
        /////////////////////////////////////////////////////////// Variable
        private string file_loc="";
        private int file_option=0;


        private List<Product> list = new List<Product>();
        /////////////////////////////////////////////////////////// Constructor
        public Main_Form()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        /////////////////////////////////////////////////////////// Functions
        private void thread_openFileDialog1()
        {
            openFileDialog1.ShowDialog();
        }
        private void thread_saveFileDialog1()
        {
            saveFileDialog1.ShowDialog();
        }
        private void openfiledialog_action()
        {
            try
            {
                openFileDialog1.AddExtension = true;
                openFileDialog1.Title = "Select CSV";
                openFileDialog1.Filter = "csv files (*.csv)|*.csv";
                openFileDialog1.Multiselect = false;
                openFileDialog1.ReadOnlyChecked = true;
                openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
                Thread thread1 = new Thread(thread_openFileDialog1);  //create new thread
                thread1.SetApartmentState(ApartmentState.STA); //Debug Error
                thread1.Start(); 
                thread1.Join(); 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Error ! Cannot open filedialog !");
            }
        }
        private void print_in_console()
        {
            try
            {
                Console.WriteLine("===========================================================================================================");
                Console.WriteLine("Index  ID             \tName                \t\t\tQuantity       \tPrice          \tGroup          ");
                for (int p = 0; p < list.Count; p++)
                {
                    int chineseCharacters = new Regex(@"[^\u4e00-\u9fa5]").Replace(list[p].name, "").Length;
                    string align_name = list[p].name.PadRight(35 - chineseCharacters);
                    Console.WriteLine(
                        $"[{p.ToString().PadLeft(3)}]  {list[p].id.PadRight(15)}\t{align_name.PadRight(20)}\t{list[p].quantity.ToString().PadRight(15)}\t{list[p].price.ToString().PadRight(15)}\t{list[p].groups.PadRight(15)}"
                        );
                };
                Console.WriteLine("===========================================================================================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Print Error !");
                MessageBox.Show("Print Error !");
                Console.WriteLine(ex);
            }
        }
        private void reload_dataGridView1()
        {
            try
            {
                Table1 table1;
                Model1 model1 = new Model1();
                Console.WriteLine("Importing data to database...");

                for (int i = 0; i < list.Count; i++)
                {
                    table1 = new Table1() { //import to database
                        PID=list[i].id,
                        Name=list[i].name,
                        Quantity=list[i].quantity,
                        Price=list[i].price,
                        Groups=list[i].groups
                    };
                    model1.Table1.Add(table1);
                }
                model1.SaveChanges(); //save to database
                Console.WriteLine("Data has been imported to database !");
                //--------------------------------- 
                dataGridView1.Rows.Clear(); //clear rows
                var list2 = model1.Table1.ToList(); //get from database

                Task load=Task.Run(() => {
                    try
                    {
                        Console.WriteLine("Converting to dataGridView from database...");
                        int k = -1;
                        foreach (Table1 i in list2)
                        {
                            k++;
                            dataGridView1.Rows.Add(k.ToString(), i.PID.ToString(), i.Name, i.Quantity.ToString(), i.Price.ToString(), i.Groups);
                        }
                        Console.WriteLine("Conversion Success !");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error ! Conversion Failed !");
                        MessageBox.Show("Error! Conversion Failed!");
                        Console.WriteLine(ex.Message);
                    }
                }); 


            }
            catch (Exception ex)
            {
                Console.WriteLine("Interface Reload Error !");
                MessageBox.Show("Interface Reload Error !");
                Console.WriteLine(ex);
            }
        }
        private void linq_label()
        {
            try
            {
                double expen_price = (from x in list select x.price).Max();
                double cheap_price = (from x in list select x.price).Min();
                List<Product> expen_name = (from x in list where (x.price == expen_price) select x).ToList();
                List<Product> cheap_name = (from x in list where (x.price == cheap_price) select x).ToList();
                int sum_quantity = (from x in list select x.quantity).Sum();
                double average_quantity = (from x in list select x.quantity).Average();
                double average_price = (from x in list select x.price).Average();

                label2.Text = $"最貴商品 : {expen_name[0].name}";
                label3.Text = $"最便宜商品 : {cheap_name[0].name}";
                label4.Text = $"最貴價格 : {expen_price}";
                label5.Text = $"最便宜價格 : {cheap_price}";
                label6.Text = String.Format("平均價格 : {0:0.00}", average_price);
                label7.Text = String.Format("平均數量 : {0:0.00}", average_quantity);
                label8.Text = $"總數量 : {sum_quantity}";
            }
            catch(Exception ex)
            {
                Console.WriteLine("Data not yet been loaded !");
                MessageBox.Show("Data not yet been loaded !");
                Console.WriteLine(ex.Message);
            }
        }
        ///////////////////////////////////////////////////////////   Event Subscriber
        private void openToolStripMenuItem_Click(object sender, EventArgs e) //open
        {
            file_option = 1;
            openfiledialog_action();
        }

        private void appendToolStripMenuItem_Click(object sender, EventArgs e) //append
        {
            file_option = 2;
            openfiledialog_action();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) //save
        {
            try
            {
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
                saveFileDialog1.Title = "Save CSV";
                saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
                Thread thread2 = new Thread(thread_saveFileDialog1);  //create new thread
                thread2.SetApartmentState(ApartmentState.STA); //Debug Error
                thread2.Start(); 
                thread2.Join(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            file_loc= openFileDialog1.FileName;
            openFileDialog1.Dispose(); //release memory

            switch (file_option)
            {
                case 1: //open
                    list.Clear();
                    goto label1;
                case 2: //append
                    label1:
                    try
                    {
                        StreamReader reader = new StreamReader(file_loc);
                        try
                        {
                            string first_line = reader.ReadLine();
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                string[] values = line.Split(',');

                                list.Add(new Product(values[0].ToString(), values[1].ToString(), values[2].ToString(), values[3].ToString(), values[4].ToString()));
                            }
                            Console.WriteLine("File loaded !");
                            MessageBox.Show("File loaded!");
                            print_in_console();
                            reload_dataGridView1();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error ! Cannot read the file !");
                            Console.WriteLine("Error ! Cannot read the file !");
                            Console.WriteLine(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error ! Cannot read the file !");
                        Console.WriteLine("Error ! File doesnt exist !");
                        Console.WriteLine(ex);
                    }
                    break;

            }

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {

                file_loc = saveFileDialog1.FileName;
                saveFileDialog1.Dispose(); //release memory

                string saves_str = "";
                saves_str += "商品編號,商品名稱,商品數量,價格,商品類別\n";
                for (int i = 0; i < list.Count; i++)
                {
                    saves_str += $"{list[i].id},{list[i].name},{list[i].quantity},{list[i].price},{list[i].groups}\n";
                }

                FileInfo saves = new FileInfo(file_loc);
                using (StreamWriter fs = saves.AppendText())
                {
                    fs.Write(saves_str);
                }

                Console.WriteLine("File saved !");
                MessageBox.Show("File saved !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ! Cannot save the file !");
                Console.WriteLine(ex.Message);
                MessageBox.Show("Error ! Cannot save the file !");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string input = textBox1.Text;            // _ to space
                string[] input2 = input.Split(' ');
                Console.WriteLine($">> {input}");

                for (int i = 0; i < input2.Length; i++)
                {
                    string temp_str = input2[i];
                    for (int j = 0; j < temp_str.Length; j++)
                    {
                        if (temp_str[j].ToString() == "_")
                        {
                            temp_str = temp_str.Remove(j, 1);
                            temp_str = temp_str.Insert(j, " ");
                        }
                    }
                    input2[i] = temp_str;
                }


                if (input2[0].ToString() == "add")
                {
                    list.Add(new Product(input2[1].ToString(), input2[2].ToString(), input2[3].ToString(), input2[4].ToString(), input2[5].ToString()));
                    Console.WriteLine("Item added !");
                    MessageBox.Show("Item added !");
                }
                else if (input2[0].ToString() == "insert")
                {
                    list.Insert(int.Parse(input2[1]), new Product(input2[2].ToString(), input2[3].ToString(), input2[4].ToString(), input2[5].ToString(), input2[6].ToString()));
                    Console.WriteLine("Item inserted !");
                    MessageBox.Show("Item inserted !");
                }
                else if (input2[0].ToString() == "remove")
                {
                    list.RemoveAt(int.Parse(input2[1]));
                    Console.WriteLine("Item removed !");
                    MessageBox.Show("Item removed !");
                }
                else if(input2[0].ToString() == "edit")
                {
                    int indexer=int.Parse(input2[1]);
                    list[indexer].id = input2[2].ToString();
                    list[indexer].name= input2[3].ToString();
                    list[indexer].quantity= int.Parse(input2[4]);
                    list[indexer].price= double.Parse(input2[5]);
                    list[indexer].groups = input2[6];
                    Console.WriteLine("Item edited !");
                    MessageBox.Show("Item edited !");
                }
                else
                {
                    MessageBox.Show("Input Error !");
                    Console.WriteLine("Input Error !");
                }
                textBox1.Text = "";
                print_in_console();
                reload_dataGridView1();
                linq_label();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Input Error !");
                Console.WriteLine("Input Error !");
                Console.WriteLine(ex.Message);
                textBox1.Text = "";
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            print_in_console();
            reload_dataGridView1();
            linq_label();
        }
    }
}
