using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                .WithRows(7)
                .WithCols(7)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            while (true)
            {
                Console.Clear();

                WriteHorizontalLine(board.Rows.First().Cells.Count);

                foreach (var row in board.Rows)
                {
                    Console.WriteLine();
                    WriteVerticalLine();

                    foreach (var cell in row.Cells)
                    {
                        Console.Write(cell.IsAlive ? " x " : "   ");
                        WriteVerticalLine();
                    }

                    Console.WriteLine();
                    WriteHorizontalLine(board.Rows.First().Cells.Count);
                }

                var oldBoard = board.Clone();

                board.NextGeneration();

                if (!board.AnyCellsAlive())
                {
                    WriteMessage("All cells are dead.");
                    break;
                }

                if(oldBoard.AreBoardsEqual(board))
                {
                    WriteMessage("The situation is stable.");
                    break;
                }

                Thread.Sleep(500);
            }

            Console.ReadLine();
        }

        private static void WriteMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(message);
        }

        private static void WriteHorizontalLine(int nrOfCells)
        {
            for (var i = 0; i <= nrOfCells * 4; i++)
                Console.Write("-");
        }

        private static void WriteVerticalLine()
        {
            Console.Write("|");
        }
    }
}