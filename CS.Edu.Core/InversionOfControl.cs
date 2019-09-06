using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CS.Edu.Core
{
    public interface IRepository
    {
        IEnumerable<int> All();
    }

    public interface IWorker
    {
        void Work();
    }

    public class Repository : IRepository
    {
        public IEnumerable<int> All()
        {
            return Enumerable.Range(0, 1000);
        }
    }

    public class Worker : IWorker
    {
        private readonly IRepository _repository;

        public Worker() : this(new Repository()) { }

        public Worker(IRepository repository)
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

    public class CompositionWithoutDI
    {
        public void Main()
        {
            var worker = new Worker();
            worker.Work();
        }
    }

    public class CompositionWithDI
    {
        public void Main()
        {
            IRepository repository = new Repository();
            IWorker worker = new Worker(repository);
            worker.Work();
        }
    }

    public class Context
    {
        public Options Options { get; } = new Options();

        public string GetRootDirectory()
        {
            return Options.GetDirectory().Root.Name;
        }
    }

    public class Options
    {
        public DirectoryInfo GetDirectory()
        {
            return new DirectoryInfo(Environment.CurrentDirectory);
        }
    }

    public class DisclosureOfStructure
    {
        public void Main()
        {
            Context context = new Context();
            Options options = context.Options;
            DirectoryInfo directory = options.GetDirectory();

            string path = directory.Root.Name;
        }
    }

    public class HidingStructure
    {
        public void Main()
        {
            Context context = new Context();
            string path = context.GetRootDirectory();
        }
    }
}