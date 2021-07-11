public class FileListModel
{
    public bool IsDirectory { get; set; } = false;
    public string Name { get; set; }
    public string FullName { get; set; }

    public override string ToString()
    {
        return FullName;
    }
}