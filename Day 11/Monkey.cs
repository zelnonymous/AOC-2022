public class Monkey
{
    public List<long> Items { get; set; }
    public string Operation { get; set; }
    public string Operand { get; set; }
    public int ValueOperand { get; set; }
    public int Test { get; set; }
    public int TrueDestination { get; set; }
    public int FalseDestination { get; set; }
    public int ItemsInspected { get; set; }
    public Monkey()
    {
        Items = new List<long>();
        Operation = string.Empty;
        Operand = string.Empty;
    }
}