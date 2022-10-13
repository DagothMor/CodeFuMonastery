using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
            if (text.Count() == 2) return text[0] == text[1];
            if (text.Count() == 1) return true;
            if (text[0] == text[text.Length - 1])
                return IsPalyndrome(text.Substring(1, text.Length - 2));
            return false;
        }

        // 5. Печать только чётных значений из списка.
        public static void WriteOnlyEvenValuesFromList(List<int> list)
        {
            if (list.Count == 0) return;
            if (list[0] % 2 == 0)
                Console.WriteLine(list[0]);
            WriteOnlyEvenValuesFromList(list.Skip(1).ToList());
        }
        // 6. Печать элементов списка с чётными индексами; 
        public static void WriteOnlyEvenIndexesFromList(List<int> list)
        {
            if (list.Count == 0) return;
            if (list.Count == 1)
            {
                Console.WriteLine(list[0]);
                return;
            }
            Console.WriteLine(list[0]);
            WriteOnlyEvenIndexesFromList(list.Skip(2).ToList());
        }

        // 7. нахождение второго максимального числа в списке
        // (с учётом, что максимальных может быть несколько, если они равны);  
        public static int FindSecondMaxNumber(List<int> list)
        {
            return list.Any() ? SecondNumberSearch(list.Skip(1).ToList(), list[0], list[0]) : 0;
        }
        public static int SecondNumberSearch(List<int> list, int firstMaxNumber, int secondMaxNumber)
        {
            if (list.Count == 0) return secondMaxNumber;

            if (firstMaxNumber < list[0])
            {
                secondMaxNumber = firstMaxNumber;
                firstMaxNumber = list[0];
                return SecondNumberSearch(list.Skip(1).ToList(), firstMaxNumber, secondMaxNumber);
            }
            if (secondMaxNumber < list[0] && firstMaxNumber != list[0])
            {
                secondMaxNumber = list[0];
                return SecondNumberSearch(list.Skip(1).ToList(), firstMaxNumber, secondMaxNumber);
            }
            return SecondNumberSearch(list.Skip(1).ToList(), firstMaxNumber, secondMaxNumber);
        }

        // поиск всех файлов в заданном каталоге,
        // включая файлы, расположенные в подкаталогах произвольной вложенности.
        public static List<string> FindFiles(string startCatalog, string fileSearchName)
        {
            var foundedFiles = new List<string>();
            BFS(startCatalog, ref foundedFiles, fileSearchName);
            return foundedFiles;
        }

        public static void BFS(string catalog, ref List<string> filesFounded, string fileSearchName)
        {
            var folders = Directory.GetDirectories(catalog);
            var filesInFolder = Directory.GetFiles(catalog);
            foreach (var file in filesInFolder)
            {
                if (Path.GetFileName(file) == fileSearchName)
                    filesFounded.Add(file);
            }
            foreach (var folder in folders)
            {
                BFS(folder, ref filesFounded, fileSearchName);
            }
        }

        // Генерация всех корректных сбалансированных
        // комбинаций круглых скобок (параметр -- количество открывающих скобок).  
        public static List<string> GenerateAllVariantsOfOpenBrackets(int count)
        {

            return new List<string>();
        }
    }
}
