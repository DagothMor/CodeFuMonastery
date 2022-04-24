
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
					if (stack.Size() == 0)
						return false;
					stack.Pop();
					continue;
				}
				stack.Push(bracket);
			}
			return stack.Size() == 0;
		}

		public static float PostfixNotationOfAnExpressionThroughString(string stringin)
		{
			var s2 = new AlgorithmsDataStructures.Stack<float>();


			foreach (var ch in stringin)
			{
				if (char.IsDigit(ch))
				{
					s2.Push(float.Parse(ch.ToString()));
				}

				if (ch == '+')
				{
					s2.Push(sum(s2.Pop(),s2.Pop()));
				}

				if (ch == '-')
				{
					s2.Push(sub(s2.Pop(), s2.Pop()));
				}

				if (ch == '/')
				{
					s2.Push(div(s2.Pop(), s2.Pop()));
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

		public static float PostfixNotationOfAnExpression(AlgorithmsDataStructures.Stack<char> stackin)
		{
			var s2 = new AlgorithmsDataStructures.Stack<float>();

			char buffer;

			while (stackin.Size() > 0 ) {
				buffer = stackin.Pop();
				if (char.IsDigit(buffer))
				{
					s2.Push(float.Parse(buffer.ToString()));
				}

				if (buffer == '+')
				{
					s2.Push(sum(s2.Pop(), s2.Pop()));
				}

				if (buffer == '-')
				{
					s2.Push(sub(s2.Pop(), s2.Pop()));
				}
				if (buffer == '*')
				{
					s2.Push(multiply(s2.Pop(), s2.Pop()));
				}
				if (buffer == '/')
				{
					s2.Push(div(s2.Pop(), s2.Pop()));
				}

				if (buffer == '=')
				{
					return s2.Peek();
				}
			}
			return s2.Peek();

		}
		public static float sum(float a, float b) => a + b;
		public static float sub(float a, float b) => b - a;
		public static float multiply(float a, float b) => a * b;
		public static float div(float a, float b) => b / a;
	}
}
