using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunWithWcf.Contracts.Utils;
using NUnit.Framework;
using System.Threading;

namespace FunWithWcf.Tests
{
    [TestFixture]
    public class SynchronizedCacheFixture
    {
        private readonly SynchronizedList<string> cache = new SynchronizedList<string>();

        public void CanHandleMultipleThreads()
        {
            var hs = new System.Collections.Hashtable();
            var task1 = Task.Factory.StartNew(Action1);
            var task2 = Task.Factory.StartNew(Action2);
            var task3 = Task.Factory.StartNew(Action3);
            var task4 = Task.Factory.StartNew(Action4);
            var task5 = Task.Factory.StartNew(Action5);
            var task6 = Task.Factory.StartNew(Action6);
            var task7 = Task.Factory.StartNew(Action7);
            var task8 = Task.Factory.StartNew(Action8);
            var task9 = Task.Factory.StartNew(Action9);

            Thread.Sleep(30000);
            task1.Wait(TimeSpan.FromSeconds(1));
            task2.Wait(TimeSpan.FromSeconds(1));
            task3.Wait(TimeSpan.FromSeconds(1));
            task4.Wait(TimeSpan.FromSeconds(1));
            task5.Wait(TimeSpan.FromSeconds(1));
            task6.Wait(TimeSpan.FromSeconds(1));
            task7.Wait(TimeSpan.FromSeconds(1));
            task8.Wait(TimeSpan.FromSeconds(1));
            task9.Wait(TimeSpan.FromSeconds(1));
        }

        private void Action2()
        {
            int index = 0;
            while (true)
            {
                cache.Add("Action2 added: " + index++);
                Thread.Sleep(100);
            }
        }
        private void Action3()
        {
            while (true)
            {
                foreach (var item in cache)
                {
                    Thread.Sleep(10);
                    Console.WriteLine(item);
                }
                Thread.Sleep(200);
            }
        }

        private void Action1()
        {
            int index = 0;
            while (true)
            {
                cache.Add("Action1 added: " + index++);
                Thread.Sleep(100);
            }
        }

        private void Action4()
        {
            int index = 0;
            while (true)
            {
                cache.Add("Action4 added: " + index++);
                Thread.Sleep(100);
            }
        }
        private void Action5()
        {
            int index = 0;
            while (true)
            {
                cache.Add("Action5 added: " + index++);
                Thread.Sleep(100);
            }
        }
        private void Action6()
        {
            int index = 0;
            while (true)
            {
                cache.Add("Action6 added: " + index++);
                Thread.Sleep(100);
            }
        }
        private void Action7()
        {
            int index = 0;
            while (true)
            {
                cache.Add("Action8 added: " + index++);
                Thread.Sleep(100);
            }
        }
        private void Action8()
        {
            int index = 0;
            while (true)
            {
                cache.Add("Action8 added: " + index++);
                Thread.Sleep(100);
            }
        }
        private void Action9()
        {
            int index = 0;
            while (true)
            {
                cache.Add("Action9 added: " + index++);
                Thread.Sleep(100);
            }
        }
    }
}
