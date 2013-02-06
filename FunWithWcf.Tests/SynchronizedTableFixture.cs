using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FunWithWcf.Contracts.Utils;
using NUnit.Framework;

namespace FunWithWcf.Tests
{
    [TestFixture]
    public class SynchronizedTableFixture
    {
        private readonly SynchronizedDictionary<string, string> table = new SynchronizedDictionary<string, string>();

        [Test]
        public void CanHandleMultipleThreads()
        {
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
                table["Action2-" + index++] = "Action2-" + index++;
                Thread.Sleep(100);
            }
        }
        private void Action3()
        {
            while (true)
            {
                foreach (var item in this.table)
                {
                    Thread.Sleep(10);
                    Console.WriteLine(item.Key + ", " + item.Value);
                }
                Thread.Sleep(200);
            }
        }

        private void Action1()
        {
            int index = 0;
            while (true)
            {
                table["Action1-" + index++] = "Action1-" + index++;
                Thread.Sleep(100);
            }
        }

        private void Action4()
        {
            int index = 0;
            while (true)
            {
                table["Action4-" + index++] = "Action4-" + index++;
                Thread.Sleep(100);
            }
        }
        private void Action5()
        {
            int index = 0;
            while (true)
            {
                table["Action5-" + index++] = "Action5-" + index++;
                Thread.Sleep(100);
            }
        }
        private void Action6()
        {
            int index = 0;
            while (true)
            {
                table["Action6-" + index++] = "Action6-" + index++;
                Thread.Sleep(100);
            }
        }
        private void Action7()
        {
            int index = 0;
            while (true)
            {
                table["Action7-" + index++] = "Action7-" + index++;
                Thread.Sleep(100);
            }
        }
        private void Action8()
        {
            int index = 0;
            while (true)
            {
                table["Action8-" + index++] = "Action8-" + index++;
                Thread.Sleep(100);
            }
        }
        private void Action9()
        {
            int index = 0;
            while (true)
            {
                table["Action9-" + index++] = "Action9-" + index++;
                Thread.Sleep(100);
            }
        }
    }
}
