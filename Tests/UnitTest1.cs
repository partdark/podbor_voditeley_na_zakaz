



using podbor_voditeley_na_zakaz;
using static podbor_voditeley_na_zakaz.Program;

namespace Tests
{
 


    [TestFixture]
    public class Tests
    {
        private podbor_voditeley_na_zakaz.Program _searching;

        private List<Driver> _drivers;

        private Order _order;

        [SetUp]
        public void Setup()
        {
            _searching = new Program();

            _drivers = new List<Driver>
            {
                new(Guid.NewGuid(), new Program.Point(10, 10)),
                new(Guid.NewGuid(), new Program.Point(12, 12)),
                new(Guid.NewGuid(), new Program.Point(50, 50)),
                new(Guid.NewGuid(), new Program.Point(52, 52)),
                new(Guid.NewGuid(), new Program.Point(48, 48)),
                new(Guid.NewGuid(), new Program.Point(55, 55)),
                new(Guid.NewGuid(), new Program.Point(45, 45)),
                new(Guid.NewGuid(), new Program.Point(100, 100)),
                new(Guid.NewGuid(), new Program.Point(200, 200)),
                new(Guid.NewGuid(), new Program.Point(51, 49))
            };

            _order = new Order(new Program.Point(50, 50));
        }

        [Test]
        public void Test1WithDrivers()
        {
            var expected = _searching.FindNearest_1(_order, _drivers).Select(d => d.id).ToList();
            var result2 = _searching.FindNearest_2(_order, _drivers).Select(d => d.id).ToList();
            var result3 = _searching.FindNearest_3(_order, _drivers).Select(d => d.id).ToList();

            Assert.That(expected.Count, Is.EqualTo(Program.Count));
            Assert.That(result2.Count, Is.EqualTo(Program.Count));
            Assert.That(result3.Count, Is.EqualTo(Program.Count));

            CollectionAssert.AreEquivalent(expected, result2);
            CollectionAssert.AreEquivalent(expected, result3);
        }

        [Test]
        public void Test1WithNoDrivers()
        {
            var noDrivers = new List<Driver>();
            var result1 = _searching.FindNearest_1(_order, noDrivers);
            var result2 = _searching.FindNearest_2(_order, noDrivers);
            var result3 = _searching.FindNearest_3(_order, noDrivers);

            Assert.That(result1, Is.Empty);
            Assert.That(result2, Is.Empty);
            Assert.That(result3, Is.Empty);

        }
    }
}