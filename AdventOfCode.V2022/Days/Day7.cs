using AdventOfCode.Core.Components;
using AdventOfCode.V2022.Core.Day7;

namespace AdventOfCode.V2022.Days
{
    internal class Day7 : IDay
    {
        public SystemDirectory Root { get; set; }

        public int DayNumber => 7;

        public string PuzzleName => "No Space Left On Device";

        public string Solution1(string[] lines)
        {
            Initialize(lines);

            return Root.GetRemovableDirectories().Sum().ToString();
        }

        public string Solution2(string[] lines)
        {
            Initialize(lines);
            var deltaSpace = 30000000 - (70000000 - Root.Size);

            return Root.GetFolders().Order().First(s => s > deltaSpace).ToString();
        }

        public void Initialize(string[] lines)
        {
            Root = new SystemDirectory();
            SystemDirectory workingDir = Root;
            List<ISystemObject> workChildren = null;

            foreach (var line in lines)
            {
                var args = line.Split(' ');

                if (args[0] == "$")
                {
                    if (workChildren != null)
                    {
                        workingDir.Children = workChildren;
                        workChildren = null;
                    }

                    if (args[1] == "ls") continue;
                    if (args[2] == "..")
                        workingDir = workingDir.Parent;
                    else
                        workingDir = workingDir.FindObject(args[2]) as SystemDirectory;
                }
                else
                {
                    workChildren ??= new();

                    if (args[0] == "dir")
                        workChildren.Add(new SystemDirectory(args[1], workingDir));
                    else
                        workChildren.Add(new SystemFile(args[1], int.Parse(args[0])));
                }
            }

            if (workChildren != null)
                workingDir.Children = workChildren;
        }
    }
}
