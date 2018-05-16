
namespace BFramework.ExpandedMath.Distributions
{
    [System.Serializable]
    public class HighOrderEquation
    {
        public HighOrderEquation(params int[] args)
        {
            Order = args;
        }

        public int[] Order { get; set; }

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
                for(int i = Order.Length - 1; i > -1; i--)
                {
                    result += Pow(x, Order[i]);
                }
                return result;
            }
        }
    }
}
