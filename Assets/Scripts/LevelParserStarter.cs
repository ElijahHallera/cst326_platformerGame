﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelParserStarter : MonoBehaviour
{
    public string filename;

    private Animator animator;

    public GameObject Rock;

    public GameObject Brick;

    public GameObject QuestionBox;

    public GameObject Stone;

    public GameObject Lava;

    public GameObject Goal;

    public Transform parentTransform;

    public float timer = 100;
    public float coins = 0;
    public float score = 0;
    public float endGameCounter = 0;
    [SerializeField] private Text TimeCounter;
    [SerializeField] private Text CoinCounter;
    [SerializeField] private Text ScoreCounter;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TimeCounter.GetComponent<Text>().text = timer.ToString();
        CoinCounter.GetComponent<Text>().text = coins.ToString();
        ScoreCounter.GetComponent<Text>().text = score.ToString();
        RefreshParse();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        //Debug.Log($"TIMER: " + timer);
        TimeCounter.text = (timer).ToString("0");

        //Ray Handler for Physics Raycast
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.name == "Brick(Clone)")
                {
                    Destroy(hit.collider.gameObject);
                    score += 100;
                    ScoreCounter.text = (score).ToString("0");
                }
                if (hit.collider.gameObject.name == "QuestionBox(Clone)")
                {
                    score += 100;
                    coins++;
                    ScoreCounter.text = (score).ToString("0");
                    CoinCounter.text = (coins).ToString("0");
                }
            }
        }

        if (timer < 0)
        {
            endGameCounter++;

            if(endGameCounter == 1){
                QuitGame();
            }
        }
    }

    public void increaseScore()
    {
        score += 100;
        ScoreCounter.text = (score).ToString("0");
    }

    public void increaseCoins()
    {
        coins++;
        CoinCounter.text = (coins).ToString("0");
    }

    public void QuitGame()
    {
        Debug.Log("GAME OVER YOU FAILED");
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        Debug.Log("Congratulations YOU won!");
    }

    private void FileParser()
    {
        string fileToParse = string.Format("{0}{1}{2}.txt", Application.dataPath, "/Resources/", filename);

        using (StreamReader sr = new StreamReader(fileToParse))
        {
            string line = "";
            int row = 0;

            while ((line = sr.ReadLine()) != null)
            {
                int column = 0;
                char[] letters = line.ToCharArray();
                foreach (var letter in letters)
                {
                    //Call SpawnPrefab
                    SpawnPrefab(letter, new Vector3(column, -row, (float)-0.5));
                    column++;
                }
                row++;
            }
            sr.Close();
        }
    }

    private void SpawnPrefab(char spot, Vector3 positionToSpawn)
    {
        GameObject ToSpawn;

        switch (spot)
        {
            case 'b': ToSpawn = Brick; break;
                //Debug.Log("Spawn Brick");
            case '?': ToSpawn = QuestionBox; break;
                //Debug.Log("Spawn QuestionBox");
            case 'x': ToSpawn = Rock; break;
                //Debug.Log("Spawn Rock");
            case 's': ToSpawn = Stone; break;
                //Debug.Log(""Spawn Rock");
            case 'g': ToSpawn = Goal; break;
                //Debug.Log("Spawn Goal");
            case 'l': ToSpawn = Lava; break;
            //default: Debug.Log("Default Entered"); break;
            default: return;
                //ToSpawn = //Brick;       break;
        }

        ToSpawn = GameObject.Instantiate(ToSpawn, parentTransform);
        ToSpawn.transform.localPosition = positionToSpawn;
    }

    public void RefreshParse()
    {
        GameObject newParent = new GameObject();
        newParent.name = "Environment";
        newParent.transform.position = parentTransform.position;
        newParent.transform.parent = this.transform;
        
        if (parentTransform) Destroy(parentTransform.gameObject);

        parentTransform = newParent.transform;
        FileParser();
    }
}
