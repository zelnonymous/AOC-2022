public class Tree
{
    public int x { get; set; }
    public int y { get; set; }
    public int height { get; set; }
    public Tree(int xpos, int ypos, int treeheight)
    {
        x = xpos;
        y = ypos;
        height = treeheight;
    }
}