using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiyazAkhmetvaliev421.Components;

namespace DiyazAkhmetvaliev421.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListPage.xaml
    /// </summary>
    public partial class ListPage : Page
    {
        public ListPage()
        {
            InitializeComponent();
            List<ProductType> types = App.db.ProductType.OrderBy(x => x.Title).ToList();
            types = types.Prepend(new ProductType() { Title = "Все типы" }).ToList();
            FilterCbx.ItemsSource = types;
            FilterCbx.DisplayMemberPath = "Name";
            FilterCbx.SelectedIndex = 0;
            Refresh();
        }
        public void Refresh()
        {
            ProductWP.Children.Clear();
            PagesSP.Children.Clear();
            List<Product> prodList = App.db.Product.ToList();

            switch (SortCbx.SelectedIndex)
            {
                case 0:
                    prodList = prodList.OrderBy(x => x.Title).ToList();
                    break;

                case 1:
                    prodList = prodList.OrderByDescending(x => x.Title).ToList();
                    break;

                case 2:
                    prodList = prodList.OrderBy(x => x.ProductionWorkshopNumber).ToList();
                    break;

                case 3:
                    prodList = prodList.OrderByDescending(x => x.ProductionWorkshopNumber).ToList();
                    break;

                case 4:
                    prodList = prodList.OrderBy(x => x.MinCostForAgent).ToList();
                    break;

                case 5:
                    prodList = prodList.OrderByDescending(x => x.MinCostForAgent).ToList();
                    break;
            }

            if (FilterCbx.SelectedIndex > 0)
            {
                string type = (FilterCbx.Items[FilterCbx.SelectedIndex] as ProductType).Title;
                prodList = prodList.Where(x => x.ProductType.Title == type).ToList();
            }

            if (SearchTbx.Text != "")
            {
                prodList = prodList.Where(x => x.Title.ToLower().Contains(SearchTbx.Text.ToLower())).ToList();
            }

            if (prodList.Count > 20)
            {
                int count = prodList.Count / 20;
                if (prodList.Count % 20 != 0) count++;
                for (int i = 0; i < count; i++)
                {
                    Button button = new Button() { Content = (i + 1).ToString(), Height = 40, Width = 40 };
                    button.Click += PagesButt_Click;
                    PagesSP.Children.Add(button);
                }
                int pagecount = int.Parse(PageTb.Text) - 1;
                try
                {
                    prodList = prodList.GetRange(pagecount * 20, 20);
                }
                catch
                {
                    prodList = prodList.Skip(pagecount * 20).ToList();
                }

            }
            foreach (Product product in prodList)
            {
                ProductUC prouc = new ProductUC(product);
                ProductWP.Children.Add(prouc);
            }
        }

        private void PagesButt_Click(object sender, RoutedEventArgs e)
        {
            PageTb.Text = (sender as Button).Content.ToString();
            Refresh();
        }

        private void ForwardButt_Click(object sender, RoutedEventArgs e)
        {
            int pagecount = int.Parse(PageTb.Text);
            if (ProductWP.Children.Count < 20) MessageBox.Show("(((");
            else if (App.db.Product.Count() - (pagecount) * 20 <= 0) MessageBox.Show("(((");
            else
            {
                PageTb.Text = (pagecount + 1).ToString();
                Refresh();
            }
        }

        private void BackButt_Click(object sender, RoutedEventArgs e)
        {
            int pagecount = int.Parse(PageTb.Text);
            if (pagecount < 2) MessageBox.Show("((");
            else
            {
                PageTb.Text = (pagecount - 1).ToString();
                Refresh();
            }
        }

        private void SortCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageTb.Text = "1";
            Refresh();
        }

        private void FilterCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageTb.Text = "1";
            Refresh();
        }

        private void SearchTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            PageTb.Text = "1";
            Refresh();
        }
    }
}
