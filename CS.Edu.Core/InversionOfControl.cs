using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CS.Edu.Core;

internal interface IContext {}

internal interface IRepository<out T>
{
    IEnumerable<T> All();
}

internal class Repository : IRepository<int>
{
    private readonly IContext _context;

    public Repository()
    {
        _context = new Context();
    }

    public Repository(IContext context)
    {
        _context = context;
    }

    public IEnumerable<int> All()
    {
        return Enumerable.Range(0, 1000);
    }
}

internal class Context : IContext
{
    public Options Options { get; } = new();

    public string GetRootDirectory()
    {
        return Options.GetDirectory().Root.Name;
    }
}

internal class Worker
{
    private readonly IRepository<int> _repository;

    internal Worker()
    {
        _repository = new Repository();
    }

    internal Worker(IRepository<int> repository)
    {
        _repository = repository;
    }

    public void Work()
    {
        foreach (var item in _repository.All())
        {
            Console.WriteLine(item);
        }
    }
}

class CompositionWithoutDI
{
    public void Main()
    {
        var worker = new Worker();
        worker.Work();
    }
}

class CompositionWithDI
{
    void Main()
    {
        IContext context = new Context();
        IRepository<int> repository = new Repository(context);
        var worker = new Worker(repository);
        worker.Work();
    }
}

class Options
{
    public DirectoryInfo GetDirectory()
    {
        return new DirectoryInfo(Environment.CurrentDirectory);
    }
}

class DisclosureOfStructure
{
    public void Main()
    {
        Context context = new();
        Options options = context.Options;
        DirectoryInfo directory = options.GetDirectory();
        string path = directory.Root.Name;
    }
}

class HidingStructure
{
    public void Main()
    {
        Context context = new();
        string path = context.GetRootDirectory();
    }
}