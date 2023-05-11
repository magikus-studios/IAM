using System;
using System.Collections.Generic;
using UnityEngine;

namespace IAM
{
    public static class ExtensionsMthods
    {
        public static Vector2Int ToVector2Int(this Vector3 vector) { return new Vector2Int((int)vector.x, (int)vector.y); }
        public static Vector3Int ToVector3Int(this Vector3 vector) { return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z); }
        public static Vector3Int ToVector3Int(this Vector2Int vector) { return new Vector3Int(vector.x, vector.y, 0); }

        public static int Wrap(this int input, int min, int max)
        {
            if (input < min) { return max - (min - input) % (max - min); }
            else { return min + (input - min) % (max - min); }
        }

        public static int RandomIndex<T>(this T[] array) { return UnityEngine.Random.Range(0, array.Length); }
        public static int RandomIndex<T>(this IList<T> list) { return UnityEngine.Random.Range(0, list.Count); }
        public static T RandomItem<T>(this T[] array) { return array[array.RandomIndex()]; }
        public static T RandomItem<T>(this IList<T> list) { return list[list.RandomIndex()]; }
        public static int RandomIndex<T>(this T[] array, params int[] indexesToIgnore) 
        {
            if (indexesToIgnore.Length == array.Length) { return 0; }

            int index = UnityEngine.Random.Range(0, array.Length - indexesToIgnore.Length);

            foreach (int ignoredIndex in indexesToIgnore) 
            {
                if (index >= ignoredIndex) { index++; }
                if (index >= array.Length) { index = 0; }
            }

            return index; 
        }
        public static int RandomIndex<T>(this IList<T> list, params int[] indexesToIgnore) 
        {
            if (indexesToIgnore.Length == list.Count) { return 0; }

            int index = UnityEngine.Random.Range(0, list.Count - indexesToIgnore.Length);

            foreach (int ignoredIndex in indexesToIgnore)
            {
                if (index >= ignoredIndex) { index++; }
                if (index >= list.Count) { index = 0; }
            }

            return index;
        }
    }
}