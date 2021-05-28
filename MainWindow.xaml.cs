using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using static System.Windows.Forms.MessageBoxIcon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Security.Cryptography;

namespace WpfApp1
{
    // این کد مربوط به کد های قسمت لاگین است


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // این متغیر برای ذخیره سازی پسورد وارد شده توسط کاربر کاربرد دارد
        private String password = "";

        // این متغیر برای ذخیره سازی وضعیت چک باکس مرا به خاطر بسپار در صفحه لاگین قرار گرفته شده است
        private bool chBox1 = false;

        // این متغیر برای ذخیره سازی وضعیت نشان دادن پسورد به صورت ستاره دار یا به صورت بدون ستاره دار است
        int flag = 0;

        // این متغیر برای ذخیره سازی رفرنس کانشکشن برای ارتباط با دیتابیس است
        private SQLiteConnection dbConnection;

        public MainWindow()
        {
            InitializeComponent();

            // متد زیر برای اتصال به دیتابیس کاربرد دارد
            initDB();

            //userNameBox.Text = getHash("پسورد");
            
            // در قطعه کد زیر، چک میشود که اگر کاربر قبلا گزینه مرا به خاطر بسپار را زده باشد، یوزر نیم را از دیتابیس استخراج کند
            //string sql1 = "SELECT user FROM remember";
            //SQLiteCommand command1 = new SQLiteCommand(sql1, dbConnection);
            //SQLiteDataReader reader2 = command1.ExecuteReader();
            //reader2.Read();
            //userNameBox.Text = reader2["user"]+"";
            userNameBox.Text = Properties.Settings.Default.RememberMe;



            // قطعه کد زیر وقتی ترو میشود که وقتی یوزرنیمی در دیتابیس ذخیره شده بود، تیک گزینه مرا به خاطر بسپار را بزند
            if (userNameBox.Text != "")
            {
                checkBox1.Background = (Brush)new BrushConverter().ConvertFrom("#454545");
                chBox1 = true;
            }

        }



        private void initDB()
        {
           // این متد فرآیند اتصال به دیتابیس را انجام میدهد
            String path = @"data\database.db";
            dbConnection = new SQLiteConnection("Data Source="+path+";Version=3;");
            dbConnection.Open();
            

        }
        private void SetSelection(PasswordBox passwordBox, int start, int length)
        {
            // این متد برای تغییر کورسر پسورد به کار میرود
            passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { start, length });
        }


        private void aboutUs_Click(object sender, RoutedEventArgs e)
        {
            // این ایونت برای نشان دادن پنجره تماس با ما نشان داده می شود
            contactSection.Visibility = Visibility.Visible;
            loginSection.Visibility = Visibility.Hidden;
        }

        private void aPicture_MouseDown(object sender, MouseEventArgs e)
        {
            // این ایونت زمانی اجرا می شود که بر روی علامت چشم در کنار پسورد کلیک شود
            // که اگر فلگ برابر صفر باشد، یعنی باید پسورد را نشان دهد و فلگ را برابر ۲ می کند

            // PasswordBox = به پسورد باکسی که پسورد را ستاره دار نشان میدهد اشاره دارد
            // PasswordBox2 = به پسورد باکسی که پسورد را ستاره دار نشان نمیدهد اشاره دارد
            if (flag==0) {
                passwordBox2.Text = passwordBox.Password;
                passwordBox.Visibility = Visibility.Hidden;
                passwordBox2.Visibility = Visibility.Visible;
                // کد زیر کورسر را به انتها منتقل میکند تا کاربر اگر حرفی را وارد کرد، در آخر عبارت وارد شده اضافه شود
                passwordBox2.CaretIndex = passwordBox2.Text.Length;
                // کد زیر فوکس را به پسوردباکس۲ تغییر میدهد. که یعنی اگر کاربر آیکن چشم را زد، بدون کلیک کردن روی پسورد باکس
                // با تایپ کردن روی کیبورد بتواند متن پسورد را تغییر دهد
                passwordBox2.Focus();
                flag = 1;
                
            }
            else
            {
                passwordBox.Password = passwordBox2.Text;
                passwordBox2.Visibility = Visibility.Hidden;
                passwordBox.Visibility = Visibility.Visible;
                passwordBox.Password = passwordBox2.Text;
                // کد زیر کورسر را به انتها منتقل میکند تا کاربر اگر حرفی را وارد کرد، در آخر عبارت وارد شده اضافه شود
                SetSelection(passwordBox, passwordBox.Password.Length, 0);
                // کد زیر فوکس را به پسوردباکس۲ تغییر میدهد. که یعنی اگر کاربر آیکن چشم را زد، بدون کلیک کردن روی پسورد باکس
                // با تایپ کردن روی کیبورد بتواند متن پسورد را تغییر دهد
                passwordBox.Focus();
                flag = 0;
            }

                
        }
      
        private void pwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // این قسمت برای پسورد ستاره دار است
            // این قسمت برای نشان دادن عبارت کلمه عبور در پشت باکس ورودی است وقتی که کاربر هیچ
            // کلمه عبوری را وارد نکرده است

            // ID2= متن ثابتی که عبارت رمز عبور را در خود دارد
            this.password = passwordBox.Password;
            if (passwordBox.Password != "")
                ID2.Visibility = Visibility.Hidden;
            else
                ID2.Visibility = Visibility.Visible;

        }

        private void pwdBox_TextChanged(object sender, RoutedEventArgs e)
        {
            // این قسمت برای پسورد بی ستاره است
            // این قسمت برای نشان دادن عبارت کلمه عبور در پشت باکس ورودی است وقتی که کاربر هیچ
            // کلمه عبوری را وارد نکرده است

            // ID2= متن ثابتی که عبارت رمز عبور را در خود دارد
            this.password = passwordBox2.Text;
            if (passwordBox2.Text != "")
                ID2.Visibility = Visibility.Hidden;
            else
                ID2.Visibility = Visibility.Visible;

        }
        private void usrBox_TextChanged(object sender, RoutedEventArgs e)
        {

            // این قسمت برای نشان دادن عبارت نام کاربری در پشت باکس ورودی است وقتی که کاربر هیچ
            // نام کاربری ای را وارد نکرده است

            // ID2= متن ثابتی که عبارت نام کاربری را در خود دارد
            if (userNameBox.Text != "")
                ID1.Visibility = Visibility.Hidden;
            else
                ID1.Visibility = Visibility.Visible;

        }


        private void forget_Click(object sender, RoutedEventArgs e)
        {
            // این ایونت کدهای مربوط به دکمه فراموش رمز عبور را دارد. که اگر کاربری رمز عبور خود را از یاد برد
            // با زدن این گزینه به آدرس ایمیل این کاربر، کد فعال سازی را بفرستد که با آن وارد نرم افزار شود
            
            // این قسمت در مراحل بعدی پروژه به طور کامل پیاده سازی می شود
            int queryRes = -1;

            // این کویری چک می کند که آیا یوزر نیم وارد شده در دیتابیس موجود است یا نه
            string sql1 = "SELECT username FROM users where username=\""+userNameBox.Text+"\"";
            SQLiteCommand command1 = new SQLiteCommand(sql1, dbConnection);
            SQLiteDataReader reader2 = command1.ExecuteReader();
            reader2.Read();
            if (reader2.HasRows)
            {
                if(reader2["username"]+"" == userNameBox.Text)
                {
                    // اگر موجود بود، متغیر زیر برابر یک می شود که در دستورات شرطی پایین به این معنی است که با موفقیت یوزر
                    // مورد نظر را در سامانه پیدا کرده است
                    queryRes = 1;
                }
            }

            if (userNameBox.Text == "")
            {
                showMessage("نام کاربری وارد نشده است", "#ff0000");
            }
            else if (queryRes == -1)
            {
                showMessage("نام کاربری در سامانه موجود نمی‌باشد", "#ff0000");
            }
            else
            {
                showMessage("رمز عبور به ایمیل این کاربر فرستاده شد", "#27AE60");
            }
            
        }


        internal void showMessage(String text, String color)
        {
            // این متد برای نشان دادن پیغام به کاربر در بالای متن صفحه ورود است

            // text = رشته ای که قرار است در متن پیام ظاهر شود  
            // color = کد هگز رنگ پس زمینه پیغام کاربر را در خود ذخیره می کند
            // xMessage = آی دی مربوط قسمت گرافیکی پیام که برای کاربر نشان داده می شود
            // messageText = آبجکتی که در محیط گرافیکی متن پیام را نشان میدهد
            xMessage.Visibility = Visibility.Visible;
            xMessage.Background = (Brush)new BrushConverter().ConvertFrom(color);
            messageText.Text = text;
        }
        private void Button_Min_Click(object sender, RoutedEventArgs e)
        {
            
            WindowState = WindowState.Minimized;

            //ShowInTaskbar is true by default, that is, whether the program icon appears in the taskbar after it is minimized
            //ShowInTaskbar = false;
        }

        private void Button_Max_Click(object sender, RoutedEventArgs e)
        {
            
            if (this.WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                mainGrid.Margin = new Thickness(0);
            }
            else
            {
                WindowState = WindowState.Maximized;
                mainGrid.Margin = new Thickness(5);
            }

            //ShowInTaskbar is true by default, that is, whether the program icon appears in the taskbar after being minimized
            //ShowInTaskbar = false;
        }
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {

            MessageYesNo msg = new MessageYesNo("آیا می‌خواهید از برنامه خارج شوید؟");

            if (msg.showMsg().Result == 1)
            {
                System.Windows.Application.Current.Shutdown();
            }



            //ShowInTaskbar is true by default, that is, whether the program icon appears in the taskbar after being minimized
            //ShowInTaskbar = false;
        }
        private void checkbox_Event(object sender, MouseButtonEventArgs e)
        {
            // این ایونت وقتی اجرا میشود که کاربر روی چک باکس مرا به خاطر بسپار کلیک کند
            if (!chBox1)
            {
                // در کد زیر بک گراند چک باکس به طوسی پررنگ تغییر داده میشود که نشان گر این است که
                // تیک چک باکس زده شده است
                checkBox1.Background = (Brush)new BrushConverter().ConvertFrom("#454545");
                chBox1 = true;     

            }
            else
            {
                // در کد زیر بک گراند چک باکس به سفید تغییر داده میشود که نشان گر این است که
                // تیک چک باکس برداشته شده است
                checkBox1.Background = Brushes.White;
                chBox1 = false;
                
            }
            
        }

        private void backToLogin_Click(object sender, RoutedEventArgs e)
        {
            // این ایونت وقتی اجرا می شود که دکمه مربوط به بازگشت در قسمت تماس با ما زده می شود تا پنجره
            // لاگین نمایش داده شود
            contactSection.Visibility = Visibility.Hidden;
            loginSection.Visibility = Visibility.Visible;
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // این ایونت وقتی اجرا میشود که دکمه خروج در پایین نرم افزار زده شده باشد
            MessageYesNo msg = new MessageYesNo("آیا می‌خواهید از برنامه خارج شوید؟");
            
            if (msg.showMsg().Result == 1)
            {
                System.Windows.Application.Current.Shutdown();
            }


        }
        public string getHash(string pass)
        {
            // این متد یک رشته را به عنوان ورودی دریافت می کند و هش آن را به عنوان خروجی میفرستد
            // وقتی این متد استفاده می شود که کاربر رمز عبور را در سامانه وارد کند و این رمز عبور به
            // هش تبدیل میشود و با هش موجود در دیتابیس مقایسه می شود تا از صحت رمز عبور وارد شده اطمینان حاصل شود
            var data = Encoding.Unicode.GetBytes(pass);
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(data);
            return Convert.ToBase64String(sha1data);
        }
        private int login(String username, String password)
        {
            // این متد وظیفه صحت بررسی یوزرنیم و پسوردی که به عنوان ووردی به آن داده شده است را دارد

            // کد زیر پسورد وارد شده توسط کاربر را هش میکند و در متغیر ریزالت ذخیره می کند
            String result = getHash(password);

            // کویری زیر در دیتابیس پسوردی را که در دیتابیس توسط یوزری که کاربر در سامانه وارد کرده است 
            // را به صورت هش بر می گرداند
            string sql = "SELECT password FROM users WHERE username=\"" + userNameBox.Text + "\"";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();

            // در این قسمت بررسی می شود که آیا کویری نتیجه خاصی را برگردانه است یا نه
            // که در اینجا زمانی نتیجه را پیدا میکند که نام کاربری درست وارد شده باشد
            if (reader.HasRows)
            {
                // در دستور شرطی زیر بررسی می شود که آیا پسوردی که به صورت هش در دیتابیس ذخیره شده است با 
                // پسوردی که کاربر در سامانه وارد کرده است و به هش تبدیل شده است یکسان است یا نه
                // اگر یکسان باشد، خروجی یک را برمیگرداند که یعنی کاربر به درستی وارد سامانه شده است
                if (reader["password"] + "" == result)
                {
                    return 1;
                }
                else
                {
                    // در غیر این صورت عدد منفی یک برگردانده میشود که به این معنی است که ورود موفقیت آمیز نبوده است
                    return -1;
                }
            }
            return -1;
        

        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            // این ایونت زمانی اجرا می شود که کاربر دکمه ورود را در صفحه لاگین زده باشد

            // در اینجا بررسی می شود که آیا یوزر نیم یا پسورد خالی است یا نه. اگر خالی بود به کاربر با نشان دادن
            // پیامی به او اطلاع رسانی کند
            if (userNameBox.Text == "" | password == "")
            {
                showMessage("نام کاربری یا رمزعبور وارد نشده است", "#ff0000");
                return;
            }
            // اگر یوزرنیم و پسورد خالی نبود اطلاعات لاگین به متد لاگین فرستاده میشود تا صحت اطلاعات وارد شده را بررسی کند
            // اگر خروجی -۱ باشد یعنی ورود موفقیت آمیز نبوده است
            int res = login(userNameBox.Text, password);
            if (res==-1)
                showMessage("نام کاربری یا رمزعبور اشتباه است", "#ff0000");
            
            // اگر خروجی یک باشد یعنی ورود موفقیت آمیز بوده است
            else if (res == 1)
            {
                // در اینجا بررسی می شود که اگر دکمه مرا به خاطر بسپار زده شده باشد، در دیتابیس نام یوزر وارد
                // شده را ذخیره کند تا در لاگین های بعدی یوزر نیم در سامانه به طور اتوماتیک وارد شود
                if (chBox1)
                {
                    // کویری زیر، یوزر نیم وارد شده را وقتی که چک باکس مرا به خاطر بسپار فعال شده باشد را ذخیره می کند
                    //string sql1 = "UPDATE remember SET user='" + userNameBox.Text + "' WHERE id=1";
                    //SQLiteCommand command1 = new SQLiteCommand(sql1, dbConnection);
                    //command1.ExecuteNonQuery();

                    Properties.Settings.Default.RememberMe = userNameBox.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    // کوئری زیر،  وقتی که چک باکس مرا به خاطر بسپار غیر فعال شده باشد، یوزرنیم ذخیره شده را پاک میکند
                    //string sql2 = "UPDATE remember SET user='' WHERE id=1";
                    //SQLiteCommand command2 = new SQLiteCommand(sql2, dbConnection);
                    //command2.ExecuteNonQuery();

                    Properties.Settings.Default.RememberMe = "";
                    Properties.Settings.Default.Save();
                }
                // در این قسمت که لاگین موفقیت آمیز بوده است، پنجره لاگین بسته میشود و پنجره اصلی برنامه به نمایش گذاشته می شود
                mainapp wind = new mainapp();
                dbConnection.Close();
                wind.Show();
                this.Hide();
            }

        }
        /// چند متد زیر برای تعریف عملکرد ویندوز تایتل سفارشی شده است
        private void windowMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
           
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void windowMouseUpEvent(object sender, MouseButtonEventArgs e)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Reset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            userNameBox.Text = "";
            passwordBox.Password = "";
            passwordBox2.Text = "";
            password = "";
        }

        private void closeMsgEvent(object sender, MouseButtonEventArgs e)
        {
            xMessage.Visibility = Visibility.Hidden;
        }
    }
}
