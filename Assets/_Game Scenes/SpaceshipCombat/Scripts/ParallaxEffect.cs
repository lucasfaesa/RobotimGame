using UnityEngine;

namespace _Game_Scenes.SpaceshipCombat.Scripts
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float maxPosX;
        [SerializeField] private float speed;
    
        private float initialPosX;
        private float defaultSpeed;
    
        void Start()
        {
            defaultSpeed = speed;
            initialPosX = this.transform.position.x;
        }

        public void SlowDown()
        {
            speed = defaultSpeed / 10;
        }

        public void NormalSpeed()
        {
            speed = defaultSpeed;
        }

        void Update()
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            if (this.transform.position.x >= maxPosX)
            {
                var transform1 = this.transform;
                var position = transform1.position;
                position = new Vector3(initialPosX, position.y, position.z);
                transform1.position = position;
            }
        }
    }
}
