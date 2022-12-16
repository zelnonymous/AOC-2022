using System.Text.RegularExpressions;
public class ElfDevice
{

    public string LastCmd { get; set; }
    public FileSystemEntry CWD { get; set; }
    public FileSystemEntry Root { get; set; }
    public List<FileSystemEntry> DeviceFS { get; set; }
    public ElfDevice()
    {
        LastCmd = string.Empty;
        Root = new FileSystemEntry("/");
        CWD = Root;
        DeviceFS = new List<FileSystemEntry>() { Root };
    }
    public void ProcessCommand(string cmd)
    {
        LastCmd = cmd;
        var tokens = cmd.Split(" ");
        if (tokens.Length < 2)
            return;
        if (tokens[0] != "cd")
            return;
        if (tokens[1] == "/")
        {
            CWD = Root;
            return;
        }
        if (tokens[1] == "..")
        {
            if (CWD.FullPath == "/")
                return;
            var pathparts = CWD.FullPath.Split("/");
            var strprevpath = string.Join('/', pathparts
                .Where(p =>  p != pathparts.LastOrDefault())
                .ToArray());
            if (strprevpath == string.Empty)
                strprevpath = "/";
            var prevpath = DeviceFS
                .FirstOrDefault(d => d.FullPath == strprevpath);
            if (prevpath == null)
                return;
            CWD = prevpath;
            return;
        }
        var strnewpath = CWD.FullPath;
        if (strnewpath.LastOrDefault() != '/')
            strnewpath += "/";
        strnewpath += tokens[1];
        var newpath = DeviceFS
            .FirstOrDefault(d => d.FullPath == strnewpath);
        if (newpath == null)
        {
            newpath = new FileSystemEntry(strnewpath);
            DeviceFS.Add(newpath);
            newpath.Parent = CWD;
            CWD.Subdirectories.Add(newpath);
        }
        CWD = newpath;
    
    }
    public void ReplayConsole()
    {
        var lines = File.ReadAllLines("input.txt");

        foreach (var line in lines)
        {
            var cmdmatch = Regex.Match(line, "^\\$ (.*)");
            if (cmdmatch.Success)
            {
                ProcessCommand(cmdmatch.Groups[1].Value);
                continue;
            }
            var filesize = Regex.Match(line, "^([0-9]*) (.*)");
            if (!filesize.Success || !Regex.Match(LastCmd, "^ls")
                .Success)
                continue;
            var name = filesize.Groups[2].Value;
            long size;
            if (!long.TryParse(filesize.Groups[1].Value, out size))
                continue;
            CWD.Files.Add(new FileEntry(name, size));
            CWD.Size += size;
        }
        SumHeirarchy(Root);
        Console.WriteLine("Part 1: " + 
            string.Join(Environment.NewLine, 
                DeviceFS
                .Select(d => d.Size)
                .Where(s => s <= 100000)
                .Sum()
            )
        );
    }
    public void SumHeirarchy(FileSystemEntry directory)
    {
        foreach(FileSystemEntry subdir in directory.Subdirectories)
        {
            SumHeirarchy(subdir);
            directory.Size += subdir.Size;
        }
    }
    static void Main(string[] args)
    {
        ElfDevice device = new ElfDevice();
        device.ReplayConsole();
    }
}