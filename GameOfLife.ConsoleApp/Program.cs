using System;
using System.Linq;
using System.Threading;

namespace GameOfLife.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var aliveCellPositions = new KeyValueList<int, int>
            {
                          { 1, 2 },
                                    { 2, 3 },
                { 3, 1 }, { 3, 2 }, { 3, 3 } 
            };

            var board = new BoardBuilder()
                .WithRows(10)
                .WithCols(10)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            while (true)
            {
                Console.Clear();

                for (var i = 0; i <= board.Rows.First().Cells.Count * 4; i++)
                    Console.Write("-");

                foreach (var row in board.Rows)
                {
                    Console.WriteLine();
                    Console.Write("|");

                    foreach (var cell in row.Cells)
                    {
                        Console.Write(cell.IsAlive ? " x " : "   ");
                        Console.Write("|");
                    }

                    Console.WriteLine();

                    for(var i = 0; i <= row.Cells.Count * 4; i++)
                        Console.Write("-");
                }

                board.NextGeneration();

                Thread.Sleep(500);

                if (!board.Rows.Any(r => r.Cells.Any(c => c.IsAlive)))
                    break;
            }

            Console.ReadLine();
        }
    }
}