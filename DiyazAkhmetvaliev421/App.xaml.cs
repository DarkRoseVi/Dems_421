using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DiyazAkhmetvaliev421.Components;

namespace DiyazAkhmetvaliev421
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static proer32Entities db =  new proer32Entities();
    }
}
