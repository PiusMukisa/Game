using System;
using System.Linq;

namespace ConnectFour
{
    public class GameState
    {
        public const int Columns = 7;
        public const int Rows = 6;
        public const int Size = Columns * Rows;

        public int[] Board { get; }

        public Player CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }
        public string StatusMessage { get; private set; } = string.Empty;

        public GameState()
        {
            Board = new int[Size];
            ResetGame();
        }

        public void ResetBoard()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            Array.Fill(Board, 0);
            CurrentPlayer = Player.Player1;
            GameOver = false;
            StatusMessage = "Player 1's turn";
        }

        public bool PlayPiece(int column)
        {
            if (GameOver)
            {
                return false;
            }

            int row = GetLowestEmptyRow(column);
            if (row < 0)
            {
                return false;
            }

            int index = row * Columns + column;
            Board[index] = (int)CurrentPlayer;

            if (HasWinner(row, column))
            {
                GameOver = true;
                StatusMessage = $"{GetCurrentPlayerName()} wins!";
                return true;
            }

            if (Board.All(cell => cell != 0))
            {
                GameOver = true;
                StatusMessage = "Draw!";
                return true;
            }

            CurrentPlayer = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
            StatusMessage = $"{GetCurrentPlayerName()}'s turn";
            return true;
        }

        public int GetLowestEmptyRow(int column)
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (Board[row * Columns + column] == 0)
                {
                    return row;
                }
            }

            return -1;
        }

        public string GetPieceClass(int cellValue)
        {
            return cellValue == (int)Player.Player1
                ? "player1"
                : cellValue == (int)Player.Player2
                    ? "player2"
                    : string.Empty;
        }

        private bool HasWinner(int row, int column)
        {
            return Count(row, column, 0, 1) + Count(row, column, 0, -1) - 1 >= 4
                || Count(row, column, 1, 0) + Count(row, column, -1, 0) - 1 >= 4
                || Count(row, column, 1, 1) + Count(row, column, -1, -1) - 1 >= 4
                || Count(row, column, 1, -1) + Count(row, column, -1, 1) - 1 >= 4;
        }

        private int Count(int row, int column, int rowDelta, int columnDelta)
        {
            int count = 0;
            int current = (int)CurrentPlayer;
            int r = row;
            int c = column;

            while (r >= 0 && r < Rows && c >= 0 && c < Columns && Board[r * Columns + c] == current)
            {
                count++;
                r += rowDelta;
                c += columnDelta;
            }

            return count;
        }

        private string GetCurrentPlayerName()
        {
            return CurrentPlayer == Player.Player1 ? "Player 1" : "Player 2";
        }
    }

    public enum Player
    {
        None = 0,
        Player1 = 1,
        Player2 = 2
    }
}
