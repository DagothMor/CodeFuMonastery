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

			var ResultLinkedList = new LinkedList2(); // Более понятно будет, если назвать переменную Result а не Out.

			var HeadOfFirstLinkedList = firstLinkedList.head; // Не первая голова, а голова первого списка.
			var HeadOfSecondLinkedList = secondLinkedList.head; // Аналогично выше.

			while (HeadOfFirstLinkedList != null || HeadOfSecondLinkedList != null)
			{
				ResultLinkedList.AddInTail(new Node(HeadOfFirstLinkedList.value + HeadOfSecondLinkedList.value));
				HeadOfFirstLinkedList = HeadOfFirstLinkedList.next;
				HeadOfSecondLinkedList = HeadOfSecondLinkedList.next;
			}
			return ResultLinkedList;


		}

		public static bool TwoLinkedListsAreEqual(LinkedList2 firstLinkedList, LinkedList2 secondLinkedList)
		{
			var HeadOfFirstLinkedList = firstLinkedList.head;
			var HeadOfSecondLinkedList = secondLinkedList.head;

			while (HeadOfFirstLinkedList != null || HeadOfSecondLinkedList != null)
			{
				if (HeadOfFirstLinkedList.value != HeadOfSecondLinkedList.value) return false;
				HeadOfFirstLinkedList = HeadOfFirstLinkedList.next;
				HeadOfSecondLinkedList = HeadOfSecondLinkedList.next;
			}
			return true;
		}
	}
}
