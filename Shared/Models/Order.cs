using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
