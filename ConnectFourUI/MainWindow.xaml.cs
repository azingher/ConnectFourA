/*=============================================================================
 * 
 * 
 *  Copyright Ayal Zingher 2016
 * 
 * 
 * 
 * ============================================================================*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

namespace ConnectFourUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string message = "";
        ConnectFourVM myViewModel = new ConnectFourVM();
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = myViewModel;
            myViewModel.UpdateView();
            GetTokens();
        }

        private void btnA_Click(object sender, RoutedEventArgs e)
        {
            if (!myViewModel.AddTile(1, out message))
                MessageBox.Show(message);
            GetTokens();
        }

        private void btnB_Click(object sender, RoutedEventArgs e)
        {
            if (!myViewModel.AddTile(2, out message))
                MessageBox.Show(message);
            GetTokens();
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            if (!myViewModel.AddTile(3, out message))
                MessageBox.Show(message);
            GetTokens();
        }

        private void btnD_Click(object sender, RoutedEventArgs e)
        {
            if (!myViewModel.AddTile(4, out message))
                MessageBox.Show(message);
            GetTokens();
        }

        private void btnE_Click(object sender, RoutedEventArgs e)
        {
            if (!myViewModel.AddTile(5, out message))
                MessageBox.Show(message);
            GetTokens();
        }

        private void btnF_Click(object sender, RoutedEventArgs e)
        {
            if (!myViewModel.AddTile(6, out message))
                MessageBox.Show(message);
            GetTokens();
        }

        private void btnH_Click(object sender, RoutedEventArgs e)
        {
            if (!myViewModel.AddTile(7, out message))
                MessageBox.Show(message);
            GetTokens();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!myViewModel.ResetGame(out message))
            {
                MessageBox.Show(message);
            }
            GetTokens();
        }

        private void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            return;
        }
        private void GetTokens()
        {
            UserControl bt;
            DataTable dt = myViewModel.MyBoard;
            PlayGrid.Children.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].ToString() == "X")
                    {
                        bt = new BlackToken();
                        Grid.SetColumn(bt, j);
                        Grid.SetRow(bt, i);
                        PlayGrid.Children.Add(bt);
                    }
                    else if (dt.Rows[i][j].ToString() == "0")
                    {
                        bt = new RedToken();
                        Grid.SetColumn(bt, j);
                        Grid.SetRow(bt, i);
                        PlayGrid.Children.Add(bt);
                    }
                    else if (dt.Rows[i][j].ToString() == " ")
                    {
                        bt = new BlueToken();
                        Grid.SetColumn(bt, j);
                        Grid.SetRow(bt, i);
                        PlayGrid.Children.Add(bt);
                    }
                    else
                    {
                        Debug.WriteLine("Received a weird value");
                        MessageBox.Show("Received an unexpected value");
                    }
                }
        }
    }
}
