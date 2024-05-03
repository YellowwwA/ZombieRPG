using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing, string _Index)
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; Index = _Index; }

    public string Type, Name, Explain, Number, Index;
    public bool isUsing;
}

public class GameManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList, CurItemList;
    public string curType = "Equipment";
    public GameObject[] Slot, UsingImage, Numbers;
    public Image[] TabImage, ItemImage;
    Color colorA;
    Color colorB;
    public Sprite[] ItemSprite;
    public GameObject ExplainPanel;
    public RectTransform[] SlotPos;
    public RectTransform CanvasRect;
    IEnumerator PointerCoroutine;
    RectTransform ExplainRect;

    public GameObject ItemPanel;
    public NetworkManager theNet;

    private AudioManager theAudio;
    public string item_pickup;
    public string item_drink;
    public string switch3_sound;

    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        theNet = FindObjectOfType<NetworkManager>();
        //��ü ������ ����Ʈ �ҷ�����
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n'); //�������� ���� ���� �� ������ ������
        for(int i = 0; i< line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5])); // �� �����ۺ� ����(ī�װ�, �̸�, ����, ����, ��������)�� ������ ȣ��
            //MyItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));
        }
        colorA = TabImage[0].GetComponent<Image>().color;
        colorB = TabImage[1].GetComponent<Image>().color;
        //Save();
        Load();
        ExplainRect = ExplainPanel.GetComponent<RectTransform>();
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        ExplainRect.anchoredPosition = anchoredPos + new Vector2(-100,-90);
    }

    public void GetItem(int itemID, int _count)
    {
        Item curItem = MyItemList.Find(x => int.Parse(x.Index) == itemID);
        if(curItem != null)
        {
            curItem.Number = (int.Parse(curItem.Number) + _count).ToString();
        }
        else
        {
            Item curAllItem = AllItemList.Find(x => int.Parse(x.Index) == itemID);

            if(curAllItem != null)
            {
                curAllItem.Number = (int.Parse(curAllItem.Number) + 1).ToString();
                MyItemList.Add(curAllItem);
            }
        }
        theAudio.Play(item_pickup);
        Save();
    }

    public void RemoveItem(int itemID) // ������ ����Ʈ �Ϸ� �� �� �Լ� ȣ��
    {
        Item curItem = MyItemList.Find(x => int.Parse(x.Index) == itemID);
        if(curItem != null)
        {
            if (int.Parse(curItem.Number) > 1)
            {
                curItem.Number = (int.Parse(curItem.Number) - 1).ToString();
            }
            else
            {
                MyItemList.Remove(curItem);
            }
        }

        Save();
    }

    public void SlotClick(int slotNum)
    {
        Item CurItem = CurItemList[slotNum];
        Item UsingItem = CurItemList.Find(x => x.isUsing == true);
        
        if(curType == "Equipment") // ������ ĳ���Ͱ� ����ִ� ���� ������ �����ϱ�
        {
            if (UsingItem != null)
                UsingItem.isUsing = false;
            CurItem.isUsing = true;
            theAudio.Play(switch3_sound);
            Pattack.instance.EquipWeapon(CurItem.Index);
        }
        else if(curType == "Use") // �Һ� �������̸� ���� �ϳ� ���̱�
        {
            if(int.Parse(CurItem.Index) == 4) // hp potion�̶��
            {
                if (int.Parse(CurItem.Number) > 1)
                    CurItem.Number = (int.Parse(CurItem.Number) - 1).ToString();
                else
                    MyItemList.Remove(CurItem);
                theAudio.Play(item_drink);
                PlayerStat.instance.RecoverHp(20);
            }
            else if (int.Parse(CurItem.Index) == 5)
            {
                PlayerStat.instance.RecoverMp(5);
                if (int.Parse(CurItem.Number) > 1)
                    CurItem.Number = (int.Parse(CurItem.Number) - 1).ToString();
                else
                    MyItemList.Remove(CurItem);
            }
            else
            {
                if (int.Parse(CurItem.Number) > 1)
                    CurItem.Number = (int.Parse(CurItem.Number) - 1).ToString();
                else
                    MyItemList.Remove(CurItem);
            }
        }
        else
        {
        }
        Save();
    }

    public void TabClick(string tabName)
    {
        //���� ������ ����Ʈ�� Ŭ���� Ÿ�Ը� �߰�
        curType = tabName;
        CurItemList = MyItemList.FindAll(x => x.Type == tabName);
        
        for (int i = 0; i < Slot.Length; i++)
        {
            //���԰� �ؽ�Ʈ ���̱�
            bool isExist = i < CurItemList.Count;
            Slot[i].SetActive(isExist);
            Slot[i].GetComponentInChildren<Text>().text = isExist ? CurItemList[i].Name : "";

            if(isExist)
            {
                ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
                if (tabName == "Equipment")
                {
                    UsingImage[i].SetActive(CurItemList[i].isUsing);
                    Numbers[i].SetActive(false);
                }
                else if (tabName == "Use")
                {
                    UsingImage[i].SetActive(false);
                    Numbers[i].SetActive(true);
                    Numbers[i].GetComponentInChildren<Text>().text = CurItemList[i].Number;
                }
                else
                {
                    UsingImage[i].SetActive(false);
                    Numbers[i].SetActive(false);
                }
            }
        }

        //���̹���
        int tabNum = 0;
        switch(tabName)
        {
            case "Equipment": tabNum = 0;
                break;
            case "Use": tabNum = 1;
                break;
            case "Quest": tabNum = 2;
                break;
            case "Etc": tabNum = 3;
                break;
        }
        
        
        for (int i = 0; i < TabImage.Length; i++)
        {
            TabImage[i].color = i == tabNum ? colorA : colorB; // ���õ� �ǿ� ���� ���� ����

        }
    }

    public void PointerEnter(int slotNum)
    {
        // ���Կ� ���콺�� �ø��� 0.5�� �Ŀ� ����â ���
        PointerCoroutine = PointerEnterDelay(slotNum);
        StartCoroutine(PointerCoroutine);

        // ����â�� �̸�, �̹���, ����, ���� ��Ÿ����
        ExplainPanel.GetComponentInChildren<Text>().text = CurItemList[slotNum].Name;
        ExplainPanel.transform.GetChild(2).GetComponent<Image>().sprite = Slot[slotNum].transform.GetChild(2).GetComponent<Image>().sprite;
        ExplainPanel.transform.GetChild(3).GetComponent<Text>().text = CurItemList[slotNum].Number + "��";
        ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = CurItemList[slotNum].Explain;
        
    }
    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
    }

    public void PointerExit(int slotNum)
    {
        StopCoroutine(PointerCoroutine); //���⼭ PointerEnterDelay�� �ν����� ���ϱ� ������ PointerEnter���� PointerCoroutine�� �ѹ� ���� �Ŀ� ���
        ExplainPanel.SetActive(false);
    }

    void Save()
    {
        string jdata = JsonConvert.SerializeObject(MyItemList);
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText"+theNet.NickNameInput.text+".txt", jdata);

        TabClick(curType);
    }
    void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText" + theNet.NickNameInput.text + ".txt");
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
    }

    public void OpenItem()
    {
        ItemPanel.SetActive(true);
    }
    public void CloseItem()
    {
        ItemPanel.SetActive(false);
    }

}
