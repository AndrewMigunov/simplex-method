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

namespace Simplex
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int n = Int32.Parse(N.Text), m = Int32.Parse(M.Text);

                table t = new table(m, n);
                t.ShowDialog();
            //this.Close();
            }
            catch 
            {
                MessageBox.Show("Вы ввели не целое число!");
                return;
            }
}
    }
}
