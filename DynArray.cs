using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures
{

	public class DynArray<T>
	{
		public T[] array;
		public int count;
		public int capacity;

		public DynArray()
		{
			count = 0;
			MakeArray(16);
		}

		public void MakeArray(int new_capacity)
		{
			if (new_capacity < 16) new_capacity = 16;

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
			if (index > count || index < 0) throw new ArgumentOutOfRangeException();
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

			if (capacity > 16 && count - 2 <= (int)capacity / 2) MakeArray((int)(capacity / 1.5));

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