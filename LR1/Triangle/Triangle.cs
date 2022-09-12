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

        static void ParseArgs(ref double a, ref double b, ref double c, string[] args)
        {
            a = Convert.ToDouble(args[0]);
            b = Convert.ToDouble(args[1]);
            c = Convert.ToDouble(args[2]);

        }

        static bool CheckArgs(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Неизвестная ошибка");
                return false;
            }

            return true;
        }

        static void CheckTriangle(double a, double b, double c)
        {
            if (IsNotATriangle(a, b, c))
            {
                Console.WriteLine("Не треугольник");
            }
            else if (IsEquilateralTriangle(a, b, c))
            {
                Console.WriteLine("Равносторонний");
            }
            else if (IsIsoscelesTriangle(a, b, c))
            {
                Console.WriteLine("Равнобедренный");
            }
            else
                Console.WriteLine("Обычный");
        }

        static void Main(string[] args)
        {
            if(!CheckArgs(args))
            {
                return;
            }          

            try
            {
                double a = 0;
                double b = 0;
                double c = 0;
                ParseArgs(ref a, ref  b, ref c, args);

                CheckTriangle(a, b, c);
            }
            catch (Exception)
            {

                Console.WriteLine("Неизвестная ошибка");
            }
        }
    }
}