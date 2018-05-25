
namespace BFramework.ExpandedMath.Distributions
{
    [System.Serializable]
    public class Parabola : HighOrderEquation
    {
        public Parabola(double a, double b, double c = 0)
        {
            Multiplier = new double[] { a, b, c };
        }
    }
}
