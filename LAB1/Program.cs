using System;
namespace LAB1
{
    static class Program
    {
        public static double MathFuncResult(double x, double y)
        {
            double leftPart = Math.Sin((Math.Pow(x, 3) + Math.Pow(y, 5)) / (2 * Math.PI));
            double cosPart = Math.Cos(x + y);
            double rightPart = Math.Pow(Math.Abs(cosPart), 1.0 / 3) * Math.Sign(cosPart);
            return leftPart + rightPart;
        }
        static void Main(string[] args)
        {
            const double PI = Math.PI;
            const double E = Math.E;
            double a, b, result;
            Console.Write("Введите a: ");
            a = double.Parse(Console.ReadLine());
            Console.Write("Введите b: ");
            b = double.Parse(Console.ReadLine());
            result = MathFuncResult(a, b);
            Console.WriteLine("Ответ = {0:F2}", result);

        }
    }
}