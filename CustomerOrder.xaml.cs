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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CustomerOrder.xaml
    /// </summary>
    
    public partial class CustomerOrder : Window
    {
        private int section = 0;
        private List<Books> orderBooks = new List<Books>();
        private SQLiteDataReader reader;
        bool isViewing = false;
        // این متغیر برای ذخیره سازی رفرنس کانشکشن برای ارتباط با دیتابیس است
        private SQLiteConnection dbConnection;
        private double id = 0;

        public CustomerOrder()
        {
            InitializeComponent();
            initDB();
            App.Current.Resources["deleteBtnVis"] = Visibility.Visible;
            dgBooks.ItemsSource = orderBooks;
            totalCostText.Visibility = Visibility.Hidden;
        }
        public CustomerOrder(double id, int section)
        {
            InitializeComponent();
            
           if(section == 1)
            {
                App.Current.Resources["deleteBtnVis"] = Visibility.Hidden;
                this.section = section;
                this.id = id;
                this.Width = 950;
                this.Height = 750;
                this.BorderThickness = new Thickness(2);
                this.BorderBrush = Brushes.DeepPink;
                dgBooks.CanUserDeleteRows = false;
                titlePage.Text = "نمایش سفارش مشتری";
                windowTitle.Text = "سامانه مدیریت کتاب‌فروشی - نمایش سفارش مشتری";
                addBookButton.Content = "پیش‌نمایش و چاپ گزارش";
                addOrderButton.Visibility = Visibility.Hidden;
                topBtn1.Visibility = Visibility.Hidden; topBtn2.Visibility = Visibility.Hidden; topBtn3.Visibility = Visibility.Hidden;
                topBtn4.Visibility = Visibility.Hidden; topBtn5.Visibility = Visibility.Hidden;

                id1Block.IsReadOnly = true; id2Block.IsReadOnly = true; id3Block.IsReadOnly = true; dateDayText.IsReadOnly = true;
                dateMonthText.IsReadOnly = true; dateYearText.IsReadOnly = true;

            } else if (section == 2)
            {
                App.Current.Resources["deleteBtnVis"] = Visibility.Visible;
                this.section = section;
                this.id = id;
                this.Width = 950;
                this.Height = 750;
                this.BorderThickness = new Thickness(2);
                this.BorderBrush = Brushes.DeepPink;
                titlePage.Text = "ویرایش سفارش مشتری";
                windowTitle.Text = "سامانه مدیریت کتاب‌فروشی - ویرایش سفارش مشتری";
                addOrderButton.Content = "ویرایش اطلاعات";
                totalCostText.Visibility = Visibility.Hidden;
                topBtn1.Visibility = Visibility.Hidden; topBtn2.Visibility = Visibility.Hidden; topBtn3.Visibility = Visibility.Hidden;
                topBtn4.Visibility = Visibility.Hidden; topBtn5.Visibility = Visibility.Hidden;
            }
            initDB();
            initShowingBook();
            isViewing = true;
            
        }


        double doublee(object input)
        {
            try
            {
                return double.Parse(input.ToString());
            }
            catch (Exception)
            {
            }
            return 0;

        }

        private void initShowingBook()
        {

            string sql1 = "SELECT * FROM Orders WHERE orderId = " + id + ";";
            SQLiteCommand command1 = new SQLiteCommand(sql1, dbConnection);
            SQLiteDataReader reader1 = command1.ExecuteReader();
            reader1.Read();
            id1Block.Text = reader1["name"] + ""; id2Block.Text = reader1["phoneNum"] + "";
            id3Block.Text = reader1["address"] + ""; dateDayText.Text = reader1["dateDay"] + "";
            dateMonthText.Text = reader1["dateMonth"] + ""; dateYearText.Text = reader1["dateYear"] + "";
            
            reader1.Close();

            dgBooks.ItemsSource = null;
            dgBooks.ItemsSource = orderBooks;


            //در این قسمت فرمان کوئیری به دیتابیس فرستاده می شود تا اطلاعات کتاب های سفارشات را از دیتابیس بگیرد
            string sql = "SELECT books.*, amount FROM books, BookOrder, Orders WHERE (Orders.orderId = "+id+") AND (books.bookId = BookOrder.bookId) AND (BookOrder.orderId = Orders.orderId) ;";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            double totalPrice = 0;
            if (reader.HasRows)
            {
                int n = 0;
                while (reader.Read())
                {
                    /// در این مرحله تک تک نتیجه های کوئیری در داخل یک اسکرول ویوو در داخل یک بوردر نمایش داده می شود
                    /// که ساختار گرافیکی این بوردر به صورت زیر تعریف شده است
                    Books currentBook = new Books(doublee(reader["bookId"]), reader["bookName"] + "", reader["author"] + "",
                                                 reader["translator"] + "", reader["editor"] + "",
                                                 reader["publisher"] + "", doublee(reader["editionNo"]),
                                                 doublee(reader["publishDate"]), doublee(reader["pages"]),
                                                 doublee(reader["volumeNo"]), doublee(reader["price"]),
                                                 doublee(reader["barcode"]), 1, doublee(reader["sold"]));

                    totalPrice += doublee(reader["price"]) * doublee(reader["amount"]);
                    currentBook.amountNonStatic = doublee(reader["amount"]);
                    orderBooks.Add(currentBook);

                    n++;

                }
                totalCostText.Text = $"جمع کل: {totalPrice} تومان";
                

            }

            reader.Close();


            
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
            if (!isViewing)
            {
                MessageYesNo msg = new MessageYesNo("آیا می‌خواهید از برنامه خارج شوید؟");

                if (msg.showMsg().Result == 1)
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }
            else
            {
                this.Close();
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
            ShowBooks showBook = new ShowBooks(1);
            showBook.Show();
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (section == 1)
            {
                List<string> orderDetail = new List<string>();
                orderDetail.Add(id+"");
                orderDetail.Add($"{dateYearText.Text}/{dateMonthText.Text}/{dateDayText.Text}");
                orderDetail.Add(id1Block.Text);
                orderDetail.Add(id2Block.Text);
                orderDetail.Add(id3Block.Text);
                Window1 window1 = new Window1(orderDetail,orderBooks);
                window1.ShowDialog();

            } else
            {
                Books.returnBook = -1;
                ShowBooks showBook = new ShowBooks(0);
                showBook.ShowDialog();
                foreach (var item in Books.bookList)
                {
                    if (item.bookId == Books.returnBook)
                    {
                        item.amountNonStatic = Books.amount;
                        orderBooks.Add(item);
                    }
                }
                dgBooks.Focus();
                dgBooks.ItemsSource = null;
                dgBooks.ItemsSource = orderBooks;
                //MessageBox.Show(Books.returnBook + "");
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddBook viewbook = new AddBook(((Books)(dgBooks.SelectedItem)).bookId, 0);
            viewbook.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            orderBooks.RemoveAt(dgBooks.SelectedIndex);
            dgBooks.ItemsSource = null;
            dgBooks.ItemsSource = orderBooks;
        }

        private void addOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int orderId = 0;
            if (section == 0)
            {
                string sql2 = "";
                string sql1 = String.Format("INSERT INTO Orders (name, phoneNum, address, dateDay, dateMonth, dateYear) " +
                            "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", id1Block.Text, id2Block.Text, id3Block.Text, dateDayText.Text,
                            dateMonthText.Text, dateYearText.Text
                            );
                sql1 = changePersianNumber(sql1);

                try
                {
                    SQLiteCommand command = new SQLiteCommand(sql1, dbConnection);
                    command.ExecuteNonQuery();

                    SQLiteCommand command1 = new SQLiteCommand("SELECT MAX(orderId) as ID FROM Orders", dbConnection);
                    SQLiteDataReader reader1 = command1.ExecuteReader();
                    reader1.Read();
                    orderId = int.Parse(reader1["ID"] + "");
                    reader1.Close();

                    foreach (var item in orderBooks)
                    {
                        sql2 = String.Format("INSERT INTO BookOrder (orderId, bookId, amount) " +
                            "VALUES ('{0}', '{1}', {2})", orderId, item.bookId, item.amountNonStatic
                            );
                        sql2 = changePersianNumber(sql2);
                        SQLiteCommand command2 = new SQLiteCommand(sql2, dbConnection);
                        command2.ExecuteNonQuery();

                    }

                    new MessageOk("اطلاعات با موفقیت ثبت شد", 1).ShowDialog();



                }
                catch (Exception d)
                {
                    /// در صورت وجود هر مشکل در ذخیره سازی داده ها این پیام نشان داده می شود
                    new MessageOk("عدم توانایی ثبت اطلاعات در دیتابیس", 2).ShowDialog();
                    //new MessageOk(d.Message, 2).ShowDialog();
                    //MessageBox.Show(d.Message);

                }
            } else if (section == 2)
            {
                string sql2 = "";
                string sql1 = String.Format("UPDATE Orders SET name = '{0}', phoneNum = '{1}', address = '{2}', dateDay = '{3}', dateMonth = '{4}', " +
                        "dateYear = '{5}' WHERE orderId = {6}",
                        id1Block.Text, id2Block.Text, id3Block.Text, dateDayText.Text,dateMonthText.Text,dateYearText.Text, id);
                sql1 = changePersianNumber(sql1);

                try
                {
                    SQLiteCommand command = new SQLiteCommand(sql1, dbConnection);
                    command.ExecuteNonQuery();
                    ////////////////////////////////////////////////////////////////

                    string sql3 = String.Format("DELETE FROM BookOrder where orderId='{0}'", id);
                    SQLiteCommand command3 = new SQLiteCommand(sql3, dbConnection);
                    command3.ExecuteNonQuery();

                    ////////////////////////////////////////////////////////////////
                    orderId = int.Parse(id+"");
                    foreach (var item in orderBooks)
                    {
                        sql2 = String.Format("INSERT INTO BookOrder (orderId, bookId, amount) " +
                            "VALUES ('{0}', '{1}', {2})", orderId, item.bookId, item.amountNonStatic
                            );
                        sql2 = changePersianNumber(sql2);
                        SQLiteCommand command2 = new SQLiteCommand(sql2, dbConnection);
                        command2.ExecuteNonQuery();

                    }

                    new MessageOk("اطلاعات با موفقیت ویرایش شد", 1).ShowDialog();
                    this.Close();

                }
                catch (Exception d)
                {
                    /// در صورت وجود هر مشکل در ذخیره سازی داده ها این پیام نشان داده می شود
                    new MessageOk("عدم توانایی ثبت اطلاعات در دیتابیس", 2).ShowDialog();
                    //new MessageOk(d.Message, 2).ShowDialog();
                    //MessageBox.Show(d.Message);

                }
            }
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private string changePersianNumber(string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;
        }
        private void initDB()
        {
            // این متد فرآیند اتصال به دیتابیس را انجام میدهد
            String path = @"data\database.db";
            dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            dbConnection.Open();

        }

    }




}
