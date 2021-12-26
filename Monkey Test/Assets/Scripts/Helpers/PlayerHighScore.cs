[System.Serializable]
public sealed class PlayerHighScore
{
    public int HighScore;

    public PlayerHighScore(Score score)
    {
        HighScore = score.score;
    }
}
