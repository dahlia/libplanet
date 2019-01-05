using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Libplanet.Base;
using Libplanet.Serialization;
using Xunit;
using static Libplanet.Tests.TestUtils;

namespace Libplanet.Tests.Serialization.Bencode
{
    public class BencodeFormatterTest
    {
        [Fact]
        public void Serialize()
        {
            var person = new Person
            {
                Name = "Swen Mun",
                Age = 30,
                Names = new List<string>
                {
                    "Swen Mun",
                    "문성원",
                    "ムンソンワン",
                },
            };
            var formatter = new BencodexFormatter<Person>();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, person);
                byte[] result = stream.ToArray();
                AssertBytesEqual(
                    new byte[]
                    {
                        0x64, 0x33, 0x3a, 0x41, 0x67, 0x65, 0x69, 0x33, 0x30,
                        0x65, 0x34, 0x3a, 0x4e, 0x61, 0x6d, 0x65, 0x75, 0x38,
                        0x3a, 0x53, 0x77, 0x65, 0x6e, 0x20, 0x4d, 0x75, 0x6e,
                        0x35, 0x3a, 0x4e, 0x61, 0x6d, 0x65, 0x73, 0x6c, 0x75,
                        0x38, 0x3a, 0x53, 0x77, 0x65, 0x6e, 0x20, 0x4d, 0x75,
                        0x6e, 0x75, 0x39, 0x3a, 0xeb, 0xac, 0xb8, 0xec, 0x84,
                        0xb1, 0xec, 0x9b, 0x90, 0x75, 0x31, 0x38, 0x3a, 0xe3,
                        0x83, 0xa0, 0xe3, 0x83, 0xb3, 0xe3, 0x82, 0xbd, 0xe3,
                        0x83, 0xb3, 0xe3, 0x83, 0xaf, 0xe3, 0x83, 0xb3, 0x65,
                        0x65,
                    },
                    result
                );
            }
        }

        [Fact]
        public void Deserialize()
        {
            byte[] serialized = ByteUtils.ParseHex(
                "64333a41676569333065343a4e616d65383a5377656e204d756e353a4e616d65736c383a5377656e204d756e393aebacb8ec84b1ec9b9031383ae383a0e383b3e382bde383b3e383afe383b36565");
            var formatter = new BencodexFormatter<Person>();
            using (var stream = new MemoryStream(serialized))
            {
                var person = (Person)formatter.Deserialize(stream);
                Assert.Equal(30, person.Age);
                Assert.Equal("Swen Mun", person.Name);
                Assert.Equal(
                    new List<string> { "Swen Mun", "문성원", "ムンソンワン" },
                    person.Names);
            }
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    [Serializable]
    internal class Person : ISerializable
    {
        public Person()
        {
        }

        protected Person(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            Age = (int)info.GetInt64("Age");
            Names =
                ((List<object>)info.GetValue("Names", typeof(List<object>)))
                .OfType<byte[]>()
                .Select(bs => Encoding.UTF8.GetString(bs))
                .ToList();
        }

        public string Name { get; internal set; }

        public int Age { get; internal set; }

        public List<string> Names { get; internal set; }

        public void GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Age", Age);
            info.AddValue("Names", Names);
        }
    }
#pragma warning restore SA1402 // File may only contain a single class
}
