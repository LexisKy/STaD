using System;

namespace Triangle
{
    public class Triangle
    {
        static bool IsNotATriangle(double a, double b, double c)
        {
            return a + b <= c || a + c <= b || b + c <= a;
        }

        static bool IsIsoscelesTriangle(double a, double b, double c)
        {
            return a == b || a == c || b == c;
        }

        static bool IsEquilateralTriangle(double a, double b, double c)
        {
            return a == b && b == c;
        }

        static bool CheckArgs(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("неизвестная ошибка");
                return false;
            }

            return true;
        }

        static void CheckTriangle(double a, double b, double c)
        {
            if (IsNotATriangle(a, b, c))
            {
                Console.WriteLine("не треугольник");
            }
            else if (IsEquilateralTriangle(a, b, c))
            {
                Console.WriteLine("равносторонний");
            }
            else if (IsIsoscelesTriangle(a, b, c))
            {
                Console.WriteLine("равнобедренный");
            }
            else
                Console.WriteLine("обычный");
        }

        static void Main(string[] args)
        {
            if(!CheckArgs(args))
            {
                return;
            }          

            try
            {
                double a = Convert.ToDouble(args[0]);
                double b = Convert.ToDouble(args[1]);
                double c = Convert.ToDouble(args[2]);

                CheckTriangle(a, b, c);
            }
            catch (Exception)
            {

                Console.WriteLine("неизвестная ошибка");
            }
        }
    }
}