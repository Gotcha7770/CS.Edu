using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace CS.Edu.Tests.Serialzation;

public class LogEntry
{
    //[XmlAttribute(DataType = "dateTime")]
    public DateTime DateTime
    {
        get;
        init;
    }

    public string Message
    {
        get;
        init;
    }
}

public class XmlSerializationTests
{
    [Fact]
    public void Serialize()
    {
        var serializer = new XmlSerializer(typeof(LogEntry));

        var dateTime = DateTime.Now;
        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        var entry = new LogEntry
        {
            DateTime = dateTime,
            Message = "Test message"
        };
        using TextWriter writer = new StringWriter();
        serializer.Serialize(writer, entry);
    }

    [Fact]
    public void DateTimeFormat()
    {
        //XmlMediaTypeFormatter
        //XmlDateTimeSerializationMode.Unspecified

        var serializer = new XmlSerializer(typeof(LogEntry));

        var entry = new LogEntry
        {
            DateTime = DateTime.Now,
            Message = "Test message"
        };
        using TextWriter writer = new StringWriter();
        serializer.Serialize(writer, entry);
    }
}