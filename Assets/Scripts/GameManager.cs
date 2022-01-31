using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Text textText;
    [SerializeField]
    Text timerText;
    [SerializeField]
    GameObject canvas;

    int levelNum = 0;
    float[] times = {3, 30, 30};
    float[] nightTimes = {3, 3000, 30};
    float[] scores = {0, 0, 0};
    string[] texts =
    {
        "You can click and throw sheeps behind the fences.\nMove: WASD, Jump: Space. You can't move them anymore when night comes. (the timer hits 0)",
        "Keep wolfs separated from sheeps before they kill them, or you lose score.\n",
        "Because of the Curse of Duality, some sheeps will change into wolf at night.\nUse Left Shift to temporarily change time to night. Keep sheeps that change to wolf separated.",
    };
    float timer = 0f;
    public static bool isNight;
    public static bool nightTrigger;
    public static bool dayTrigger;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(canvas);
        textText.text = texts[levelNum];
        timer = times[levelNum];
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = ((int)timer).ToString();
        if (nightTrigger)
        {
            nightTrigger = false;
        }
        if (dayTrigger)
        {
            dayTrigger = false;
        }
        if (timer <= 0f)
        {
            if (isNight)
            {
                levelNum++;
                int aliveCount = 0;
                for (int i = 0; i < SheepAI.sheepList.Count; i++)
                {
                    GameObject sheep = SheepAI.sheepList[i];
                    if (!sheep.GetComponent<SheepAI>().isDead && !WolfAI.wolfsCanReach(sheep.transform.position))
                    {
                        aliveCount++;
                    }
                }
                scores[levelNum] = (float)(aliveCount) / (SheepAI.sheepList.Count);
                isNight = false;
                dayTrigger = true;
                SheepAI.sheepList.Clear();
                WolfAI.nightWolfList.Clear();
                SceneManager.LoadScene(levelNum);
                textText.text = texts[levelNum];
                timer = times[levelNum];
            }
            else
            {
                isNight = true;
                nightTrigger = true;
                textText.text = "";
                timer = nightTimes[levelNum];
            }
        }
    }
}
