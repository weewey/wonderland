using System;
using System.Threading.Tasks;
using Authentication;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public GameObject StatsPanel;
    public GameObject StatsButton;
    [SerializeField] protected GameObject attributesPanel;
    [SerializeField] protected GameObject socialIndexObject;

    private Text attributesTextComponent;
    private Text socialIndexTextComponent;
    private HttpClient httpClient;

    public void Awake()
    {
        httpClient = new HttpClient(new JsonSerializationOption());
        attributesTextComponent = attributesPanel.GetComponent<Text>();
        socialIndexTextComponent = socialIndexObject.GetComponent<Text>();
    }
    public void OpenPanel()
    {
        if (StatsPanel != null)
        {
            StatsPanel.SetActive(true);
            UpdateStats();
            if (StatsButton != null)
            {
                StatsButton.SetActive(false);
            }
        }
    }

    public void ClosePanel()
    {
        if (StatsPanel != null)
        {
            StatsPanel.SetActive(false);
            if (StatsButton != null)
            {
                StatsButton.SetActive(true);
            }
        }
    }
    
    
    public async void UpdateStats()
    {
        CharacterMetadata characterMetadata = await GetCharacterMetadata();
        SetMetadataText(characterMetadata);
        SetSocialIndexText(characterMetadata);
    }
    
    private async Task<CharacterMetadata> GetCharacterMetadata()
    {
        string pubKey = GetCharacterPlayerInfo(PlayFabAuthService.Instance.GetWalletAddress()).CharAddr;
        return await httpClient.Get<CharacterMetadata>(String.Format(
            "https://underland-wonderland.herokuapp.com/token-metadata?pubKey={0}",
            pubKey));
    }
    
    public PlayersInfo GetCharacterPlayerInfo(String walletAddress)
    {
        if (walletAddress != null && Constants.WalletToPlayersInfosMap.ContainsKey(walletAddress))
        {
            return Constants.WalletToPlayersInfosMap[walletAddress];
        }

        return Constants.WalletToPlayersInfosMap[Constants.YewWeeWallet];
    }

    private void SetMetadataText(CharacterMetadata metadata)
    {
        var attributeText = "";
        foreach (var attribute in metadata.attributes)
        {
            attributeText += $"{attribute.trait_type}: {attribute.value} \n";
        }

        attributesTextComponent.text = attributeText;
    }
    
    private void SetSocialIndexText(CharacterMetadata characterMetadata)
    {
        socialIndexTextComponent.text = $"Social Index: {characterMetadata.social_index}";
    }
}