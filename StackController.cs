
using System;

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

		public static int PostfixNotationOfAnExpressionThroughString(string stringin)
		{
			//var s1 = new AlgorithmsDataStructures.Stack<int>();
			var s2 = new AlgorithmsDataStructures.Stack<int>();


			foreach (var ch in stringin)
			{
				if (char.IsDigit(ch))
				{
					s2.Push(Int32.Parse(ch.ToString()));
				}

				if (ch == '+')
				{
					s2.Push(sum(s2.Pop(),s2.Pop()));
				}

				if (ch == '*')
				{
					s2.Push(multiply(s2.Pop(), s2.Pop()));
				}

				if (ch == '=')
				{
					return s2.Peek();
				}
			}
			return s2.Peek();
			
		}

		public static int PostfixNotationOfAnExpression(AlgorithmsDataStructures.Stack<char> stackin)
		{
			//var s1 = new AlgorithmsDataStructures.Stack<int>();
			var s2 = new AlgorithmsDataStructures.Stack<int>();

			char buffer;

			while (stackin.Size() > 0 ) {
				buffer = stackin.Pop();
				if (char.IsDigit(buffer))
				{
					s2.Push(Int32.Parse(buffer.ToString()));
				}

				if (buffer == '+')
				{
					s2.Push(sum(s2.Pop(), s2.Pop()));
				}

				if (buffer == '*')
				{
					s2.Push(multiply(s2.Pop(), s2.Pop()));
				}

				if (buffer == '=')
				{
					return s2.Peek();
				}
			}
			return s2.Peek();

		}
		public static int sum(int a, int b) => a + b;
		public static int multiply(int a, int b) => a * b;
	}
}
