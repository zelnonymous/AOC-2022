/*
Premise: Opponent choices can be Rock (A), Paper (B), or
Scissors (C).  My choices can be Rock (X), Paper (Y), or
Scissors (Z).  Desired result for a round can be lose (X),
draw (Y), or win (Z). This is a lookup dictionary to determine 
what I should choose based on my desired result.  For exmple,
if my opponent chooses Rock (A) and I desire to lose (X), then
I should choose Scizzors (Z), so MyChoice["A"]["X"] = "Z".
*/
var MyChoice = new Dictionary<string, Dictionary<string, string>>()
{
    {"A", new Dictionary<string, string>() {
        {"X", "Z"}, {"Y", "X"}, {"Z", "Y"}
    }},
    {"B", new Dictionary<string, string>() {
        {"X", "X"}, {"Y", "Y"}, {"Z", "Z"}
    }},
    {"C", new Dictionary<string, string>() {
        {"X", "Y"}, {"Y", "Z"}, {"Z", "X"}
    }}
};
/*
Another lookup dictionary to get my score for a round based on my
opponent's choice and my own choice.  A loss is 0 points,
a draw is 3 points, and a win is 6 points.
*/
var MatchResults = new Dictionary<string, Dictionary<string, int>>()
{
    {"A", new Dictionary<string, int>() {
        {"X", 3}, {"Y", 6}, {"Z", 0}
    }},
    {"B", new Dictionary<string, int>() {
        {"X", 0}, {"Y", 3}, {"Z", 6}
    }},
    {"C", new Dictionary<string, int>() {
        {"X", 6}, {"Y", 0}, {"Z", 3}
    }}
};
/*
Additional points are awarded based on my selection; 1 point
for rock, two for paper, and 3 for scissors.
*/
var SelectionBonus = new Dictionary<string, int>() {
    {"X", 1},
    {"Y", 2},
    {"Z", 3}
};

var totalScore = 0;
var lines = File.ReadAllLines("input.txt");
foreach (var line in lines)
{
    if (line.Trim() == string.Empty)
        continue;
    var game = line.Split(" ");
    if (game.Length < 2)
        continue;
    var selection = MyChoice[game[0]][game[1]];
    totalScore += MatchResults[game[0]][selection]
        + SelectionBonus[selection];
}
Console.WriteLine(totalScore);