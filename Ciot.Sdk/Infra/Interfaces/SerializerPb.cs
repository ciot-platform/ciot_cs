using Ciot.Sdk.Domain.Interfaces;
using Google.Protobuf;
using System;

namespace Ciot.Sdk.Infra.Interfaces
{
    public class SerializerPb : ISerializer
    {
        private static readonly Lazy<SerializerPb> lazy =
            new Lazy<SerializerPb>(() => new SerializerPb());

        public static SerializerPb Instance => lazy.Value;

        public T Deserialize<T>(byte[] data)
        {
            var message = Activator.CreateInstance<T>();
            if(message is IMessage pbMessage)
            {
                pbMessage.MergeFrom(data);
                return (T)pbMessage;
            }
            throw new InvalidCastException("Type T must implement IMessage from Google.Protobuf");
        }

        public byte[] Serialize<T>(T obj)
        {
            if(obj is IMessage pbMessage)
            {
                return pbMessage.ToByteArray();
            }
            throw new InvalidCastException("Type T must implement IMessage from Google.Protobuf");
        }
    }
}
