using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBar : MonoBehaviour
{
    [SerializeField] private GameObject segmentPrefab;
    public int numOfSegments;
    private int prevNumOfSegments;
    private GameObject[] segments;

    public int val;
    private int prevVal;
    
    // Update is called once per frame
    void Update()
    {
        if (numOfSegments < 0)
        {
            numOfSegments = 0;
        }
        val = Mathf.Clamp(val, 0, numOfSegments);
        
        if (segmentPrefab != null)
        {
            if (prevNumOfSegments != numOfSegments)
            {
                UpdateSegments();
            }
            if (val != prevVal)
            {
                UpdateFill();
            }

            prevNumOfSegments = numOfSegments;
            prevVal = val;
        }
    }

    private void UpdateSegments()
    {
        if (segments != null && segments.Length > 0)
        {
            foreach (GameObject segment in segments)
            {
                Destroy(segment);
            }
        }
        segments = new GameObject[numOfSegments];
        for(int i = 0; i < numOfSegments; i++)
        {
            Vector3 temp = new Vector3(segmentPrefab.GetComponent<RectTransform>().rect.width * i, 0, 0);
            segments[i] = Instantiate(segmentPrefab, transform);
            segments[i].transform.localPosition += temp;
        }
        UpdateFill();
    }

    private void UpdateFill()
    {
        for (int i = 0; i < val; i++)
        {
            segments[i].GetComponent<Segment>().Fill();
        }
        for(int i = val; i < numOfSegments; i++)
        {
            segments[i].GetComponent<Segment>().Empty();
        }
    }

    public void Increase()
    {
        val++;
    }

    public void Decrease()
    {
        val--;
    }

}
