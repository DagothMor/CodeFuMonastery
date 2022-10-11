using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFuMonastery.Recursion
{
    
    public static class Recursion
    {
        // возведение числа N в степень M;
        public static int Exponentiation( int number, int power)
        {
            if (power == 0 || number == 1) return 1;
            if(power == 1) return number;
            power -= 1;
            return number * Exponentiation(number, power);
        }

        // вычисление суммы цифр числа;
        public static int SumOfDigitsInNumber(string number) 
        {
            if (number.Length == 0) return 0;
            return int.Parse(number[0].ToString()) + SumOfDigitsInNumber(number.Substring(1,number.Length-1));
        }
    }
}
