using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.Tools
{
    public class TodoNotes : MonoBehaviour
    {
        [SerializeField] private List<ToDoListObjects> todoListObjectsList = new();

        [Serializable]
        class ToDoListObjects
        {
            [TextArea(5, 10)] 
            public string note;
            public bool completed;
        }
    }
}
