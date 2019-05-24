using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public event System.Action OnScoreChange;

    [SerializeField]
    private int m_TotalScore;
    public int TotalScore
    {
        get
        {
            return m_TotalScore;
        }
    }

    [SerializeField]
    private int m_CurrentScore;
    public int CurrentScore
    {
        get
        {
            return m_CurrentScore;
        }
    }

    [SerializeField]
    private int m_Kills;
    public int Kills
    {
        get
        {
            return m_Kills;
        }
        set
        {
            m_Kills = value;
        }
    }

    [SerializeField]
    private int m_Deaths;
    public int Deaths
    {
        get
        {
            return m_Deaths;
        }
        set
        {
            m_Deaths = value;
        }
    }

    [SerializeField]
    private int m_Headshots;
    public int Headshots
    {
        get
        {
            return m_Headshots;
        }
        set
        {
            m_Headshots = value;
        }
    }

    public virtual void AddScore(int score)
    {
        m_TotalScore += score;
        m_CurrentScore += score;

        if(OnScoreChange != null)
            OnScoreChange();
    } 

    public virtual void UseScore(int amount)
    {
        m_CurrentScore -= amount;

        if(OnScoreChange != null)
            OnScoreChange();
    }

    public virtual double GetKD()
    {
        return (double) Kills / Deaths;
    }
}
