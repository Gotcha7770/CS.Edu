using AutoMapper;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.MappingTests;

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

        public DestinationData[] Items { get; set; }
    }

    class SingleObjectToArrayConverter<TSource, TDest> : ITypeConverter<TSource, TDest[]>
    {
        public TDest[] Convert(TSource source, TDest[] destination, ResolutionContext context)
        {
            return [context.Mapper.Map<TDest>(source)];
        }
    }

    class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<SourceData, DestinationData>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value != null ? src.Value.ToString() : default));

            CreateMap<SourceData, DestinationData[]>()
                .ConvertUsing<SingleObjectToArrayConverter<SourceData, DestinationData>>();

            CreateMap<Source, Destination>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Data));
        }
    }

    private readonly MapperConfiguration _configuration = new MapperConfiguration(cfg => cfg.AddProfile<TestProfile>());

    [Fact]
    public void Configuration_IsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map()
    {
        var mapper = new Mapper(_configuration);
        var source = new Source
        {
            Id = 42,
            Data = new SourceData { Value = null }
        };

        var destination = mapper.Map<Destination>(source);

        destination.Should()
            .BeEquivalentTo(new Destination
            {
                Id = 42,
                Items = [new DestinationData()]
            });
    }
}