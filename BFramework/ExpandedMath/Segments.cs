
namespace BFramework.ExpandedMath
{
    public class Segments
    {
        public Segments(params int[] args)
        {
            Count = args.Length;
            if (Count < 1)
            {
                NumberArray = new int[] { 0};
                Count = NumberArray.Length;
                return;
            }
            for (int i = 1; i < Count; i++)
            {
                int temp = args[i], j;
                for (j = i - 1; j >= 0 && temp < args[j]; j--)
                {
                    args[j + 1] = args[j];
                }
                args[j + 1] = temp;
            }
            NumberArray = args;
        }

        private int[] _numberArray;
        public int Count;
        public int Max
        {
            get
            {
                return NumberArray[Count - 1];
            }
        }
        public int Min
        {
            get
            {
                return NumberArray[0];
            }
        }

        public int[] NumberArray
        {
            get
            {
                return _numberArray;
            }
            set
            {
                _numberArray = value;
            }
        }

        public int this[int value]
        {
            get
            {
                int result = 0;
                for (int i = 0; i < Count - 1; i++)
                {
                    if (value >= NumberArray[i] && value < NumberArray[i + 1])
                    {
                        result = i + 1;
                        return result;
                    }
                }
                if (value >= NumberArray[Count - 1])
                {
                    return Count + 1;
                }
                return result;
            }
        }

        public override string ToString()
        {
            string result = "Segments(";
            int length = NumberArray.Length;
            for (int i = 0; i < length - 1; i++)
            {
                result += NumberArray[i] + ", ";
            }
            result += NumberArray[length - 1] + ")";
            return result;
        }
    }
}
