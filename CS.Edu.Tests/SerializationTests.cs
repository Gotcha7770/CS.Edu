using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace CS.Edu.Tests;

[TestFixture]
public class SerializationTests
{
    [DataContract]
    class TestClass
    {
        [DataMember]
        private double _value;

        [DataMember]
        public string Name { get; set; }

        private int IntegerValue => (int)_value;

        public void SetValue(double value)
        {
            _value = value;
        }
    }

    [Serializable]
    class TestClass2
    {
        public string Name { get; set; }

        public int Value { get; set; }
    }

    [Test]
    public void BinaryFormatter()
    {
        //var item = new TestClass();
        var item = new TestClass2
        {
            Name = "TestClass",
            Value = 42
        };

        using (var stream = new MemoryStream())
        {
            var bf = new BinaryFormatter();
            bf.Serialize(stream, item);

            stream.Position = 0;
            var result = bf.Deserialize(stream);
        }
    }

    [Test]
    public void DataContractSerializer()
    {
        var item = new TestClass
            //var item = new TestClass2
            {
                Name = "TestClass",
                //Value = 42
            };

        item.SetValue(42.42);

        //XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateTextWriter(someStream,Encoding.UTF8 );
        //dcs.WriteObject(xdw, p);

        using (var stream = new MemoryStream())
        {
            //DataContractSerializer dcs = new DataContractSerializer(typeof(TestClass2));
            DataContractSerializer dcs = new DataContractSerializer(typeof(TestClass));
            dcs.WriteObject(stream, item);

            stream.Position = 0;
            var result = dcs.ReadObject(stream);
        }
    }
}