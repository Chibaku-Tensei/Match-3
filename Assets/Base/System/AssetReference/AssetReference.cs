using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

namespace Base.AssetReference
{
    public class AssetReference : Singleton<AssetReference>
    {
        [Header("Prefab Ref"), SerializeField] 
        private List<PrefabRefItem> prefabList = new List<PrefabRefItem>();
        
        [Header("Sound Ref"), SerializeField, Space]
        private List<SoundRefItem> soundList = new List<SoundRefItem>();
        
        #if UNITY_EDITOR
        [Button("Create Enum Type")]
        private void CreateEnumType()
        {
            string fileName = "AssetRefEnum";
            string prefabEnumName = "PrefabRefName";
            string soundEnumName = "SoundRefName";

            string[] prefabEnumEntries = new string[prefabList.Count];
            string[] soundEnumEntries = new string[soundList.Count];

            for (int i = 0; i < prefabList.Count; ++i)
            {
                prefabEnumEntries[i] = prefabList[i].nameOfPrefab;
            }

            for (int i = 0; i < soundList.Count; ++i)
            {
                soundEnumEntries[i] = soundList[i].nameOfSound;
            }

            string filePathAndName = "Assets/" + fileName + ".cs";

            using (StreamWriter writer = new StreamWriter(filePathAndName))
            {
                writer.WriteLine("public enum " + prefabEnumName);
                writer.WriteLine("{");
                for (int i = 0; i < prefabEnumEntries.Length; ++i)
                {
                    writer.WriteLine("\t" + prefabEnumEntries[i] + ", ");
                }
                writer.WriteLine("}");
                
                writer.WriteLine("public enum " + soundEnumName);
                writer.WriteLine("{");
                for (int i = 0; i < soundEnumEntries.Length; ++i)
                {
                    writer.WriteLine("\t" + soundEnumEntries[i] + ", ");
                }
                writer.WriteLine("}");
            }
            
            AssetDatabase.Refresh();
        }

        [Button("Delete Enum Type")]
        private void DeleteEnumType()
        {
            string fileName = "AssetRefEnum";
            string path = "Assets/" + fileName + ".cs";
            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
            }
            AssetDatabase.Refresh();
        }
        #endif
        
        
        public Transform GetPrefabAsset(Enum a)
        {
            var result = prefabList.Find(item => item.nameOfPrefab.Equals(a.ToString()));
            if (result.prefab)
            {
                return result.prefab;
            }

            return null;
        }

        public AudioClip GetSoundAsset(Enum a)
        {
            var result = soundList.Find(item => item.nameOfSound.Equals(a.ToString()));
            if (result.clip)
            {
                return result.clip;
            }
            return null;
        }
    }
}

