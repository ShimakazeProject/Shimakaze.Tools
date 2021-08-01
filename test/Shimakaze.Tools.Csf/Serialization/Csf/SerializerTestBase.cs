using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shimakaze.Tools.Csf.Serialization.Csf;

namespace Shimakaze.Models.Csf.Serialization.Csf.Test
{
    public abstract class SerializerTestBase<TSerializer, T>
        where TSerializer : ICsfSerializer<T>, IAsyncCsfSerializer<T>, new()
        where T : notnull
    {
        protected abstract byte[] TestData { get; }

        protected abstract T TestObject { get; }

        protected readonly TSerializer Serializer = new();

        [TestMethod]
        public virtual void SerializeTest()
        {
            var data1 = Serializer.Serialize(TestObject);
            Console.WriteLine(TestObject.ToString());
            Console.WriteLine(BitConverter.ToString(data1));
        }
        [TestMethod]
        public virtual async Task SerializeAsyncTest()
        {
            MemoryStream stream = new();
            await Serializer.SerializeAsync(TestObject, stream);
            stream.Seek(0, SeekOrigin.Begin);
            byte[] data = stream.ToArray();
            Console.WriteLine(TestObject.ToString());
            Console.WriteLine(BitConverter.ToString(data));
        }
        [TestMethod]
        public virtual void DeserializeTest()
        {
            var head = Serializer.Deserialize(TestData);
            Console.WriteLine(head.ToString());
            Console.WriteLine(BitConverter.ToString(TestData));
        }
        [TestMethod]
        public virtual async Task DeserializeAsyncTest()
        {
            MemoryStream stream = new(TestData);
            var head = await Serializer.DeserializeAsync(stream, new byte[24]);
            Console.WriteLine(head.ToString());
            Console.WriteLine(BitConverter.ToString(TestData));
        }
    }
}