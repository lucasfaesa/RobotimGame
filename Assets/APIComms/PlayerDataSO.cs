using System;
using API_Mestrado_Lucas;
using UnityEngine;

namespace APIComms
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerDataSO : ScriptableObject
    {
        [HideInInspector] public string playerSimpleLoginName;
        [HideInInspector] public string playerSimpleLoginLastName;
        
        #if UNITY_EDITOR
            [SerializeField] private bool useDebugPlayer;
        #endif

        [SerializeField] private bool guestMode;
        public bool GuestMode { get => guestMode; set => guestMode = value; }


        public void SetPlayerNameAndLastNameOnSimpleLogin(string name, string lastName)
        {
            playerSimpleLoginName = name;
            playerSimpleLoginLastName = lastName;
        }
        
        private StudentDTO _studentData;
        public StudentDTO StudentData
        {
            get
            {
                #if UNITY_EDITOR
                    if (useDebugPlayer)
                        return DebugPlayer();
                    else
                        return _studentData;
                #else
                    return _studentData;
                #endif
            }
        }

        public void SetStudentData(StudentDTO studentDto)
        {
            _studentData = studentDto;
        }

        private StudentDTO DebugPlayer()
        {
            return new StudentDTO
            {
                Id = 1,
                Name = "Lucas",
                TeacherId = 1,
                GroupClassId = 1,
                CreationDate = new DateTime( 2021,12,11,00,00,00),
                LastLoginDate = DateTime.Now
            };
        }

        private void OnEnable()
        {
            Reset();
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        private void OnDisable()
        {
            Reset();
        }

        public void Reset()
        {
            _studentData = null;
        }
    
    }
}
