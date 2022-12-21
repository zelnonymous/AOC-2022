using System.Text.RegularExpressions;

var input = "example.txt";

var data = File.ReadAllLines(input);
var Monkeys = new Dictionary<int, Monkey>();

void LoadMonkeys()
{
    Monkeys = new Dictionary<int, Monkey>();
    var monkeyData = new List<string>();
    foreach (var line in data)
    {
        if(line == string.Empty)
        {
            ParseMonkey(monkeyData);
            monkeyData = new List<string>();
            continue;
        }
        monkeyData.Add(line);
    }
    if (monkeyData.Count > 0)
        ParseMonkey(monkeyData);
}
void ParseMonkey (List<string> MonkeyData)
{
    var NumberExpr = new Regex(@"^Monkey ([0-9]*)");
    var ItemsExpr = new Regex(@"^\s*Starting items: ([0-9\, ]*)");
    var OperExpr = new Regex(@"^\s*Operation: new = old ([\+\*]) (.*)");
    var TestExpr = new Regex(@"^\s*Test: divisible by ([0-9]*)");
    var TrueExpr = new Regex(@"^\s*If true: throw to monkey ([0-9]*)");
    var FalseExpr = new Regex(@"^\s*If false: throw to monkey ([0-9]*)");
    Monkey monkey = new Monkey();
    var number = -1;
    foreach(string line in MonkeyData)
    {
        var NumberExprMatch = NumberExpr.Match(line);
        var ItemsExprMatch = ItemsExpr.Match(line);
        var OperExprMatch = OperExpr.Match(line);
        var TestExprMatch = TestExpr.Match(line);
        var TrueExprMatch = TrueExpr.Match(line);
        var FalseExprMatch = FalseExpr.Match(line);
        if (NumberExprMatch.Success)
            int.TryParse(NumberExprMatch.Groups[1].Value, out number);
        if (ItemsExprMatch.Success)
        {
            monkey.Items.AddRange(
                ItemsExprMatch.Groups[1].Value.Split(", ")
                    .Select(itm => {
                        long itemWorry = -1;
                        long.TryParse(itm, out itemWorry);
                        return itemWorry;
                    })
                    .ToList()
            );
            if (monkey.Items.Any(itm => itm == -1))
                throw new Exception($"Invalid item worry level in {line}");
        }
        if (OperExprMatch.Success)
        {
            monkey.Operation = OperExprMatch.Groups[1].Value;
            monkey.Operand = OperExprMatch.Groups[2].Value;
            if (monkey.Operand != "old")
            {
                int opval = 0;
                if (!int.TryParse(monkey.Operand, out opval))
                    throw new Exception($"Invalid operand value in {line}");
                monkey.ValueOperand = opval;
            }
        }
        if (TestExprMatch.Success)
        {
            int testval = -1;
            if (!int.TryParse(TestExprMatch.Groups[1].Value, out testval))
               throw new Exception($"Invalid test value in {line}");
            monkey.Test = testval;
        }
        if (TrueExprMatch.Success)
        {
            int truedest = -1;
            if (!int.TryParse(TrueExprMatch.Groups[1].Value, out truedest))
                throw new Exception($"Invalid true destination in {line}");
            monkey.TrueDestination = truedest;
        }
        if (FalseExprMatch.Success)
        {
            int falsedest = -1;
            if (!int.TryParse(FalseExprMatch.Groups[1].Value, out falsedest))
                throw new Exception($"Invalid false destination in {line}");
            monkey.FalseDestination = falsedest;
        }
    }
    Monkeys!.Add(number, monkey);
}
void MonkeyAround(bool worried)
{
    if (Monkeys == null)
    {
        Console.WriteLine("No monkeys here.");
        return;
    }
    foreach (var monkeyEntry in Monkeys)
    {
        
        var monkey = monkeyEntry.Value;
        monkey.Items = monkey.Items
            .Select(item => 
                monkey.Operation == "*" && monkey.Operand == "old" ?
                    item * item :
                monkey.Operation == "*" ? 
                    item * monkey.ValueOperand :
                monkey.Operation == "+" && monkey.Operand == "old" ?
                    item + item :
                item + monkey.ValueOperand
            ).ToList();
        monkey.ItemsInspected += monkey.Items.Count;
        if (!worried)
        {
            monkey.Items = monkey.Items
                .Select(item =>
                    (long)Math.Floor(item / 3d)
                ).ToList();
        }
        foreach (long item in monkey.Items)
        {
            if (item % monkey.Test == 0)
                Monkeys[monkey.TrueDestination].Items.Add(item);
            else
                Monkeys[monkey.FalseDestination].Items.Add(item);
        }
        monkey.Items = new List<long>();
    }
}
void MonkeyDiagnostics()
{
    if (Monkeys == null)
    {
        Console.WriteLine("No monkeys here.");
        return;
    }
    foreach(var monkeyEntry in Monkeys)
    {
        var diag = $"Monkey {monkeyEntry.Key}: ";
        diag += monkeyEntry.Value.ItemsInspected;
        Console.WriteLine(diag);
    }
}
string GetMonkeyBusiness()
{
    if (Monkeys == null)
    {
        Console.WriteLine("No monkeys here.");
        return string.Empty;
    }
    var activeMonkeyCounts = Monkeys
        .Select(m => m.Value.ItemsInspected)
        .OrderByDescending(i => i)
        .Take(2);
    long total = 1;
    foreach (int activeMonkeyCount in activeMonkeyCounts)
    {
        total = total * activeMonkeyCount;
    }
    return total.ToString();
}
LoadMonkeys();
var round = 1;
while (round <= 20)
{
    MonkeyAround(false);
    round++;
}
MonkeyDiagnostics();
var monkeyBusiness = GetMonkeyBusiness();
Console.WriteLine($"Part 1: {monkeyBusiness}");
LoadMonkeys();
round = 1;
while (round <= 10000)
{
    MonkeyAround(true);
    round++;
}
MonkeyDiagnostics();
monkeyBusiness = GetMonkeyBusiness();
Console.WriteLine($"Part 2: {monkeyBusiness}");