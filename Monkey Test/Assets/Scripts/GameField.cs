using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameField : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _gameFieldPanel;
    [SerializeField] private GameObject _tileDefaultPrefab;
    [SerializeField] private InfoPanel _infoPanel;
    [SerializeField] private Vector2 _leftTopTileMargin;
    [SerializeField] private int _fieldTilesColumnCount;
    [SerializeField] private int _fieldTilesRowCount;

    private RectTransform _gameFieldRect;
    private RectTransform _tileDefaultRect;

    [SerializeField] private ObjectPool _scoreIndicatorsPool;

    [Header("Timers and delays")]
    [SerializeField] private int _minNumberOfActiveTiles;
    private int _NumberOfActiveTiles;

    [Tooltip("delay between tile's fragments show-hide/close animations")]
    [SerializeField] private float _smoothness;
    [SerializeField] private float _delayToFillTiles;
    [SerializeField] private float _timeToRemeberOneTile;
    private float _timeToRememberOrder;
    [SerializeField] private float _delayBetweenOrderShowcase;
    [SerializeField] private float _timeAfterOrderShowcase;

    [SerializeField] private float _resetDelay;

    private Tile[] _activeTiles;
    private int _currentOrder;

    private bool _isFirstRun = true;
    private bool _isPaused;
    private bool _isTimerActive;
    private float _spawnTimer;
    [SerializeField] private float _spawnTimerTemp = 0.1f;
    [SerializeField] private List<Tile> _tileList;

    public event System.Action OnEndGameFieldClear;

    void Start()
    {
        _gameFieldRect = _gameFieldPanel.GetComponent<RectTransform>();
        _tileDefaultRect = _tileDefaultPrefab.GetComponent<RectTransform>();

        GridLayoutGroup gameFieldGrid = _gameFieldPanel.GetComponent<GridLayoutGroup>();
        gameFieldGrid.cellSize = _tileDefaultRect.sizeDelta;
        gameFieldGrid.spacing = _leftTopTileMargin;


        _fieldTilesRowCount = Mathf.FloorToInt(_gameFieldRect.rect.height / (_tileDefaultRect.rect.height + _leftTopTileMargin.y));
        _fieldTilesColumnCount = Mathf.FloorToInt(_gameFieldRect.rect.width / (_tileDefaultRect.rect.width + _leftTopTileMargin.x));

        _NumberOfActiveTiles = _minNumberOfActiveTiles;
        _spawnTimer = _spawnTimerTemp;

        _infoPanel.timer.OnTimerStateChange += ChangeTimerState;

        SpawnTiles();

        StartCoroutine(StartTimer(0.6f)); // starts the game with timer
    }

    private IEnumerator StartTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        _infoPanel.timer.StartCountdown();
    }

    private IEnumerator FillTiles(float delayToFillTiles)
    {
        yield return new WaitForSeconds(delayToFillTiles);

        Tile[] activeTiles = new Tile[_NumberOfActiveTiles];
        List<Tile> tileTemp = new List<Tile>();
        tileTemp.AddRange(_tileList);

        for (int i = 0; i < _NumberOfActiveTiles; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, tileTemp.Count);
            activeTiles[i] = tileTemp[randomIndex];
            tileTemp.RemoveAt(randomIndex);
        }
        
        _activeTiles = activeTiles;

        for (int i = 0; i < _NumberOfActiveTiles; i++)
        {
            yield return new WaitForSeconds(_smoothness);
            _activeTiles[i].SetOrder(i);
        }

        _timeToRememberOrder = _timeToRemeberOneTile * _NumberOfActiveTiles;
        StartCoroutine(CloseActiveTiles(_timeToRememberOrder));
    }

    public void CheckOrder(int order)
    {
        if (_activeTiles[_currentOrder].order == order)
        {
            _currentOrder++;
            if (_currentOrder == _NumberOfActiveTiles)
            {
                StartCoroutine(ActiveTilesShowcase(_delayBetweenOrderShowcase));
                _NumberOfActiveTiles++;
            }
        }
        else
        {
            StartCoroutine(ActiveTilesShowcase(_delayBetweenOrderShowcase));
            if (_NumberOfActiveTiles > _minNumberOfActiveTiles)
                _NumberOfActiveTiles--;
        }
    }

    private IEnumerator ActiveTilesShowcase(float delayBetweenOrderShowcase)
    {
        int CorrectTiles = 0;
        foreach (Tile tile in _activeTiles)
            tile.Open();

        for (int i = 0; i < _activeTiles.Length; i++)
        {
            yield return new WaitForSeconds(delayBetweenOrderShowcase);
            if (i >= _currentOrder)
            {
                _activeTiles[i].AnimateShake(-1);
            }
            else
            {
                _activeTiles[i].AnimateShake(1);
                int score = Mathf.RoundToInt(Mathf.Pow(2, _activeTiles[i].order + 1));
                StartCoroutine(AddScore(score));
                _activeTiles[i].ShowScoreIndicator(score);
                CorrectTiles++;
            }
        }
        ChangeDifficulty(_NumberOfActiveTiles);

        StartCoroutine(ResetActiveTiles(_timeAfterOrderShowcase));
        _currentOrder = 0;
    }

    private IEnumerator AddScore(int score)
    {
        yield return new WaitForSeconds(.1f); // shake animation duration
        _infoPanel.score.AddScore(score);
    }

    private IEnumerator CloseActiveTiles(float timer)
    {
        yield return new WaitForSeconds(timer);
        foreach (Tile tile in _activeTiles)
        {
            yield return new WaitForSeconds(_smoothness);
            tile.Close();
        }
    }

    private IEnumerator ResetActiveTiles(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (Tile tile in _activeTiles)
        {
            yield return new WaitForSeconds(_smoothness);
            tile.AnimateTextDisappear();
            tile.ClearListeners();
        }

        if (_isTimerActive)
            StartCoroutine(FillTiles(_delayToFillTiles));
        else
            OnEndGameFieldClear();
    }

    private void ClearField()
    {
        StopAllCoroutines();
        foreach (Tile tile in _tileList)
        {
            tile.Reset();
        }
    }
   
    private void SpawnTiles()
    {
        for (int h = 0; h < _fieldTilesColumnCount; h++)
        {
            for (int w = 0; w < _fieldTilesRowCount; w++)
            {
                Tile tile = Instantiate(_tileDefaultPrefab, _gameFieldRect.transform).GetComponent<Tile>();
                tile.Inject(this , _scoreIndicatorsPool);
                tile.Render(false);
                _tileList.Add(tile);
            }
        }

        StartCoroutine(StartTileShowAnimation());
    }

    private void HideTiles()
    {
        StartCoroutine(StartTileHideAnimation());
    }

    private IEnumerator StartTileShowAnimation()
    {
        for (int h = 0; h < _tileList.Count; h++)
        {
            yield return new WaitForSeconds(_spawnTimer);
            _spawnTimer *= 0.5f;
            _tileList[h].AnimateSpawn();
        }
        _spawnTimer = _spawnTimerTemp;
    }

    private IEnumerator StartTileHideAnimation()
    {
        _spawnTimer *= _spawnTimer;
        for (int h = _tileList.Count - 1; h >= 0; h--)
        {
            yield return new WaitForSeconds(_spawnTimer);
            _spawnTimer *= 1.05f;
            _tileList[h].AnimateHide();
        }
        _spawnTimer = _spawnTimerTemp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            PauseGame(true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PauseGame(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FillTiles(_delayToFillTiles));
        }
    }

    private void ChangeTimerState(bool state)
    {
        _isTimerActive = state;
        if(_isFirstRun)
            PlayGame(state);
    }

    private void PlayGame(bool state)
    {
        if (state)
        {
            StartCoroutine(FillTiles(_delayToFillTiles));
            ResetProgress();
            ResetDifficulty();
            _isFirstRun = false;
        }
        else
        {
            Debug.LogWarning("Trying to run game while already playing");
        }
    }

    public void PauseGame(bool state)
    {
        if (state)
        {
            _isPaused = true;
            ClearField();
            ResetOrder();
        }
        else
        {
            _isPaused = false;
            StartCoroutine(ResetActiveTiles(0));
        }
    }

    public void Reset()
    {
        ResetProgress();
        ResetDifficulty();
    }

    public void Restart()
    {
        ResetProgress();
        ResetDifficulty();

        PlayGame(true);
    }

    private void ResetProgress()
    {
        _NumberOfActiveTiles = _minNumberOfActiveTiles;
        _infoPanel.difficultyLevel.SetDifficulty(_NumberOfActiveTiles);
    }


    private void ResetOrder()
    {
        _currentOrder = 0;
    }

    private void ResetDifficulty()
    {
        _infoPanel.score.ResetScore();
    }

    private void ChangeDifficulty(int difficulty)
    {
        _infoPanel.difficultyLevel.ChangeDifficulty(difficulty);
    }

    private void OnDestroy()
    {
        _infoPanel.timer.OnTimerStateChange -= ChangeTimerState;
    }
}
