using System;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Controllers
{
    public class SocialIndexController : MonoBehaviour
    {
        [SerializeField] protected GameObject attributesPanel;
        [SerializeField] protected GameObject socialIndexObject;
        private Text attributesTextComponent;
        private Text socialIndexTextComponent;
        private HttpClient httpClient;

        private async void Awake()
        {
            httpClient = new HttpClient(new JsonSerializationOption());
            attributesTextComponent = attributesPanel.GetComponent<Text>();
            socialIndexTextComponent = socialIndexObject.GetComponent<Text>();
            print(socialIndexTextComponent.text);
            CharacterMetadata characterMetadata = await GetCharacterMetadata();
            SetMetadataText(characterMetadata);
            SetSocialIndexText(characterMetadata);
        }

        private void SetSocialIndexText(CharacterMetadata characterMetadata)
        {
            socialIndexTextComponent.text = $"Social Index: {characterMetadata.social_index}";
        }

        private async Task<CharacterMetadata> GetCharacterMetadata()
        {
            string pubKey = "2s6iAvdspq3zCEzncayTbMn1x5xzFwYWe5To1ZacXU6w";

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