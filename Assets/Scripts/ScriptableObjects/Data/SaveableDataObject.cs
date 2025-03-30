using System;
using System.IO;
using UnityEngine;

namespace ScriptableObjects.Data
{
    public abstract class SaveableDataObject<T> : ScriptableObject where T : new()
    {
        private const string SaveFilePathExtension = @"Saves\TowerDefense\";
        
        [SerializeField] protected string saveFileName;
        [SerializeField] protected T dataObject;
        [SerializeField] bool _isLoaded = false;

        public T DataObject
        {
            get
            {
                if (!_isLoaded)
                {
                    return Load();
                }
                
                return dataObject;
            }
        }

        /// <summary>
        /// Save the file and optionally if the directory doesnt exist already create a savefile for it
        /// </summary>
        public virtual void Save()
        {
            string saveFileDirectoryPath = Path.Combine(Application.persistentDataPath,SaveFilePathExtension);
            string saveFilePath = Path.Combine(saveFileDirectoryPath, $"{saveFileName}.json");
            string jsonContent = JsonUtility.ToJson(dataObject);
           
            //If the file exists just write the json content to it
            if (!Directory.Exists(saveFileDirectoryPath))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(saveFileDirectoryPath);
            }
            
            if (File.Exists(saveFilePath))
            {
                 File.WriteAllText(saveFilePath, jsonContent);
            }
            //If not then just create a new file 
            else
            {
                using (FileStream newlyCreatedFileFileStream = File.Create(saveFilePath))
                {
                    File.WriteAllText(saveFilePath,jsonContent);
                    newlyCreatedFileFileStream.Close();
                }
            }
            
        }
        
        protected virtual T Load()
        {
            string saveFileDirectoryPath = Path.Combine(Application.persistentDataPath,SaveFilePathExtension);
            string saveFilePath = Path.Combine(saveFileDirectoryPath, $"{saveFileName}.json");
            
            if (!Directory.Exists(saveFileDirectoryPath)
                || !File.Exists(saveFilePath))
            {
                //If the file or directory doesnt exist just create either of them by calling save
                dataObject = new T();
                _isLoaded = true;
                Save();
                
                return dataObject;
            }

            string fileContents = File.ReadAllText(saveFilePath);
            
            dataObject = JsonUtility.FromJson<T>(fileContents);
            _isLoaded = true;
            
            return dataObject;
        }
    }
}