using System;
using System.Threading.Tasks;
using Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class SocialIndexController : MonoBehaviour
    {
        [SerializeField] protected GameObject attributesPanel;
        [SerializeField] protected GameObject socialIndexObject;
        [SerializeField] protected RawImage attributesPanelImage;
        [SerializeField] protected Texture yewweeImage;
        [SerializeField] protected Texture darylImage;
        [SerializeField] protected Texture varickImage;
        [SerializeField] protected Texture zavierImage;

        private Text attributesTextComponent;
        private Text socialIndexTextComponent;
        private HttpClient httpClient;

        private void Awake()
        {
            Debug.Log("fetching social index");
            httpClient = new HttpClient(new JsonSerializationOption());
            attributesTextComponent = attributesPanel.GetComponent<Text>();
            socialIndexTextComponent = socialIndexObject.GetComponent<Text>();
            SetAttributesPanelImage();
            UpdateStats();
        }

        public async void UpdateStats()
        {
            CharacterMetadata characterMetadata = await GetCharacterMetadata();
            SetMetadataText(characterMetadata);
            SetSocialIndexText(characterMetadata);
        }
        
        public static PlayersInfo GetCharacterPlayerInfo(String walletAddress)
        {
            if (walletAddress != null && Constants.WalletToPlayersInfosMap.ContainsKey(walletAddress))
            {
                return Constants.WalletToPlayersInfosMap[walletAddress];
            }

            return Constants.WalletToPlayersInfosMap[Constants.YewWeeWallet];
        }

        private void SetAttributesPanelImage()
        {
            String player = GetCharacterPlayerInfo(PlayFabAuthService.Instance.GetWalletAddress()).Name;
            Debug.Log($"rendering player image name {player}");
            if (player.Equals(Constants.Daryl))
            {
                attributesPanelImage.texture = darylImage;
            }
            else if (player.Equals(Constants.Varick))
            {
                attributesPanelImage.texture = varickImage;
            }
            else if (player.Equals(Constants.Zavier))
            {
                attributesPanelImage.texture = zavierImage;
            }
            else
            {
                attributesPanelImage.texture = yewweeImage;
            }
        }

        private void SetSocialIndexText(CharacterMetadata characterMetadata)
        {
            socialIndexTextComponent.text = $"Social Index: {characterMetadata.social_index}";
        }

        private async Task<CharacterMetadata> GetCharacterMetadata()
        {
            string pubKey = GetCharacterPlayerInfo(PlayFabAuthService.Instance.GetWalletAddress()).CharAddr;
            return await httpClient.Get<CharacterMetadata>(String.Format(
                "https://underland-wonderland.herokuapp.com/token-metadata?pubKey={0}",
                pubKey));
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
    }
}