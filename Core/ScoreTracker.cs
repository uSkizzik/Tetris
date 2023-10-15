using Tetris.Tetrominos;

namespace Tetris.Core;

enum EAction
{
    LINE_CLEAR,
    TETRIS_LINE_CLEAR,
    PERFECT_CLEAR,
    TETRIS_PERFECT_CLEAR
}

public class ScoreTracker
{
    private int _score;
    private int _comboProgress = -1;
    private bool _b2bLineClear;
    private bool _b2bPerfectClear;

    public int Score
    {
        get => _score;
    }

    public void TetronimoLocked(int clearedLines, bool[,] matrix)
    {
        int toAdd = 0;
        
        if (clearedLines > 0)
        {
            _comboProgress++;
            toAdd += 50 * _comboProgress; // * level;
        }
        else _comboProgress = -1;
        
        AddLineClearPoints(toAdd, clearedLines, matrix);
    }

    public void TetronimoSoftDropped()
    {
        _score += 1;
    }

    public void TetronimoHardDropped(int startingY, int endY)
    {
        _score += (endY - startingY) * 2;
    }

    // public void TSpin(bool[,] matrix, Tetromino tetromino)
    // {
    //     Maybe some day
    // }

    private void AddScore(int points, EAction actionType)
    {
        if (actionType == EAction.TETRIS_LINE_CLEAR && _b2bLineClear)
            points = (int) Math.Round(points * 1.5);
        
        if (actionType == EAction.TETRIS_PERFECT_CLEAR && _b2bPerfectClear)
            points = 3200;
        
        _score += points; // * level
        
        if (actionType == EAction.LINE_CLEAR)
            _b2bLineClear = false;
        else if (actionType == EAction.TETRIS_LINE_CLEAR)
            _b2bLineClear = true;
        
        if (actionType == EAction.TETRIS_PERFECT_CLEAR)
            _b2bPerfectClear = true;
        else
            _b2bPerfectClear = false;
    }

    private bool IsMatrixClear(bool[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void AddLineClearPoints(int score, int linesCleared, bool[,] matrix)
    {
        bool isMatrixClear = IsMatrixClear(matrix);
        
        switch (linesCleared)
        {
            case 1:
                AddScore(score + 100, EAction.LINE_CLEAR);
                if (isMatrixClear) AddScore(800, EAction.PERFECT_CLEAR);
                
                break;
            
            case 2:
                AddScore(score + 300, EAction.LINE_CLEAR);
                if (isMatrixClear) AddScore(1200, EAction.PERFECT_CLEAR);
                
                break;
            
            case 3:
                AddScore(score + 500, EAction.LINE_CLEAR);
                if (isMatrixClear) AddScore(1800, EAction.PERFECT_CLEAR);
                
                break;
            
            case 4:
                AddScore(score + 800, EAction.TETRIS_LINE_CLEAR);
                if (isMatrixClear) AddScore(2000, EAction.TETRIS_PERFECT_CLEAR);
                
                break;
        }
    }
}