using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsDataStructures
{

	public class Node<T>
	{
		public T value;
		public Node<T> next, prev;

		public Node(T _value)
		{
			value = _value;
			next = null;
			prev = null;
		}
	}

	public class OrderedList<T>
	{
		public Node<T> head, tail;
		private bool _ascending;

		public OrderedList(bool asc)
		{
			head = null;
			tail = null;
			_ascending = asc;
		}

		public int Compare(T v1, T v2)
		{
			int result = 0;
			if (typeof(T) == typeof(String))
			{
				result = FirstValueIsBiggerByLexicographicOrder(v1.ToString().Trim(), v2.ToString().Trim());
			}
			else
			{
				var intFirstVar = Int32.Parse(v1.ToString());
				var intSecondVar = Int32.Parse(v2.ToString());
				if (intFirstVar < intSecondVar) result = -1;
				if (intFirstVar > intSecondVar) result = 1;
			}
			return result;
		}

		public void Add(T value)
		{
			var nodeWithInputedValue = new Node<T>(value);
			// Узел с входящим значением.
			var node = head;
			int isAscending = _ascending ? 1 : -1;
			if (node == null)
			{
				head = nodeWithInputedValue;
				head.next = null;
				head.prev = null;
				tail = nodeWithInputedValue;
				return;
			}
				if (Compare(head.value, nodeWithInputedValue.value) == isAscending)
				{
					nodeWithInputedValue.next = node;
					node.prev = nodeWithInputedValue;
					head = nodeWithInputedValue;
					return;
				}
				while (node.next != null)
				{
					if (Compare(node.value, nodeWithInputedValue.value) == isAscending)
					{
						nodeWithInputedValue.prev = node.prev;
						nodeWithInputedValue.next = node;
						node.prev.next = nodeWithInputedValue;
						node.prev = nodeWithInputedValue;
						return;
					}
					node = node.next;
				}
				if (Compare(node.value, nodeWithInputedValue.value) == isAscending)
				{
					nodeWithInputedValue.prev = node.prev;
					nodeWithInputedValue.next = node;
					node.prev.next = nodeWithInputedValue;
					node.prev = nodeWithInputedValue;
					return;
				}
				tail.next = nodeWithInputedValue;
				nodeWithInputedValue.prev = tail;
				tail = nodeWithInputedValue;
				return;
		}

		public Node<T> Find(T val)
		{
			var nodeWithInputedValue = new Node<T>(val);
			if (this._ascending == true && Compare(head.value, val) == 1) return null;
			if (this._ascending == true && Compare(tail.value, val) == -1) return null;
			if (this._ascending == false && Compare(head.value, val) == -1) return null;
			if (this._ascending == false && Compare(tail.value, val) == 1) return null;
			var node = BinarySearch(GetAll(), val);
			return node;
		}

		public void Delete(T ValueToDelete)
		// Значение для удаления.
		{
			var node = head;
			while (node != null)
			{
				if (Compare(node.value, ValueToDelete) == 0)
				{
					if (node.prev == null)
					{
						head = node.next;
						node = head;
						if (node == null) tail = null;
						else node.prev = null;
						break;
					}
					if (node.next != null)
					{
						node.prev.next = node.next;
						node.next.prev = node.prev;
					}
					else
					{
						node.prev.next = null;
						tail = node.prev;
					}
					break;
				}
				node = node.next;
			}
		}

		public void Clear(bool asc)
		{
			_ascending = asc;
			head = null;
			tail = null;
		}

		public int Count()
		{
			Node<T> node = head;
			int count = 0;
			while (node != null)
			{
				count++;
				node = node.next;
			}
			return count;
		}

		List<Node<T>> GetAll()
		{
			List<Node<T>> resultListOfNodes = new List<Node<T>>();
			// результат в виде списка нод.
			Node<T> node = head;
			while (node != null)
			{
				resultListOfNodes.Add(node);
				node = node.next;
			}
			return resultListOfNodes;
		}
		private int FirstValueIsBiggerByLexicographicOrder(string firstValue, string secondValue)
		{
			// 7.3 Вместо i нагляднее всего будет использовать charIndex
			var isAscending = this._ascending?1:-1;
			if (this._ascending)
			{
				if (firstValue == secondValue) return 0;
				if (firstValue.Length > secondValue.Length)
				{
					for (int charIndex = 0; charIndex < secondValue.Length; charIndex++)
					{
						if (secondValue[charIndex] == firstValue[charIndex]) continue;
						if (secondValue[charIndex] < firstValue[charIndex]) return 1;
						if (secondValue[charIndex] > firstValue[charIndex]) return -1;
					}
					return 1;
				}
				for (int charIndex = 0; charIndex < firstValue.Length; charIndex++)
				{
					if (secondValue[charIndex] == firstValue[charIndex]) continue;
					if (secondValue[charIndex] < firstValue[charIndex]) return 1;
					if (secondValue[charIndex] > firstValue[charIndex]) return -1;
				}
				return -1;
			}
			else 
			{
				if (firstValue == secondValue) return 0;
				if (firstValue.Length > secondValue.Length)
				{
					for (int charIndex = 0; charIndex < secondValue.Length; charIndex++)
					{
						if (secondValue[charIndex] == firstValue[charIndex]) continue;
						if (secondValue[charIndex] < firstValue[charIndex]) return 1;
						if (secondValue[charIndex] > firstValue[charIndex]) return -1;
					}
					return 1;
				}
				for (int charIndex = 0; charIndex < firstValue.Length; charIndex++)
				{
					if (secondValue[charIndex] == firstValue[charIndex]) continue;
					if (secondValue[charIndex] < firstValue[charIndex]) return 1;
					if (secondValue[charIndex] > firstValue[charIndex]) return -1;
				}
				return -1;
			}
		}

		private Node<T> BinarySearch(List<Node<T>> inputedListOfNodes, T inputedValue)
		// Искомое значение.
		{
			if (inputedListOfNodes.Count == 0) return null;
			var startPointOfAreaSearch = 0;
			// 7.4 начальная точка площади поиска
			var endPointOfAreaSearch = inputedListOfNodes.Count() - 1;
			// 7.4 конечная точка площади поиска
			int middlePointOfAreaSearch;
			// медианная точка площади поиска
			int isAscending = this._ascending ? 1 : -1;

			int CompareMiddlePointOfAreaSearchOfInputedListWithInputedValue;
			// Сравнение значения медианной точки площади поиска во входящем списке с искомым значением.

			while (startPointOfAreaSearch <= endPointOfAreaSearch)
			{
				middlePointOfAreaSearch = (int)((startPointOfAreaSearch + endPointOfAreaSearch) / 2);
				CompareMiddlePointOfAreaSearchOfInputedListWithInputedValue = Compare(inputedListOfNodes[middlePointOfAreaSearch].value, inputedValue);
				if (CompareMiddlePointOfAreaSearchOfInputedListWithInputedValue == 0)
				{
					return inputedListOfNodes[middlePointOfAreaSearch];
				}
				if (CompareMiddlePointOfAreaSearchOfInputedListWithInputedValue == isAscending) endPointOfAreaSearch = middlePointOfAreaSearch - 1;
				if (CompareMiddlePointOfAreaSearchOfInputedListWithInputedValue == -isAscending) startPointOfAreaSearch = middlePointOfAreaSearch + 1;
			}
			return null;
		}
	}

}