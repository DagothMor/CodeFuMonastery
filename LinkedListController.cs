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

			var FirstHead = firstLinkedList.head;
			var secondHead = secondLinkedList.head;

			while (FirstHead != null || secondHead != null)
			{
				ReturnLinkedList.AddInTail(new Node(FirstHead.value + secondHead.value));
				FirstHead = FirstHead.next;
				secondHead = secondHead.next;
			}
			return ReturnLinkedList;


		}

		public static bool TwoLinkedListsAreEqual(LinkedList firstLinkedList, LinkedList secondLinkedList)
		{
			var FirstHead = firstLinkedList.head;
			var secondHead = secondLinkedList.head;

			while (FirstHead != null || secondHead != null)
			{
				if (FirstHead.value != secondHead.value) return false;
				FirstHead = FirstHead.next;
				secondHead = secondHead.next;
			}
			return true;
		}
	}
}
