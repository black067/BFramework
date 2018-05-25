
namespace BFramework.ExpandedMath.Distributions
{
    [System.Serializable]
    public class HighOrderEquation
    {
        public HighOrderEquation(params double[] args)
        {
            Multiplier = args;
        }

        public double[] Multiplier { get; set; }

        private double Pow(double x, int n)
        {
            double r = 1;
            for(int i = n; i > 0; i--)
            {
                r *= x;
            }
            return r;
        }

        public double this[double x]
        {
            get
            {
                double result = 0;
                for(int i = Multiplier.Length - 1; i > -1; i--)
                {
                    result += Multiplier[i] == 0 ? 0 : (Multiplier[i] * Pow(x, i));
                }
                return result;
            }
        }
    }
}
