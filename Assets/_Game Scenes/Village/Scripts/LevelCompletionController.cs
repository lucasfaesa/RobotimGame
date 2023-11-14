using UnityEngine;

namespace _Village.Scripts
{
    public class LevelCompletionController : MonoBehaviour
    {
        [SerializeField] private LevelCompletionSO levelCompletion;
    
        // Start is called before the first frame update
        void Start()
        {
            levelCompletion.Reset();
        }

    }
}
