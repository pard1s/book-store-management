using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{

    public class Books
    {
        
        public static List<Books> bookList = new List<Books>();

        public Books(double bookId, string bookName, string author, string translator, string editor,
                    string publisher, double editionNo, double publishDate, double pages,
                    double volumeNo, double price, double barcode, double state, double sold)
        {
            this.bookId = bookId;
            this.bookName = bookName;
            this.author = author;
            this.translator = translator;
            this.editor = editor;
            this.publisher = publisher;
            this.editionNo = editionNo;
            this.publishDate = publishDate;
            this.pages = pages;
            this.volumeNo = volumeNo;
            this.price = price;
            this.barcode = barcode;
            this.state = state;
            this.sold = sold;
            this.amountNonStatic = 1;
            
        }

        public double bookId { get; set; }
        public String bookName { get; set; }
        public String author { get; set; }
        public String translator { get; set; }
        public String editor { get; set; }
        public String publisher { get; set; }
        public double editionNo { get; set; }
        public double publishDate { get; set; }
        public double pages { get; set; }
        public double volumeNo { get; set; }
        public double price { get; set; }
        public double barcode { get; set; }
        public double state { get; set; }
        public double sold { get; set; }
        public static double amount { get; set; }
        public double amountNonStatic { get; set; }
        public static double returnBook { get; set; }


    }
}
