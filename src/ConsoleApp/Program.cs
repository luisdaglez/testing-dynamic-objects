using System;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var test1 = new ReadTest();
            test1.RunReadTest(readListCount: 1000, readCount: 50_000_000, testIterations: 4);

            var test2 = new WriteTest();
            test2.RunWriteTest(writeListCount: 5_000_000, testIterations: 4);

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}