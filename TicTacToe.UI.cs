using UnityEngine;
using UnityEngine.UI;

namespace TicTacToeGame
{
    public partial class TicTacToe
    {
        private void CreateUI()
        {
            gameCanvas = new GameObject("GameCanvas").AddComponent<Canvas>();
            gameCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = gameCanvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(800, 600);
            gameCanvas.gameObject.AddComponent<GraphicRaycaster>();
            gameCanvas.transform.SetParent(transform);

            GameObject mainPanel = new GameObject("MainPanel");
            mainPanel.transform.SetParent(gameCanvas.transform);
            RectTransform mainRect = mainPanel.AddComponent<RectTransform>();
            mainRect.anchorMin = new Vector2(0, 0);
            mainRect.anchorMax = new Vector2(1, 1);
            mainRect.pivot = new Vector2(0.5f, 0.5f);
            mainRect.sizeDelta = new Vector2(0, 0);
            mainRect.anchoredPosition = Vector2.zero;

            Image mainImage = mainPanel.AddComponent<Image>();
            mainImage.color = new Color(0.1f, 0.1f, 0.1f);

            GameObject statusTextObject = new GameObject("StatusText");
            statusTextObject.transform.SetParent(mainPanel.transform);
            RectTransform statusRect = statusTextObject.AddComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0, 1);
            statusRect.anchorMax = new Vector2(1, 1);
            statusRect.pivot = new Vector2(0.5f, 1);
            statusRect.sizeDelta = new Vector2(0, 50);
            statusRect.anchoredPosition = new Vector2(0, -25);

            statusText = statusTextObject.AddComponent<Text>();
            statusText.alignment = TextAnchor.MiddleCenter;
            statusText.fontSize = 24;
            statusText.color = Color.white;
            defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            statusText.font = defaultFont;
            statusText.text = ConstString.PlayerTurnText;

            boardContainer = new GameObject("BoardContainer");
            boardContainer.transform.SetParent(mainPanel.transform);
            RectTransform boardRect = boardContainer.AddComponent<RectTransform>();
            boardRect.anchorMin = new Vector2(0.5f, 0.5f);
            boardRect.anchorMax = new Vector2(0.5f, 0.5f);
            boardRect.pivot = new Vector2(0.5f, 0.5f);
            boardRect.sizeDelta = new Vector2(0, 0);
            boardRect.anchoredPosition = Vector2.zero;

            GridLayoutGroup grid = boardContainer.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(CELL_SIZE, CELL_SIZE);
            grid.spacing = new Vector2(CELL_SPACING, CELL_SPACING);
            grid.childAlignment = TextAnchor.MiddleCenter;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = BOARD_SIZE;

            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    CreateCell(row, col);
                }
            }

            GameObject restartButtonObject = new GameObject("RestartButton");
            restartButtonObject.transform.SetParent(mainPanel.transform);
            RectTransform buttonRect = restartButtonObject.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0);
            buttonRect.anchorMax = new Vector2(0.5f, 0);
            buttonRect.pivot = new Vector2(0.5f, 0);
            buttonRect.sizeDelta = new Vector2(150, 50);
            buttonRect.anchoredPosition = new Vector2(0, 25);

            restartButton = restartButtonObject.AddComponent<Button>();

            Image buttonImage = restartButtonObject.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.2f, 0.8f);

            GameObject buttonTextObject = new GameObject("ButtonText");
            buttonTextObject.transform.SetParent(restartButtonObject.transform);

            RectTransform textRect = buttonTextObject.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 0);
            textRect.anchorMax = new Vector2(1, 1);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = new Vector2(0, 0);
            textRect.anchoredPosition = Vector2.zero;

            Text buttonText = buttonTextObject.AddComponent<Text>();
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.fontSize = 20;
            buttonText.color = Color.white;
            buttonText.font = defaultFont;
            buttonText.text = ConstString.RestartButtonText;

            restartButton.onClick.AddListener(ResetGame);
        }

        private void CreateCell(int row, int col)
        {
            GameObject cellObject = new GameObject($"Cell_{row}_{col}");
            cellObject.transform.SetParent(boardContainer.transform);

            RectTransform cellRect = cellObject.AddComponent<RectTransform>();
            cellRect.sizeDelta = new Vector2(0, 0);

            Image cellImage = cellObject.AddComponent<Image>();
            cellImage.color = cellBackgroundColor;

            Button cellButton = cellObject.AddComponent<Button>();
            cellButton.transition = Selectable.Transition.ColorTint;
            cellButton.colors = new ColorBlock
            {
                normalColor = cellBackgroundColor,
                highlightedColor = cellHighlightColor,
                pressedColor = cellHighlightColor,
                selectedColor = cellBackgroundColor,
                disabledColor = cellBackgroundColor,
                colorMultiplier = 1f,
                fadeDuration = 0.1f
            };

            GameObject textObject = new GameObject("CellText");
            textObject.transform.SetParent(cellObject.transform);

            RectTransform textRect = textObject.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 0);
            textRect.anchorMax = new Vector2(1, 1);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = new Vector2(0, 0);
            textRect.anchoredPosition = Vector2.zero;

            Text cellText = textObject.AddComponent<Text>();
            cellText.alignment = TextAnchor.MiddleCenter;
            cellText.fontSize = 48;
            cellText.font = defaultFont;
            cellText.enabled = false;

            Cell cell = cellObject.AddComponent<Cell>();
            cell.Initialize(row, col, cellText, this);

            cellButton.onClick.AddListener(cell.OnClick);

            int index = row * BOARD_SIZE + col;
            cells[index] = cell;
        }
    }
}