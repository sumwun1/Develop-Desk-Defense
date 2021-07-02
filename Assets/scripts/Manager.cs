using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public int rateIndex;
    public int current;
    public int record;
    public GameObject desks;
    public GameObject workButton;
    public GameObject helpButton;
    public GameObject slowButton;
    public GameObject fastButton;
    //public GameObject sellButton;
    public GameObject skipButton;
    public GameObject statButton;
    public GameObject closeButton;
    public GameObject pauseButton;
    public GameObject cancelButton;
    public GameObject retakeButton;
    public GameObject forfeitButton;
    public GameObject centerPanel;
    public GameObject bottomPanel;
    public GameObject turnsText;
    public GameObject clicksText;
    public GameObject unlockText;
    public GameObject retakeText;
    public GameObject skipText;
    public GameObject selectedImage;
    public GameObject unlockImage;
    public Text aText;
    public Text tpsText;
    public Text currentText;
    public Text recordText;
    public Text centerText;
    public Text bottomText;
    public AudioSource titleAudio;
    public AudioSource unlockAudio;
    public AudioSource failAudio;
	public Pin pin;
    public GameObject[] supplies;
    public GameObject[] supplyButtons;
    public Texture[] textures;
    public AudioSource[] workAudio;
	bool moveHomework;
    bool showStats;
    bool showOverwrite;
    bool triggerBottle;
    bool triggerFolder;
    int a;
    int turns;
    int clicks;
    int tutorial;
    int unlocked;
    int supplyId;
    int bottleState;
    int folderState;
    /*int spawning;
    int willSpawn;*/
    int workAudioIndex;
	float period;
	float time;
    string state;
    //GameObject selectedSupply;
    float[] tempoFactors;
    string[] rateStrings;
    string[] descriptions;
	Homework[] homeworks;

    // Start is called before the first frame update
    void Start()
    {
        UpdateA(1);
        showStats = false;
        showOverwrite = false;
        turns = 0;
        clicks = 0;
        unlocked = 0;
        tutorial = 0;
        supplyId = -1;
        workAudioIndex = 3;
        tempoFactors = new float[4];
        tempoFactors[0] = 12f / 13f;
        tempoFactors[1] = 1f;
        tempoFactors[2] = 4f / 3f;
        tempoFactors[3] = 1f;
        rateStrings = new string[6];
        rateStrings[0] = "hyperslow";
        rateStrings[1] = "very slow";
        rateStrings[2] = "slow";
        rateStrings[3] = "fast";
        rateStrings[4] = "very fast";
        rateStrings[5] = "hyperfast";
        descriptions = new string[4];
        descriptions[0] = "Pencils do nearby homework. They're stronger against geometry (white) homework.\n\nStrength: 6\nRange: 1";
        descriptions[1] = "Erasers do all homework but do more to closer homework. They're stronger against history (green) homework.\n" + 
            "\nStrength: 0 to 5\nTrigger Range: 1\nEffect Range: 6";
        descriptions[2] = "Bottles do a constant amount divided over all homework, and then refill for 3 turns. " +
            "They're stronger against chemistry (red) homework.\n\nStrength: 2 to 32\nTrigger Range: 0\nEffect Range: 6";
        descriptions[3] = "Folders delay all homework for 1 turn. Group projects (blue) get done when delayed.\n"+ 
            "\nStrength: 0\nTrigger Range: 0\nEffect Range: 6";
        state = "title";
        //Select();
    }

    // Update is called once per frame
    void Update()
    {
        Supply[] supplies = GameObject.FindObjectsOfType<Supply>();
        workButton.SetActive(state == "select" || state == "help");
        helpButton.SetActive(state == "select" || state == "help");
        slowButton.SetActive((state == "select" || state == "help" || state == "pause") && rateIndex > 0);
        fastButton.SetActive((state == "select" || state == "help" || state == "pause") && rateIndex < 5);
        statButton.SetActive(state == "select" || state == "help");
        //sellButton.SetActive((state == "select" || state == "help") && desks.GetComponent<Desks>().Occupied(false));
        closeButton.SetActive(state == "help");
        pauseButton.SetActive(state == "work" || state == "pause");
        cancelButton.SetActive(state == "place");
        retakeButton.SetActive(state == "title" || state == "fail" || state == "unlock");
        skipButton.SetActive(state == "title" || state == "unlock");
        forfeitButton.SetActive(state == "work" || state == "pause");
        centerPanel.SetActive(state == "title" || state == "help" || state == "fail" || state == "unlock" || tutorial == 1 || tutorial == 3);
        bottomPanel.SetActive(tutorial >= 4 && tutorial <= 5);
        selectedImage.SetActive(state == "place");
        unlockImage.SetActive(state == "unlock");
        period = tempoFactors[workAudioIndex] / Mathf.Pow(2, rateIndex);

        for (int b = 0; b < unlocked; b++)
        {
            supplyButtons[b].SetActive(state == "select" || state == "help");
        }

        if (state == "work"){
            time -= Time.deltaTime;

            if (time <= 0){
                if (homeworks.Length < 1 && pin.GetRemaining() < 1)
                {
                    current++;

                    if (current > record)
                    {
                        UpdateA(current - record);
                        record = current;
                        currentText.text = "Current: " + current;
                        recordText.text = "Best: " + record;

                        if (record == 2 || record == 3 || record == 5 || record == 7)
                        {
                            for (int b = 0; b < 4; b++)
                            {
                                workAudio[b].Stop();
                            }

                            unlockImage.GetComponent<RawImage>().texture = textures[unlocked];
                            unlocked++;
                            centerText.text = "Unlocked new supply!";
                            retakeText.GetComponent<Text>().text = "description";
                            //retakeText.SetActive(true);
                            unlockText.SetActive(true);
                            //skipText.SetActive(true);
                            unlockAudio.Play();
                            state = "unlock";
                            return;
                        }
                    }

                    currentText.text = "Current: " + current;
                    recordText.text = "Best: " + record;
                    //moveHomework = true;
                    NextRound();
                }
                else if (moveHomework){
					homeworks = GameObject.FindObjectsOfType<Homework>();
                    /*Temporary[] temporaries = GameObject.FindObjectsOfType<Temporary>();
                    //Debug.Log(temporaries.Length);*/
                    if(folderState < 2)
                    {
                        for (int b = 0; b < homeworks.Length; b++)
                        {
                            homeworks[b].Turn();
                        }

                        bool stupidVariable = pin.Turn();
                        //Debug.Log("turned");
                    }

                    homeworks = GameObject.FindObjectsOfType<Homework>();
                    /*if (willSpawn > 0)
                    {
                        willSpawn--;
                    }*/
                    moveHomework = false;
                }
                else{
                    //Debug.Log(folderState);

                    if (bottleState > 0)
                    {
                        bottleState--;
                    }

                    if (folderState > 0)
                    {
                        folderState--;
                    }

                    for (int b = 0; b < supplies.Length; b++)
                    {
                        bool stupidVariable = supplies[b].Turn();
                    }

                    if (triggerBottle && bottleState <= 0)
                    {
                        for (int b = 0; b < homeworks.Length; b++)
                        {
                            homeworks[b].TakeDamage(Mathf.FloorToInt(32f / homeworks.Length), 2);
                        }

                        bottleState = 4;
                    }

                    if (triggerFolder && folderState <= 0)
                    {
                        for (int b = 0; b < homeworks.Length; b++)
                        {
                            homeworks[b].TakeDamage(0, 3);
                        }

                        folderState = 2;
                    }

                    for(int b = 0; b < homeworks.Length; b++)
                    {
                        homeworks[b].CheckHealth();
                    }

                    triggerBottle = false;
                    triggerFolder = false;
                    homeworks = GameObject.FindObjectsOfType<Homework>();
                    turns++;
                    turnsText.GetComponent<Text>().text = "Turns: " + turns;
                    moveHomework = true;
                }
								
				time = period;
			}
        }
    }

    public void NextRound()
    {
        moveHomework = true;
        time = 0;
        bottleState = 0;
        folderState = 0;
        pin.StartRound();
        /*int[] homeworkIndexes = new int[4];
        int factor = current;

        /*while (factor > 1f)
        {
            spawning++;
            factor /= 2;
        }*/

        //willSpawn = spawning;
    }

    public void StartWork()
    {
        if(state == "help")
        {
            centerText.text = "This button starts the homework.";
            return;
        }
        
        if(state == "select")
        {
            current = 1;
            state = "work";
            workAudioIndex = (workAudioIndex + 1) % 4;
            workAudio[workAudioIndex].Play();
            SetTutorial(1, 2);
            NextRound();
            return;
        }

        Debug.Log("tried to start work when state was " + state);
    }

    public void SelectSupply(int id)
    {
        if (state == "help")
        {
            showOverwrite = true;
            centerText.text = descriptions[id];
            return;
        }
        
        if (state == "select")
        {
            if (a >= 2)
            {
                //change sprite of selected supply
                supplyId = id;
                selectedImage.GetComponent<RawImage>().texture = textures[id];
                SetTutorial(3, 4);
                TogglePlace();
            }
            else
            {
                centerText.text = "Your grades are too low!";
                state = "help";
            }

            return;
        }

        Debug.Log("tried to select supply when state was " + state);
    }

    public void Select()
    {
        unlockText.SetActive(false);
        skipText.SetActive(false);
        //retakeText.SetActive(false);
        //skipButton.SetActive(false);
        titleAudio.Stop();
        failAudio.Stop();
        state = "select";
        SetTutorial(2, 3);
        homeworks = GameObject.FindObjectsOfType<Homework>();

        for (int b = 0; b < homeworks.Length; b++)
        {
            Destroy(homeworks[b].gameObject);
        }

        for (int b = 0; b < 4; b++)
        {
            workAudio[b].Stop();
        }

        if(tutorial == 1)
        {
            centerText.text = "Click \">\" to start doing homework. Round 1 has zero assignments, but later rounds will have more.";
        }

        if(tutorial == 3)
        {
            centerText.text = "Now that you unlocked a supply and can afford it, click the button on the right to buy it.";
        }
    }

    public void RetakeButton()
    {
        if(state == "unlock")
        {
            ToggleHelp();
            SelectSupply(unlocked - 1);
            return;
        }

        SetTutorial(0, 1);
        Select();
    }

    public void TogglePlace()
    {
        if(state == "place")
        {
            Select();
            return;
        }
        
        if(state == "select")
        {
            state = "place";
            return;
        }

        Debug.Log("tried to toggle place when state was " + state);
    }

    public void ToggleHelp()
    {
        if(state == "help")
        {
            centerText.text = "This button shows the panel that you are looking at right now.";
            return;
        }
        
        if(state == "select")
        {
            centerText.text = "To buy supplies, click the supply's button and click a desk. Each supply costs 2 A's." + 
                "\n\nClick a supply you already bought to sell it and get a full refund." +
                "\n\nLeave this panel open and click the other buttons to see what they do.";
            state = "help";
            return;
        }
        
        if(state == "unlock")
        {
            state = "help";
            return;
        }
        
        Debug.Log("tried to toggle help when state was " + state);
    }

    public void ChangeTps(bool faster)
    {
        if(state == "select" || state == "pause")
        {
            if (faster)
            {
                rateIndex++;
            }
            else
            {
                rateIndex--;
            }

            tpsText.text = rateStrings[rateIndex];
        }
        else if(state == "help")
        {
            if (faster)
            {
                centerText.text = "This button doubles turns/second.";
            }
            else
            {
                centerText.text = "This button halves turns/second.";
            }
        }
    }

    public void Pause()
    {
        if(state == "work")
        {
            workAudio[workAudioIndex].Pause();
            state = "pause";
            return;
        }
        
        if(state == "pause")
        {
            //time = 0;
            workAudio[workAudioIndex].UnPause();
            state = "work";
            return;
        }

        Debug.Log("tried to pause when state was " + state);
    }

    public void Fail()
    {
        for (int b = 0; b < 4; b++)
        {
            workAudio[b].Stop();
        }

        centerText.text = "You got a B.";
        retakeText.GetComponent<Text>().text = "retake";
        //retakeText.SetActive(true);
        failAudio.Play();
        state = "fail";
    }

    /*public void SellAll()
    {
        if (state == "help")
        {
            centerText.text = "This button sells all your supplies.";
        }
        else if (state == "select")
        {
            for (int b = 0; b < supplies.Length; b++)
            {
                supplies[b].GetComponent<Supply>().Sell();
            }
        }
    }*/

    public void SetTutorial(int previous, int next)
    {
        if (tutorial == previous)
        {
            //Debug.Log(previous + " " + next);
            tutorial = next;

            if(next == 4)
            {
                bottomText.text = "Click a desk to place a supply there.";
            }

            if (next == 5)
            {
                bottomText.text = "You can click a supply you already bought to sell it and get a full refund.";
            }
        }
    }

    public void ToggleStats()
    {
        if(state == "help")
        {
            centerText.text = "This button shows or hides statistics, including clicks and turns. "
                + "Clicking this button does not count as a click.";
            return;
        }

        if(state == "select")
        {
            clicksText.GetComponent<Text>().text = "Clicks: " + clicks;
            turnsText.GetComponent<Text>().text = "Turns: " + turns;
            showStats = !showStats;
            clicksText.SetActive(showStats);
            turnsText.SetActive(showStats);
            return;
        }

        Debug.Log("tried to toggle stats when state was " + state);
    }

    public void Click()
    {
        clicks++;
        clicksText.GetComponent<Text>().text = "Clicks: " + clicks;
    }

    public void UpdateA(int addition)
    {
        a += addition;
        aText.text = a + " A's";
    }

    public void Skip()
    {
        SetTutorial(0, 6);
        Select();
    }

    public void TriggerBottle()
    {
        triggerBottle = true;
    }

    public void TriggerFolder()
    {
        triggerFolder = true;
    }

    public float GetPeriod()
    {
        return (period);
    }

    public int GetSupplyId()
    {
        return (supplyId);
    }

    public string GetState()
    {
        return (state);
    }

    public int GetA()
    {
        return (a);
    }

    public bool GetOverwrite()
    {
        return (showOverwrite);
    }

    /*public int GetSpawning()
    {
        return (spawning);
    }*/
}