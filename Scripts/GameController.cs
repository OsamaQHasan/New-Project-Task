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
    [SerializeField] TMP_Text scoreText, comboText, levelText, winText, highscoreText;
    [SerializeField] int scorePerMatch;
    [SerializeField] Button continueButton;
    bool gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        winText.gameObject.SetActive(false);//hide the win text
        UpdateHighscore(PlayerPrefs.GetInt("highscore", 0));//update level to saved score
        if (PlayerPrefs.HasKey("level") && PlayerPrefs.GetInt("level", 1) > 1)//if there is a saved level above 1
        {
            UpdateScore(PlayerPrefs.GetInt("score", 0));//update score to saved score
            UpdateCombo(PlayerPrefs.GetInt("combo", 0));//update combo to saved score
            UpdateLevel(PlayerPrefs.GetInt("level", 1));//update level to saved score
            
        }
        else
        {
            continueButton.interactable = false;
        }
        selectedCards = new Queue<Card>();
        boardGenerator = GetComponent<BoardGenerator>();
    }
    public void ResetHighscore()
    {
        PlayerPrefs.SetInt("highscore", 0);//set high score to 0
        UpdateHighscore(0);
    }
    void ChangeMusic(AudioClip clip)
    {
        musicAudioSource.loop = true; //set music loop to true
        musicAudioSource.clip = clip; //set the clip to the new clip
        musicAudioSource.Play(); //play music

    }
    void PlaySFX(AudioClip clip)
    {
        SFXAudioSource.PlayOneShot(clip); //play one shot of the SFX clip
    }
    public void StartGame()
    {
        winText.gameObject.SetActive(false);//hide win text (in case this is a replay)
        continueButton.interactable = true;//make the contiue button enabled (for pause menu)
        Time.timeScale = 1; //set time scale to 1 (in case it was paused)
        gameStarted = true; //set gameStarted to 1
        //reset all variables
        UpdateScore(0); 
        UpdateCombo(0);
        UpdateLevel(1);
        SaveGame();
       //deactivate the pause menu
        pausePanel.SetActive(false);

        //set up the board
        StartCoroutine(SetUpBoardCoroutine());

        //put the game music on
        ChangeMusic(gameMusic);
        
    }
    public void ContinueGame()
    {
        
        continueButton.interactable = true;
        Time.timeScale = 1;//set timescale to 1 (in case of pause
        pausePanel.SetActive(false);//deactivate pause menu
        if (!gameStarted)//if the game is not started yet
        {
            gameStarted = true;
            StartCoroutine(SetUpBoardCoroutine());//start the game
        }
        ChangeMusic(gameMusic);//set music to game music

    }
    public void ExitGame()
    {
        Application.Quit();//quit
    }
    public void PauseGame()
    {
        Time.timeScale = 0;//set timescale to 0
        pausePanel.SetActive(true);//activate the pause panel
        ChangeMusic(menuMusic);//set music to menu music
    }
    // Update is called once per frame
    void Update()
    {
        if(selectedCards.Count > 1) { //as long as there is more than 1 card in the queue
            //deque the first 2 cards
            Card selectedCard1 = selectedCards.Dequeue();
            Card selectedCard2 = selectedCards.Dequeue();

            if (selectedCard1.cardValueString != selectedCard2.cardValueString)//if they have the same value
            {
                //flip cards back
                selectedCard1.StartFlipCard(showCardFor);
                selectedCard2.StartFlipCard(showCardFor);

                //play failed SFX
                PlaySFX(failMatch);

                //set combo to 0
                UpdateCombo(0);
            }
            else
            {
                //play match SFX
                PlaySFX(successMatch);
                //hide the cards
                selectedCard1.CardMatched(showCardFor);
                selectedCard2.CardMatched(showCardFor);

                //update the score
                UpdateScore(score + (combo == 0 ? 1 : combo) * scorePerMatch);//if combo was 0 it becomes 1, otherwise multiply it with the scorePerMatch

                UpdateCombo(combo + 1);//add 1 to the combo

                //remove 2 from the card count
                cards -= 2;
                if(cards <= 0)//if no more cards remain
                {
                    if (level >= finalLevel)//if this is the final level
                    {
                        winText.text = "CONGRATULATIONS! YOU BEAT THE GAME YOUR SCORE IS " + score + "<br>CAN YOU GET MORE?";//set the win text showing the score
                        PlaySFX(gameOver);//play game over sound
                        winText.gameObject.SetActive(true);//show win text
                        continueButton.interactable = false;//disable continue button
                        pausePanel.SetActive(true);//show pause panel

                    }else {

                        PlaySFX(levelUp);//play level up SFX
                        UpdateLevel(level + 1); //add 1 to the level
                        StartCoroutine(SetUpBoardCoroutine(showCardFor));//set the board
                        
                    }
                    
                }
                

            }
        }
    }
    void SaveGame()
    {
        //save variables to player prefs
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("combo", combo);
    }
    IEnumerator SetUpBoardCoroutine(float wait = 0)
    {
        //wait if required
        yield return new WaitForSeconds(wait);

        //set x and y to original x and y
        int x = this.x;
        int y = this.y;

        //loop until you reach current level
        for (int i = 0; i < level - 1; i++)
        {
            do
            {//either add 1 to x or y, depending on which is larger
                if(x <= y)
                {
                    x++;
                }
                else
                {
                    y++;
                }
            } while ((x * y) % 2 != 0);//keep doing so until the number of cards is even
        }
        cards = x * y;//number of cards is x * y
        boardGenerator.SetUpBoard(x, y);//set up the board with the new x and y
    }
    private void UpdateScore(int score)
    {
        this.score = score;
        scoreText.text = "SCORE: " + score;
        UpdateHighscore(score);

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
    private void UpdateHighscore(int highscore)
    {
        if (highscore >= PlayerPrefs.GetInt("highscore", 0))
        {
            PlayerPrefs.SetInt("highscore", highscore);
            highscoreText.text = "HIGHSCORE: " + highscore;
        }
        

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
