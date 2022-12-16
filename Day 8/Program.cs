var input = "input.txt";

var lines = File.ReadAllLines(input)
    .ToList();
List<Tree> trees = new List<Tree>();

// Parse Tree Data
for (var x = 0; x < lines.Count; x++)
{
    for (var y = 0; y < lines[x].Length; y++)
    {
        int height = 0;
        int.TryParse(lines[x][y].ToString(), out height);
        trees.Add(new Tree(y, x, height));
    }
}

// Part 1: tree visibility
var visibletrees = trees.Count(tree => 
        // Trees to the left
        trees.Where(rowtree => rowtree.x < tree.x && rowtree.y == tree.y)
        .All(rowtree => rowtree.height < tree.height) ||
        // Trees to the right
        trees.Where(rowtree => rowtree.x > tree.x && rowtree.y == tree.y)
        .All(rowtree => rowtree.height < tree.height) ||
        // Trees above
        trees.Where(coltree => coltree.y < tree.y && coltree.x == tree.x)
        .All(rowtree => rowtree.height < tree.height) ||
        // Trees below
        trees.Where(coltree => coltree.y > tree.y && coltree.x == tree.x)
        .All(coltree => coltree.height < tree.height)
);
Console.WriteLine($"Part 1: {visibletrees}");

// Part 2: Highest scenic score
var maxscore = trees.Max(tree => {
    var rowsize = trees.Where(rowtree => rowtree.x == tree.x)
        .Max(rowtree => rowtree.y);
    var colsize = trees.Where(coltree => coltree.y == tree.y)
        .Max(coltree => coltree.x);
    // Left score
    var left = tree.x == 0 ? 0 : 
        (tree.x - (trees.Where(rowtree => rowtree.x < tree.x && rowtree.y == tree.y)
            .LastOrDefault(rowtree => rowtree.height >= tree.height)?.x ?? 0));
    // Right score
    var right = tree.x == rowsize ? 0 : 
        ((trees.Where(rowtree => rowtree.x > tree.x && rowtree.y == tree.y)
            .FirstOrDefault(rowtree => rowtree.height >= tree.height)?.x ?? 
                rowsize) - tree.x);
    // Up score
    var up = tree.y == 0 ? 0 : 
        (tree.y - (trees.Where(coltree => coltree.y < tree.y && coltree.x == tree.x)
            .LastOrDefault(coltree => coltree.height >= tree.height)?.y ?? 0));
    // Down score
    var down = tree.y == colsize ? 0 :  
        ((trees.Where(coltree => coltree.y > tree.y && coltree.x == tree.x)
            .FirstOrDefault(coltree => coltree.height >= tree.height)?.y ?? 
                colsize) - tree.y);
    return left * right * up * down;
});
Console.WriteLine($"Part 2: {maxscore}");