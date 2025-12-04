using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static podbor_voditeley_na_zakaz.Program;


namespace podbor_voditeley_na_zakaz
{
    [MemoryDiagnoser]
    public class BenchmarkFinding
    {
        private podbor_voditeley_na_zakaz.Program _searching;

        private List<Driver> _drivers;

        private Order _order;


        [Params(10, 150, 500)]
        public int Drivers;

        private const int X = 10000;
        private const int Y = 10000;


        [GlobalSetup]
        public void Setup()
        {
            _searching = new Program();
            _drivers = new List<Driver>();
            _searching.N = X;
            _searching.M = Y;

            var random = new Random(23);

            for (var i = 0; i < Drivers; i++)
            {
                var location = new Program.Point(random.Next(X + 1), random.Next(Y + 1));
                _drivers.Add(new Driver(Guid.NewGuid(), location));
            }
            _order = new Order(new Program.Point(random.Next(X + 1), random.Next(Y + 1)));

        }

        [Benchmark(Baseline = true)]
        public void F1()
        {
            _searching.FindNearest_1(_order, _drivers);
        }

        [Benchmark]
        public void F2()
        {
            _searching.FindNearest_2(_order, _drivers);
        }

        [Benchmark]
        public void F3 ()
        {
            _searching.FindNearest_3(_order, _drivers);
        }

    }
}
