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
    private static readonly string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "uSkizzik's Tetris");
    private static readonly string highscorePath = Path.Combine(dataPath, "highscore.txt");
    
    private int _highScore;
    private int _score;
    
    private int _level = 1;
    private int _levelProgress;
    
    private int _comboProgress = -1;
    private bool _b2bLineClear;
    private bool _b2bPerfectClear;

    private readonly TetrisGame _game;

    public ScoreTracker(TetrisGame game)
    {
        _game = game;

        if (!Directory.Exists(dataPath))
            Directory.CreateDirectory(dataPath);
        
        LoadHighScore();
    }

    public int HighScore
    {
        get => _highScore;
    }

    public int Score
    {
        get => _score;
    }
    
    public int Level
    {
        get => _level;
        set => _level = value;
    }
    
    public int LevelProgress
    {
        get => _levelProgress;
    }
    
    public int LineClearsRequired
    {
        get => 10;
    }

    public void Reset()
    { 
        _score = 0;
            
        _level = 1;
        _levelProgress = 0;

        _comboProgress = -1;
        _b2bLineClear = false;
        _b2bPerfectClear = false;
    }

    public void TetronimoLocked(int clearedLines, bool[,] matrix)
    {
        int toAdd = 0;
        
        if (clearedLines > 0)
        {
            _comboProgress++;
            toAdd += 50 * _comboProgress * _level;
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

        _score += points * _level;
        
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
        
        AddLevelProgress(linesCleared);
    }

    private void AddLevelProgress(int linesCleared)
    {
        _levelProgress = Math.Clamp(_levelProgress += linesCleared, 0, LineClearsRequired);

        if (_levelProgress >= LineClearsRequired)
        {
            _level++;
            _levelProgress = 0;
        }

        int maxLevel = 20;
        int baseGravity = 1200;
        double exponent = Math.Pow((double)1 / baseGravity, 1.0 / (maxLevel - 1));
        
        _game.SetMoveTime((int) (baseGravity * Math.Pow(exponent, Math.Clamp(_level, 1, maxLevel) - 1)));

        int baseLockTime = 500;
        int minLockTime = 75;
        
        _game.LockTime = baseLockTime - (_level - 1) * ((baseLockTime - minLockTime) / maxLevel - 1);
    }

    private void LoadHighScore()
    {
        if (!File.Exists(highscorePath))
            WriteScore();
        
        StreamReader stream = new StreamReader(highscorePath);
        _highScore = int.Parse(stream.ReadToEnd());
        stream.Close();
    }
    
    public void WriteScore()
    {
        StreamWriter stream = new StreamWriter(highscorePath, false);
        stream.Write(_highScore);
        stream.Close();
    }
    
    public void SaveScore()
    {
        if (_score <= _highScore) return;
        _highScore = _score;
        
        WriteScore();
    }
}