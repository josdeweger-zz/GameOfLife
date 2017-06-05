using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    /// <summary>
    /// A Board with a given number of rows, each containing a given number of cells.
    /// E.g.: given 3 rows and 3 columns the Board can be visualized as:
    /// 
    ///          |   Col 0   |   Col 1   |   Col 2   |
    /// ---------|-----------|-----------|-----------|
    /// row 0    |  [0][0]   |   [0][1]  |   [0][2]  |
    /// ---------|-----------|-----------|-----------|
    /// row 1    |  [1][0]   |   [1][1]  |   [1][2]  |
    /// ---------|-----------|-----------|-----------|
    /// row 2    |  [2][0]   |   [2][1]  |   [2][2]  |
    /// ---------|-----------|-----------|-----------|
    /// </summary>
    public class Board
    {
        public List<Row> Rows { get; private set; }

        private readonly int _rows;
        private readonly int _cols;

        public Board(int rows, int cols)
        {
            if (rows <= 0)
                throw new ArgumentException($"{nameof(rows)} can not be 0 or less");

            if (cols <= 0)
                throw new ArgumentException($"{nameof(cols)} can not be 0 or less");

            _rows = rows;
            _cols = cols;

            Rows = new List<Row>(_rows);
        }

        public void Create(List<KeyValuePair<int, int>> aliveCellPositions)
        {
            Rows = Enumerable
                .Range(0, _rows)
                .Select((rowNum, row) => new Row()
                {
                    Cells = Enumerable
                        .Range(0, _cols)
                        .Select((colNum, cell) =>
                        {
                            var isAlive = aliveCellPositions.Contains(new KeyValuePair<int, int>(rowNum, colNum));
                            return new Cell(isAlive);
                        })
                        .ToList()
                })
                .ToList();
        }

        public Cell TryGetCell(int rowNum, int colNum)
        {
            if(rowNum > Rows.Count - 1)
                throw new ArgumentOutOfRangeException($"Row {rowNum} does not exist");
            
            if (colNum > Rows.ElementAt(rowNum).Cells.Count - 1)
                throw new ArgumentOutOfRangeException($"Column {colNum} does not exist");

            return Rows.ElementAt(rowNum).Cells.ElementAt(colNum);
        }

        public void NextGeneration()
        {
            var nextGeneration = Enumerable
                .Range(0, _rows)
                .Select((rowNum, row) => new Row()
                {
                    Cells = Enumerable
                        .Range(0, _cols)
                        .Select((colNum, cell) => CreateNextGenerationCell(rowNum, colNum))
                        .ToList()
                })
                .ToList();

            Rows = nextGeneration;
        }

        private Cell CreateNextGenerationCell(int rowNum, int colNum)
        {
            var previousGenerationAlive = TryGetCell(rowNum, colNum).IsAlive;

            var nextGenerationCell = new Cell(previousGenerationAlive);

            var neighbours = GetCellNeighbours(rowNum, colNum);

            nextGenerationCell.NextGeneration(neighbours);

            return nextGenerationCell;
        }

        private List<Cell> GetCellNeighbours(int rowNum, int colNum)
        {
            var neighboursRowStart = rowNum > 0 ? rowNum - 1 : 0;
            var neighboursRowEnd = rowNum == Rows.Count - 1 ? rowNum : rowNum + 1;
            var neighboursColStart = colNum > 0 ? colNum - 1 : 0;
            var neighboursColEnd = colNum == Rows[rowNum].Cells.Count - 1 ? colNum : colNum + 1;

            var neighbours = new List<Cell>();

            for (var sectionRowNum = neighboursRowStart; sectionRowNum <= neighboursRowEnd; sectionRowNum++)
            {
                for (var sectionColNum = neighboursColStart; sectionColNum <= neighboursColEnd; sectionColNum++)
                {
                    if (!(sectionRowNum == rowNum && sectionColNum == colNum))
                        neighbours.Add(TryGetCell(sectionRowNum, sectionColNum));
                }
            }

            return neighbours;
        }
    }
}