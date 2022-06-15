using AlgorithmsDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFuMonastery
{
	public static class LinkedListController
	{
		public static LinkedList SumTwoLinkedLists(LinkedList firstLinkedList, LinkedList secondLinkedList)
		{
			if (firstLinkedList.Count() != secondLinkedList.Count()) return new LinkedList();

			var ReturnLinkedList = new LinkedList();

			var HeadOfFirstLinkedList = firstLinkedList.head;
			var HeadOfSecondLinkedList = secondLinkedList.head;

			while (HeadOfFirstLinkedList != null || HeadOfSecondLinkedList != null)
			{
				ReturnLinkedList.AddInTail(new Node(HeadOfFirstLinkedList.value + HeadOfSecondLinkedList.value));
				HeadOfFirstLinkedList = HeadOfFirstLinkedList.next;
				HeadOfSecondLinkedList = HeadOfSecondLinkedList.next;
			}
			return ReturnLinkedList;


		}

		public static bool TwoLinkedListsAreEqual(LinkedList firstLinkedList, LinkedList secondLinkedList)
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
