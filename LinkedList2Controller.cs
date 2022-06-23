using AlgorithmsDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFuMonastery
{
    public static class LinkedList2Controller
    {
        public static LinkedList2 SumTwoLinkedLists(LinkedList2 firstLinkedList, LinkedList2 secondLinkedList)
        {
            if (firstLinkedList.Count() != secondLinkedList.Count()) return new LinkedList2();

            var result = new LinkedList2();
            // Результат связного списка.

            // 6.4 было HeadNodeOfFirstLinkedList стало HeadOfFirstLinkedList
            // Головная нода первого связного списка.
            var headOfFirst = firstLinkedList.head;

            var headOfSecond = secondLinkedList.head;

            while (headOfFirst != null || headOfSecond != null)
            {
                result.AddInTail(new Node(headOfFirst.value + headOfSecond.value));
                headOfFirst = headOfFirst.next;
                headOfSecond = headOfSecond.next;
            }
            return result;


        }

        public static bool TwoLinkedListsAreEqual(LinkedList2 firstLinkedList, LinkedList2 secondLinkedList)
        {
            // 7.1 twoLinkedListsAreEqual Добавил переменную для удобочитаемости
            var twoLinkedListsAreEqual = true;
            var headOfFirst = firstLinkedList.head;
            var headOfSecond = secondLinkedList.head;

            while (headOfFirst != null || headOfSecond != null)
            {
                if (headOfFirst.value != headOfSecond.value)
                {
                    twoLinkedListsAreEqual = false;
                    return twoLinkedListsAreEqual;
                }
                headOfFirst = headOfFirst.next;
                headOfSecond = headOfSecond.next;
            }
            return twoLinkedListsAreEqual;
        }
    }
}
