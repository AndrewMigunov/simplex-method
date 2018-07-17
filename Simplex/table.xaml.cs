using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Simplex
{
    /// <summary>
    /// Логика взаимодействия для table.xaml
    /// </summary>
    public partial class table : Window
    {
        private TextBox[,] matrixBoxs;
        private TextBox[] freeColumnBoxs;
        private TextBox[] intFunctionBoxs;
        private ComboBox[] inequalitiesComboBoxs;
        //private TextBlock[,] matrixBlocks;
        public table(int m,int n)
        {
            InitializeComponent();
            matrixBoxs = new TextBox[m, n];
            freeColumnBoxs = new TextBox[m];
            intFunctionBoxs = new TextBox[n];
            inequalitiesComboBoxs = new ComboBox[m];
            
            for(int i = 0; i < m; i++)
            {
                inequalitiesComboBoxs[i] = new ComboBox() {FontSize=30 };
                inequalitiesComboBoxs[i].Items.Add(new ComboBoxItem() {Name="less",Content="<=",FontSize=30 });
                inequalitiesComboBoxs[i].Items.Add(new ComboBoxItem() { Name = "greater", Content = ">=", FontSize = 30 }); 
            }

            for (int i = 0; i < n + 2; i++)
            {
                intFun.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }
            RowDefinitionCollection row=SystemTable.RowDefinitions;
            for(int i = 0; i < m; i++)
            {
                row.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }
            ColumnDefinitionCollection column = SystemTable.ColumnDefinitions;
           
            for (int i = 0; i < n+2; i++)
            {
                column.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }
            for(int i = 0; i < m; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    matrixBoxs[i, j] = new TextBox() { Name = $"mat{i}{j}",Text="",FontSize=30 };
                    SystemTable.Children.Add(matrixBoxs[i, j]);
                    Grid.SetRow(matrixBoxs[i, j], i);
                    Grid.SetColumn(matrixBoxs[i, j], j);
                }
            }
            for (int i = 0; i < m; i++)
            {
                SystemTable.Children.Add(inequalitiesComboBoxs[i]);
                Grid.SetColumn(inequalitiesComboBoxs[i], n);
                Grid.SetRow(inequalitiesComboBoxs[i], i);
            }
            for (int i = 0; i < m; i++)
            {
                freeColumnBoxs[i]= new TextBox() { Name = $"freeCol{i}", Text ="", FontSize = 30 };
                SystemTable.Children.Add(freeColumnBoxs[i]);
                Grid.SetColumn(freeColumnBoxs[i], n + 1);
                Grid.SetRow(freeColumnBoxs[i], i);
            }
            for (int i = 0; i < n; i++)
            {
                intFunctionBoxs[i] = new TextBox() { Name = $"intFunc{i}", Text ="", FontSize = 30 };
                intFun.Children.Add(intFunctionBoxs[i]);
                Grid.SetColumn(intFunctionBoxs[i], i + 1);
                Grid.SetRow(intFunctionBoxs[i], 0);
            }

        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {

            double[,] matrix;
            double[] freeColumn;
            double[] intFuncfion;
            bool[] isLess;
            try
            {
                matrix = new double[matrixBoxs.GetLength(0), matrixBoxs.GetLength(1)];
                for (int i = 0; i < matrixBoxs.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixBoxs.GetLength(1); j++)
                    {
                        
                        matrix[i, j] = Double.Parse(matrixBoxs[i,j].Text);
                    }
                }
                freeColumn = new double[freeColumnBoxs.Length];
                for (int i = 0; i < freeColumnBoxs.Length; i++)
                {
                    freeColumn[i] = Double.Parse(freeColumnBoxs[i].Text);
                }
                intFuncfion = new double[intFunctionBoxs.Length];
                for (int i = 0; i < intFunctionBoxs.Length;i++)
                {
                    intFuncfion[i] = Double.Parse(intFunctionBoxs[i].Text);
                }
                isLess = new bool[inequalitiesComboBoxs.Length];
                for(int i = 0; i < inequalitiesComboBoxs.Length; i++)
                {
                    if (inequalitiesComboBoxs[i].SelectedIndex == 0)
                    {
                        isLess[i] = true;
                    }
                    else
                    {
                        if (inequalitiesComboBoxs[i].SelectedIndex == 1)
                        {
                            isLess[i] = false;
                        }
                        else
                        {
                            throw new Exception("Выберете знак неравенства");
                        }
                    }
                }
                SimplexMethod a = new SimplexMethod(matrix, freeColumn, intFuncfion,isLess);
                double[] result = new double[intFunctionBoxs.Length];
                a.Calculate(result);
                Ok.FontSize=25;
                Ok.Content = "(";
                for (int i = 0; i < intFunctionBoxs.Length - 1; i++)
                {
                    Ok.Content += $"{Math.Round(result[i],3)};";
                }
                Ok.Content += $"{Math.Round(result[intFunctionBoxs.Length - 1],3)})";
                double sum = 0;
                for(int i = 0; i < intFuncfion.Length; i++)
                {
                    sum += intFuncfion[i] * result[i];
                }
                Ok.Content += $"  F(x)={Math.Round(sum,3)}";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }
    }
}
