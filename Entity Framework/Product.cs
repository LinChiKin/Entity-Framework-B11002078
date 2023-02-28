using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework
{
    public class Product
    {
        public Product()
        {
            id = "";
            groups = "";
            name = "";
            quantity = 0;
            price = 0;
        }
        public Product(Product product)
        {
            this.id = product.id;
            this.groups = product.groups;
            this.name = product.name;
            this.quantity = product.quantity;
            this.price = product.price;
        }
        public Product(string id, string name, string str_quantity, string str_price, string groups)
        {
            this.id = id;
            this.groups = groups;
            this.name = name;
            bool quantity_bool = int.TryParse(str_quantity, out int qu_int);
            bool price_bool = double.TryParse(str_price, out double price_int);
            if (quantity_bool)
            {
                this.quantity = qu_int;
            }
            else
            {
                this.quantity = 0;
            }
            if (price_bool)
            {
                this.price = price_int;
            }
            else
            {
                this.price = 0;
            }
        }
        public string id { get; set; }
        public string groups { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }


    }
}
