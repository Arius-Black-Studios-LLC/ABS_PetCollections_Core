using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine.UI;
using Unity.VisualScripting;


namespace ABS_SaveLoadSystem
{
    public class AuthenticationManager : MonoBehaviour
    {


        public event Action<PlayerInfo, string> OnSignedIn;
        public PlayerInfo playerInfo;
        public Slider loadingScreenPrefab;



        //Called When Login Button Is pressed in start up screen
        //NOTE: Game Seems to freeze once player data is collected
        public async void LoginButtonPressed()
        {
            await InitSignIn();
        }

        async void Awake()
        {

            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            PlayerAccountService.Instance.SignedIn += SignedIn;
        }

        private async void SignedIn()
        {
            try
            {
                var accessToken = PlayerAccountService.Instance.AccessToken;
                Debug.Log("Access Token: " + accessToken);
                await SignInWithUnityAsync(accessToken);

                Debug.Log("Finished Sign in async.");
            }
            catch (Exception e)
            {

            }
        }

        public async Task InitSignIn()
        {
            try
            {
                await PlayerAccountService.Instance.StartSignInAsync();
            }
            catch ( Exception e)
            {
                Console.WriteLine(e);
            }
        }

        async Task SignInWithUnityAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
                Debug.Log("SignIn is successful.");

                playerInfo = AuthenticationService.Instance.PlayerInfo;
                var name = await AuthenticationService.Instance.GetPlayerNameAsync();

                if (OnSignedIn != null && playerInfo != null && !string.IsNullOrEmpty(name))
                {
                    OnSignedIn?.Invoke(playerInfo, name);
                }
                else
                {
                    Debug.LogError("Failed to invoke OnSignedIn: Missing required data.");
                }


                //TODO MOVE TO OTHR FUNCTION
                Debug.Log("Loading Player Data");
                await PlayerManager.saveLoadManager.LoadAllPlayerDataAsync();
                Debug.Log("Loading Scene...");
                var sceneLoadOperation = SceneManager.LoadSceneAsync("MainScene");
                while (!sceneLoadOperation.isDone)
                {
                    Debug.Log($"Loading Progress: {sceneLoadOperation.progress * 100}%");
                    loadingScreenPrefab.value = sceneLoadOperation.progress * 100;
                    await Task.Yield(); // Ensure the main thread remains responsive
                }



                Debug.Log("Scene Loaded Successfully.");
                // Start the energy refill process as soon as the game begins
    
            }
            catch (AuthenticationException ex)
            {
                Debug.LogError($"Authentication failed: {ex.Message}");
                // Show user feedback, e.g., "Login failed. Please try again."
            }
            catch (RequestFailedException ex)
            {
                Debug.LogError($"Request failed: {ex.Message}");
                // Handle API-specific issues
            }
            catch (Exception e)
            {
                Debug.LogError($"Unexpected error: {e.Message}");
                // Handle unexpected errors
            }

        }

    }
}