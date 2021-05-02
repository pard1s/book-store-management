using System;
using System.Collections.Generic;
using System.Data.SQLite;
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


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ShowBooks.xaml
    /// </summary>
    

    public partial class ShowBooks : Window
    {
        SQLiteConnection dbConnection;
        List<int> ids;
        public ShowBooks()
        {
            InitializeComponent();
            initDB();
            initShowing();

        }

        private void initShowing()
        {
            //در این قسمت فرمان کوئیری به دیتابیس فرستاده می شود تا اطلاعات کتاب ها را از دیتابیس بگیرد
            string sql = "SELECT * FROM books";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            
            SQLiteDataReader reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                int n = 0;
                while (reader.Read())
                {
                    /// در این مرحله تک تک نتیجه های کوئیری در داخل یک اسکرول ویوو در داخل یک بوردر نمایش داده می شود
                    /// که ساختار گرافیکی این بوردر به صورت زیر تعریف شده است
                    
                    String textt1 = reader["bookName"].ToString();
                    String textt2 = String.Format("نویسنده: {0}، ناشر: {1}، سال انتشار: {2} قیمت: {3} تومان", reader["author"]+"", reader["publisher"], reader["publishDate"], reader["price"]);
                    RowDefinition row = new RowDefinition
                    {
                        Height = new GridLength(90)
                    };

                    Border newBoarder = new Border
                    {
                        Background = (Brush)new BrushConverter().ConvertFrom("#424242"),
                        Opacity = 0.7,
                        Width = 750,
                        Height = 70,
                        CornerRadius = new CornerRadius(10),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Cursor = Cursors.Hand,
                        Tag = reader["barcode"].ToString()

                };

                    Grid grid1 = new Grid();
                    newBoarder.Child = grid1;

                    ColumnDefinition col1 = new ColumnDefinition
                    {
                        Width = new GridLength(50)
                    };
                    ColumnDefinition col2 = new ColumnDefinition
                    {
                        Width = new GridLength(50)
                    };
                    ColumnDefinition col3 = new ColumnDefinition();

                    grid1.ColumnDefinitions.Add(col1);
                    grid1.ColumnDefinitions.Add(col2);
                    grid1.ColumnDefinitions.Add(col3);

                    Image img1 = new Image
                    {
                        Source = new BitmapImage(new Uri(@"/img/ed.png", UriKind.Relative)),
                        Width = 32

                    };
                    Image img2 = new Image
                    {
                        Source = new BitmapImage(new Uri(@"/img/del.png", UriKind.Relative)),
                        Width = 32

                    };
                    grid1.Children.Add(img1);
                    grid1.Children.Add(img2);

                    Grid.SetColumn(img1, 0);
                    Grid.SetColumn(img2, 1);

                    /////////////////////////////////////////////////////////

                    newBoarder.MouseUp += new MouseButtonEventHandler(showBookIcon);

                    ////////////////////////////////////////////////////////

                    Grid grid2 = new Grid();
                    grid1.Children.Add(grid2);
                    Grid.SetColumn(grid2, 2);


                    RowDefinition row2 = new RowDefinition();
                    RowDefinition row3 = new RowDefinition();
                    grid2.RowDefinitions.Add(row2);
                    grid2.RowDefinitions.Add(row3);

                    TextBlock txt1 = new TextBlock
                    {
                        Text = textt1,
                        FontSize = 18,
                        Foreground = Brushes.White,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#IRANSANS"),
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 10, 20, 0)

                    };

                    TextBlock txt2 = new TextBlock
                    {
                        Text = textt2,
                        FontSize = 13,
                        Foreground = Brushes.White,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#IRANSANS"),
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 20, 0)

                    };

                    grid2.Children.Add(txt1);
                    grid2.Children.Add(txt2);

                    Grid.SetRow(txt1, 0);
                    Grid.SetRow(txt2, 1);

                    ////////////////////////////////////////////////////////
                    bookListGrid.RowDefinitions.Add(row);
                    bookListGrid.Children.Add(newBoarder);
                    Grid.SetRow(newBoarder, bookListGrid.RowDefinitions.Count - 1);
                    bookListScroll.Height = 600;

                    n++;
                }
                
                
            } else
            {
                /// وقتی کتابی جهت نمایش موجود نباشد این پیام را نشان خواهد داد
                
                listStackPanel.Visibility = Visibility.Hidden;
                noBookFoundText.Visibility = Visibility.Visible;
            }
        }

        private void showBookIcon(object sender, MouseButtonEventArgs e)
        {
            /// این متد زمانی فراخوانی میشود که کاربر روی یک کتاب کلیک کرده و بارکد کتاب در دیتابیس سرچ می شود تا اطلاعات کامل
            /// کتاب را در پنجره 
            /// AddBook.xaml
            /// به نمایش گذاشته شود
            string numShow = (string)((Border)sender).Tag;
            //if (sender is Border item)
            //{
            //item
            //}
            AddBook viewbook = new AddBook(int.Parse(numShow));
            viewbook.Show();
        }
        private void initDB()
        {
            // این متد فرآیند اتصال به دیتابیس را انجام میدهد
            String path = @"data\database.db";
            dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            dbConnection.Open();

        }
        /// چند متد زیر برای تعریف عملکرد ویندوز تایتل سفارشی شده است
        private void Button_Min_Click(object sender, RoutedEventArgs e)
        {

            WindowState = WindowState.Minimized;
        }

        private void Button_Max_Click(object sender, RoutedEventArgs e)
        {

            if (this.WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }

  
        }
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {

            MessageYesNo msg = new MessageYesNo("آیا می‌خواهید از برنامه خارج شوید؟");

            if (msg.showMsg().Result == 1)
            {
                System.Windows.Application.Current.Shutdown();
            }

        }

        private void windowMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainapp mainwind = new mainapp();
            mainwind.Show();
            this.Close();
        }
    }
}
