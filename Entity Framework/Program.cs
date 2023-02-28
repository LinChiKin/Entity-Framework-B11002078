using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework
{
    internal class Program
    {
        private static async Task main_form_open()
        {
            Main_Form form1= new Main_Form();
            Console.WriteLine("Main_Form opened !");
            form1.ShowDialog();
            Console.WriteLine("Main_Form closed !");
        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Now Opening Main_Form...");
            Task task1=Task.Run(main_form_open);






            Console.ReadLine();
            
        }
    }
}
