using System;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static int SumProducts(this long number, int multiplier, Func<int, int> changeMult)
        {
            int sum = 0;
            do
            {
                var digit = (int)(number % 10);
                sum += multiplier * digit;
                multiplier = changeMult(multiplier);
                number /= 10;
            } 
            while (number > 0);

            return sum;
        }
    }

	public static class ControlDigitAlgo
	{
        public static int Upc(this long number)
        {
			int checkDigit = number.SumProducts(3, (m => 4 - m)) % 10;

            return checkDigit == 0
                ? 0
                : 10 - checkDigit;
		}

        public static int Isbn13(this long number)
        {
			int checkDigit = number.SumProducts(1, (m => 4 - m)) % 10;

            return checkDigit == 0
                ? 0
                : 10 - checkDigit;
        }

        public static int Isbn10(this long number)
        {
            int checkDigit = number.SumProducts(2, (m => ++m)) % 11;

            if (checkDigit == 0) return '0';
            if (checkDigit == 1) return 'X';
            return (11 - checkDigit).ToString()[0];
        }
    }
}
