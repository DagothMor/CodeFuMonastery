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

            //Console.WriteLine(Recursion.IsPalyndrome("010"));
            //Console.WriteLine(Recursion.IsPalyndrome("0120"));
            //Console.WriteLine(Recursion.IsPalyndrome("012"));
            //Console.WriteLine(Recursion.IsPalyndrome("01010"));
            //Console.WriteLine(Recursion.IsPalyndrome("abcdedcba"));
            //var list = new List<int>() {1,1,2,4,6,1,1,8,8,10,19,1,2 };
            //var listEvenIndexes = new List<int>() { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20 };
            //Recursion.WriteOnlyEvenValuesFromList(list);
            //Recursion.WriteOnlyEvenIndexesFromList(listEvenIndexes);
            var list = new List<int>() { -1,10,11,0,1,2,2,3};
            Console.WriteLine(Recursion.FindSecondMaxNumber(list,null,null)); 
            Console.ReadKey();
        }
    }
}
