using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VBTools;

namespace MLRPrediction
{
    public static class CustomFunctions
    {
        // Declare a function that takes a variable number of arguments
        public static double PROD(params double[] args)
        {
            double product = 1.0;

            for (int i = 0; i < args.Length; i++)
                product *= args[i]; ;

            return product;
        }

        // Declare a function that takes a variable number of arguments
        public static double SUM(params double[] args)
        {
            double sum = 0.0;
            sum = args.Sum();
            return sum;
        }

        // Declare a function that takes a variable number of arguments
        public static double MIN(params double[] args)
        {
            double min = Double.MaxValue;
            min = args.Min();
            return min;
        }

        // Declare a function that takes a variable number of arguments
        public static double MAX(params double[] args)
        {
            double max = Double.MinValue; ;
            max = args.Max();
            return max;
        }

        // Declare a function that takes a variable number of arguments
        public static double MEAN(params double[] args)
        {
            double avg = 0.0;
            avg = args.Average();
            return avg;
        }

        // Declare a function that takes a variable number of arguments
        public static double LOG10(double arg)
        {
            double retVal = 0.0;

            if (Math.Abs(arg) < 1.0d)
                retVal = 0.0;
            else
                retVal = Math.Sign(arg) * Math.Log10(Math.Abs(arg));

            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        public static double LN(double arg)
        {
            double retVal = 0.0;
            if (Math.Abs(arg) < 1.0d)
                retVal = 0.0;
            else
                retVal = Math.Sign(arg) * Math.Log(Math.Abs(arg));
            
            return retVal;
        }


        // Declare a function that takes a variable number of arguments
        //public static double INVERSE(double min, double arg)
        public static double INVERSE(double arg, double min)
        {
            double retVal = 0.0;
            if (Support.IsNearZero(arg))
                retVal = Math.Pow(min, -1);
            else
                retVal = Math.Pow(arg, -1);

            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        public static double SQUARE(double arg)
        {
            double retVal = arg * arg;
            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        public static double SQUAREROOT(double arg)
        {
            double retVal = Math.Sign(arg) * Math.Sqrt (Math.Abs(arg));
            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        public static double QUADROOT(double arg)
        {
            double retVal = Math.Sign(arg) * Math.Pow(Math.Abs(arg), 0.25);            
            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        //public static double POLYNOMIAL(double intercept, double xCoeff, double x2Coeff, double arg)
        public static double POLYNOMIAL(double arg, double intercept, double xCoeff, double x2Coeff)
        {
            double retVal = intercept + (xCoeff * arg) + (x2Coeff * arg * arg);
            return retVal;
        }

        //dup for name change
        public static double POLY(double arg, double intercept, double xCoeff, double x2Coeff)
        {
            double retVal = intercept + (xCoeff * arg) + (x2Coeff * arg * arg);
            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        //public static double POWER(double exp, double arg)
        public static double POWER(double arg, double exp)
        {
            double retVal = Math.Sign(arg) * Math.Pow(Math.Abs(arg), exp);    
            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        public static double WindA_comp(double windDirection, double windMagnitude, double beachOrientation)
        {
            VBTools.WindComponents wndCmp = new VBTools.WindComponents(windMagnitude, windDirection, beachOrientation);
            double retVal = wndCmp._Ucomp;
            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        public static double WindO_comp(double windDirection, double windMagnitude, double beachOrientation)
        {
            VBTools.WindComponents wndCmp = new VBTools.WindComponents(windMagnitude, windDirection, beachOrientation);
            double retVal = wndCmp._Vcomp;
            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        public static double CurrentA_comp(double currentDirection, double currentMagnitude, double beachOrientation)
        {
            WaterCurrentComponents waterCmp = new WaterCurrentComponents(currentMagnitude, currentDirection, beachOrientation);
            double retVal = waterCmp._Ucomp;
            return retVal;
        }

        // Declare a function that takes a variable number of arguments
        public static double CurrentO_comp(double currentDirection, double currentMagnitude, double beachOrientation)
        {
            WaterCurrentComponents waterCmp = new WaterCurrentComponents(currentMagnitude, currentDirection, beachOrientation);
            double retVal = waterCmp._Vcomp;
            return retVal;
        }
    }
}
