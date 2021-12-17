using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class PlayerHighScore
{
    public int HighScore;

    public PlayerHighScore(Score score)
    {
        HighScore = score.score;
    }
}
