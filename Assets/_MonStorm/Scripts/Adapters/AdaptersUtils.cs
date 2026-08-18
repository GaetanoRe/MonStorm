using UnityEngine;

namespace MonStorm.Adapters
{
    public class AdaptersUtils
    {
        public static Vector3 NumericsToUnityVector3(System.Numerics.Vector3 numericsVector) => new(numericsVector.X, numericsVector.Y, numericsVector.Z);
        public static System.Numerics.Vector3 UnityToNumericsVector3(Vector3 vector) => new(vector.x, vector.y, vector.z);

        public static Vector3 AddUnityWithNumericsVector3(Vector3 vector, System.Numerics.Vector3 numericsVector)
        {
            vector.x += numericsVector.X;
            vector.y += numericsVector.Y;
            vector.z += numericsVector.Z;
            return vector;
        }
    }
}
