/*
Each item is represented by a single character 
(case sensitive) and each character has an 
incremental numeric value starting with a = 1
and ending with Z = 52
*/
string ItemTypes = "abcdefghijklmnopqrstuvwxyz";
ItemTypes += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

var Priorities = new Dictionary<char, int>();
int priority = 0;
foreach(char c in ItemTypes.ToCharArray())
{
    priority++;
    Priorities.Add(c, priority);
}

// Read rucksacks and get item info

var lines = File.ReadAllLines("input.txt");
var total = 0;
var badgetotal = 0;
var groupSacks = new List<string>();
foreach (var line in lines)
{
    // The first half of the letters represent
    // items in the first compartment of the rucksack
    var compartment1 = line.Substring(0, line.Length / 2);
    // The second half of the letters represent items
    // in the second compartment
    var compartment2 = line.Substring(line.Length / 2, line.Length - 
        compartment1!.Length);
    // Find items that exist in both compartments.
    // Should only be one item.
    var commonItemList = compartment1.ToCharArray().Intersect(
        compartment2.ToCharArray());
    if (commonItemList.Count() != 1)
    {
        Console.WriteLine("ERROR: More or less than one common item");
        return;
    }
    var commonItem = commonItemList.FirstOrDefault();
    if (!Priorities.ContainsKey(commonItem))
    {
        Console.WriteLine($"ERROR: Item priority not found: {commonItem}");
        return;
    }
    // Sum the priority values of each item that is in
    // both compartments of each sack.
    total += Priorities[commonItem];
    
    // For part 2, every three elves are a group and
    // we need to find the common item they all share.
    groupSacks.Add(line);
    if (groupSacks.Count == 3)
    {
        //Find the common item
        commonItemList = groupSacks[0]
            .Intersect(groupSacks[1])
            .Intersect(groupSacks[2]);
        if (commonItemList.Count() != 1)
        {
            Console.WriteLine("ERROR: More or less than one common item");
            return;
        }

        commonItem = commonItemList.FirstOrDefault();
        if (!Priorities.ContainsKey(commonItem))
        {
            Console.WriteLine($"ERROR: Item priority not found: {commonItem}");
            return;
        }
        badgetotal += Priorities[commonItem];
        // Reset for the next group of 3
        groupSacks = new List<string>();
    }
}
Console.WriteLine(badgetotal);
