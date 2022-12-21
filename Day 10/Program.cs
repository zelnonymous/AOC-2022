using System.Text.RegularExpressions;

var input = "input.txt";

var instructions = File.ReadAllLines(input);
var x = 1;
var cycles = new List<Cycle>();

foreach(var instruction in instructions)
{
    if (Regex.Match(instruction, "^noop").Success)
    {
        cycles.Add(new Cycle(cycles.Count + 1, x));
        continue;
    }
    var chkadd = Regex.Match(instruction, "^addx ((-)?[0-9]*)");
    if (!chkadd.Success)
        continue;
    int modval = 0;
    int.TryParse(chkadd.Groups[1].Value, out modval);
    cycles.Add(new Cycle(cycles.Count + 1, x));
    cycles.Add(new Cycle(cycles.Count + 1, x));
    x += modval;
}

var sigstrsum = cycles
    .Where(c => c.cyclenum == 20 || 
        (c.cyclenum - 20) % 40 == 0)
    .Sum(c => c.cyclenum * c.x);

Console.WriteLine($"Part 1: {sigstrsum}");

Console.WriteLine("Part 2:");
int xpos = 0;
foreach (Cycle cycle in cycles)
{
    if (xpos == 40)
    {
        xpos = 0;
        Console.WriteLine();
    }
    if (cycle.x == xpos || cycle.x + 1 == xpos || 
        cycle.x - 1 == xpos)
    {
        Console.Write("#");
        xpos++;
        continue;
    }
    Console.Write(".");
    xpos++;
}
Console.WriteLine();
