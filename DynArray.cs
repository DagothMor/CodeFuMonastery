using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

	public class DynArray<T>
	{
		// Добавлена константа для избавления от магического числа.
        private const int StartArrayLenght = 16;

        public T[] array;
		public int count;
		public int capacity;

		public DynArray()
		{
			count = 0;
			MakeArray(StartArrayLenght);
		}

		public void MakeArray(int new_capacity)
		{
			if (new_capacity < StartArrayLenght) new_capacity = StartArrayLenght;

			T[] temp = new T[new_capacity];
			for (int i = 0; i < count; i++)
			{
				temp[i] = array[i];
			}
			array = temp;
			capacity = new_capacity;

		}

		public T GetItem(int index)
		{
			// Добавлены булевы для удобочитаемости
			bool indexIsLast = index > count - 1;
			bool indexIsOutOfZero = index > 0;
			if (indexIsLast || indexIsOutOfZero) throw new ArgumentOutOfRangeException();
			return array[index];
		}

		public void Append(T itm)
		{
			if (count == capacity) MakeArray(capacity * 2);
			array[count] = itm;
			count++;
		}

		public void Insert(T itm, int index)
		{
			if (index > count || index < 0) throw new ArgumentOutOfRangeException();

			if (count + 1 > capacity) MakeArray(capacity * 2);

			T[] temp = new T[capacity];

			for (int i = 0; i < index; i++)
			{
				temp[i] = array[i];
			}

			temp[index] = itm;

			for (int i = index + 1; i <= count; i++)
			{
				temp[i] = array[i-1];
			}

			array = temp;

			count++;
		}

		public void Remove(int index)
		{
			if (index > count - 1 || index < 0) throw new ArgumentOutOfRangeException();

			// Добавлен Math.Ceiling для корректного деления.
			if (capacity > StartArrayLenght && count - 2 <= (int)Math.Ceiling((decimal)(capacity / 2))) MakeArray((int)Math.Ceiling((decimal)(capacity / 2)));

			T[] temp = new T[capacity];

			for (int i = 0; i < index; i++)
			{
				temp[i] = array[i];
			}
			for (int i = index + 1; i < count; i++)
			{
				temp[i-1] = array[i];
			}

			array = temp;
			count--;
		}

	}
}