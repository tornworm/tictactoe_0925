using UnityEngine;
using UnityEngine.UI;

namespace TicTacToeGame
{
    public partial class TicTacToe
    {
        private void AIMove()
        {
            if (gameOver || isPlayerTurn)
                return;

            int[] bestMove = FindBestMove();
            int row = bestMove[0];
            int col = bestMove[1];

            board[row, col] = 2;
            cells[row * BOARD_SIZE + col].SetSymbol(ConstString.AISymbol, aiColor);
            isPlayerTurn = true;
            statusText.text = ConstString.PlayerTurnText;

            int result = CheckGameResult();
            if (result != 0)
            {
                EndGame(result);
            }
        }

        private int[] FindBestMove()
        {
            int bestScore = int.MinValue;
            int[] bestMove = new int[] { -1, -1 };

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == 0)
                    {
                        board[i, j] = 2;
                        int score = Minimax(0, false);
                        board[i, j] = 0;

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = new int[] { i, j };
                        }
                    }
                }
            }

            return bestMove;
        }

        private int Minimax(int depth, bool isMaximizing)
        {
            int result = EvaluateBoard();

            if (result != 0)
            {
                return result == 2 ? 10 - depth : depth - 10;
            }

            if (IsBoardFull())
            {
                return 0;
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    for (int j = 0; j < BOARD_SIZE; j++)
                    {
                        if (board[i, j] == 0)
                        {
                            board[i, j] = 2;
                            int score = Minimax(depth + 1, false);
                            board[i, j] = 0;
                            bestScore = Mathf.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    for (int j = 0; j < BOARD_SIZE; j++)
                    {
                        if (board[i, j] == 0)
                        {
                            board[i, j] = 1;
                            int score = Minimax(depth + 1, true);
                            board[i, j] = 0;
                            bestScore = Mathf.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }
    }
}