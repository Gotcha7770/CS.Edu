using AutoMapper;
using Xunit;

namespace CS.Edu.Tests.MappingTests;

//https://stackoverflow.com/questions/65382286/automapper-problem-with-mapping-records-type

public class MappingRecord
{
    private record Input(int Value, string Name);

    private record Output(int Value, string FullName);

    [Fact]
    public void Test()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Input, Output>()
                .ForCtorParam(nameof(Output.FullName), opt => opt.MapFrom(s => s.Name));
            //.ForMember(dest => dest.FullName, opt => opt.MapFrom(s => s.Name));
        });
        var mapper = new Mapper(configuration);

        var input = new Input(2, "John");
        var output = mapper.Map<Output>(input);
    }
}