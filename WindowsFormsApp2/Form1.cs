using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Npgsql;
using System.Drawing;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        // Строка подключения к базе данных
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=drimik;Database=pract";
        DataTable products;
        public Form1()
        {
            InitializeComponent();
            LoadProducts();
            LoadStores();
            LoadCont();
            LoadEmp();
        }

        private void LoadProducts()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query2 = "SELECT product_id, name, brand, category, price, quantity FROM products";


                    using (var adapter = new NpgsqlDataAdapter(query2, connection))
                    {
                        DataTable dt2 = new DataTable();
                        adapter.Fill(dt2);
                        dataGridView2.DataSource = dt2;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения: " + ex.Message);
                }
            }
        }
        private void LoadCont()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query2 = "SELECT * FROM contractors";


                    using (var adapter = new NpgsqlDataAdapter(query2, connection))
                    {
                        DataTable dt2 = new DataTable();
                        adapter.Fill(dt2);
                        dataGridView3.DataSource = dt2;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения: " + ex.Message);
                }
            }
        }
        private void LoadEmp()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query2 = "SELECT e.employee_id, e.full_name, e.position, s.name AS store_name FROM employees e JOIN stores s ON e.store_id = s.store_id";


                    using (var adapter = new NpgsqlDataAdapter(query2, connection))
                    {
                        DataTable dt2 = new DataTable();
                        adapter.Fill(dt2);
                        dataGridView4.DataSource = dt2;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения: " + ex.Message);
                }
            }
        }
        private void LoadStores()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query2 = "SELECT * FROM stores";


                    using (var adapter = new NpgsqlDataAdapter(query2, connection))
                    {
                        DataTable dt2 = new DataTable();
                        adapter.Fill(dt2);
                        dataGridView1.DataSource = dt2;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения: " + ex.Message);
                }
            }
        }

        private void LoadProductsToGrid()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM products";
                using (var adapter = new NpgsqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
            }
        }

        private void LoadStoresToGrid()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM stores";
                using (var adapter = new NpgsqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void LoadContToGrid()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM contractors";
                using (var adapter = new NpgsqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView3.DataSource = dt;
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            textBox6.Text = row.Cells["product_id"].Value.ToString();
            textBox12.Text = row.Cells["name"].Value.ToString();
            textBox11.Text = row.Cells["brand"].Value.ToString();
            textBox10.Text = row.Cells["category"].Value.ToString();
            textBox9.Text = row.Cells["quantity"].Value.ToString();
            textBox8.Text = row.Cells["price"].Value.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Добавляем новую запись
                    string insertQuery = "INSERT INTO products (name, brand, category, photo, price, quantity) VALUES (@name, @brand, @category, @photo, @price, @quantity)";
                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBox1.Text);
                        cmd.Parameters.AddWithValue("@brand", textBox2.Text);
                        cmd.Parameters.AddWithValue("@category", textBox3.Text);
                        cmd.Parameters.AddWithValue("@photo", "dffdfdf");
                        cmd.Parameters.AddWithValue("@price", decimal.Parse(textBox5.Text));
                        cmd.Parameters.AddWithValue("@quantity", int.Parse(textBox4.Text));
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запись добавлена");

                    // Обновляем таблицу
                    LoadProductsToGrid(); // Метод, который снова загружает данные в dataGridView2
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                // Получаем строку, в которой находится выбранная ячейка
                int rowIndex = dataGridView2.SelectedCells[0].RowIndex;

                // Получаем значение product_id из этой строки
                DataGridViewRow selectedRow = dataGridView2.Rows[rowIndex];

                if (selectedRow.Cells["product_id"].Value != null)
                {
                    int productId = Convert.ToInt32(selectedRow.Cells["product_id"].Value);

                    using (var conn = new NpgsqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            string deleteQuery = "DELETE FROM products WHERE product_id = @id";

                            using (var cmd = new NpgsqlCommand(deleteQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", productId);
                                cmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Запись удалена.");

                            // Обновляем таблицу
                            LoadProductsToGrid(); // метод, который загружает таблицу в dataGridView2
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при удалении: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выбранная строка не содержит допустимого product_id.");
                }
            }
            else
            {
                MessageBox.Show("Выберите ячейку для удаления записи.");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string insertQuery = "INSERT INTO stores (name, address) VALUES (@name, @address)";
                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBox14.Text);
                        cmd.Parameters.AddWithValue("@address", textBox15.Text);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Магазин добавлен!");
                    LoadStoresToGrid(); // Обновляем dataGridView1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string insertQuery = "INSERT INTO contractors (name, contact_info) VALUES (@name, @contact_info)";
                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBox18.Text);
                        cmd.Parameters.AddWithValue("@contact_info", textBox16.Text);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Магазин добавлен!");
                    LoadContToGrid(); // Обновляем dataGridView1
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string insertQuery = "INSERT INTO employees (full_name, position, store_id) VALUES (@full_name, @position, @store_id)";
                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@full_name", textBox21.Text);
                        cmd.Parameters.AddWithValue("@position", textBox19.Text);
                        cmd.Parameters.AddWithValue("@store_id", Convert.ToInt32(textBox22.Text));
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Магазин добавлен!");
                    LoadEmp();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                }
            }
        }
    }
}
