using System;
using XillioServiceLibrary;

namespace TestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine();
            try
            {
                XillioService service = new XillioService();
                
                service.Start();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }
        }
    }
}