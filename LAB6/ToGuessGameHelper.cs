using System;
namespace LAB6
{
    public class ToGuessGameHelper
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Result { get; protected set; }
        public int CountOfTryes { get; protected set; }
        public int MaxCountOfTryes { get; protected set; }
        public ToGuessGameHelper(double x, double y, int maxCountOfTryes=3) 
        {
            this.MaxCountOfTryes=maxCountOfTryes;
            Init(x, y);
        }
        public void Init(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.Result = Math.Round(MathFuncResult(x, y),2);
            this.CountOfTryes = 0;
        }
        public bool Try(double answer)
        {
            bool tryResult = answer == this.Result;
            if (!tryResult) CountOfTryes++;
            return tryResult;
        }
        public static double MathFuncResult(double x, double y)
        {
            double leftPart = Math.Sin((Math.Pow(x, 3) + Math.Pow(y, 5)) / (2 * Math.PI));
            double cosPart = Math.Cos(x + y);
            double rightPart = Math.Pow(Math.Abs(cosPart), 1.0 / 3) * Math.Sign(cosPart);
            return leftPart + rightPart;
        }
    }
}
