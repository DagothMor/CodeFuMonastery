
using System;

namespace CodeFuMonastery
{
    public static class StackController
    {
        public static bool BalancedRoundBrackets(string stringin)
        {
            var stack = new AlgorithmsDataStructures.Stack<char>();
            // 7.1 stackIsEmpty.
            var stackIsEmpty = true;
            foreach (var bracket in stringin)
            {
                if (bracket == ')')
                {
                    stackIsEmpty = stack.Size() == 0;
                    if (stackIsEmpty)
                        return false;
                    stack.Pop();
                    continue;
                }
                stack.Push(bracket);
            }
            stackIsEmpty = stack.Size() == 0;
            return stackIsEmpty;
        }

        public static float PostfixNotationOfAnExpressionThroughString(string stringin)
        {
            var s2 = new AlgorithmsDataStructures.Stack<float>();

            float first, second;

            foreach (var ch in stringin)
            {
                if (char.IsDigit(ch))
                {
                    s2.Push(float.Parse(ch.ToString()));
                    continue;
                }

                if (ch == '=')
                {
                    return s2.Peek();
                }

                if (ch == ' ' || char.IsLetter(ch) )
                {
                    continue;
                }

                second = s2.Pop();
                first = s2.Pop();

                if (ch == '+')
                {
                    s2.Push(sum(first, second));
                }

                if (ch == '-')
                {
                    s2.Push(sub(first, second));
                }

                if (ch == '/')
                {
                    s2.Push(div(first, second));
                }

                if (ch == '*')
                {
                    s2.Push(multiply(first, second));
                }
            }
            return s2.Peek();

        }

        public static float PostfixNotationOfAnExpression(AlgorithmsDataStructures.Stack<char> stackin)
        {
            var s2 = new AlgorithmsDataStructures.Stack<float>();

            char buffer;
            
            float first, second;

            while (stackin.Size() > 0)
            {
                buffer = stackin.Pop();

                if (char.IsDigit(buffer))
                {
                    s2.Push(float.Parse(buffer.ToString()));
                    continue;
                }

                if (buffer == '=')
                {
                    return s2.Peek();
                }

                second = s2.Pop();
                first = s2.Pop();

                if (buffer == '+')
                {
                    s2.Push(sum(first, second));
                }

                if (buffer == '-')
                {
                    s2.Push(sub(first, second));
                }

                if (buffer == '*')
                {
                    s2.Push(multiply(first, second));
                }
                if (buffer == '/')
                {
                    s2.Push(div(first, second));
                }
            }
            return s2.Peek();

        }
        public static float sum(float a, float b) => a + b;
        public static float sub(float a, float b) => a - b;
        public static float multiply(float a, float b) => a * b;
        public static float div(float a, float b) => a / b;
    }
}
