using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ShowBooks.xaml
    /// </summary>

    public partial class ShowBooks : Window
    {
        int typee = 0, section = 0;
        bool isVewing = false;
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
        int intt(object input)
        {
            try
            {
                return int.Parse(input.ToString());
            }
            catch (Exception)
            {
            }
            return 0;

        }
        SQLiteConnection dbConnection;
        List<int> ids;
        public ShowBooks()
        {
            InitializeComponent();
            initDB();
            initShowing();
            typee = 0;
        }
        public ShowBooks(int section)
        {
            
            InitializeComponent();
            initDB();
            
            this.section = section;
            // اگر مربوط به انتخاب کتاب ها باشد
            if (section==0)
            {
                typee = 1;
                this.Width = 950;
                this.Height = 650;
                this.BorderThickness = new Thickness(2);
                this.BorderBrush = Brushes.DeepPink;
                num1Btn.Visibility = Visibility.Hidden;
                num2Btn.Visibility = Visibility.Hidden;
                num3Btn.Visibility = Visibility.Hidden;
                num4Btn.Visibility = Visibility.Hidden;
                num5Btn.Visibility = Visibility.Hidden;
                addButtonTop.Visibility = Visibility.Hidden;
                amountGrid.Visibility = Visibility.Visible;
                //this.Background = new BitmapImage(new Uri("/img/ed.png", UriKind.Relative));
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "img/Image 6.jpg")));
                titleWindow.Text = "سامانه مدیریت کتاب‌فروشی - انتخاب کتاب برای سفارش";
                titleSection.Text = "کتاب مورد نظر را انتخاب کنید";

                isVewing = true;
                initShowing();

                // اگر مربوط به نمایش سفارش ها باشد
            } else if (section==1)
            {
                titleSection.Text = "لیست سفارشات";
                addButtonText.Text = "افزودن سفارش";
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "img/Image 6.jpg")));
                initShowingOrders();
            }
            


        }
        ///////////////////////////////           For Books             /////////////////////////////////////////
        private void initShowing()
        {
            Books.bookList.RemoveRange(0, Books.bookList.Count);
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
                    Books currentBook = new Books(doublee(reader["bookId"]), reader["bookName"] + "", reader["author"] + "",
                                                 reader["translator"] + "", reader["editor"] + "",
                                                 reader["publisher"] + "", doublee(reader["editionNo"]),
                                                 doublee(reader["publishDate"]), doublee(reader["pages"]),
                                                 doublee(reader["volumeNo"]), doublee(reader["price"]),
                                                 doublee(reader["barcode"]), 1, doublee(reader["sold"]));
                    Books.bookList.Add(currentBook);

                    addBookToGrid(currentBook);
                }
                
                
            } else
            {
                /// وقتی کتابی جهت نمایش موجود نباشد این پیام را نشان خواهد داد
                
                listStackPanel.Visibility = Visibility.Hidden;
                noBookFoundText.Text = "کتابی جهت نمایش وجود ندارد";
                noBookFoundText.Visibility = Visibility.Visible;
            }
            reader.Close();
        }

        ///////////////////////////////           For Orders            /////////////////////////////////////////
        private void initShowingOrders()
        {
            Orders.ordersList.RemoveRange(0, Orders.ordersList.Count);
            //در این قسمت فرمان کوئیری به دیتابیس فرستاده می شود تا اطلاعات سفارش ها را از دیتابیس بگیرد
            string sql = "SELECT * FROM Orders";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                int n = 0;
                while (reader.Read())
                {
                    /// در این مرحله تک تک نتیجه های کوئیری در داخل یک اسکرول ویوو در داخل یک بوردر نمایش داده می شود
                    /// که ساختار گرافیکی این بوردر به صورت زیر تعریف شده است
                    Orders currentOrder = new Orders(doublee(reader["orderId"]), reader["name"] + "",
                                                     doublee(reader["phoneNum"]), reader["address"] +"",
                                                     intt(reader["dateDay"]), intt(reader["dateMonth"]),
                                                     intt(reader["dateYear"]));
                    Orders.ordersList.Add(currentOrder);

                    addOrderToGrid(currentOrder);
                }


            }
            else
            {
                /// وقتی کتابی جهت نمایش موجود نباشد این پیام را نشان خواهد داد

                listStackPanel.Visibility = Visibility.Hidden;
                noBookFoundText.Text = " سفارشی جهت نمایش وجود ندارد";
                noBookFoundText.Visibility = Visibility.Visible;
            }
            reader.Close();
        }


        private void editClick(object sender, RoutedEventArgs e) 
        {

            string numShow = (string)((Button)sender).Tag;
            if (section == 0)
            {
                AddBook viewbook = new AddBook(double.Parse(numShow), 1);
                viewbook.ShowDialog();
                bookListGrid.Children.Clear();
                bookListGrid.RowDefinitions.Clear();
                initShowing();
            } else if (section == 1)
            {
                CustomerOrder window = new CustomerOrder(doublee(numShow), 2);
                window.ShowDialog();
                bookListGrid.Children.Clear();
                bookListGrid.RowDefinitions.Clear();
                initShowingOrders();

            }
            
            searchTextBox.Text = "";
        }
        private void delClick(object sender, RoutedEventArgs e)
        {
            string tag = (string)((Button)sender).Tag;
            if (section==0)
            {
                foreach (Books item in Books.bookList)
                {
                    if (item.bookId + "" == tag)
                    {
                        MessageYesNo msg = new MessageYesNo($"آیا می‌خواهید کتاب \"{item.bookName}\" را حذف کنید؟");
                        int res = msg.showMsg().Result;
                        if (res == 1)
                        {
                            try
                            {
                                string sql = String.Format("DELETE FROM books where bookId='{0}'", tag);
                                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                                command.ExecuteNonQuery();

                                new MessageOk("کتاب مورد نظر با موفقیت حذف شد", 1).ShowDialog();
                                bookListGrid.Children.Clear();
                                bookListGrid.RowDefinitions.Clear();
                                initShowing();
                                searchTextBox.Text = "";
                                break;

                            }
                            catch (Exception g)
                            {
                                new MessageOk("عدم توانایی در حذف کتاب", 2).ShowDialog();
                                //MessageBox.Show(g.Message);
                            }

                        }
                    }
                }
            } else if (section == 1)
            {
                
                foreach (Orders item in Orders.ordersList)
                {
                    if (item.orderId + "" == tag)
                    {
                        MessageYesNo msg = new MessageYesNo($"آیا می‌خواهید سفارش شماره #{item.orderId+1000} به نام {item.name} را حذف کنید؟");
                        int res = msg.showMsg().Result;
                        if (res == 1)
                        {
                            try
                            {
                                string sql = String.Format("DELETE FROM Orders where orderId='{0}'", tag);
                                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                                command.ExecuteNonQuery();

                                string sql2 = String.Format("DELETE FROM BookOrder where orderId='{0}'", tag);
                                SQLiteCommand command2 = new SQLiteCommand(sql2, dbConnection);
                                command2.ExecuteNonQuery();

                                new MessageOk("سفارش مورد نظر با موفقیت حذف شد", 1).ShowDialog();
                                bookListGrid.Children.Clear();
                                bookListGrid.RowDefinitions.Clear();
                                initShowingOrders();
                                searchTextBox.Text = "";
                                break;

                            }
                            catch (Exception g)
                            {
                                new MessageOk("عدم توانایی در حذف سفارش", 2).ShowDialog();
                                //MessageBox.Show(g.Message);
                            }

                        }
                    }
                }
            }
            
        }
        private void showBookIcon(object sender, MouseButtonEventArgs e)
        {
            /// این متد زمانی فراخوانی میشود که کاربر روی یک کتاب کلیک کرده و بارکد کتاب در دیتابیس سرچ می شود تا اطلاعات کامل
            /// کتاب را در پنجره 
            /// AddBook.xaml
            /// به نمایش گذاشته شود
            string numShow = (string)((Border)sender).Tag;
            if (section == 0)
            {
                if (typee == 0)
                {
                    AddBook viewbook = new AddBook(Double.Parse(numShow), 0);
                    viewbook.Show();
                }
                else if (typee == 1)
                {
                    Books.returnBook = double.Parse(numShow);
                    Books.amount = doublee(amountTextBox.Text);
                    this.Close();
                }
            } else if (section == 1)
            {
                CustomerOrder window = new CustomerOrder(doublee(numShow),1);
                window.Show();
            }
            
            
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
            if (!isVewing)
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
            mainapp mainwind = new mainapp();
            mainwind.Show();
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int n = 0;
            bookListGrid.Children.Clear();
            bookListGrid.RowDefinitions.Clear();
            listStackPanel.Visibility = Visibility.Visible;
            noBookFoundText.Visibility = Visibility.Hidden;
            if (section == 0)
            {
                for (int i = 0; i < Books.bookList.Count; i++)
                {
                    /// در این مرحله تک تک نتیجه های کوئیری در داخل یک اسکرول ویوو در داخل یک بوردر نمایش داده می شود
                    /// که ساختار گرافیکی این بوردر به صورت زیر تعریف شده است
                    Books currentBook = Books.bookList[i];
                    if (currentBook.bookName.IndexOf(searchTextBox.Text) != -1 || currentBook.author.IndexOf(searchTextBox.Text) != -1 ||
                        currentBook.editor.IndexOf(searchTextBox.Text) != -1 || currentBook.barcode.ToString().IndexOf(searchTextBox.Text) != -1 ||
                        currentBook.publisher.IndexOf(searchTextBox.Text) != -1 || currentBook.price.ToString().IndexOf(searchTextBox.Text) != -1 ||
                        currentBook.translator.IndexOf(searchTextBox.Text) != -1 || currentBook.publishDate.ToString().IndexOf(searchTextBox.Text) != -1)
                    {
                        addBookToGrid(currentBook);
                        n++;
                    }

                }
                if (n == 0)
                {
                    /// وقتی کتابی جهت نمایش موجود نباشد این پیام را نشان خواهد داد

                    listStackPanel.Visibility = Visibility.Hidden;
                    noBookFoundText.Text = "کتاب مورد نظر پیدا نشد";
                    noBookFoundText.Visibility = Visibility.Visible;
                }
            } else if (section == 1)
            {
                for (int i = 0; i < Orders.ordersList.Count; i++)
                {
                    /// در این مرحله تک تک نتیجه های کوئیری در داخل یک اسکرول ویوو در داخل یک بوردر نمایش داده می شود
                    /// که ساختار گرافیکی این بوردر به صورت زیر تعریف شده است
                    Orders currentOrder = Orders.ordersList[i];
                    if (currentOrder.name.IndexOf(searchTextBox.Text) != -1 || currentOrder.address.IndexOf(searchTextBox.Text) != -1 ||
                        currentOrder.dateDay.ToString().IndexOf(searchTextBox.Text) != -1 || currentOrder.phoneNum.ToString().IndexOf(searchTextBox.Text) != -1 ||
                        (currentOrder.orderId+1000).ToString().IndexOf(searchTextBox.Text) != -1 || currentOrder.dateDay.ToString().IndexOf(searchTextBox.Text) != -1 ||
                        currentOrder.dateMonth.ToString().IndexOf(searchTextBox.Text) != -1 || currentOrder.dateYear.ToString().IndexOf(searchTextBox.Text) != -1)
                    {
                        addOrderToGrid(currentOrder);
                        n++;
                    }

                }
                if (n == 0)
                {
                    /// وقتی کتابی جهت نمایش موجود نباشد این پیام را نشان خواهد داد

                    listStackPanel.Visibility = Visibility.Hidden;
                    noBookFoundText.Text = "سفارش مورد نظر پیدا نشد";
                    noBookFoundText.Visibility = Visibility.Visible;
                }
            }


        }
        ///////////////////////////////           For Books             /////////////////////////////////////////
        private void addBookToGrid(Books currentBook)
        {

            String textt1 = currentBook.bookName;
            String textt2 = String.Format("نویسنده: {0}، ناشر: {1}، سال انتشار: {2} قیمت: {3} تومان", currentBook.author, currentBook.publisher, currentBook.publishDate, currentBook.price);
            RowDefinition row = new RowDefinition
            {
                Height = new GridLength(80)
            };

            Border newBoarder = new Border
            {
                Background = (Brush)new BrushConverter().ConvertFrom("#424242"),
                Opacity = 0.7,
                Width = 730,
                Height = 70,
                CornerRadius = new CornerRadius(10),
                HorizontalAlignment = HorizontalAlignment.Right,
                Cursor = Cursors.Hand,
                Tag = currentBook.bookId.ToString()

            };

            if (typee == 1)
            {
                row.Height = new GridLength(70);
                newBoarder.Height = 60;
                newBoarder.Width = 600;
            }

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

            if (typee == 0)
            {
                Button editBtn = new Button
                {
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(-4),
                    Tag = currentBook.bookId.ToString(),
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/img/ed.png", UriKind.Relative)),
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 32
                    }

                };
                Button delBtn = new Button
                {
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(-4),
                    Tag = currentBook.bookId.ToString(),
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/img/del.png", UriKind.Relative)),
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 32

                    }
                };

                grid1.Children.Add(editBtn);
                grid1.Children.Add(delBtn);

                Grid.SetColumn(editBtn, 0);
                Grid.SetColumn(delBtn, 1);
                editBtn.Click += editClick;
                delBtn.Click += delClick;
            }

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
            if (typee == 1)
            {
                txt1.FontSize = 14;
            }
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
        }

        private void addBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if(section == 0)
            {
                AddBook addBook = new AddBook();
                addBook.Show();
                this.Close();
            } else if(section == 1)
            {
                CustomerOrder addOrder = new CustomerOrder();
                addOrder.Show();
                this.Close();
            }
            
        }


        ///////////////////////////////           For Orders            /////////////////////////////////////////
        private void addOrderToGrid(Orders currentOrder)
        {

            String textt1 = $"سفارش #{currentOrder.orderId + 1000}";
            String textt2 = String.Format("{4}/{3}/{2} :اسم مشتری: {0}، شماره تماس: {1}، تاریخ انجام سفارش", currentOrder.name, currentOrder.phoneNum, currentOrder.dateDay, currentOrder.dateMonth, currentOrder.dateYear);
            RowDefinition row = new RowDefinition
            {
                Height = new GridLength(80)
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
                Tag = currentOrder.orderId.ToString()

            };

            if (typee == 1)
            {
                row.Height = new GridLength(70);
                newBoarder.Height = 60;
                newBoarder.Width = 600;
            }

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

            if (typee == 0)
            {
                Button editBtn = new Button
                {
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(-4),
                    Tag = currentOrder.orderId.ToString(),
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/img/ed.png", UriKind.Relative)),
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 32
                    }

                };
                Button delBtn = new Button
                {
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(-4),
                    Tag = currentOrder.orderId.ToString(),
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/img/del.png", UriKind.Relative)),
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = 32

                    }
                };

                grid1.Children.Add(editBtn);
                grid1.Children.Add(delBtn);

                Grid.SetColumn(editBtn, 0);
                Grid.SetColumn(delBtn, 1);
                editBtn.Click += editClick;
                delBtn.Click += delClick;
            }

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
            if (typee == 1)
            {
                txt1.FontSize = 14;
            }
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
        }
    }
}
