using BL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            PersonManager pm = new PersonManager();
            Person p = new Person { FirstName = "dede", LastName = "gg", Email = "awa@www", PhoneNumber = "23432432" };
            pm.Add(p);
        }
    }
}
