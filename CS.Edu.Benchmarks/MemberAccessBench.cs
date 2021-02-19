using System;
using BenchmarkDotNet.Attributes;
using FastMember;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class MemberAccessBench
    {
        [Benchmark]
        public DateTime MemberAccessWithReflection()
        {
            var obj = new PropsOnClass { A = 123, B = "abc", C = DateTime.Now, D = null };
            return (DateTime)typeof(PropsOnClass).GetProperty("C")
                .GetValue(obj);
        }

        [Benchmark]
        public DateTime MemberAccessWithFastMember_TypeAccessor()
        {
            var obj = new PropsOnClass { A = 123, B = "abc", C = DateTime.Now, D = null };
            var accessor = TypeAccessor.Create(typeof(PropsOnClass));
            return (DateTime)accessor[obj, "C"];
        }

        [Benchmark]
        public DateTime MemberAccessWithFastMember_ObjectAccessor()
        {
            var obj = new PropsOnClass { A = 123, B = "abc", C = DateTime.Now, D = null };
            var accessor = ObjectAccessor.Create(obj);
            return (DateTime)accessor["C"];
        }
    }

    public class PropsOnClass
    {
        public int A { get; set; }
        public string B { get; set; }
        public DateTime C { get; set; }
        public object D { get; set; }
    }
}