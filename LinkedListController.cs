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

			var linkedListOut = new LinkedList();

			var firstHead = firstLinkedList.head;
			var secondHead = secondLinkedList.head;

			while (firstHead != null || secondHead != null)
			{
				linkedListOut.AddInTail(new Node(firstHead.value + secondHead.value));
				firstHead = firstHead.next;
				secondHead = secondHead.next;
			}
			return linkedListOut;


		}

		public static bool TwoLinkedListsAreEqual(LinkedList firstLinkedList, LinkedList secondLinkedList)
		{
			var firstHead = firstLinkedList.head;
			var secondHead = secondLinkedList.head;

			while (firstHead != null || secondHead != null)
			{
				if (firstHead.value != secondHead.value) return false;
				firstHead = firstHead.next;
				secondHead = secondHead.next;
			}
			return true;
		}
	}
}
