public class CompartmentRange
{
    public int Min { get; set; }
    public int Max { get; set; }
    public CompartmentRange(int cmin, int cmax)
    {
        Min = cmin;
        Max = cmax;
    }
}
public class Group
{
    public CompartmentRange First { get; set; }
    public CompartmentRange Second { get; set; }
    public Group(int firstmin, int firstmax, 
        int secondmin, int secondmax)
    {
        First = new CompartmentRange(firstmin, firstmax);
        Second = new CompartmentRange(secondmin, secondmax);
    }
}
public class Program
{
    static void Main(string[] args)
    {
        List<Group> groups = new List<Group>();
        var lines = File.ReadAllLines("input.txt");
        foreach (var line in lines)
        {
            if (line.Trim() == string.Empty)
                continue;
            var group = line.Split(",");
            if (group.Length != 2)
            {
                Console.WriteLine("Error: Invalid data.");
                return;
            }
            var strfirst = group[0].Split("-");
            var strsecond = group[1].Split("-");
            if (strfirst.Length != 2 || strsecond.Length != 2)
            {
                Console.WriteLine("Error: Invalid data.");
                return;
            }
            int firstmin, firstmax, secondmin, secondmax;
            if (!int.TryParse(strfirst[0], out firstmin) ||
                !int.TryParse(strfirst[1], out firstmax) ||
                !int.TryParse(strsecond[0], out secondmin) ||
                !int.TryParse(strsecond[1], out secondmax))
            {
                Console.WriteLine("Error: Invalid data.");
                return;
            }
            groups.Add(new Group(firstmin, firstmax, secondmin,
                secondmax));
        }
        Console.WriteLine("Groups full overlap: " + groups.Where(g => 
            (
                g.First.Min >= g.Second.Min &&
                g.First.Max <= g.Second.Max
            ) || (
                g.Second.Min >= g.First.Min &&
                g.Second.Max <= g.First.Max
            )
        ).Count());
        Console.WriteLine("Groups any overlap: " + groups.Where(g =>
            (g.First.Min >= g.Second.Min && g.First.Min <= g.Second.Max) ||
            (g.First.Max >= g.Second.Min && g.First.Max <= g.Second.Max) ||
            (g.Second.Min >= g.First.Min && g.Second.Min <= g.First.Max) ||
            (g.Second.Max >= g.First.Min && g.Second.Max <= g.First.Max)
        ).Count());
    }
}