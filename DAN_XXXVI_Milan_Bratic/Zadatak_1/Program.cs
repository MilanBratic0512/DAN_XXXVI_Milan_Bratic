using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        private static readonly object locker = new object();
        static int[] numbers = new int[10000];
        static Random rnd = new Random();

        static void Matrix()
        {
            lock (locker)
            {
                int[,] matrix = new int[100, 100];
                Monitor.Wait(locker);
                int index = 0;
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        matrix[i, j] = numbers[index];
                        index++;
                    }
                }
            }
        }

        static void GenerateNumber()
        {
            lock (locker)
            {
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = rnd.Next(10, 100);
                }
                Monitor.Pulse(locker);
            }
        }
        static void Main(string[] args)
        {
            Thread thread1 = new Thread(Matrix);
            Thread thread2 = new Thread(GenerateNumber);
            thread1.Start();
            thread2.Start();
        }
    }
}
