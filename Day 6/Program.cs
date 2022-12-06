// Number of unique characters constituting markers
int packetbuffersize = 4;
int messagebuffersize = 14;

// Process characters from the input (data) into
// a buffer of the specified size until all characters
// in the buffer are unique, at which point return the
// number of characters processed to reach that state.
int GetMarker(string data, int buffersize)
{
    int charcnt = 0;
    Queue<char> buffer = new Queue<char>();
    foreach (char d in data)
    {
        charcnt++;
        if (buffer.Count == buffersize)
            buffer.Dequeue();
        buffer.Enqueue(d);
        if (buffer.Distinct().Count() == buffersize)
            return charcnt;
    }
    return 0;
}

// Read data from input.  Only the first line is used.
var lines = File.ReadAllLines("input.txt");
var data = lines.FirstOrDefault();
if (data == null)
    return;

// Get results
int packetmarker = GetMarker(data, packetbuffersize);
int messagemarker = GetMarker(data, messagebuffersize);
Console.WriteLine($"Start of Packet: {packetmarker}");
Console.WriteLine($"Start of Message: {messagemarker}");