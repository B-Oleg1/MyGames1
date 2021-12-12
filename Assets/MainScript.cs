using Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField]
    private Transform _questionObject;
    [SerializeField]
    private Transform _objectWithQuestions;
    [SerializeField]
    private Transform _objectObjectsWithButtons;

    [SerializeField]
    private Sprite[] _allItemsSprites;
    //private Dictionary<ShopItem, Sprite> _allItemsSprites;

    [SerializeField]
    private StatisticCommand[] _allCommandStatistics;

    private void Awake()
    {
        for (int i = 0; i < _objectWithQuestions.childCount; i++)
        {
            if (_objectWithQuestions.GetChild(i).CompareTag("QuestionObject"))
            {
                ModelsScript.allSceneWithQuestion[i] = _objectWithQuestions.GetChild(i).GetComponent<Transform>();
            }
        }

        //ModelsScript.allSceneWithQuestion = _objectWithQuestions.GetComponentsInChildren<Transform>();

        print(ModelsScript.allSceneWithQuestion.Length);
        ModelsScript.allBtns = _objectObjectsWithButtons.GetComponentsInChildren<Button>();

        _questionObject.gameObject.SetActive(false);
        foreach (var item in ModelsScript.allSceneWithQuestion)
        {
            item.gameObject.SetActive(false);
        }

        ModelsScript.Players = new List<Player>
        {
            new Player
            {
                Name = "�������1",
                Score = 0,
                ShopScore = 0,
                ProgressBattlePass = 0,
                Items = new List<ShopItem>(),
                BattlePassItems = new List<ShopItem>()
            },
            new Player
            {
                Name = "�������2",
                Score = 0,
                ShopScore = 0,
                ProgressBattlePass = 0,
                Items = new List<ShopItem>(),
                BattlePassItems = new List<ShopItem>()
            },
            new Player
            {
                Name = "�������3",
                Score = 0,
                ShopScore = 0,
                ProgressBattlePass = 0,
                Items = new List<ShopItem>(),
                BattlePassItems = new List<ShopItem>()
            }
        };

        GenerateNewBattlePass(0);

        UpdateStatistic(0);
    }

    private void Update()
    {
        if (ModelsScript.needUpdateCommandId >= 0 && ModelsScript.needUpdateCommandId <= 2)
        {
            UpdateStatistic(ModelsScript.needUpdateCommandId);
            ModelsScript.needUpdateCommandId = -1;
        }
    }

    public void OnClickBsn(int i)
    {
        var btns = ModelsScript.allBtns[i];

        ModelsScript.currentQuestion = i;
        ModelsScript.currentPointsForQuestions = int.Parse(btns.GetComponentInChildren<Text>().text);

        _questionObject.gameObject.SetActive(true);
        _objectWithQuestions.gameObject.SetActive(true);
        ModelsScript.allSceneWithQuestion[i].gameObject.SetActive(true);

        btns.enabled = false;
        var tmp = btns.GetComponent<Image>().color;
        tmp.a = 0;
        btns.GetComponent<Image>().color = tmp;
        btns.GetComponentInChildren<Text>().text = string.Empty;
    }

    public static void GiveOutPrize(int commandId)
    {
        if (ModelsScript.Players[commandId].ProgressBattlePass >= 2)
        {
            if (ModelsScript.Players[commandId].ProgressBattlePass >= 2 && ModelsScript.Players[commandId].ProgressBattlePass < 4)
            {
                ModelsScript.Players[commandId].Items.Add(ModelsScript.Players[commandId].BattlePassItems[0]);
            }
            else if (ModelsScript.Players[commandId].ProgressBattlePass >= 4 && ModelsScript.Players[commandId].ProgressBattlePass < 6)
            {
                ModelsScript.Players[commandId].Items.Add(ModelsScript.Players[commandId].BattlePassItems[1]);
            }
            else if (ModelsScript.Players[commandId].ProgressBattlePass >= 6 && ModelsScript.Players[commandId].ProgressBattlePass < 7)
            {
                ModelsScript.Players[commandId].Items.Add(ModelsScript.Players[commandId].BattlePassItems[2]);
            }
            else if (ModelsScript.Players[commandId].ProgressBattlePass >= 7)
            {
                ModelsScript.Players[commandId].Items.Add(ModelsScript.Players[commandId].BattlePassItems[3]);
            }
            ModelsScript.Players[commandId].ProgressBattlePass = 0;
            ModelsScript.needUpdateCommandId = commandId;
            GenerateNewBattlePass(commandId);
        }
    }

    private static void GenerateNewBattlePass(int commandId)
    {
        ModelsScript.Players[commandId].BattlePassItems = new List<ShopItem>
        {
            (ShopItem)UnityEngine.Random.Range(0,3),
            (ShopItem)UnityEngine.Random.Range(3,5),
            (ShopItem)UnityEngine.Random.Range(5,9),
            (ShopItem)UnityEngine.Random.Range(9,12)
        };
        ModelsScript.needUpdateCommandId = commandId;
    }

    private void UpdateStatistic(int commandId)
    {
        _allCommandStatistics[commandId].score.text = ModelsScript.Players[commandId].Score.ToString();
        _allCommandStatistics[commandId].scoreShop.text = ModelsScript.Players[commandId].ShopScore.ToString();

        for (int i = 0; i < _allCommandStatistics[commandId].allItems.childCount; i++)
        {
            _allCommandStatistics[commandId].allItems.GetChild(i).GetChild(1).GetComponent<Text>().text = ModelsScript.Players[commandId].Items.Count(item => item == (ShopItem)i).ToString();
        }

        _allCommandStatistics[commandId].battlePassSlider.value = 0.142857f * ModelsScript.Players[commandId].ProgressBattlePass;

        for (int i = 0; i < _allCommandStatistics[commandId].battlePassItemsObject.childCount; i++)
        {
            var shopItemId = (int)Enum.Parse(typeof(ShopItem), ModelsScript.Players[commandId].BattlePassItems[i].ToString());
            _allCommandStatistics[commandId].battlePassItemsObject.GetChild(i).GetComponent<Image>().sprite = _allItemsSprites[shopItemId];
        }
    }
}

[System.Serializable]
public class StatisticCommand
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI scoreShop;
    public Transform allItems;
    public Slider battlePassSlider;
    public Transform battlePassItemsObject;
}