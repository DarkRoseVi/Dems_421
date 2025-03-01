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

namespace DiyazAkhmetvaliev421.Components
{
    /// <summary>
    /// Логика взаимодействия для ProductUC.xaml
    /// </summary>
    public partial class ProductUC : UserControl
    {
        public Product product;
        public ProductUC(Product product)
        {
            InitializeComponent();
            this.product = product;
            NameTb.Text = product.ProductType.Title + " | " + product.Title;
            ArticuleTb.Text = product.ArticleNumber.ToString();

            List<string> materials = new List<string>();
            decimal price = 0;
            foreach (ProductMaterial pm in App.db.ProductMaterial.Where(x => x.ProductID == product.ID).ToList())
            {
                materials.Add(pm.Material.Title);
                price += (decimal)pm.Material.Cost * (decimal)pm.Count;
            }
            MaterialsTb.Text = String.Join(", ", materials);
            PriceTB.Text = price.ToString();
        }

    }
}