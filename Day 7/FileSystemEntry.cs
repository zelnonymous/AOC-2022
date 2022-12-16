public class FileSystemEntry {
    public string FullPath { get; set; }
    public long Size { get; set; }
    public List<FileEntry> Files { get; set; }
    public FileSystemEntry? Parent { get; set; }
    public List<FileSystemEntry> Subdirectories { get; set; }
    public FileSystemEntry(string path)
    {
        FullPath = path;
        Files = new List<FileEntry>();
        Subdirectories = new List<FileSystemEntry>();
    }
}