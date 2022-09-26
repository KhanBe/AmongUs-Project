using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//엔진 증기/스파크 랜덤 배출 스크립트
public class EngineBody : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> steams = new List<GameObject>();

    [SerializeField]//배치해둔 4개의 스파크 리스트
    private List<SpriteRenderer> sparks = new List<SpriteRenderer>();

    [SerializeField]//12개의 스파크 스프라이트 리스트
    private List<Sprite> sparkSprites = new List<Sprite>();

    //스파크 위치 인덱스
    private int nowIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var steam in steams)
        {
            StartCoroutine(RandomSteam(steam));
        }
        StartCoroutine(SparkEngine());
    }

    private IEnumerator RandomSteam(GameObject steam)
    {
        while (true)
        {
            float timer = Random.Range(0.5f, 1.5f);
            while (timer >= 0f)
            {
                //null : 한 프레임동안 기다림
                yield return null;
                timer -= Time.deltaTime;
            }

            steam.SetActive(true);
            timer = 0f;
            while (timer <= 0.6f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            steam.SetActive(false);
        }
    }

    private IEnumerator SparkEngine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while (true)
        {
            float timer = Random.Range(0.2f, 1.5f);
            while (timer >= 0f)
            {
                yield return null;
                timer -= Time.deltaTime;
            }
            //sparkSprites에서 무작위스프라이트를 뽑아서 현재스파크가 표현되어야하는 위치으 옵젝에 빠르게 보여주도록만들고
            int[] indices = new int[Random.Range(2, 7)];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = Random.Range(0, sparkSprites.Count);
            }

            for (int i = 0; i < indices.Length; i++)
            {
                yield return wait;
                sparks[nowIndex].sprite = sparkSprites[indices[i]];
            }

            sparks[nowIndex++].sprite = null;//초기화
            if (nowIndex >= sparks.Count) nowIndex = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
