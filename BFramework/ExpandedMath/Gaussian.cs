using System;
using System.Collections.Generic;

namespace BFramework.ExpandedMath
{
    [Serializable]
    public class Gaussian
    {
        public Gaussian(double mean, double standardDeviation)
        {
            Mean = mean;
            StandardDeviation = standardDeviation;
            Variance = StandardDeviation * StandardDeviation;
            _leftPart = 1 / (Math.Pow(2 * PI, 0.5) * StandardDeviation);
            _partOfExponential = - 1 / (2 * Variance);
        }

        private const double PI = Math.PI;

        public double Mean { get; set; }

        public double StandardDeviation { get; set; }

        public double Variance { get; set; }

        private double _leftPart { get; set; }

        private double _partOfExponential { get; set; }

        private double GetLastPart(double x)
        {
            double r = x - Mean;
            return r * r;
        }

        public override bool Equals(object obj)
        {
            var gaussian = obj as Gaussian;
            return gaussian != null &&
                   Mean == gaussian.Mean &&
                   StandardDeviation == gaussian.StandardDeviation;
        }

        public override int GetHashCode()
        {
            var hashCode = 1081252967;
            hashCode = hashCode * -1521134295 + Mean.GetHashCode();
            hashCode = hashCode * -1521134295 + StandardDeviation.GetHashCode();
            return hashCode;
        }

        public float this[double x]
        {
            get
            {
                return (float)(_leftPart * Math.Exp(_partOfExponential * GetLastPart(x)));
            }
        }

        public int this[int x]
        {
            get
            {
                return (int)Math.Round(_leftPart * Math.Exp(_partOfExponential * GetLastPart(x)));
            }
        }

        public static bool operator ==(Gaussian gaussian1, Gaussian gaussian2)
        {
            return EqualityComparer<Gaussian>.Default.Equals(gaussian1, gaussian2);
        }

        public static bool operator !=(Gaussian gaussian1, Gaussian gaussian2)
        {
            return !(gaussian1 == gaussian2);
        }
    }
}
