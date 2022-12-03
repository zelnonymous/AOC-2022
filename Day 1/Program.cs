using System.IO;
/* 
Each line is calories for an item an elf is carrying.
Blank lines are separators between elves.
Sum the total calories for each elf and get the top
three values.
*/
var calcounts = new List<int>();
var curcals = 0;
var lines = File.ReadAllLines("input.txt");
foreach (var line in lines)
{
    if (line == string.Empty)
    {
        calcounts.Add(curcals);
        curcals = 0;
        continue;
    }
    var linecals = 0;
    if (!int.TryParse(line, out linecals))
        continue;
    curcals += linecals;
}
Console.WriteLine(calcounts
    .OrderByDescending(c => c)
    .Take(3)
    .Sum());
