using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1(List<string> orderDetail, List<Books> orderBooks)
        {
            InitializeComponent();
            double totalPrice = 0;


            while (idText.Inlines.Count > 0)
                idText.Inlines.Remove(idText.Inlines.ElementAt(0));
            while (dateText.Inlines.Count > 0)
                dateText.Inlines.Remove(dateText.Inlines.ElementAt(0));
            while (nameText.Inlines.Count > 0)
                nameText.Inlines.Remove(nameText.Inlines.ElementAt(0));
            while (phoneText.Inlines.Count > 0)
                phoneText.Inlines.Remove(phoneText.Inlines.ElementAt(0));
            while (addressText.Inlines.Count > 0)
                addressText.Inlines.Remove(addressText.Inlines.ElementAt(0));
            while (totalPriceText.Inlines.Count > 0)
                totalPriceText.Inlines.Remove(totalPriceText.Inlines.ElementAt(0));

            idText.Inlines.Add(new Run($"شماره سفارش: {int.Parse(orderDetail[0]) + 1000}"));
            dateText.Inlines.Add(new Run($"تاریخ سفارش: {orderDetail[1]}"));
            nameText.Inlines.Add(new Run($"نام مشتری: {orderDetail[2]}"));
            phoneText.Inlines.Add(new Run($"شماره تماس: {orderDetail[3]}"));
            addressText.Inlines.Add(new Run($"آدرس: {orderDetail[4]}"));
            for (int i = 0; i < orderBooks.Count; i++)
            {
                TableRowGroup trg = new TableRowGroup();
                TableRow tr = new TableRow();
                TableCell tc = null;
                ////////////////////////////////////////////////////////////////
                tc = new TableCell(new Paragraph(new Run($"{orderBooks[i].price}")));
                tr.Cells.Add(tc);
                tc = new TableCell(new Paragraph(new Run($"{orderBooks[i].barcode}")));
                tr.Cells.Add(tc);
                tc = new TableCell(new Paragraph(new Run($"{orderBooks[i].amountNonStatic}")));
                tr.Cells.Add(tc);
                tc = new TableCell(new Paragraph(new Run($"{orderBooks[i].bookName}")));
                tr.Cells.Add(tc);
                tc = new TableCell(new Paragraph(new Run($"{i+1}")));
                tr.Cells.Add(tc);

                totalPrice += orderBooks[i].price * orderBooks[i].amountNonStatic;
                /////////////////////////////////////////////////////////
                trg.Rows.Add(tr);
                tableContent.RowGroups.Add(trg);
            }
            totalPriceText.Inlines.Add(new Run($"جمع کل: {totalPrice} تومان"));

        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument fd = invoicePage;
            PrintDialog pd = new PrintDialog();
            fd.PageHeight = pd.PrintableAreaHeight;
            fd.PageWidth = pd.PrintableAreaWidth;
            fd.PagePadding = new Thickness(50);
            fd.ColumnGap = 0;
            fd.ColumnWidth = pd.PrintableAreaWidth;

            if (pd.ShowDialog().Value)
            {
                IDocumentPaginatorSource dps = fd;

                pd.PrintDocument(dps.DocumentPaginator, "Invoice");
            }
            Close();
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
                this.Close();

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

    }
}
