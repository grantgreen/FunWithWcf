﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FunWithWcf.Contracts;
using FunWithWcf.Contracts.Utils;

namespace FunWithWcfClient.cs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter username: ");
            var userName = Console.ReadLine();

            var client = new Client(userName);
            client.Connect();
            var key = "";
            while (key != "q")
            {
                //Thread.Sleep(1000);
                key = Console.ReadLine();
                client.Write(key);
            }
        }


    }

    internal class Client : IFunServiceCallback
    {
        private ClientChannelWrapper<IFunService> client;
        private readonly string user;
        private Timer timer;
        public Client(string user)
        {


            this.user = user;
        }
        public void Connect()
        {
            this.client = ServiceAgentLocator.CreateServiceInstance<IFunService>(this, "127.0.0.1");

            try
            {
                Console.WriteLine("Connecting...");
                this.client.Service.Connect(this.user);
                Console.WriteLine("Connected to server");
                timer = new Timer(GetUsers, "Get users...", 1000, 1000);
            }
            catch (Exception)
            {
                Console.WriteLine("Connection failed...");
            }
        }

        void GetUsers(object data)
        {
            var users = this.client.Service.GetUsers();
            foreach (var item in users)
            {
                Console.WriteLine("User connected: " + item);
            }

            var config = this.client.Service.GetConfiguration();
            foreach (var o in config)
            {
                //Console.WriteLine(o.Key + ", " + o.Value);
            }
            var last = config.Last();
            Console.WriteLine(last.Key + ", " + last.Value);
        }

        public void Write(string message)
        {
            this.client.Service.Write(this.user, message);
        }

        public void Update(string user, string message)
        {
            Console.WriteLine(user + " wrote: " + message);
        }

        public bool IsConnected { get; private set; }
    }
}
