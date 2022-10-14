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
            if (text.Count() < 2) return true;
            if (text[0] == text[text.Length - 1])
                return IsPalyndrome(text.Substring(1, text.Length - 2));
            return false;
        }

        // 5. Печать только чётных значений из списка.
        public static void WriteOnlyEvenValuesFromList(List<int> list, int index)
        {
            if (index >= list.Count) return;
            if (list[index] % 2 == 0)
                Console.WriteLine(list[index]);
            WriteOnlyEvenValuesFromList(list, index + 1);
        }
        // 6. Печать элементов списка с чётными индексами; 
        public static void WriteOnlyEvenIndexesFromList(List<int> list, int index)
        {
            if (index > list.Count) return;
            if (index % 2 == 0)
            {
                Console.WriteLine(list[index]);
            }
            WriteOnlyEvenIndexesFromList(list, index + 2);
        }

        // 7. нахождение второго максимального числа в списке
        // (с учётом, что максимальных может быть несколько, если они равны);  
        public static int FindSecondMaxNumber(List<int> list)
        {
            if (list.Count > 1) 
            {
                var min = Math.Min(list[0], list[1]);
                var max = Math.Max(list[0], list[1]);
                return SecondNumberSearch(list, max, min, 0);
            }
            return 0;
        }
        public static int SecondNumberSearch(List<int> list, int firstMaxNumber, int secondMaxNumber, int index)
        {
            if (index == list.Count) return secondMaxNumber;

            if (firstMaxNumber < list[index])
            {
                secondMaxNumber = firstMaxNumber;
                firstMaxNumber = list[index];
                return SecondNumberSearch(list, firstMaxNumber, secondMaxNumber, index + 1);
            }
            if (secondMaxNumber < list[index] && firstMaxNumber != list[index])
            {
                secondMaxNumber = list[index];
                return SecondNumberSearch(list, firstMaxNumber, secondMaxNumber, index + 1);
            }
            return SecondNumberSearch(list, firstMaxNumber, secondMaxNumber, index + 1);
        }

        // поиск всех файлов в заданном каталоге,
        // включая файлы, расположенные в подкаталогах произвольной вложенности.
        public static List<string> FindFiles(string startCatalog, string fileSearchName)
        {
            var foundedFiles = new List<string>();
            foundedFiles.AddRange(DFS(startCatalog,fileSearchName));
            return foundedFiles;
        }

        public static List<string> DFS(string catalog, string fileSearchName)
        {
            var folders = Directory.GetDirectories(catalog);
            var filesInFolder = Directory.GetFiles(catalog);
            var list = new List<string>();
            foreach (var folder in folders)
            {
                list.AddRange(DFS(folder, fileSearchName));
            }
            foreach (var file in filesInFolder)
            {
                if (Path.GetFileName(file) == fileSearchName)
                    list.Add(file);
            }
            return list;
        }

        // Генерация всех корректных сбалансированных
        // комбинаций круглых скобок (параметр -- количество открывающих скобок).  
        public static List<string> GenerateAllVariantsOfOpenBrackets(int count)
        {

            return new List<string>();
        }
    }
}
