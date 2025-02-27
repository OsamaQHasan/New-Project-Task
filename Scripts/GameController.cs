using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Queue<Card> selectedCards;
    private BoardGenerator boardGenerator;
    [SerializeField] AudioClip gameMusic, menuMusic, cardFlip, successMatch, failMatch, gameOver, levelUp;
    [SerializeField] AudioSource musicAudioSource, SFXAudioSource;
    [SerializeField] int x, y, finalLevel;
    [SerializeField] float showCardFor;
    [SerializeField] GameObject pausePanel;
    int combo = 0,score = 0, level = 1;
    public int cards = 0;
    [SerializeField] TMP_Text scoreText, comboText, levelText, winText;
    [SerializeField] int scorePerMatch;
    [SerializeField] Button continueButton;
    bool gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        winText.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("level") && PlayerPrefs.GetInt("level", 1) > 1)
        {
            UpdateScore(PlayerPrefs.GetInt("score", 0));
            UpdateCombo(PlayerPrefs.GetInt("combo", 0));
            UpdateLevel(PlayerPrefs.GetInt("level", 1));
        }
        else
        {
            continueButton.interactable = false;
        }
        selectedCards = new Queue<Card>();
        boardGenerator = GetComponent<BoardGenerator>();
    }
    void ChangeMusic(AudioClip clip)
    {
        musicAudioSource.loop = true;
        musicAudioSource.clip = clip;
        musicAudioSource.Play();

    }
    void PlaySFX(AudioClip clip)
    {
        SFXAudioSource.PlayOneShot(clip);
    }
    public void StartGame()
    {
        winText.gameObject.SetActive(false);
        continueButton.interactable = true;
        Time.timeScale = 1;
        gameStarted = true;
        UpdateScore(0);
        UpdateCombo(0);
        UpdateLevel(1);
        SaveGame();
       
        pausePanel.SetActive(false);
        StartCoroutine(SetUpBoardCoroutine());
        ChangeMusic(gameMusic);
        
    }
    public void ContinueGame()
    {
        continueButton.interactable = true;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        if (!gameStarted)
        {
            gameStarted = true;
            StartCoroutine(SetUpBoardCoroutine());
        }
        ChangeMusic(gameMusic);

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        ChangeMusic(menuMusic);
    }
    // Update is called once per frame
    void Update()
    {
        if(selectedCards.Count > 1) { 
            Card selectedCard1 = selectedCards.Dequeue();
            Card selectedCard2 = selectedCards.Dequeue();

            if (selectedCard1.cardValueString != selectedCard2.cardValueString)
            {
                selectedCard1.StartFlipCard(showCardFor);
                selectedCard2.StartFlipCard(showCardFor);
                PlaySFX(failMatch);
                UpdateCombo(0);
            }
            else
            {
                selectedCard1.CardMatched(showCardFor);
                selectedCard2.CardMatched(showCardFor);
                UpdateScore(score + (combo == 0 ? 1 : combo) * scorePerMatch);
                UpdateCombo(combo + 1);
                cards -= 2;
                if(cards <= 0)
                {
                    if (level >= finalLevel)
                    {
                        winText.text = "CONGRATULATIONS! YOU BEAT THE GAME YOUR SCORE IS " + score + "<br>CAN YOU GET MORE?";
                        PlaySFX(gameOver);
                        winText.gameObject.SetActive(true);
                        continueButton.interactable = false;
                        pausePanel.SetActive(true);
                        return;
                    }
                    PlaySFX(levelUp);
                    UpdateLevel(level + 1);
                    StartCoroutine(SetUpBoardCoroutine(showCardFor));
                    return;
                }
                PlaySFX(successMatch);

            }
        }
    }
    void SaveGame()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("combo", combo);
    }
    IEnumerator SetUpBoardCoroutine(float wait = 0)
    {
        yield return new WaitForSeconds(wait);
        int x = this.x;
        int y = this.y;
        for (int i = 0; i < level - 1; i++)
        {
            do
            {
                if(x <= y)
                {
                    x++;
                }
                else
                {
                    y++;
                }
            } while ((x * y) % 2 != 0);
        }
        cards = x * y;
        boardGenerator.SetUpBoard(x, y);
    }
    private void UpdateScore(int score)
    {
        this.score = score;
        scoreText.text = "SCORE: " + score;

    }
    private void UpdateLevel(int level)
    {
        
        this.level = level;
        SaveGame();
        levelText.text = "LEVEL: " + level;

    }
    private void UpdateCombo(int combo)
    {
        this.combo = combo;
        comboText.text = "COMBO: " + combo;
    }
    public void SelectCard(Card card)
    {
        if (!selectedCards.Contains(card))
        {
            selectedCards.Enqueue(card);
            card.StartFlipCard();
            PlaySFX(cardFlip);
        }
            
    }
}
