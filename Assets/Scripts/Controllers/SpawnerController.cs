using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerController : MonoBehaviour {
    [Header("Prefabs")]    
    public GameObject wordPrefab;
    public GameObject[] barriersPrefabs;

    [Header("Settings")]
    [Range(0, 10)]
    [Tooltip("* 10%")]
    public int wordSpawnRate = 5;
    [Tooltip("Interval between each spawn (in seconds)")]
    public float spawnInterval = 3;

    private float timeCounter = 0;

	void Start () {
        
	}
	
	void Update () {
        timeCounter += Time.deltaTime;
        if(timeCounter >= spawnInterval) {
            //Raffle a number
            float rn = UnityEngine.Random.Range(1, 11);

            //Spawn a word -- if it's raffled or if there are no barriers to spawn
            if(wordSpawnRate >= rn || barriersPrefabs.Length == 0) {
                //Get the word from the database
                DatabaseController dbc = new DatabaseController(DatabaseController.DB_URL);
                DataTable dataTable = dbc.RunQuery("SELECT * FROM PALAVRA order by random() limit 1");
                string strings = "";
                foreach(DataRow row in dataTable.Rows)
                {
                    Debug.Log(row[1].ToString());
                    strings = row["PPALAVRA"].ToString();
                }


                //Creates a new instance of wordPrefab
                GameObject wordObj = Instantiate(wordPrefab, GameObject.Find("Canvas").transform) as GameObject;
                //Select what to shows, text or image -- [Text = Child(0)  ||  Image = Child(1)]
                wordObj.transform.GetChild(0).gameObject.SetActive(true);
                wordObj.transform.GetChild(1).gameObject.SetActive(false);
                wordObj.GetComponentInChildren<Text>().text = strings;
                //wordObj.GetComponentInChildren<Image>().color = Color.blue;

            }
            //Spawn a barrier
            else {
                //Select a barrier from the array
                int barrierIndex = UnityEngine.Random.Range(0, barriersPrefabs.Length);
                GameObject spawnObject = Instantiate(barriersPrefabs[barrierIndex], GameObject.Find("Spawner").transform) as GameObject;
                spawnObject.transform.position = new Vector3(8, -2);
            }
                
            timeCounter = 0;
        }
	}
}
