using System;

namespace BackgroundServiceTestLib
{
    public class Class1
    {
        public void Run(string text)
        {
            Console.WriteLine($"{text} - {DateTime.Now.ToString()}");
        }
    }
}
