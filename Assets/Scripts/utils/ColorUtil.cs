using UnityEngine;

namespace Assets.Scripts.utils
{
    static class ColorUtil
    {

        public static Color GetRandomColor()
        {
            return new Color(Random.Range(0.0F, 1.0F), Random.Range(0.0F, 1.0F), Random.Range(0.0F, 1.0F));
        }
    }
}
