using System;
using System.Threading;

namespace MultiThreadSample
{
    class Program
    {
        static int passangerCount = 0;
        static Mutex mutex = new Mutex();
        static readonly object m = new object();
        
        static void AddPassangers(int i)
        {
            Monitor.Enter(m);
            mutex.WaitOne();
            while (passangerCount < 200)
            {
                passangerCount += 1;
                if (passangerCount < 200)
                {
                    Console.WriteLine("new passanger at gate" + i);
                    Monitor.Pulse(m);
                    Monitor.Wait(m);
                }
            }
            Monitor.PulseAll(m);
            mutex.ReleaseMutex();
            Monitor.Exit(m);
        }

        static void PrintPassanger() {
            Monitor.Enter(m);
            mutex.WaitOne();
            if (passangerCount == 0)
            {
                Monitor.Wait(m);
            }
            while (passangerCount < 200)
            {
                Console.WriteLine("passanger count is " + passangerCount);
                Monitor.Pulse(m);
                Monitor.Wait(m);
            }
            mutex.ReleaseMutex();
            Monitor.Exit(m);   
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Start Main Thread");
            Thread thread1 = new Thread((i) => AddPassangers((int)i));
            Thread thread2 = new Thread((i) => AddPassangers((int)i));
            Thread thread3 = new Thread(PrintPassanger);

            thread1.Start(1);
            thread2.Start(2);
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();

        }
    }
}
