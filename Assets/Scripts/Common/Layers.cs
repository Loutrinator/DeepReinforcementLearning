using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Common {
    [InitializeOnLoad]
    public static class Layers {
        private static readonly Dictionary<string, int> LayersInt;

        static Layers() {
            LayersInt = new Dictionary<string, int>();
            for (int i = 0; i <= 31; ++i) {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName)) {
                    LayersInt[layerName] = i;
                }
            }
        }

        public static int IntValue(string value) {
            return LayersInt[value];
        }
    }
}