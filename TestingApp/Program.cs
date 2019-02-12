using System;
using XillioAPIService;
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

                LogService.logToConsole = true;
                
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