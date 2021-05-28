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
using System.Windows.Shapes;

namespace WpfApp1
{   // این قسمت مربوط به کد های صفحه اصلی برنامه است

    /// <summary>
    /// Interaction logic for mainapp.xaml
    /// </summary>
    public partial class mainapp : Window
    {
        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public mainapp()
        {
            InitializeComponent();
            
            // این قسمت برای وقتی است که کاربر در مدت زمان طولانی ای در سامانه فعالیت نداشته باشد
            // چون فعلا فقط قسمت لاگین برنامه پیاده سازی شده است برای نشان دادن مفهوم سشن اکسپایر بعد از
            // ده ثانیه برنامه به طور اتوماتیک لاگ آف می شود و به صفحه لاگین بر میگردد

            // کد زیر بعد از ۱۰ ثانیه ایونت تایم اوت سشن را اجرا میکند
            //dispatcherTimer.Tick += timeoutSession;
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            //dispatcherTimer.Start();
        }
        private void timeoutSession(object sender, EventArgs e)
        {
            // این ایونت وقتی اجرا می شود که بعد از مدت زمان طولانی که از سامانه استفاده نشده است، از سامانه خارج
            // می شود و به صفحه لاگین برگردانده می شود
            dispatcherTimer.Stop();
            MainWindow wind = new MainWindow();
            wind.showMessage("به دلیل عدم فعالیت طی زمان طولانی، دوباره وارد شوید", "#ff0000");
            wind.Show();
            this.Close();
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
            ShowBooks showBook = new ShowBooks();
            showBook.Show();
            this.Close();
        }

        private void Border_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            ShowBooks showBook = new ShowBooks(1);
            showBook.Show();
            this.Close();
        }
    }
}
