using AlgorithmsDataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFuMonastery
{
	public static class QueueController
	{
		public static void CircleShiftQueue<T>(AlgorithmsDataStructures.Queue<T> queue,int steps)
		{
			for (int i = steps; i > 0; i--)
			{
				queue.Enqueue(queue.Dequeue());
			}
		}
	}
}
