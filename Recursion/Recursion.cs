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
        public static int Exponentiation(int number, int power)
        {
            if (power == 0 || number == 1) return 1;
            if (power == 1) return number;
            power -= 1;
            return number * Exponentiation(number, power);
        }

        // вычисление суммы цифр числа;
        public static int SumOfDigitsInNumber(string number)
        {
            if (number.Length == 0) return 0;
            return int.Parse(number[0].ToString()) + SumOfDigitsInNumber(number.Substring(1, number.Length - 1));
        }

        // расчёт длины списка, для которого
        // разрешена только операция удаления первого элемента pop(0)
        // (и получение длины конечно);  
        public static int LengthOfList<T>(Stack<T> stack)
        {
            if (stack.Count == 0) return 0;
            stack.Pop();
            return 1 + LengthOfList(stack);
        }
        // проверка, является ли строка палиндромом;
        public static bool IsPalyndrome(string text)
        {
            if (text.Count() == 3) return text[0] == text[text.Length - 1];
            if (text.Count() == 2) return false;
            if (text.Count() == 1) return true;
            if (text[0] == text[text.Length - 1])
                return IsPalyndrome(text.Substring(1, text.Length - 2));
            return false;
        }

    }
}
