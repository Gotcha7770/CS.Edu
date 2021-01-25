using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class ThreadingTests
    {
        private static int _doggosPetted;

        [Test]
        public void IncremetFromDifferentThreads()
        {
            void PetDoggo()
            {
                _doggosPetted += 1;
            }

            var tasks = Enumerable.Range(0, 50)
                .Select(x => Task.Run(PetDoggo))
                .ToArray();

            Parallel.For(1, 50, (i, s) =>
            {
                PetDoggo();
            });

            Assert.That(_doggosPetted, Is.Not.EqualTo(50));
        }
    }
}