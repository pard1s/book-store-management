using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Orders
    {
        public Orders(double orderId, string name, double phoneNum, string address, int dateDay, int dateMonth, int dateYear)
        {
            this.orderId = orderId;
            this.name = name;
            this.phoneNum = phoneNum;
            this.address = address;
            this.dateDay = dateDay;
            this.dateMonth = dateMonth;
            this.dateYear = dateYear;
        }
        public static List<Orders> ordersList = new List<Orders>();
        public double orderId { get; set; }
        public string name { get; set; }
        public double phoneNum { get; set; }
        public string address { get; set; }
        public int dateDay { get; set; }
        public int dateMonth { get; set; }
        public int dateYear { get; set; }
    }
}
