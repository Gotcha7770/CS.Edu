using System;
using Xunit;
using Segment = CS.Edu.Tests.IO.PathTests.Segment;

namespace CS.Edu.Tests.TestCases;

public class PathParsingCases :TheoryData<string, bool, Segment[]>
{
    public PathParsingCases()
    {
        Add("", false, []);
        Add("/", true, []);
        Add("//", true, []);
        Add("/home", true, [new Segment("home")]);
        Add("//home", true, [new Segment("home")]);
        Add("user", false, [new Segment("user")]);
        Add("user/", false, [new Segment("user")]);
        Add("user//", false, [new Segment("user")]);
        Add("/home/user/documents", true, [new Segment("home"), new Segment("user"), new Segment("documents")]);
        Add("user/documents", false, [new Segment("user"), new Segment("documents")]);
        Add("/home/user/../cfg", true, [new Segment("home"), new Segment("user"), new Segment(".."), new Segment("cfg")]);
        Add("user/../cfg/..", false, [new Segment("user"), new Segment(".."), new Segment("cfg"), new Segment("..")]);
    }
}