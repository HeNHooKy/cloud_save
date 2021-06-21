using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PlayFabAuthentication
    {
        private static readonly string PlayFabTitle = "66659";

        private static Action<Dictionary<string, UserDataRecord>> _setLocalData;
        private static Dictionary<string, string> _data;

        public static void Authentication(string customId, Dictionary<string, string> data, Action<Dictionary<string, UserDataRecord>> setLocalData)
        {
            _setLocalData = setLocalData;
            _data = data;

            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                /*
                Please change the titleId below to your own titleId from PlayFab Game Manager.
                If you have already set the value in the Editor Extensions, this can be skipped.
                */
                PlayFabSettings.staticSettings.TitleId = PlayFabTitle;
            }

            var request = new LoginWithCustomIDRequest { CustomId = customId, CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private static void OnLoginSuccess(LoginResult result)
        {
            PlayfabCloudSaveProvider.UpdateProcess(result.PlayFabId, _data, _setLocalData);
            Debug.Log("Data saved!");
        }

        private static void OnLoginFailure(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong:");
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}
