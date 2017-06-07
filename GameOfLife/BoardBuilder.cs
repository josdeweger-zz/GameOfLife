namespace GameOfLife
{
    public class BoardBuilder
    {
        private int _rows;
        private int _cols;
        private KeyValueList<int, int> _aliveCellPositions;

        public BoardBuilder()
        {
            _aliveCellPositions = new KeyValueList<int, int>();
        }

        public BoardBuilder WithRows(int rows)
        {
            _rows = rows;
            return this;
        }

        public BoardBuilder WithCols(int cols)
        {
            _cols = cols;
            return this;
        }

        public BoardBuilder WithAliveCellsOn(KeyValueList<int, int> positions)
        {
            _aliveCellPositions = positions;
            return this;
        }

        public Board Build()
        {
            var board = new Board(_rows, _cols, _aliveCellPositions);

            return board;
        }
    }
}