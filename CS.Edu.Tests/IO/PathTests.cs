using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Tests.TestCases;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.IO;

public class PathTests
{
    // https://ru.wikipedia.org/wiki/POSIX
    // https://opensource.com/article/19/8/understanding-file-paths-linux
    // https://pubs.opengroup.org/onlinepubs/009695399/basedefs/xbd_chap04.html#tag_04_11

    public readonly record struct Path
    {
        private const char PosixSeparator = '/';
        private readonly Segment[] _segments;

        public static Path Empty { get; } = new(Array.Empty<Segment>(), false);

        public Path(string value)
            :this(value.Split(PosixSeparator, StringSplitOptions.RemoveEmptyEntries).Select(x => new Segment(x)), value.StartsWith('/'))
        {
        }

        internal Path(IEnumerable<Segment> segments, bool isAbsolute)
        {
            _segments = segments.ToArray();
            IsAbsolute = isAbsolute;
        }

        public bool IsAbsolute { get; }
        public IReadOnlyCollection<Segment> Segments => _segments;

        public static Path operator +(Path one, Path other)
        {
            return new Path([..one.Segments, ..other.Segments], one.IsAbsolute);
        }

        public static bool CanResolve(Path one, Path other) => !(one.IsAbsolute && other.IsAbsolute);

        public static bool CanResolve(params Path[] values)
        {
            return values.Count(path => path.IsAbsolute) <= 1;
        }

        public static Path Resolve(Path one, Path other)
        {
            return CanResolve(one, other) ? Resolve(one) + Resolve(other) : Empty;
        }

        public static Path Resolve(params Path[] values)
        {
            return CanResolve(values) ? values.Aggregate(Empty, Resolve) : Empty;
        }
    }

    public readonly record struct Segment
    {
        private readonly string _representation;

        //TODO: portable filenames
        internal Segment(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            _representation = value;
            IsGoUp = value is "..";
        }

        public bool IsGoUp { get; }

        public override string ToString()
        {
            return _representation;
        }
    }

    [Theory]
    [ClassData(typeof(PathParsingCases))]
    public void PathParsing_Segments(string value, bool isAbsolute, Segment[] expected)
    {
        var path = new Path(value);

        path.IsAbsolute.Should()
            .Be(isAbsolute);
        path.Segments.Should()
            .BeEquivalentTo(expected);
    }
}