using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{

    public GameObject personPrefab;
    public int populationSize = 10;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    int trialTime = 5;
    int generation = 1;

    // Start is called before the first frame update
    // 1
    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();
        // 2
        Vector3 pos = new Vector3(Random.Range(-7f, 7f), Random.Range(-2f, 2f), 0);
        bool isParent = Random.Range(0, 100) > 5;
        bool isParent1 = Random.Range(0, 10) < 5;
        if (isParent)
        {
            float x_offset = Random.Range(-3f, 3f);
            float y_offset = Random.Range(-3f, 3f);
            float x = isParent1 ? dna1.x : dna2.x;
            float y = isParent1 ? dna1.y : dna2.y;
            pos.x = x + x_offset;
            pos.y = y + y_offset;
        }
        
        // 3
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);



        // 4
        if (isParent)
        {
            offspring.GetComponent<DNA>().r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
            offspring.GetComponent<DNA>().g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
            offspring.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
            offspring.GetComponent<DNA>().s = Random.Range(0, 10) < 5 ? dna1.s : dna2.s;
            offspring.GetComponent<DNA>().x = isParent1 ? dna1.x : dna2.x;
            offspring.GetComponent<DNA>().y = isParent1 ? dna1.y : dna2.y;
        }
        else
        {
            offspring.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().s = Random.Range(0.1f, 0.3f);
            offspring.GetComponent<DNA>().x = pos.x;
            offspring.GetComponent<DNA>().y = pos.y;
        }
        return offspring;
    }
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9f, 9f), Random.Range(-4.5f, 4.5f), 0);
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
            go.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().s = Random.Range(0.1f, 0.3f);
            go.GetComponent<DNA>().x = pos.x;
            go.GetComponent<DNA>().y = pos.y;
            population.Add(go);
        }
    }

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 31;
        guiStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(7, 10, 100, 20), "Generation: " + generation, guiStyle);

        GUI.Label(new Rect(7, 65, 100, 20), "Trial Time: " + (int)elapsed, guiStyle);
    }



    void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<DNA>().timeToDie).ToList();

        population.Clear();

        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }
    

        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
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
