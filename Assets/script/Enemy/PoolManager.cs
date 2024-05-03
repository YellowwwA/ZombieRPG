using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{   
    //�������� ������ ����
    public GameObject[] prefabs;
    
    // Ǯ ��� ����Ʈ
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; //�迭 �ʱ�ȭ

        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>(); //����Ʈ �ʱ�ȭ
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        //������ Ǯ�� ��Ȱ��ȭ�� ���ӿ�����Ʈ ����
        foreach(GameObject item in pools[index])
        {   
            if (!item.activeSelf)
            {
                //�߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //��ã������
        if(!select)
        {
            //���Ӱ� ���� �� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        if (index == 1)//�ε��� 1 == �������� ó���� ��Ȱ��ȭ�ؼ� ����
            select.SetActive(false);

        return select;
    }
}
