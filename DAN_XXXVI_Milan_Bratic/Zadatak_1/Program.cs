using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        //path to the file
        static string path = "../../MatrixFile.txt";
        private static readonly object locker = new object();
        //array of int for numbers, leter this array will fill the matrix
        static int[] numbers = new int[10000];
        static int[,] matrix;
        //list of odd numbers
        static List<int> oddNumbers = new List<int>();
        static Random rnd = new Random();

        /// <summary>
        /// method for initialization and fill matrix
        /// </summary>
        static void Matrix()
        {
            lock (locker)
            {
                matrix = new int[100, 100];
                //thread wait until the array is filled
                Monitor.Wait(locker);
                int index = 0;
                //fill the matrix with array of numbers 
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
        /// <summary>
        /// method for generate random numbers, and place to the array
        /// </summary>
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
        /// <summary>
        /// method for odd numbers and write to the file. Fill the list with odd numbers from the matrix, and place them to the file
        /// </summary>
        static void OddMatrixNumber()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (matrix[i, j] % 2 != 0)
                    {
                        oddNumbers.Add(matrix[i, j]);
                    }
                }
            }
            using (TextWriter tw = new StreamWriter(path))
            {
                for (int i = 0; i < oddNumbers.Count; i++)
                {
                    tw.WriteLine(oddNumbers[i]);
                }
            }
        }
        /// <summary>
        /// method for read numbers from the file
        /// </summary>
        static void ReadNumbersFromTheFile()
        {
            using (TextReader tr = new StreamReader(path))
            {
                Console.WriteLine(tr.ReadToEnd());
            }
        }
        static void Main(string[] args)
        {
            Thread thread1 = new Thread(Matrix);
            Thread thread2 = new Thread(GenerateNumber);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            Thread thread3 = new Thread(OddMatrixNumber);
            thread3.Start();
            thread3.Join();
            Thread thread4 = new Thread(ReadNumbersFromTheFile);
            thread4.Start();
            Console.ReadLine();
        }
    }
}
