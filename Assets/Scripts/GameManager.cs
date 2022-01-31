using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

abstract class Level
{
    public enum LevelType
    {
        Notif, Day, Night
    }

    public readonly int time;
    public readonly LevelType levelType;
    public Level(int time, LevelType type)
    {
        this.time = time;
        levelType = type;
    }
}

class Notif:Level
{
    public readonly string storyText;
    public readonly string helpText;
    public Notif(int time, string storyText="", string helpText=""): base(time, LevelType.Notif)
    {
        this.storyText = storyText;
        this.helpText = helpText;
    }
}

class Day:Level
{
    public readonly string text;
    public readonly string sceneName;
    public Day(int time, string sceneName, string text="") : base(time, LevelType.Day)
    {
        this.text = text;
        this.sceneName = sceneName;
    }
}

class Night : Level
{
    public readonly string text;
    public Night(int time, string text="") : base(time, LevelType.Night)
    {
        this.text = text;
    }
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Text textText;
    [SerializeField]
    Text timerText;
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    GameObject notifObject;
    [SerializeField]
    Text notifText;

    int levelIndex = -1;
    Level[] levels = {
        new Notif(10, "Shepherd and the Curse of Duality"),
        new Notif(15, "The shepherd is taking care of the sheeps.", "Move: WASD\tJump: Space\tHold sheep: LeftClick"),
        new Day(35, "L1", "You can drag and throw sheeps behind the fences.\nThe timer shows when the night comes."),
        new Night(20, "You can't move sheeps when night comes."),
        new Notif(10, "Wolves are around.", "Don't let the wolves kill the sheeps."),
        new Day(30, "L2"),
        new Night(30, "Wolves are faster at night.\nRemember, you can't move the sheeps when the night comes."),
        new Notif(10, "The shepherd has an ability! Not so useful yet...", "Use Left Shift to temporarily change the time to night."),
        new Day(30, "L3", "Wolves are not faster at temporarily night."),
        new Night(30),
        new Notif(5, "Some nights are hard...", "Throwing a sheep is faster than carrying it."),
        new Day(30, "L4", "Throwing a sheep is faster than carrying it."),
        new Night(30),
        new Notif(15, "One night, the shepherd finds out some sheeps are cursed!\nThe Curse of Duality!", "Some sheeps will change into wolves at night."),
        new Day(30, "L5", "You can temporarily change the time to night\nand see which sheeps are going to change to wolves.\nBut they can still kill other's at temporary nights!"),
        new Night(30, "You can use Left Shift to temporarily change the time to night"),
        new Notif(15, "", "Try to remember which sheeps are wolves instead of constantly using temporary night."),
        new Day(30, "L6"),
        new Night(30),
        new Notif(15, "", "Find a way!"),
        new Day(30, "L7"),
        new Night(30),
        new Notif(15, "To make things worst, some wolves also change into sheeps at night!\nBut you are too kind hearted to let them die...", "Save them all!"),
        new Day(30, "L8"),
        new Night(30),
        new Notif(15, "", "Throw!"),
        new Day(40, "L9"),
        new Night(30),
        new Notif(15, "", "Very confusing..."),
        new Day(30, "L10"),
        new Night(30),
        new Notif(15, "", "I promise, these levels are doable!"),
        new Day(30, "L11"),
        new Night(30),
    };

    float timer = 0f;
    public static bool isNight;
    public static bool nightTrigger;
    public static bool dayTrigger;
    public void skip()
    {
        timer = 0;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(canvas);
    }
    private void Update()
    {
        timer -= Time.deltaTime * 5;
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
            if (levelIndex >= 0)
            {
                Level previousLevel = levels[levelIndex];
                if (previousLevel.levelType == Level.LevelType.Notif)
                {
                    notifObject.SetActive(false);
                }
                else if (previousLevel.levelType == Level.LevelType.Day)
                {
                }
                else if (previousLevel.levelType == Level.LevelType.Night)
                {
                    isNight = false;
                }
            }
            levelIndex++;
            Level currentLevel = levels[levelIndex];
            timer = currentLevel.time;
            if (currentLevel.levelType == Level.LevelType.Notif)
            {
                Notif notifLevel = (Notif)currentLevel;
                notifObject.SetActive(true);
                notifText.text = notifLevel.storyText + "\n\n<color=yellow>" + notifLevel.helpText + "</color>";
                SceneManager.LoadScene("Notif");
            }
            else if (currentLevel.levelType == Level.LevelType.Day)
            {
                SheepAI.sheepList.Clear();
                WolfAI.nightWolfList.Clear();
                Day dayLevel = (Day)currentLevel;
                textText.text = dayLevel.text;
                dayTrigger = true;
                SceneManager.LoadScene(dayLevel.sceneName);
            }
            else if (currentLevel.levelType == Level.LevelType.Night)
            {
                Night nightLevel = (Night)currentLevel;
                textText.text = nightLevel.text;
                nightTrigger = true;
                isNight = true;
            }
        }
        else if (isNight)
        {
            int aliveCount = 0;
            for (int i = 0; i < SheepAI.sheepList.Count; i++)
            {
                GameObject sheep = SheepAI.sheepList[i];
                if (!sheep.GetComponent<SheepAI>().isDead)// && !WolfAI.wolfsCanReach(sheep.transform.position))
                {
                    aliveCount++;
                }
            }
            float score = (float)(aliveCount) / (SheepAI.sheepList.Count);
        }
    }
}
