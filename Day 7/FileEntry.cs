public class FileEntry {
    public string FileName { get; set; }
    public long Size { get; set; }
    public FileEntry(string name, long length)
    {
        FileName = name;
        Size = length;
    }
}