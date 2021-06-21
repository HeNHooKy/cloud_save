using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace Assets.Scripts
{
    public static class PlayfabCloudSaveProvider
    {
        public static void UpdateProcess(string playFabeId, Dictionary<string, string> data, Action<Dictionary<string, UserDataRecord>> updateLocalData)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = playFabeId,
                Keys = null
            }, result => {
                Debug.Log("Got user data");
                var cloudData = result.Data;
                var lastUpdate = LastUpdateProvider.Get();

                if (cloudData != null && cloudData.Any())
                {
                    if (lastUpdate == null)
                    {
                        updateLocalData(cloudData);
                        UpdateLastUpdatedData(playFabeId);
                        return;
                    }

                    if (!IsDataEqualsByDateTime(cloudData, lastUpdate))
                    {
                        Debug.Log("cloud and last update date are not equal");
                        updateLocalData(cloudData);
                        UpdateLastUpdatedData(playFabeId);
                        return;
                    }
                }

                UpdateCloudData(playFabeId, data);
            }, (error) => {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });
        }

        private static void UpdateCloudData(string playFabeId, Dictionary<string, string> playerData)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = playerData
            },
            result => {
                Debug.Log("Successfully updated user data");
                UpdateLastUpdatedData(playFabeId);
            },
            error => {
                Debug.Log("Got error setting user data Ancestor to Arthur");
                Debug.Log(error.GenerateErrorReport());
            });
        }

        private static void UpdateLastUpdatedData(string playFabeId)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = playFabeId,
                Keys = null
            }, result => {
                Debug.Log("Last update saved");
                LastUpdateProvider.Set(result.Data);

            }, (error) => {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });
        }

        private static bool IsDataEqualsByDateTime(Dictionary<string, UserDataRecord> d1, Dictionary<string, UserDataRecord> d2)
        {
            foreach (var one in d1)
            {
                var d1LastUpdated = one.Value.LastUpdated;
                var d2LastUpdated = d2[one.Key].LastUpdated;
                if ( !d1LastUpdated.Equals(d2LastUpdated) )
                {
                    return false;
                }
            }

            return true;
        }
    }
}
