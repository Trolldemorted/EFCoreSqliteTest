using DatabaseManager;
using System;
using System.Threading;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                FooDBContext.PlaceOrder();
                Thread.Sleep(100);
            }
        }
    }
}
