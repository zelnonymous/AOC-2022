using System.Numerics;

var input = "input.txt";

// Define starting position
var start = new Vector2(0, 0);
// Head is the first knot.  It is used for both
// parts of the puzzle
var head = new Knot(start);
// Tail is the second knot.  It is only used for
// part 1.
var tail = new Knot(start);
// For part 2, we need 10 total knots; the head
// and 9 knots that follow it.
List<Knot> Chain = new List<Knot>();
for (var k = 0; k < 9; k++)
    Chain.Add(new Knot(start));

var instructions = File.ReadAllLines(input);

// A function to update the position of a "trailing" knot.
// The idea is that when a knot in front moves, the knot 
// behind it may need to move as well.  This calculates that move.
void Follow(Knot lead, Knot trail, string direction)
{
    if (Math.Abs(lead.CurrentPosition.X - trail.CurrentPosition.X) <= 1 &&
        Math.Abs(lead.CurrentPosition.Y - trail.CurrentPosition.Y) <= 1)
    {
        // Trailing knot is in range, no need to move it
        return;
    }
    var newtrailpos = new Vector2(
        trail.CurrentPosition.X, 
        trail.CurrentPosition.Y
    );
    if (lead.CurrentPosition.X == trail.CurrentPosition.X)
    {
        if (lead.CurrentPosition.Y > trail.CurrentPosition.Y) {
            // Lead knot is in the same column, but a higher row
            newtrailpos.Y++;
        } else if (lead.CurrentPosition.Y < trail.CurrentPosition.Y) {
            // Lead knot is in the same column, but a lower row
            newtrailpos.Y--;
        }
    } else if (lead.CurrentPosition.X < trail.CurrentPosition.X) {
        if (lead.CurrentPosition.Y > trail.CurrentPosition.Y)
        {
            // Lead knot is in a lower column, but a higher row
            newtrailpos.X--;
            newtrailpos.Y++;
        } else if (lead.CurrentPosition.Y == trail.CurrentPosition.Y) {
            // Lead knot is in a lower column, but the same row
            newtrailpos.X--;
        } else {
            // Lead knot is in a lower column and a lower row
            newtrailpos.X--;
            newtrailpos.Y--;
        }
    } else if (lead.CurrentPosition.X > trail.CurrentPosition.X) {
        if (lead.CurrentPosition.Y > trail.CurrentPosition.Y)
        {
            // Lead knot is in a higher column and a higher row
            newtrailpos.X++;
            newtrailpos.Y++;
        } else if (lead.CurrentPosition.Y == trail.CurrentPosition.Y) {
            // Lead knot is in a higher column, but the same row
            newtrailpos.X++;
        } else {
            // Lead knot is in a higher column, but a lower row
            newtrailpos.X++;
            newtrailpos.Y--;
        }
    }
    trail.Move(newtrailpos);
}

foreach (var instruction in instructions)
{
    // Instruction is two parts, a direction and a number of steps
    var parts = instruction.Split(" ");
    if (parts.Length != 2)
    {
        Console.WriteLine($"Error reading instruction {instruction}!");
        continue;
    }
    var direction = parts[0];
    var steps = 0;
    if (!int.TryParse(parts[1].ToString(), out steps) || steps < 1)
    {
        Console.WriteLine($"Error reading instruction {instruction}!");
        continue;
    }
    // Once we have the instruction, we'll follow each step individually
    // and update the positions of knots accordingly.
    for (var step = 1; step <= steps; step++)
    {
        // Depending on the direction, first update the position of the lead
        // knot by one in the appropriate cardinal direction
        var newheadpos = new Vector2(head.CurrentPosition.X, head.CurrentPosition.Y);
        if (direction == "U")
            newheadpos.Y++;
        if (direction == "D")
            newheadpos.Y--;
        if (direction == "L")
            newheadpos.X--;
        if (direction == "R")
            newheadpos.X++;
        head.Move(newheadpos);
        // This is for part 1.  Update the position of the tail to follow
        // the head
        Follow(head, tail, direction);

        // This is for part 2.  Sequentially update the position of each
        // knot in the chain based on the knot in front of it.
        for (var k = 0; k < 9; k++)
        {
            if(k == 0)
                Follow(head, Chain[k], direction);
            else
                Follow(Chain[k - 1], Chain[k], direction);
        }
    }
}

// Knots track their own unique visited positions, so we just
// need to check the count of those visited positions.
Console.WriteLine($"Part 1: {tail.VistedPositions.Count}");
Console.WriteLine($"Part 2: {Chain[8].VistedPositions.Count}");