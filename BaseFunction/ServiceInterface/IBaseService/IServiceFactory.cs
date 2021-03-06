﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFunction.ServiceInterface
{
    public interface IServiceFactory : IDisposable
    {
        T CreateInstance<T>();

        T CreateInterface<T>();

        T Create<T>();
    }
}
