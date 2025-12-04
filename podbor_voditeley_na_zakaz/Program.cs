public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}



public record struct Point(int X, int Y);

public record Driver(Guid id, Point CurrentLocation);

public record Order(Point Location);