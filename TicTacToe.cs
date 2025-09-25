using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TicTacToeGame
{
    public partial class TicTacToe : MonoBehaviour
    {
        // 游戏中常量
        private const int BOARD_SIZE = 3;
        private const float CELL_SIZE = 100f;
        private const float CELL_SPACING = 10f;

        // 游戏状态
        private int[,] board = new int[BOARD_SIZE, BOARD_SIZE];
        private bool isPlayerTurn = true;
        private bool gameOver = false;

        // UI引用
        private Canvas gameCanvas;
        private GameObject boardContainer;
        private Cell[] cells = new Cell[BOARD_SIZE * BOARD_SIZE];
        private Text statusText;
        private Button restartButton;

        // 外观设置
        private Color playerColor = Color.blue;
        private Color aiColor = Color.red;
        private Color cellBackgroundColor = Color.gray;
        private Color cellHighlightColor = Color.white;
        private Font defaultFont;

        void Start()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            InitializeBoard();
            CreateUI();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    board[i, j] = 0;
                }
            }
            gameOver = false;
            isPlayerTurn = true;
        }

        public void ProcessPlayerMove(int row, int col)
        {
            if (gameOver || !isPlayerTurn || board[row, col] != 0)
                return;

            board[row, col] = 1;
            cells[row * BOARD_SIZE + col].SetSymbol(ConstString.PlayerSymbol, playerColor);
            isPlayerTurn = false;
            statusText.text = ConstString.AIThinkingText;

            int result = CheckGameResult();
            if (result == 0)
            {
                Invoke(nameof(AIMove), 0.5f);
            }
            else
            {
                EndGame(result);
            }
        }

        private int CheckGameResult()
        {
            int winner = EvaluateBoard();
            if (winner != 0)
            {
                return winner;
            }

            if (IsBoardFull())
            {
                return -1;
            }

            return 0;
        }

        private int EvaluateBoard()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != 0)
                {
                    return board[i, 0];
                }
            }

            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (board[0, j] == board[1, j] && board[1, j] == board[2, j] && board[0, j] != 0)
                {
                    return board[0, j];
                }
            }

            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != 0)
            {
                return board[0, 0];
            }

            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != 0)
            {
                return board[0, 2];
            }

            return 0;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void EndGame(int result)
        {
            gameOver = true;

            switch (result)
            {
                case 1:
                    statusText.text = ConstString.PlayerWinText;
                    statusText.color = playerColor;
                    break;
                case 2:
                    statusText.text = ConstString.AIWinText;
                    statusText.color = aiColor;
                    break;
                case -1:
                    statusText.text = ConstString.DrawText;
                    statusText.color = Color.gray;
                    break;
            }
        }

        private void ResetGame()
        {
            InitializeBoard();

            foreach (Cell cell in cells)
            {
                cell.ResetCell();
            }

            statusText.text = ConstString.PlayerTurnText;
            statusText.color = Color.white;
        }

        public class Cell : MonoBehaviour
        {
            private int row;
            private int col;
            private Text cellText;
            private TicTacToe gameManager;

            public void Initialize(int row, int col, Text text, TicTacToe manager)
            {
                this.row = row;
                this.col = col;
                this.cellText = text;
                this.gameManager = manager;
            }

            public void OnClick()
            {
                gameManager.ProcessPlayerMove(row, col);
            }

            public void SetSymbol(string symbol, Color color)
            {
                cellText.text = symbol;
                cellText.color = color;
                cellText.enabled = true;
                GetComponent<Button>().interactable = false;
            }

            public void ResetCell()
            {
                cellText.text = "";
                cellText.enabled = false;
                GetComponent<Button>().interactable = true;
            }
        }
    }
}