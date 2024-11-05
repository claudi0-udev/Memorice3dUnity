using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Sprite> itemSprites;//lista de todos los sprites en directorio
    public Vector2 gameSize;//2x2, 2x4, 4x4, etc.....

    [SerializeField] private GameObject itemContainer; //caja contenedora de items

    [SerializeField] private Transform itemPositionObjectContainer;
    [SerializeField] private List<Vector2> itemPositionList;

    [SerializeField] private List<GameObject> itemsInGame;

    [SerializeField] private List<Sprite> selectedSprites;

    //[SerializeField] private int touchesCount = 0;//contador de toques

    [SerializeField] private LayerMask touchLayerMask;

    [SerializeField] private Item firstItemSelected, secondItemSelected;

    [SerializeField] private int maxMatches;//maximo de coincidencias para terminar el juego
    [SerializeField] private int matchesCount;//contador de matches del jugador

    [SerializeField] private SoundController soundController;

    public ParticleSystem onTouchParticleSystem;

    public GameObject correctMatchParticleSystem;

    public float gameTime, previousGameTime;
    private bool startTimer;
    public Text gameTimeText;

    private int attemps;

    [SerializeField] private GameObject gameEndMenu;

    private bool isGamePaused = false;


    void Start()
    {
        isGamePaused = false;
        Application.targetFrameRate = 60;
        
        gameSize.x = GameSettings.gameDifficulty_static;
        maxMatches = (int)(gameSize.x * gameSize.y) / 2;

        startTimer = true;

        LoadItemSpritesToList();
        PositioningItems();
    }

    void Update()
    {
        if(isGamePaused) return;
        TouchPhases();
        GameTimer();
    }

    void LoadItemSpritesToList()
    {
        Object[] sprites;
        sprites =  Resources.LoadAll("food", typeof(Sprite));
        
        foreach (var t in sprites)
        {
            //Debug.Log(t.name);
            itemSprites.Add((Sprite)t);
        }
        Debug.Log("Items Cargados");

    }

    void SetItemPositionList()
    {
        int childs = itemPositionObjectContainer.childCount;
        for(int i = 0; i < childs; i++ )
        {
            itemPositionList.Add(itemPositionObjectContainer.GetChild(i).transform.position);
        }
    }

    void PositioningItems()
    {
        SetItemPositionList();
        SelectDifferentSprites();

        int itemsToSet = (int)(gameSize.x * gameSize.y);

        for(int i = 0; i < itemsToSet/2; i++)
        {
            itemsInGame.Add(Instantiate(itemContainer));

            itemsInGame[i].GetComponent<Item>().itemValue = i;
            itemsInGame[i].GetComponent<Item>().spriteImage = selectedSprites[i];
            itemsInGame[i].GetComponent<Item>().itemName = itemsInGame[i].gameObject.name = selectedSprites[i].name;

        }

        for(int i = 0; i < itemsToSet/2; i++)
        {
            GameObject clone = Instantiate(itemsInGame[i]);
            clone.name = itemsInGame[i].name + "_2";
            itemsInGame.Add(clone);
        }

        SortItemsInGame();
    }

    void SelectDifferentSprites()
    {
        int spritesToSet = (int)(gameSize.x * gameSize.y)/2;
        while(selectedSprites.Count < spritesToSet)
        {
            Sprite sprite = itemSprites[Random.Range(0, itemSprites.Count)];
            bool spriteCanBeSet = true;
            foreach(Sprite s in selectedSprites)
            {
                if(sprite == s)
                    spriteCanBeSet = false;
            }

            if(spriteCanBeSet)
                selectedSprites.Add(sprite);

        }
    }

    void SortItemsInGame()
    {
        for (int i = 0; i < itemsInGame.Count; i++)
        {
            GameObject temp = itemsInGame[i];
            int randomIndex = Random.Range(i, itemsInGame.Count);
            itemsInGame[i] = itemsInGame[randomIndex];
            itemsInGame[randomIndex] = temp;

            //posicionar items en la escena
            itemsInGame[i].transform.position = itemPositionList[i];
        }
    }

    void TouchPhases()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if(touch.phase == TouchPhase.Began)
            {
                RaycastHit hit3D;
                if(Physics.Raycast(Camera.main.ScreenToWorldPoint((Vector3)touch.position), Vector3.forward, out hit3D, 20.0f, touchLayerMask))
                {
                    if (hit3D.transform.CompareTag("Item"))
                    {
                        //Debug.Log("Item : " + hit3D.transform.gameObject.name);
                        if(hit3D.transform.GetComponent<Item>().canBeSelected)
                        {
                            if(firstItemSelected == null)
                            {
                                firstItemSelected = hit3D.transform.GetComponent<Item>();
                                firstItemSelected.canBeSelected = false;
                                firstItemSelected.box.SetActive(false);
                                firstItemSelected.questionMark.SetActive(false);
                                firstItemSelected.itemFrame.SetActive(true);
                                firstItemSelected.spriteContainer.gameObject.SetActive(true);

                                onTouchParticleSystem.transform.position = hit3D.transform.position + new Vector3(0, 0, -5);
                                onTouchParticleSystem.Play();//VFX
                                soundController.ClickSoundPlay(); //Audio FX

                            }                                
                            else
                            {
                                secondItemSelected = hit3D.transform.GetComponent<Item>();
                                secondItemSelected.canBeSelected = false;
                                secondItemSelected.box.SetActive(false);
                                secondItemSelected.questionMark.SetActive(false);
                                secondItemSelected.itemFrame.SetActive(true);
                                secondItemSelected.spriteContainer.gameObject.SetActive(true);

                                onTouchParticleSystem.transform.position = hit3D.transform.position + new Vector3(0, 0, -5);
                                onTouchParticleSystem.Play();  //VFX                              
                                soundController.ClickSoundPlay(); //Audio FX

                                foreach(GameObject g in itemsInGame)
                                {
                                    g.GetComponent<Item>().canBeSelected = false;
                                }
                                StartCoroutine(CheckMatches());
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator CheckMatches()
    {
        if(firstItemSelected.itemValue == secondItemSelected.itemValue)
        {
            Debug.Log("Item Match!!");
            firstItemSelected.gameObject.GetComponent<BoxCollider>().enabled = false;
            secondItemSelected.gameObject.GetComponent<BoxCollider>().enabled = false;    
            matchesCount++;

            //VFX
            Instantiate(correctMatchParticleSystem, firstItemSelected.transform.position + (Vector3.forward * -5), Quaternion.identity);
            Instantiate(correctMatchParticleSystem, secondItemSelected.transform.position + (Vector3.forward * -5), Quaternion.identity);

            //Audio FX
            soundController.CorrectSoundPlay();

            if(maxMatches == matchesCount)
            {
                Debug.Log("JUEGO TERMINADO!!!!!!!!"); 
                startTimer = false;
                //gameTimeText.gameObject.SetActive(false);  
                yield return new WaitForSeconds(.4f);
                gameEndMenu.SetActive(true);
                EndGamePanel endGamePanel = gameEndMenu.GetComponent<EndGamePanel>();
                endGamePanel.SetAttemps(attemps);
                endGamePanel.SetGameTime(gameTimeText.text);
                endGamePanel.SetScore(CalculateScore());
                endGamePanel.SetBestScore(GameSettings.bestGameScore_static);
            }      

        }
        else
        {
            yield return new WaitForSeconds(.5f);
            firstItemSelected.box.SetActive(true);
            firstItemSelected.questionMark.SetActive(true);
            firstItemSelected.itemFrame.SetActive(false);
            firstItemSelected.spriteContainer.gameObject.SetActive(false);

            yield return new WaitForSeconds(.5f);
            secondItemSelected.box.SetActive(true);
            secondItemSelected.questionMark.SetActive(true);
            secondItemSelected.itemFrame.SetActive(false);
            secondItemSelected.spriteContainer.gameObject.SetActive(false);


            //Audio FX
            soundController.ErrorSoundPlay();
            Debug.Log("Match Error!!");
            attemps++;
        }

        firstItemSelected = null;
        secondItemSelected = null;

        foreach(GameObject g in itemsInGame)
        {
            g.GetComponent<Item>().canBeSelected = true;
        }

        yield return new WaitForSeconds(.5f);
        Debug.Log("Fin Corrutina!!");

        yield break;
    }

    void GameTimer()
    {
        if(!startTimer) return;
        gameTime += Time.deltaTime;
        if ((int)gameTime == (int)(previousGameTime + .7f))
            return;

        int hours = (int)((gameTime / 60) / 60);
        int minutes = (int)(gameTime / 60) % 60;
        int seconds = (int)(gameTime % 60);

        //gameTimeText.text = TimeFormatter(hours) + " : " + TimeFormatter(minutes) + " : " + TimeFormatter(seconds);
        gameTimeText.text = $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";

        Debug.Log("Ejecutando TIMER");
        previousGameTime = gameTime;
    }

    int CalculateScore()
    {
        int score = 0;
        int maxScore = 100;
        int discountPerTime = 15;
        for(int i = 0; i < maxMatches; i++)
        {
            maxScore += 200;
            discountPerTime--;
        }
        score = maxScore - (int)(gameTime * discountPerTime) - (attemps * 5);
        if(score > GameSettings.bestGameScore_static) GameSettings.bestGameScore_static = score;
        if(score <= 0) score = 10;
        Debug.Log(" MAXSCORE : " + maxScore);
        return score;
    }

    public void PauseGame()
    {
        Debug.Log("Paused Game");
        isGamePaused = true;
        startTimer = false;
    }
    public void ContinueGame()
    {
        Debug.Log("Game Continued");
        isGamePaused = false;
        startTimer = true;
    }


}
