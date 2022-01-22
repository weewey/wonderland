using System;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Authentication
{
    public class PlayFabAuthService
    {
        public delegate void LoginSuccessEvent();

        public static event LoginSuccessEvent OnLoginSuccess;

        public delegate void PlayFabErrorEvent(PlayFabError error);

        public static event PlayFabErrorEvent OnPlayFabError;

        public GetPlayerCombinedInfoRequestParams InfoRequestParams;

        private string _playFabId;

        public static PlayFabAuthService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayFabAuthService();
                }

                return _instance;
            }
        }

        private static PlayFabAuthService _instance;

        public PlayFabAuthService()
        {
            _instance = this;
        }

        public void Authenticate(string customId)
        {
            LoginWithCustomId(customId,
                result =>
                {
                    RequestPhotonToken(
                        AuthenticateWithPhoton,
                        ErrorLoginCallback(null));
                }
            );
        }

        private void RequestPhotonToken(
            Action<GetPhotonAuthenticationTokenResult> successCallback,
            Action<PlayFabError> errorCallback)
        {
            var getPhotonAuthenticationTokenRequest = new GetPhotonAuthenticationTokenRequest
            {
                PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
            };
            PlayFabClientAPI.GetPhotonAuthenticationToken(getPhotonAuthenticationTokenRequest,
                successCallback,
                errorCallback);
        }

        private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
        {
            Debug.Log("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");

            var customAuth = new AuthenticationValues {AuthType = CustomAuthenticationType.Custom};
            customAuth.AddAuthParameter("username", _playFabId);
            customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);
            PhotonNetwork.AuthValues = customAuth;
            OnLoginSuccess?.Invoke();
        }

        private Action<LoginResult> LoggedInCallback(Action<LoginResult> callback)
        {
            Action<LoginResult> resultCallback = (result) =>
            {
                _playFabId = result.PlayFabId;
                callback?.Invoke(null);
            };
            return resultCallback;
        }

        private Action<PlayFabError> ErrorLoginCallback(Action<LoginResult> callback)
        {
            Action<PlayFabError> errorCallback = (error) =>
            {
                callback?.Invoke(null);
                OnPlayFabError?.Invoke(error);

                Debug.LogError(error.GenerateErrorReport());
            };
            return errorCallback;
        }

        private void LoginWithCustomId(string customId, Action<LoginResult> callback)
        {
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
                {
                    TitleId = PlayFabSettings.TitleId,
                    CustomId = customId,
                    CreateAccount = true,
                    InfoRequestParameters = InfoRequestParams
                },
                LoggedInCallback(callback),
                ErrorLoginCallback(callback));
        }
    }
}