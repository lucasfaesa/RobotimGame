using System;
using System.Collections;
using System.Text;
using _Lobby._LevelSelector.Scripts;
using API_Mestrado_Lucas;
using APIComms;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

namespace _Game_Scenes.MainMenu.Scripts
{
    public class SimpleLoginController : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private ApiCommsControllerSO apiCommsController;
        [SerializeField] private PlayerDataSO playerDataSO;
        [SerializeField] private SceneLoaderControllerSO sceneLoaderController;
        [Space]
        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField lastName;
        [Space]
        [SerializeField] private UnityEvent usernameOrPasswordEmpty;
        [SerializeField] private UnityEvent tryingToLogIn;
        [SerializeField] private UnityEvent loginError;
        [SerializeField] private UnityEvent loginSuccess;
        [SerializeField] private UnityEvent groupClassError;

        private Coroutine loginRoutine;
        private bool logginIn;

        private int textFieldIndex = 0;
        
        private void Start()
        {
            if (!string.IsNullOrEmpty(playerDataSO.playerSimpleLoginName))
            {
                username.text = playerDataSO.playerSimpleLoginName;
                lastName.text = playerDataSO.playerSimpleLoginLastName;
            }
        }

        public void Login()
        {
            if (string.IsNullOrEmpty(username.text) || string.IsNullOrWhiteSpace(username.text))
            {
                usernameOrPasswordEmpty?.Invoke();
                return;
            }
        
            loginRoutine =  StartCoroutine(StudentLogin(false));
        }
        
        public void GuestLogin()
        {
            loginRoutine =  StartCoroutine(StudentLogin(true));
        }

        private IEnumerator StudentLogin(bool guest)
        {
            tryingToLogIn?.Invoke();
            logginIn = true;

            var studentData = new StudentLoginDTO();
            
            if (guest)
            {
                studentData.Username = "convidado";
                studentData.Password = "convidado";
            }
            else
            {
                studentData.Username = username.text.Trim() + " " + lastName.text.Trim();
                studentData.Username = studentData.Username.Trim();
                studentData.Password = "placeholderPassword";
                
                playerDataSO.SetPlayerNameAndLastNameOnSimpleLogin(username.text, lastName.text);
            }

            using (UnityWebRequest www = UnityWebRequest.Post(ApiPaths.STUDENT_TEMP_LOGIN(apiCommsController.UseCloudPath), "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(studentData));
                www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log(www.error);
                    loginError?.Invoke();
                    username.Select();
                    username.ActivateInputField();
                    logginIn = false;
                }
                else
                {
                    playerDataSO.SetStudentData(JsonConvert.DeserializeObject<StudentDTO>(www.downloadHandler.text));

                    if (playerDataSO.StudentData.GroupClassId == null)
                    {
                        groupClassError?.Invoke();
                        Debug.Log("Group class error");
                        logginIn = false;
                        username.Select();
                        username.ActivateInputField();
                        www.Dispose();
                        yield break;
                    }
                    Debug.Log("Api Comms Success");
                    loginSuccess?.Invoke();
                }
            
                www.Dispose();
            }
        }

        private void Update()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame && !logginIn)
            {
                username.Select();
                username.ActivateInputField();
                Login();
            }

            if (Keyboard.current.tabKey.wasPressedThisFrame && !logginIn)
            {
                textFieldIndex++;

                if (textFieldIndex > 1)
                    textFieldIndex = 0;
                
                switch (textFieldIndex)
                {
                    case 0:
                        username.Select();
                        username.ActivateInputField();
                        break;
                    case 1:
                        lastName.Select();
                        lastName.ActivateInputField();
                        break;
                }
                
            }
        }

        private void OnApplicationQuit()
        {
            if(loginRoutine != null)
                StopCoroutine(loginRoutine);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    
    }
}
