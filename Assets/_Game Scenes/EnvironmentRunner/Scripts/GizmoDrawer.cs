using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class GizmoDrawer : MonoBehaviour
    {
        public Color gizmoColor = Color.yellow;
        public float gizmoRadius = 1f;

        // Draw Gizmos in both the scene view and the game view.
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);
        }

        // Draw Gizmos only when the GameObject is selected in the scene view.
        // Uncomment the method below and comment the OnDrawGizmos method to use it.
        /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
    */
    }
}

