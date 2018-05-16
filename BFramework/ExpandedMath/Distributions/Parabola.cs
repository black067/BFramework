
namespace BFramework.ExpandedMath.Distributions
{
    public class Parabola : HighOrderEquation
    {
        public Parabola(int a, int b, int c = 0)
        {
            Order = new int[] { a, b, c };
        }
    }
}
