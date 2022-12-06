using System.Text.RegularExpressions;

// Configuration parameters //
// Last line of data before instructions begin
int dataend = 9;
// Number of columns of crates
int columncnt = 9;
// Verbose output: if true, each instruction will be printed
// along with the updated state after its execution
bool verbose = false;

// Sorted list to store the crate data state. The key is
// the column number and the value is the list of crates
// in that column
var CrateState = new SortedList<int, List<string>>();

// Called before each operation to set the initial state
// based on the input from the data file
void InitializeCrateState(string[] lines)
{
    CrateState = new SortedList<int, List<string>>();
    for (int lineNum = 0; lineNum <= dataend - 1; lineNum++)
    {
        for(int column = 0; column <= columncnt - 1; column++)
        {
            var crate = lines[lineNum].Substring(column * 4, 3);
            var match = Regex.Match(crate, @"^\[([A-Z])\]$");
            if (!match.Success)
                continue;
            if (!CrateState.ContainsKey(column + 1))
                CrateState.Add(column + 1, new List<string>());
            CrateState[column + 1].Add(match.Groups[1].Value);
        }
    }
}
// Perform crate move operations.  Lines can either be the
// unaltered file content or just the instruction portion.
// Lines that don't look like instructions are ignored.  The
// second parameter indicates wether the crane is model 9001
// (Moves multiple crates at a time, retaining the original order)
// or 9000 (Only one crate at a time, so order of the moved crates
// ends up reversed).
void MoveCrates(string[] lines, bool is9001)
{
    if (CrateState == null)
        return;
    for (int lineNum = dataend + 1; lineNum <= lines.Length - 1; lineNum++)
    {
        var instruction = lines[lineNum];
        var instexpr = "^move ([0-9]*) from ([0-9]*) to ([0-9]*)$";
        var match = Regex.Match(instruction, instexpr);
        if (!match.Success)
            continue;
        int count, src, dest;
        if (!int.TryParse(match.Groups[1].Value, out count) ||
            !int.TryParse(match.Groups[2].Value, out src) ||
            !int.TryParse(match.Groups[3].Value, out dest))
            continue;
        if (verbose)
            Console.WriteLine(instruction);
        var cratesToMove = CrateState[src].GetRange(0, count);
        if (!is9001)
            cratesToMove.Reverse();
        CrateState[src].RemoveRange(0, count);
        CrateState[dest].InsertRange(0, cratesToMove);
        if (verbose)
            PrintCrateState();
    }
}
// Prints the crate state formatted the same as the
// examples
void PrintCrateState() {
    if (CrateState == null)
        return;
    var tallest = CrateState.Max(c => c.Value.Count);
    for (var height = tallest; height > 0; height --)
    {
        string row = "";
        for (var c = 1; c <= columncnt; c++)
        {
            var cscol = CrateState[c];
            if (cscol.Count >= height)
                row += $"[{cscol[cscol.Count - height]}] ";
            else
                row += "    ";
        }
        Console.WriteLine(row);
    }
    for (var c = 1; c <= columncnt; c++)
    Console.Write($" {c}  ");
    Console.WriteLine();
}

// Begin operations.  Read input file and parse
// the data portion:
var lines = File.ReadAllLines("input.txt");
InitializeCrateState(lines);

// Print the initial state for diagnostics
// and to prove that the data was parsed successfully
Console.WriteLine("Starting state: ");
PrintCrateState();

// Move crates using model 9000 behavior
MoveCrates(lines, false);

// Get the top crate in each column and join into
// a single string
var topcrates = string.Join("", 
    CrateState.Select(c => c.Value[0]));
PrintCrateState();
Console.WriteLine($"Top crates for model 9000: {topcrates}");

// Reset the crate state
InitializeCrateState(lines);

// Move crates using model 9001 behavior
MoveCrates(lines, true);

// Get the top crate in each column and join into
// a single string
topcrates = string.Join("", 
    CrateState.Select(c => c.Value[0]));
PrintCrateState();
Console.WriteLine($"Top crates for model 9001: {topcrates}");