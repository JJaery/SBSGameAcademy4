using System;

namespace InterfacePractice
{
    class Program
    {
        static void Main(string[] args)
        {
            Person me = new Person();
            Calculator calc = new Calculator();

            DoSomething(me);
            DoSomething(calc);
        }

        static void DoSomething(iSum target)
        {
            int result = target.Sum(100, 100);
            Console.WriteLine(result);
        }
    }

    class Person : iSum
    {
        public int Sum(int a, int b)
        {
            return a + b + 1;
        }
    }

    class Calculator : iSum
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }


    interface iSum
    {
        int Sum(int a, int b);
    }
}
