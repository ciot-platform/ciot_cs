using System;
using System.Collections.Generic;
using System.Text;

namespace Ciot.Sdk.Domain.Interfaces
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T obj);
        T Deserialize<T>(byte[] data);
    }
}
