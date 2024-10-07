using System.Linq;
using CS.Edu.Core.Extensions;
using DynamicData.Kernel;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions;

public class OptionalsTests
{
    private record Phone(string Number);

    [Fact]
    public void Optionals_Consuming()
    {
        Optional<Phone>[] phones =
        [
            Optional.Some(new Phone("123456")),
            Optional.None<Phone>()
        ];

        var single = from phone in phones[0]
            select phone;

        var onlyValues = phones.SelectMany(x => x.AsEnumerable());

        onlyValues = phones
            .SelectMany(x => x.AsEnumerable(), (x, y) => y);

        onlyValues =
            from phone in phones
            from value in phone.AsEnumerable()
            select value;

        // Doesn`t work!
        //
        // onlyValues = phones
        //     .SelectMany(x => x.SelectMany(p => p));
        //
        // onlyValues =
        //     from phone in phones
        //     from value in phone
        //     select value;

        onlyValues.Should()
            .BeEquivalentTo([new Phone("123456")]);
    }
}