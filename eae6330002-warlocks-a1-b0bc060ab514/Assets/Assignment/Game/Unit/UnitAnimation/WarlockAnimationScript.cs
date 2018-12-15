using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockAnimationScript : MonoBehaviour, IAnimationScript {

    [SerializeField]
    private Animator m_WarlockAnimator = null;

    #region IAnimationScript
    public void SetMoving(bool i_bMoving)   { m_WarlockAnimator.SetBool("m_bMoving", i_bMoving); }
    public void SetCasting(bool i_bCasting) { m_WarlockAnimator.SetBool("m_bCasting", i_bCasting); }
    public void SetDead(bool i_bDead)   { m_WarlockAnimator.SetBool("m_bDead", i_bDead); }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
