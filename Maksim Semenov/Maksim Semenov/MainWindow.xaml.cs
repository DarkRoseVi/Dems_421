using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProductApp
{
    public partial class MainWindow : Window
    {
        string connectionString = "Server=LAPTOP-83OK110A;Database=demodemo;Integrated Security=True;";

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID, Title, MinCostForAgent FROM Product";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ID = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            MinCostForAgent = reader.GetDecimal(2)
                        });
                    }
                }
            }

            ProductListView.ItemsSource = products;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID, Title, MinCostForAgent FROM Product WHERE Title LIKE @search";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Product> products = new List<Product>();

                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ID = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                MinCostForAgent = reader.GetDecimal(2)
                            });
                        }

                        ProductListView.ItemsSource = products;
                    }
                }
            }
        }

        private void ChangePrice_Click(object sender, RoutedEventArgs e)
        {
            var selectedProducts = ProductListView.SelectedItems.Cast<Product>().ToList();
            if (!selectedProducts.Any())
            {
                MessageBox.Show("Выберите продукты для изменения цены.");
                return;
            }

            string input = Microsoft.VisualBasic.Interaction.InputBox("Введите новую цену:", "Изменение цены", selectedProducts.First().MinCostForAgent.ToString());
            if (decimal.TryParse(input, out decimal newPrice))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var product in selectedProducts)
                    {
                        string updateQuery = "UPDATE Product SET MinCostForAgent = @price WHERE ID = @id";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@price", newPrice);
                            cmd.Parameters.AddWithValue("@id", product.ID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                LoadData();
            }
            else
            {
                MessageBox.Show("Введите корректное число.");
            }
        }
    }

    public class Product
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal MinCostForAgent { get; set; }
    }
}
