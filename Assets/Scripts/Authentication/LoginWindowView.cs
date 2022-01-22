using System.Threading.Tasks;
using AllArt.Solana;
using AllArt.Solana.Utility;
using PlayFab;
using PlayFab.ClientModels;
using Solnet.Rpc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Authentication
{
    public class LoginWindowView : MonoBehaviour
    {
        private Label _loginStatusLabel;

        private TextField _walletPublicKeyInput;

        public Button JoinGameButton;

        private SolanaRpcClient _rpcClient;

        private readonly PlayFabAuthService _authService = PlayFabAuthService.Instance;

        private void OnEnable()
        {
            var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
            _loginStatusLabel = rootVisualElement.Q<Label>("login-status-label");
            JoinGameButton = rootVisualElement.Q<Button>("join-game-btn");
            _walletPublicKeyInput = rootVisualElement.Q<TextField>("wallet-public-key-input");
            JoinGameButton.RegisterCallback<ClickEvent>(OnJoinButtonClickedCallback);
        }

        private async void OnJoinButtonClickedCallback(ClickEvent ev)
        {
            if (_walletPublicKeyInput.value != "" && _rpcClient != null && await WalletExists())
            {
                LoginWithPublicKey();
                return;
            }

            _loginStatusLabel.visible = true;
            _loginStatusLabel.text = "No wallet found";
            _loginStatusLabel.style.color = new StyleColor(Color.red);
        }

        private void LoginWithPublicKey()
        {
            _authService.Authenticate(_walletPublicKeyInput.value);
        }

        private async Task<bool> WalletExists()
        {
            var accountInfo = await AccountUtility.GetAccountData(_walletPublicKeyInput.value, _rpcClient);
            return accountInfo != null;
        }

        void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            _rpcClient = new SolanaRpcClient(WalletBaseComponent.devNetAdress);
            PlayFabAuthService.OnLoginSuccess += LoadNextScene;
            PlayFabAuthService.OnPlayFabError += OnPlayFabError;
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene(Constants.LobbyScene);
        }

        private void OnPlayFabError(PlayFabError error)
        {
            switch (error.Error)
            {
                case PlayFabErrorCode.InvalidEmailAddress:
                case PlayFabErrorCode.InvalidPassword:
                case PlayFabErrorCode.InvalidEmailOrPassword:
                    _loginStatusLabel.text = "Invalid Email or Password";
                    break;
                case PlayFabErrorCode.AccountNotFound:
                    _loginStatusLabel.text = "Account not found";
                    break;
                default:
                    _loginStatusLabel.text = error.GenerateErrorReport();
                    break;
            }

            Debug.Log(error.Error);
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}