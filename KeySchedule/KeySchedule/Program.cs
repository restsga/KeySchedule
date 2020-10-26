using System;

namespace KeySchedule
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            DFS dfs = new DFS();
            dfs.Search(0b1100010001000, 0);

            Console.WriteLine("End");
        }
    }
}
