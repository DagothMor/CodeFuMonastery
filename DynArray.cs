using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

	public class DynArray<T>
	{
        private const int START_CAPACITY = 16;
        private const int MULTIPLY_CAPACITY = 2;
        private const double DIVIDE_CAPACITY = 1.5;

        public T[] array;
		public int count;
		public int capacity;

		public DynArray()
		{
			count = 0;
			MakeArray(START_CAPACITY);
		}

		public void MakeArray(int new_capacity)
		{
			if (new_capacity < START_CAPACITY) new_capacity = START_CAPACITY;

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
			if (count == capacity) MakeArray(capacity * MULTIPLY_CAPACITY);
			array[count] = itm;
			count++;
		}

		public void Insert(T itm, int index)
		{
			if (index > count || index < 0) throw new ArgumentOutOfRangeException();

			if (count + 1 > capacity) MakeArray(capacity * MULTIPLY_CAPACITY);

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
			if (capacity > START_CAPACITY && count - 2 <= (int)capacity / MULTIPLY_CAPACITY) MakeArray((int)(capacity / DIVIDE_CAPACITY));

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