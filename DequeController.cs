using AlgorithmsDataStructures;

namespace CodeFuMonastery
{
	public static class DequeController
	{
		public static bool IsPalindrome(string word)
		{
			var deque = StringToDeque(word);
			char head, tail;
			while (deque.Size() > 1) 
			{
				head = deque.RemoveFront();
				tail = deque.RemoveTail();
				if (head != tail) return false;
			}
			return true;
		}
		private static Deque<char> StringToDeque(string word)
		{
			var deque = new Deque<char>();
			foreach (var c in word) { deque.AddTail(c); }
			return deque;
		}
	}
}
