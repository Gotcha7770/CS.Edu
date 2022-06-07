using System.Collections.Generic;
using AutoMapper;
using NUnit.Framework;

namespace CS.Edu.Tests.MappingTests;

[TestFixture]
public class AutoMapperSingleObjectToArrayTests
{
    class SourceData
    {
        public int? Value { get; set; }
    }

    class Source
    {
        public long Id { get; set; }

        public SourceData Data { get; set; }
    }

    class DestinationData
    {
        public string Value { get; set; }
    }

    class Destination
    {
        public long Id { get; set; }

        public List<DestinationData> Items { get; set; }
    }

    class SingleObjectToArrayConverter<T> : ITypeConverter<T, List<T>>
    {
        public List<T> Convert(T source, List<T> destination, ResolutionContext context)
        {
            return new List<T> { source };
        }
    }

    class SingleObjectToArrayConverter<TSource, TDest> : ITypeConverter<TSource, List<TDest>>
    {
        public List<TDest> Convert(TSource source, List<TDest> destination, ResolutionContext context)
        {
            return new List<TDest> { context.Mapper.Map<TDest>(source) };
        }
    }

    class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<SourceData, DestinationData>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value != null ? src.Value.ToString() : default));

            CreateMap<SourceData, List<DestinationData>>().ConvertUsing<SingleObjectToArrayConverter<SourceData, DestinationData>>();

            CreateMap<Source, Destination>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Data));
        }
    }

    private readonly MapperConfiguration _configuration = new MapperConfiguration(cfg => cfg.AddProfile<TestProfile>());

    [Test]
    public void Configuration_IsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    public void Map()
    {
        var mapper = new Mapper(_configuration);
        var source = new Source
        {
            Id = 42,
            Data = new SourceData { Value = null }
        };

        var destination = mapper.Map<Destination>(source);

        Assert.AreEqual(source.Id, destination.Id);
        Assert.AreEqual(source.Data.Value, destination.Items[0].Value);
    }
}