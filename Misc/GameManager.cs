using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int _WALLCOUNTINHEIGHT = 6;
    private const int _WALLCOUNTINWIDTH = 6;

    private static int _bestScoreValue = 0;
    private static float _currentScoreValue = 0;

    private Pool _pool;
    private LevelConstructor _levelConstructor;
    private GameObject _deadLine;

    private GameObject _poolPrefab;
    private GameObject _background;

    private GameObject _mainMenu;
    private GameObject _showMenuButton;
    private Text _bestScore;
    private Text _currentScore;
    private GameObject _resumeButton;

    private Player _player;

    private void Awake()
    {
        _poolPrefab = (GameObject)Resources.Load("Prefabs/Wall");

        _deadLine = GameObject.FindGameObjectWithTag("DeadLine");
        _background = GameObject.FindGameObjectWithTag("Background");

        _pool = Pool.GetInstance();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 0;
        _mainMenu.SetActive(true);
        _showMenuButton.SetActive(false);

    }

    public void Resume()
    {
        _mainMenu.SetActive(false);
        _showMenuButton.SetActive(true);
        Time.timeScale = 1;
    }



    private void Start()
    {
        Time.timeScale = 1;
        _currentScoreValue = 0;

        _pool.ClearPool();
        for (int i = 0; i < _WALLCOUNTINHEIGHT * (_WALLCOUNTINWIDTH + 5); i++)
        {
            GameObject newPoolObject = Instantiate(_poolPrefab);
            if (newPoolObject != null)
                _pool.AddToPool(newPoolObject);
        }

        _levelConstructor = new LevelConstructor(_WALLCOUNTINHEIGHT, _WALLCOUNTINWIDTH);
        _levelConstructor.GenerateStartLevel();

        Vector3 scaleVector = _levelConstructor.GetScaleVector();
        Vector3 _cameraTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10f));
        Vector3 _cameraBottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10f));

        _deadLine.transform.localScale = new Vector3(scaleVector.x * _WALLCOUNTINWIDTH, 0.1f, 1f);
        _deadLine.transform.position = new Vector3(_cameraTopLeft.x + (scaleVector.x * _WALLCOUNTINWIDTH / 2), _cameraBottomRight.y - scaleVector.y * 2, 10f);
        _deadLine.transform.Rotate(new Vector3(0,0,10), Space.World);

        _background.transform.localScale = new Vector3((_cameraBottomRight.x - _cameraTopLeft.x) * 1.5f, (_cameraTopLeft.y - _cameraBottomRight.y) * 1.5f, 0.1f);
        _background.transform.position = new Vector3(_cameraTopLeft.x + scaleVector.x * (_WALLCOUNTINWIDTH / 2), _cameraBottomRight.y + scaleVector.y * (_WALLCOUNTINHEIGHT / 2), 11f);

        var go = GameObject.Find("Player");
        _player = go.GetComponent<Player>();

        _mainMenu = GameObject.Find("MainMenu");
        _showMenuButton = GameObject.Find("MenuButton");
        _bestScore = GameObject.Find("BestScoreValue").GetComponent<Text>();
        _bestScore.text = _bestScoreValue.ToString();
        _currentScore = GameObject.Find("ScoreValue").GetComponent<Text>();
        _resumeButton = GameObject.Find("ResumeButton");
        _mainMenu.SetActive(false);
    }

    public void CreateNewLine()
    {
        _levelConstructor.GenerateLine();
    }

    public void Death(GameObject collidedWall)
    {
        Time.timeScale = 0;

        if (_currentScoreValue > _bestScoreValue)
        {
            _bestScoreValue = Convert.ToInt32(_currentScoreValue);
            _bestScore.text = _bestScoreValue.ToString();
        }
            

        StartCoroutine(OnGameOver(collidedWall));
    }

    private IEnumerator OnGameOver(GameObject gm)
    {
        if (_player)
            _player.RestrictMoving();

        if (gm)
        {
            Animator wallAnimator = gm.GetComponent<Animator>();
            wallAnimator.enabled = true;
            wallAnimator.Play("WallDeathAnim");

            while (wallAnimator.GetCurrentAnimatorStateInfo(0).IsName("WallDeathAnim") &&
                    wallAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
        else
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(5f);
                break;
            }
        }

        ShowMainMenu();
        _resumeButton.SetActive(false);
    }

    private void FixedUpdate()
    {
        _currentScoreValue += 0.2f;

        _currentScore.text = Convert.ToInt32(_currentScoreValue).ToString();
    }
}
