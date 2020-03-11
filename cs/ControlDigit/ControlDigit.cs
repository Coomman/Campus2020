using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class Extensions
    {
        public static List<int> GetDigitArray(this long n)
        {
            var digits = new List<int>(n == 0L ? 1 : (n > 0L ? 1 : 2) + (int)Math.Log10(Math.Abs((double)n)));
            do
            {
                digits.Add((int)(n % 10));
                n /= 10;
            }
            while (n > 0);

            return digits;
        }

        public static List<int> GetAlternatingArray(this int number, int length, Func<int, int> changeFunc)
        {
            var arr = new int[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = number;
                number = changeFunc(number);
            }

            return arr.ToList();
        }

        public static int InnerProduct(this List<int> digits, List<int> weights, Func<int, int, int> singleOp, Func<int, int, int> rangeOp)
        {
            return digits.Zip(weights, singleOp).Aggregate(rangeOp);
        }
    }

	public static class ControlDigit
	{
        public static int Upc(this long number)
        {
            var digits = number.GetDigitArray();
            var weights = 3.GetAlternatingArray(digits.Count, m => 4 - m);

			int checkDigit = digits.InnerProduct(weights, ((d, m) => d * m), ((x, y) => x + y)) % 10;

            return checkDigit == 0
                ? 0
                : 10 - checkDigit;
		}

        public static int Isbn13(this long n)
        {
            var digits = n.GetDigitArray();
            var weights = 1.GetAlternatingArray(digits.Count, m => 4 - m);

            int checkDigit = digits.InnerProduct(weights, ((d, m) => d * m), ((x, y) => x + y)) % 10;

            return checkDigit == 0
                ? 0
                : 10 - checkDigit;
        }

        public static int Isbn10(this long n)
        {
            var digits = n.GetDigitArray();
            var weights = 2.GetAlternatingArray(digits.Count, (m => ++m));

            int checkDigit = digits.InnerProduct(weights, ((d, m) => d * m), ((x, y) => x + y)) % 11;

            if (checkDigit == 0) return '0';
            if (checkDigit == 1) return 'X';
            return (11 - checkDigit).ToString()[0];
        }

        public static int Snils(this long n)
        {
            var digits = n.GetDigitArray();
            var weights = 1.GetAlternatingArray(digits.Count, (m => ++m));

            int checkSum = digits.InnerProduct(weights, ((d, m) => d * m), ((x, y) => x + y)) % 101;

            return checkSum == 100
                ? 0
                : checkSum;
        }
    }
}
