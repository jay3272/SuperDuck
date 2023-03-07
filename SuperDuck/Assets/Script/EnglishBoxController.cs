using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnglishBoxController : MonoBehaviour
{
    public static string levelWord;
    private int levelWordCount;
    public string level;
    public Text uiTxtLevelWord;

    private int[] axisXpoint = {-50,-40,-35,-25,-20,-15,-10,5,10,15};
    private int[] axisYpoint = {-2,2,8,14,25,30};

    // Start is called before the first frame update
    void Start()
    {
        TextAsset binAsset;
        //讀取csv二進位制檔案
        switch (level)
        {
            case "EZ":
                binAsset = Resources.Load ("ezword", typeof(TextAsset)) as TextAsset;     
            break;
            case "Normal":
                binAsset = Resources.Load ("normalword", typeof(TextAsset)) as TextAsset;     
            break;
            case "Hell":
                binAsset = Resources.Load ("hellword", typeof(TextAsset)) as TextAsset;     
            break;
            default :
                binAsset = Resources.Load ("ezword", typeof(TextAsset)) as TextAsset;     
            break;
        }
        string [] lineArray = binAsset.text.Split ("\r\n", System.StringSplitOptions.RemoveEmptyEntries);
        int iRandomWord = Random.Range(0, lineArray.Length);
        
        levelWord = lineArray[iRandomWord];
        uiTxtLevelWord.text = "單字: " + lineArray[iRandomWord];
        loadEnglishImage();

        Debug.Log("");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void loadEnglishImage()
    {
        try
        {            
            char[] charLvWord = levelWord.ToCharArray();
            levelWordCount = charLvWord.Length;
            int iRandomX,iRandomY;
            Object cubePreb;
            GameObject cube;

            List<int> idxRandomListX = new List<int>();
            List<int> indRandomListY = new List<int>();

            idxRandomListX = GenerateRandomList(axisXpoint.Length-1,0,axisXpoint.Length-1);
            indRandomListY = GenerateRandomList(axisYpoint.Length-1,0,axisYpoint.Length-1);

            for (int i = 0; i < levelWordCount; i++)
            {     
                switch (i)
                {
                    case 0:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[0]], axisXpoint[idxRandomListX[0]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[0]], axisYpoint[indRandomListY[0]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 1:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[1]], axisXpoint[idxRandomListX[1]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[1]], axisYpoint[indRandomListY[1]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 2:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[2]], axisXpoint[idxRandomListX[2]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[2]], axisYpoint[indRandomListY[2]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 3:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[3]], axisXpoint[idxRandomListX[3]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[3]], axisYpoint[indRandomListY[3]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 4:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[4]], axisXpoint[idxRandomListX[4]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[4]], axisYpoint[indRandomListY[4]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 5:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[5]], axisXpoint[idxRandomListX[5]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[3]], axisYpoint[indRandomListY[3]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 6:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[6]], axisXpoint[idxRandomListX[6]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[2]], axisYpoint[indRandomListY[2]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 7:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[7]], axisXpoint[idxRandomListX[7]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[1]], axisYpoint[indRandomListY[1]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 8:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[8]], axisXpoint[idxRandomListX[8]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[0]], axisYpoint[indRandomListY[0]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    case 9:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[9]], axisXpoint[idxRandomListX[9]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[2]], axisYpoint[indRandomListY[2]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;

                    default:
                        iRandomX = Random.Range(axisXpoint[idxRandomListX[0]], axisXpoint[idxRandomListX[0]+1]);
                        iRandomY = Random.Range(axisYpoint[indRandomListY[0]], axisYpoint[indRandomListY[0]+1]);
                        cubePreb = Resources.Load("eng/" + charLvWord[i], typeof(GameObject));
                        cube = Instantiate(cubePreb, new Vector2(iRandomX, iRandomY), Quaternion.identity) as GameObject;
                    break;
                }
 
                Debug.Log("load English Image.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            throw;
        }

    }

    /// <summary>
    /// 生成指定个数的不重复随机数列表
    /// </summary>
    /// <param name="length">列表的长度[不得大于min和max之间随机数的总个数]</param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static List<int> GenerateRandomList(int length, int min, int max)
    {
        List<int> randomList = new List<int>();
        if (length <= (max - min)) 
        {
            for (var i = 0; i < length; i++)
            {
                int random = Random.Range(min, max);
                if (randomList.Contains(random))
                {
                    i--;
                    continue;
                }
                else
                {
                    randomList.Add(random);
                }
            }
        }
        return randomList;
    }

}
