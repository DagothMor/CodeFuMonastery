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

			var ResultLinkedList = new LinkedList2();
			// Результат связного списка.

			var HeadNodeOfFirstLinkedList = firstLinkedList.head;
			// Головная нода первого связного списка.
			var HeadNodeOfSecondLinkedList = secondLinkedList.head;

			while (HeadNodeOfFirstLinkedList != null || HeadNodeOfSecondLinkedList != null)
			{
				ResultLinkedList.AddInTail(new Node(HeadNodeOfFirstLinkedList.value + HeadNodeOfSecondLinkedList.value));
				HeadNodeOfFirstLinkedList = HeadNodeOfFirstLinkedList.next;
				HeadNodeOfSecondLinkedList = HeadNodeOfSecondLinkedList.next;
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
