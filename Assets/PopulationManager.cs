using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ling;
using System.Linq;

public class PopulationManager : MonoBehaviour
{

    public GameObject personPrefab;
    public int populationsize = 10;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    int trialTime = 10;
    int generation = 1;

    // Start is called before the first frame update

    GUIStyle guiStyle = new GUIStyle();

    void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        List<GameObject> sortedList = population.OrderBy (o => o.GetComponent<DNA>().timeToDie).ToList();

        population.Clear();

        for (int i = (int) (sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i]), sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }
    }

    private void OnGUI()
    {
        guiStyle.fontSize = 31;
        guiStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(7, 10, 100, 20), "Generation: " + generation, guiStyle);

        GUI.Label(new Rect(7, 65, 100, 20), "Trial Time: : " + (int)elapsed, guiStyle);
    }

    void Start()
    {
        for (int i = 0; i < populationsize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f), 0);
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
            go.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
            population.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
