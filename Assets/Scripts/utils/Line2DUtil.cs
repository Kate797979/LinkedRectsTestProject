using UnityEngine;

namespace Assets.Scripts.utils
{
    static class Line2DUtil
    {

        public static Quaternion Get2DLineAngle(Vector3 startPosition, Vector3 endPosition)
        {
            var heading = endPosition - startPosition;
            var angle = Vector3.Angle(Vector3.right, heading) * Mathf.Sign(heading.y);
            
            return Quaternion.Euler(0, 0, angle);
        }

        public static float Get2DLineLength(Vector3 startPosition, Vector3 endPosition)
        {
            var heading = endPosition - startPosition;

            return heading.magnitude;
        }

        public static (float length, Quaternion angle) Get2DLineParams(Vector3 startPosition, Vector3 endPosition)
        {
            return (Get2DLineLength(startPosition, endPosition), Get2DLineAngle(startPosition, endPosition));
        }

    }
}
