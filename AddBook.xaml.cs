using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        bool isVewing = false;

        // این متغیر برای ذخیره سازی رفرنس کانشکشن برای ارتباط با دیتابیس است
        private SQLiteConnection dbConnection;
        public AddBook()
        {
            // این کانستراکتور زمانی فراخوانی میشود که درخواست اضافه کردن کتاب از سوی کاربر داده شود

            InitializeComponent();
            initDB();
            
        }
        public AddBook(int id)
        {
            // این کانستراکتور وقتی فراخوانده می شود که بارکد کتاب به این کلاس ارسال شود و به جای پنجره اضافه کردن کتاب
            // به پنجره نمایش جزئیات کتاب تغییر داده می شود
            // و در حین ورود به این پنجره بارکد ارسال شده در دیتابیس جستجو می شود و اطلاعات کامل کتاب به نمایش گذاشته می شود
            InitializeComponent();
            initDB();
            this.Width = 900;
            this.Height = 650;
            this.BorderThickness = new Thickness(2);
            this.BorderBrush = Brushes.DeepPink;
            windowTitle.Text = "سامانه مدیریت کتاب‌فروشی - جزئیات بیشتر";
            isVewing = true;
            string sql = String.Format("SELECT * FROM books where barcode='{0}'",id);
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            /// کد زیر برای مخفی کردن دکمه های بالایی این پنجره برای این نوع پنجره است
            mainGrid.RowDefinitions[0].Height = new GridLength(0);
            mainGrid.Margin = new Thickness(0, 70, 0, 0);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();

            ID1_block.IsReadOnly = true; ID2_block.IsReadOnly = true; ID3_block.IsReadOnly = true; ID4_block.IsReadOnly = true; ID5_block.IsReadOnly = true;
            ID6_block.IsReadOnly = true; ID7_block.IsReadOnly = true; ID8_block.IsReadOnly = true; ID9_block.IsReadOnly = true; ID10_block.IsReadOnly = true;
            ID11_block.IsReadOnly = true;
            titlePage.Text = "جزئیات بیشتر";
            addButton.Visibility = Visibility.Hidden;
            if (reader.HasRows)
            {
                ID1_block.Text = reader["bookName"] + "";
                ID2_block.Text = reader["author"] + "";
                ID3_block.Text = reader["translator"] + "";
                ID4_block.Text = reader["editor"] + "";
                ID5_block.Text = reader["publisher"] + "";
                ID6_block.Text = reader["editionNo"] + "";
                ID9_block.Text = reader["publishDate"] + "";
                ID7_block.Text = reader["pages"] + "";
                ID10_block.Text = reader["volumeNo"] + "";
                ID8_block.Text = reader["price"] + "";
                ID11_block.Text = reader["barcode"] + "";
            }
        }
        
        
        private void initDB()
        {
            // این متد فرآیند اتصال به دیتابیس را انجام میدهد
            String path = @"data\database.db";
            dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            dbConnection.Open();

        }
        private void Box_TextChanged(object sender, RoutedEventArgs e)
        {


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           //// دکمه اعمال تغییرات
            if (ID1_block.Text == "" | ID2_block.Text == "" | ID3_block.Text == "" | ID4_block.Text == "" | ID5_block.Text == ""
                   | ID6_block.Text == "" | ID7_block.Text == "" | ID8_block.Text == "" | ID9_block.Text == "" | ID10_block.Text == ""
                   | ID11_block.Text == "")
            {
                new MessageOk("برخی از اطلاعات وارد نشده اند", 2).ShowDialog();
            }
            else
            {
                /// در کد زیر متن کوئیری اس کیو لایت برای اجرا شدن اماده می شود تا داده ها در دیتابیس ذخیره شود
                int state = 1;
                string sql = String.Format("INSERT INTO books (bookName, author, translator, editor, publisher, " +
                    "editionNo, publishDate, pages, volumeNo, price, barcode, state) " +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}','{11}')",
                    ID1_block.Text, ID2_block.Text, ID3_block.Text, ID4_block.Text, ID5_block.Text, ID6_block.Text, ID9_block.Text, ID7_block.Text, ID10_block.Text,
                    ID8_block.Text, ID11_block.Text, state);
                sql = changePersianNumber(sql);

                try
                {
                    SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    reader.Read();
                    new MessageOk("اطلاعات با موفقیت ثبت شد", 1).ShowDialog();
                    ID1_block.Text = ""; ID2_block.Text = ""; ID3_block.Text = ""; ID4_block.Text = ""; ID5_block.Text = "";
                    ID6_block.Text = ""; ID7_block.Text = ""; ID8_block.Text = ""; ID9_block.Text = ""; ID10_block.Text = ""; ID11_block.Text = "";

                }
                catch (Exception d)
                {
                    /// در صورت وجود هر مشکل در ذخیره سازی داده ها این پیام نشان داده می شود
                    new MessageOk("عدم توانایی ثبت اطلاعات در دیتابیس", 2).ShowDialog();
                    //new MessageOk(d.Message, 2).ShowDialog();

                }
            }

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
            if (isVewing)
            {
                this.Close();
            }
            else
            {
                MessageYesNo msg = new MessageYesNo("آیا می‌خواهید از برنامه خارج شوید؟");

                if (msg.showMsg().Result == 1)
                {
                    System.Windows.Application.Current.Shutdown();
                }
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
        private string changePersianNumber(string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;
        }
    }
}
