namespace aoc_2024_09;

internal class Program
{
    private static readonly AocFile EmptyFile = new(-1, 0);
    
    private static void Main()
    {
        Console.WriteLine("Advent of Code 2024 - Day 9");
        
        var input = File.ReadAllLines("input.txt").First().AsSpan();
        //var input = File.ReadAllLines("sample.txt").First().AsSpan();
        
        var memory = new List<AocFile>();
        
        var fileId = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var length = int.Parse(input[i].ToString());
            var isEmpty = i % 2 != 0;
            
            var file = isEmpty ? EmptyFile : new AocFile(fileId, length);
            if (!isEmpty)
            {
                fileId++;
            }
            
            for (var j = 0; j < length; j++)
            {
                memory.Add(isEmpty ? EmptyFile : file);
            }
        }

        Console.WriteLine("Input size: " + input.Length);
        Console.WriteLine("Memory size: " + memory.Count);
        
        //Part1(memory);
        Part2(memory, debug: false);
    }

    private static void Part1(List<AocFile> memory)
    {
        //PrintMemory(memory);
        
        var emptyLocationIndex = FindFirstEmptyLocation(memory);
        for (var i = memory.Count - 1; i >= 0; i--)
        {
            if (emptyLocationIndex >= i)
            {
                break;
            }
            if (memory[i] != EmptyFile)
            {
                var file = memory[i];
                memory[i] = EmptyFile;
                memory[emptyLocationIndex] = file;
                emptyLocationIndex = FindFirstEmptyLocation(memory);
                //PrintMemory(memory);
            }
        }

        Console.WriteLine($"Part 1: {ComputeChecksum(memory)}");
    }
    
    private static void Part2(List<AocFile> memory, bool debug = false)
    {
        if (debug)
        {
            PrintMemory(memory);
        }

        for (var i = memory.Count - 1; i >= 0; i--)
        {
            if (memory[i] != EmptyFile)
            {
                var file = memory[i];

                if (debug)
                {
                    Console.WriteLine($"{i}: ID: {file.FileId} | L: {file.Length} | M: {file.IsMoved}");
                }
                
                var (gapStartIndex, _) = FindGap(memory, file.Length);
                
                if (gapStartIndex == -1 || gapStartIndex >= i)
                {
                    i -= file.Length - 1;
                    continue;
                }

                if (debug)
                {
                    Console.WriteLine("Gap found at: " + gapStartIndex);
                }
                
                file.IsMoved = true;
                
                var length = file.Length;
                while (length > 0)
                {
                    memory[i] = EmptyFile;
                    memory[gapStartIndex] = file;
                    i--;
                    length--;
                    gapStartIndex++;
                }
                i++;
                
                if (debug)
                {
                    PrintMemory(memory);
                }
            }
            else
            {
                if (debug)
                {
                    Console.WriteLine($"{i}: Empty");
                }
            }
        }

        if (debug)
        {
            PrintMemory(memory);
            Console.WriteLine();
            Console.WriteLine();
        }
        
        Console.WriteLine($"Part 2: {ComputeChecksum(memory)}");
    }
    
    private static long ComputeChecksum(List<AocFile> memory)
    {
        var checksum = 0L;

        for (var i = 0; i < memory.Count; i++)
        {
            var file = memory[i];
            
            if (file == EmptyFile)
            {
                continue;
            }
            
            checksum += i * file.FileId;
        }

        return checksum;
    }
    
    private static void PrintMemory(List<AocFile> memory)
    {
        foreach (var file in memory)
        {
            Console.Write(file.FileId == -1 ? "." : file.FileId.ToString());
        }
        Console.WriteLine();
    }
    
    private static int FindFirstEmptyLocation(List<AocFile> memory, int start = 0)
    {
        if (start < 0 || start >= memory.Count)
        {
            return -1;
        }
        
        for (var i = start; i < memory.Count; i++)
        {
            if (memory[i].FileId == -1)
            {
                return i;
            }
        }

        return -1;
    }

    private static (int, int) FindGap(List<AocFile> memory, int targetLength = 1)
    {
        var findFirstEmptyLocation = FindFirstEmptyLocation(memory);
        var start = findFirstEmptyLocation;
        
        while (findFirstEmptyLocation != -1)
        {
            var length = 0;
            var i = findFirstEmptyLocation;
            
            while (i > 0 && i < memory.Count && memory[i] == EmptyFile)
            {
                start++;
                length++;
                i++;
                
                if (targetLength == length)
                {
                    return (findFirstEmptyLocation, length);
                }
            }
            
            findFirstEmptyLocation = FindFirstEmptyLocation(memory, start);
            start = findFirstEmptyLocation;
        }
        
        return (-1, -1);
    }
}

internal class AocFile(int FileId, int Length, bool IsMoved = false)
{
    public int FileId { get; } = FileId;
    public int Length { get; } = Length;
    public bool IsMoved { get; set; } = IsMoved;
}