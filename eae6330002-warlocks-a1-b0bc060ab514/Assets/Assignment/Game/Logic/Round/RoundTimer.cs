using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimer : MonoBehaviour {

    private float RoundStartTime;
    private float RoundElapsedTime = 0f;

    [SerializeField]
    private float IslandShrinkInterval = 5f;

    [SerializeField]
    private Island m_Island = null;


    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        RoundElapsedTime += Time.deltaTime;
        //print("Current round time: " + (RoundElapsedTime));
        
    }

    public void StartRoundTimer()
    {
        RoundStartTime = Time.time;
        print("Round started at: " + RoundStartTime);
        InvokeRepeating("ShrinkIsland", IslandShrinkInterval, IslandShrinkInterval);
    }

    public void EndTimer()
    {
        CancelInvoke("ShrinkIsland");
    }

    public void Reset()
    {
        //RoundStartTime = Time.time; ;
        RoundElapsedTime = 0f;

        m_Island.ResetIslandSize();

        CancelInvoke("ShrinkIsland");
        //InvokeRepeating("ShrinkIsland", IslandShrinkInterval, IslandShrinkInterval);
    }

    private void ShrinkIsland()
    {
        m_Island.Shrink();
    }

    public Island getIsland()
    {
        return m_Island;
    }

}
