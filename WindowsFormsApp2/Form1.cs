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
            LoadProducts(); // Загружаем данные при запуске формы
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

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            textBox7.Text = row.Cells["product_id"].Value.ToString();
            textBox1.Text = row.Cells["name"].Value.ToString();
            textBox2.Text = row.Cells["brand"].Value.ToString();
            textBox3.Text = row.Cells["category"].Value.ToString();
            textBox4.Text = row.Cells["quantity"].Value.ToString();
            textBox5.Text = row.Cells["price"].Value.ToString();

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
    }
}
