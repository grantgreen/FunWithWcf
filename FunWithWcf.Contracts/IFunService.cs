﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FunWithWcf.Contracts.Utils;

namespace FunWithWcf.Contracts
{
    [ServiceContract(CallbackContract = typeof(IFunServiceCallback))]
    public interface IFunService
    {
        [OperationContract]
        void Write(string user, string message);
        [OperationContract]
        void Connect(string user);
        [OperationContract]
        SynchronizedList<string> GetUsers();
        [OperationContract]
        SynchronizedDictionary<object, object> GetConfiguration();

    }

    public interface IFunServiceCallback
    {
        [OperationContract]
        void Update(string user, string message);
    }

}
