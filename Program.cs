using CodeFuMonastery.Recursion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsDataStructures
{
    class Program
    {
        static void Main(string[] args)
        {
            //         Console.WriteLine(Recursion.Exponentiation(1, 1));
            //Console.WriteLine(Recursion.Exponentiation(2, 1));
            //Console.WriteLine(Recursion.Exponentiation(2, 2));
            //Console.WriteLine(Recursion.Exponentiation(2, 4));
            //Console.WriteLine(Recursion.Exponentiation(3, 2));
            //Console.WriteLine(Recursion.Exponentiation(3, 3));
            //Console.WriteLine(Recursion.Exponentiation(0, 1));
            //Console.WriteLine(Recursion.Exponentiation(0, 0));
            //Console.WriteLine(Recursion.SumOfDigitsInNumber("0"));
            //Console.WriteLine(Recursion.SumOfDigitsInNumber("1"));
            //Console.WriteLine(Recursion.SumOfDigitsInNumber("2"));
            //Console.WriteLine(Recursion.SumOfDigitsInNumber("123"));
            //Console.WriteLine(Recursion.SumOfDigitsInNumber("1234"));
            //Console.WriteLine(Recursion.SumOfDigitsInNumber("1235"));
            //Console.WriteLine(Recursion.SumOfDigitsInNumber("999"));
            //Console.WriteLine(Recursion.SumOfDigitsInNumber("333"));

            //var stack1 = new System.Collections.Generic.Stack<int>();

            //Console.WriteLine(Recursion.LengthOfList<int>(stack1));

            Console.WriteLine(Recursion.IsPalyndrome("010"));
            Console.WriteLine(Recursion.IsPalyndrome("0120"));
            Console.WriteLine(Recursion.IsPalyndrome("012"));
            Console.WriteLine(Recursion.IsPalyndrome("01010"));
            Console.WriteLine(Recursion.IsPalyndrome("abcdedcba"));

            Console.ReadKey();
        }
    }
}
