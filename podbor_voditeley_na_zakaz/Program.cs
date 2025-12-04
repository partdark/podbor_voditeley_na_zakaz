


public class Program
{
    public const int Count = 5;
    public const int N = 5;
    public const int M = 5;
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }

    public record struct Point(int X, int Y);

    public record Driver(Guid id, Point CurrentLocation);

    public record Order(Point Location);

    public double CalculateDustanceSq(Point point1, Point point2) => Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2);


    public List<Driver> FindNearest_1(Order order, List<Driver> drivers)
    {
        return drivers.Select(
            driver => new
            {
                Driver = driver,
                Distanse = CalculateDustanceSq(order.Location, driver.CurrentLocation)
            })
            .OrderBy(d => d.Distanse)
            .Take(Count)
            .Select(d => d.Driver)
            .ToList();
    }

    public List<Driver> FindNearest_2(Order order, List<Driver> drivers)
    {
        var queue = new PriorityQueue<Driver, double>();

        foreach (var driver in drivers)
        {
            var distance = CalculateDustanceSq(order.Location, driver.CurrentLocation);
            queue.Enqueue(driver, -distance);

            if (queue.Count > Count)
            {
                queue.Dequeue();
            }

        }
        return queue.UnorderedItems.Select(i => i.Element).Reverse().ToList();
    }

    public List<Driver> FindNearest_3(Order order, List<Driver> drivers)
    {
        var targetDriversPerCell = Count; 
        var mapArea = (double)M * N;
        var targetCellCount = Math.Max(1.0,(double) drivers.Count / targetDriversPerCell);
        var idealCellArea = mapArea / targetCellCount;
        var cellsize = (int)Math.Max(1.0, Math.Sqrt(idealCellArea));

        var grid = new Dictionary<Point, List<Driver>>();

        foreach (var driver in drivers)
        {
            var cell = new Point(driver.CurrentLocation.X / cellsize, driver.CurrentLocation.Y / cellsize);
            if (!grid.TryGetValue(cell, out var driverInCell))
            {
                driverInCell = new List<Driver>();
                grid[cell] = driverInCell;
            }
            driverInCell.Add(driver);
        }

        var queue = new PriorityQueue<Driver, double>();
        var orderCell = new Point(order.Location.X / cellsize, order.Location.Y / cellsize);
        var radius = 0;

        while (true)
        {
            var driversFound = false;
            for (var x = orderCell.X - radius; x <= orderCell.X + radius; x++)
            {
                for (var y = orderCell.Y - radius; y <= orderCell.Y + radius; y++)
                {
                    if  (radius > 0 && Math.Abs(x - orderCell.X) < radius && Math.Abs(y - orderCell.Y) < radius)
                    { continue; }
                    if (grid.TryGetValue(new Point(x,y), out var driverCell))
                    {
                        driversFound = true;
                        foreach (var driver in driverCell)
                        {
                            var distance = CalculateDustanceSq(order.Location, driver.CurrentLocation);
                            queue.Enqueue(driver, -distance);
                            if (queue.Count > Count)
                            {
                                queue.Dequeue();
                            }
                        }
                    }
                }
            }
            if (queue.Count == Count)
            {
               if (queue.TryPeek(out _, out var priorityNegative))
                {
                    var maxD = - priorityNegative;
                    var minD = (radius + 1) * cellsize;
                    if (minD  * minD  > maxD)
                    {
                        break;
                    }
                }
            }
            
            else if (!driversFound && radius > 5)
            {
                break;
            }
            radius++;
        }
        return queue.UnorderedItems.Select(i => i.Element).Reverse().ToList();
    }

}




