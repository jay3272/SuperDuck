using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnglishCreate : MonoBehaviour
{
    public GameObject EnglishWord;

    // Start is called before the first frame update
    void Start()
    {
        CreatEnglish("ap");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatEnglish(string word)

    {
        for (int i = 0; i < word.Length; i++)
        {            
            EnglishWord.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>(word.Substring(i, 1));
            
            float x;
            x = Random.Range(-6, 6);
            Instantiate(EnglishWord, new Vector3(x, 2.8f, 0), Quaternion.identity);
        }

    }
}
