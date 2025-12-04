


public class Program
{
    public const int Count = 5;
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

}




