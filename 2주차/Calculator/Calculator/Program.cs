using System;
using System.Collections.Generic;

namespace Calculator
{

    class Program
    {
        static void Main(string[] args)
        {
            bClass b = new bClass();
            cClass c = new cClass();

            aClass a = b;

            a.SomeThing();
            a = c;
            a.SomeThing();
        }
    }
    abstract class aClass
    {
        public virtual void SomeThing()
        {
            
        }
    }

    class cClass : aClass
    {
        public override void SomeThing()
        {
            Console.WriteLine("C");
        }
    }

    class bClass : aClass
    {
        public override void SomeThing()
        {
            Console.WriteLine("B");
        }
    }

    /// <summary>
    /// 계산기 클래스
    /// </summary>
    class Calculator
    {
        private int calcCount = 0;

        public int Sum(int a,int b)
        {
            calcCount++;
            return a + b;
        }
        public int Subtract(int a, int b)
        {
            calcCount += 1;
            return a - b;
        }
        public int Divide(int a, int b)
        {
            calcCount += 1;
            return a / b;
        }
        public int Multiply(int a, int b)
        {
            calcCount += 1;
            return a * b;
        }
        public int Modulo(int a, int b)
        {
            calcCount += 1;
            return a % b;
        }

        public void Print()
        {
            Console.WriteLine($"총 {calcCount}회 계산되었습니다.");
            Console.WriteLine("총 " + calcCount + "회 계산되었습니다.");
            Console.WriteLine(string.Format("총 {0}회 계산되었습니다.", calcCount));
        }
    }
}
