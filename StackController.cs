
namespace CodeFuMonastery
{
	public static class StackController
	{
		public static bool BalancedRoundBrackets(string stringin)
		{
			var stack = new AlgorithmsDataStructures.Stack<char>();
			foreach (var bracket in stringin)
			{
				if (bracket == ')')
				{
					if (stack.Peek() == default)
						return false;
					stack.Pop();
					continue;
				}
				stack.Push(bracket);
			}
			return stack.Size() == 0;
		}
	}
}
