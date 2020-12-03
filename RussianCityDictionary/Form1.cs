using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RussianCityDictionary
{
    public partial class Form1 : Form
    {
        DataSet dataSet;
        MySqlDataAdapter adapter;
        string connectionstr ="server=localhost;port=3306;username=root;password=somepass1234;database=russianscitydb";
        MySqlCommandBuilder commandBuilder;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dbload();
            //// TODO: данная строка кода позволяет загрузить данные в таблицу "russianscitydbDataSet.geo_district". При необходимости она может быть перемещена или удалена.
            ////this.geo_districtTableAdapter.Fill(this.russianscitydbDataSet.geo_district);
            //MySqlCommand command = new MySqlCommand();
            //command.Connection = db.connector;
            //command.CommandText = "SELECT * FROM russianscitydb.geo_city;";
            //MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            //// Создаем объект Dataset
            //DataSet ds = new DataSet();
            //// Заполняем Dataset
            //adapter.Fill(ds,"Cities");
            //dataGridView1.DataSource = ds.Tables["Cities"].DefaultView;

            dataGridView3.Columns[0].HeaderText = "Id";
            dataGridView3.Columns[1].HeaderText = "Id Региона";
            dataGridView3.Columns[2].HeaderText = "Город";

            //dataGridView1.Columns[0].Visible = false;
        }

        private void Dbload()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstr))
            {
                adapter = new MySqlDataAdapter("SELECT * FROM russianscitydb.geo_city;", connection);
                dataSet = new DataSet();
                adapter.Fill(dataSet, "russianscitydb.geo_city");
                dataGridView3.DataSource = dataSet.Tables["russianscitydb.geo_city"];
                adapter.SelectCommand = new MySqlCommand("SELECT * FROM russianscitydb.geo_district;", connection);
                adapter.Fill(dataSet, "geo_district");
                adapter.SelectCommand = new MySqlCommand("SELECT * FROM russianscitydb.geo_regions;", connection);
                adapter.Fill(dataSet, "geo_regions");
                dataGridView1.DataSource = dataSet.Tables[1];
                dataGridView2.DataSource = dataSet.Tables[2];
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstr))
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter(
                    $"SELECT * FROM russianscitydb.geo_regions as rg where rg.district_id ={dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()}", 
                    connection);
                adapter.Fill(ds);
                dataGridView2.DataSource = ds.Tables[0];

            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstr))
            {
                DataSet ds = new DataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter(
                    $"SELECT * FROM russianscitydb.geo_city as ct where ct.region_id ={dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString()}",
                    connection);
                adapter.Fill(ds);
                dataGridView3.DataSource = ds.Tables[0];

            }
        }

        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Process.Start($"https://www.google.com/maps/place/"+dataGridView3.Rows[e.RowIndex].Cells[2].Value);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter =
        String.Format("name like '{0}%'", textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dbload();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter =
            String.Format("name like '{0}%'",textBox2.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            (dataGridView3.DataSource as DataTable).DefaultView.RowFilter =
            String.Format("name like '{0}%'", textBox3.Text);
        }
    }
}
