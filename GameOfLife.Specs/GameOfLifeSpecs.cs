using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace GameOfLife.Specs
{
    public class GameOfLifeSpecs
    {
        [Fact]
        public void WhenBuildingTheBoardWith2RowsAnd2Cols_ItShouldBeCreated()
        {
            var result = new BoardBuilder()
                .WithRows(2)
                .WithCols(2)
                .Build();

            result.Rows.ElementAt(0).Cells.Count.Should().Be(2);
            result.Rows.ElementAt(1).Cells.Count.Should().Be(2);
        }

        [Fact]
        public void WhenBuildingTheBoardWith4RowsAnd7Cols_ItShouldBeCreated()
        {
            var result = new BoardBuilder()
                .WithRows(7)
                .WithCols(4)
                .Build();

            result.Rows.ElementAt(0).Cells.Count.Should().Be(4);
            result.Rows.ElementAt(6).Cells.Count.Should().Be(4);
        }

        [Fact]
        public void WhenBuildingTheBoardWithoutRows_ItThrows()
        {
            Action result = () => new BoardBuilder()
                .WithCols(2)
                .Build();

            result.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void WhenBuildingTheBoardWithANegativeNumberOfRows_ItThrows()
        {
            Action result = () => new BoardBuilder()
                .WithRows(-1)
                .WithCols(2)
                .Build();

            result.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void WhenBuildingTheBoardWithoutCols_ItThrows()
        {
            Action result = () => new BoardBuilder()
                .WithRows(2)
                .Build();

            result.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void WhenBuildingTheBoardWithANegativeNumberOfCols_ItThrows()
        {
            Action result = () => new BoardBuilder()
            .WithRows(2)
            .WithCols(-2)
            .Build();

            result.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void WhenTryingToGetAnExistingCellFromTheBoard_ItReturnsTheCell()
        {
            var board = new BoardBuilder()
                .WithRows(1)
                .WithCols(1)
                .Build();

            var result = board.TryGetCell(0, 0);

            result.Should().BeOfType<Cell>();
        }

        [Fact]
        public void WhenTryingToGetACellOutsideTheRowRange_ItThrows()
        {
            var board = new BoardBuilder()
                .WithRows(1)
                .WithCols(1)
                .Build();

            Action result = () => board.TryGetCell(2, 0);

            result.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WhenTryingToGetACellOutsideTheColumnRange_ItThrows()
        {
            var board = new BoardBuilder()
                .WithRows(1)
                .WithCols(1)
                .Build();

            Action result = () => board.TryGetCell(0, 2);

            result.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WhenBuildingTheBoardWithOneLivingCell_OnlyTheCellOnThatPositionIsAlive()
        {
            var aliveCellPositions = new KeyValueList<int, int> { { 1, 0 } };

            var result = new BoardBuilder()
                .WithRows(2)
                .WithCols(1)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            result.TryGetCell(0, 0).IsAlive.Should().BeFalse();
            result.TryGetCell(1, 0).IsAlive.Should().BeTrue();
        }

        [Fact]
        public void WhenBuildingTheBoardWithThreeLivingCells_OnlyTheThreeCellsOnThesePositionsAreAlive()
        {
            var aliveCellPositions = new KeyValueList<int, int>
            {
                { 0, 0 },
                { 1, 1 },
                { 2, 2 }
            };

            var result = new BoardBuilder()
                .WithRows(3)
                .WithCols(3)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            result.TryGetCell(0, 0).IsAlive.Should().BeTrue();
            result.TryGetCell(0, 1).IsAlive.Should().BeFalse();
            result.TryGetCell(0, 2).IsAlive.Should().BeFalse();

            result.TryGetCell(1, 0).IsAlive.Should().BeFalse();
            result.TryGetCell(1, 1).IsAlive.Should().BeTrue();
            result.TryGetCell(1, 2).IsAlive.Should().BeFalse();

            result.TryGetCell(2, 0).IsAlive.Should().BeFalse();
            result.TryGetCell(2, 1).IsAlive.Should().BeFalse();
            result.TryGetCell(2, 2).IsAlive.Should().BeTrue();
        }

        [Fact]
        public void WhenGeneratingTheNextGenerationForAnAliveCellWithLessThan2AliveNeighbours_TheCellDies()
        {
            var aliveCellPositions = new KeyValueList<int, int> { { 0, 0 } };

            var board = new BoardBuilder()
                .WithRows(2)
                .WithCols(2)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            board.NextGeneration();

            board.TryGetCell(0, 0).IsAlive.Should().BeFalse();
        }

        [Fact]
        public void WhenGeneratingTheNextGenerationForAnAliveCellWith2AliveNeighbours_TheCellStaysAlive()
        {
            var aliveCellPositions = new KeyValueList<int, int>
            {
                { 0, 0 },
                { 0, 1 },
                { 1, 0 }
            };

            var board = new BoardBuilder()
                .WithRows(2)
                .WithCols(2)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            board.NextGeneration();

            board.TryGetCell(0, 0).IsAlive.Should().BeTrue();
        }

        [Fact]
        public void WhenGeneratingTheNextGenerationForAnAliveCellWith3AliveNeighbours_TheCellStaysAlive()
        {
            var aliveCellPositions = new KeyValueList<int, int>
            {
                { 0, 0 },
                { 0, 1 },
                { 1, 0 },
                { 1, 1 }
            };

            var board = new BoardBuilder()
                .WithRows(2)
                .WithCols(2)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            board.NextGeneration();

            board.TryGetCell(0, 0).IsAlive.Should().BeTrue();
        }

        [Fact]
        public void WhenGeneratingTheNextGenerationForAnAliveCellWithMoreThan3AliveNeighbours_TheCellDies()
        {
            var aliveCellPositions = new KeyValueList<int, int>
            {
                { 0, 0 },
                { 0, 1 },
                { 0, 2 },
                { 1, 0 },
                { 1, 1 }
            };

            var board = new BoardBuilder()
                .WithRows(3)
                .WithCols(3)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            board.NextGeneration();

            board.TryGetCell(1, 1).IsAlive.Should().BeFalse();
        }

        [Fact]
        public void WhenGeneratingTheNextGenerationForADeadCellWith3AliveNeighbours_TheCellResurrects()
        {
            var aliveCellPositions = new KeyValueList<int, int>
            {
                { 0, 0 },
                { 0, 1 },
                { 0, 2 }
            };

            var board = new BoardBuilder()
                .WithRows(3)
                .WithCols(3)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            board.NextGeneration();

            board.TryGetCell(1, 1).IsAlive.Should().BeTrue();
        }

        [Fact]
        public void WhenGeneratingTheNextGenerationForADeadCellWith2AliveNeighbours_TheCellStaysDead()
        {
            var aliveCellPositions = new KeyValueList<int, int>
            {
                { 0, 0 },
                { 0, 1 }
            };

            var board = new BoardBuilder()
                .WithRows(3)
                .WithCols(3)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            board.NextGeneration();

            board.TryGetCell(1, 1).IsAlive.Should().BeFalse();
        }

        [Fact]
        public void WhenGeneratingTheNextGenerationForADeadCellWith4AliveNeighbours_TheCellStaysDead()
        {
            var aliveCellPositions = new KeyValueList<int, int>
            {
                { 0, 0 },
                { 0, 1 },
                { 2, 0 },
                { 2, 1 }
            };

            var board = new BoardBuilder()
                .WithRows(3)
                .WithCols(3)
                .WithAliveCellsOn(aliveCellPositions)
                .Build();

            board.NextGeneration();

            board.TryGetCell(1, 1).IsAlive.Should().BeFalse();
        }
    }
}
