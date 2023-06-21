using UnityEngine;
namespace Extension
{
    public static class VectorExtension 
    {
        public static Vector2Int Vector3ToVectro2Int(this Vector3 vector)
            => new Vector2Int((int)vector.x, (int)vector.y);
        public static Vector3 Vector2IntToVectro3(this Vector2Int vector)
            => new Vector3(vector.x, vector.y, 0);
    }
}