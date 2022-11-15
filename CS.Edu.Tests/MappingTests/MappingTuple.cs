using AutoMapper;
using Xunit;

namespace CS.Edu.Tests.MappingTests;

//https://stackoverflow.com/questions/68975014/how-to-map-from-a-tuple-without-specifying-each-member-field

public class MappingTuple
{
    private class Foo
    {
        public int Value { get; set; }
    }

    private class Bar
    {
        public string Name { get; set; }
    }

    private class Output
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }

    [Fact]
    public void Test()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Foo, Output>();
            cfg.CreateMap<Bar, Output>();

            cfg.CreateMap<(Foo Foo, Bar Bar), Output>()
                .IncludeMembers(x => x.Foo, x => x.Bar);
        });
        var mapper = new Mapper(configuration);

        var foo = new Foo { Value = 2 };
        var bar = new Bar { Name = "John" };
        var output = mapper.Map<Output>((foo, bar));
    }
}