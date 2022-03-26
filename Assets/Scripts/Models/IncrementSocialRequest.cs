using System;

[Serializable]
public class IncrementSocialRequest
{
    public string pubKey;
    public int incrementSocialBy;

    public IncrementSocialRequest(string pubKey, int incrementSocialBy)
    {
        this.pubKey = pubKey;
        this.incrementSocialBy = incrementSocialBy;
    }
}