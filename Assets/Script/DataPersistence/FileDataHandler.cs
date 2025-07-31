using UnityEngine;
using System.IO;
using System;

namespace DataPersistance
{
    /// <summary>
    /// Handles reading and writing game data to and from a file.
    /// Supports optional encryption for basic data obfuscation.
    /// </summary>
    public class FileDataHandler
    {
        private readonly string dataDirPath = "";
        private readonly string dataFileName = "";

        private readonly bool useEncryption = false;
        private readonly string encryptionCodeWord = "word";

        /// <summary>
        /// Constructor for FileDataHandler.
        /// </summary>
        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncrypt)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
            this.useEncryption = useEncrypt;
        }

        /// <summary>
        /// Loads game data from file.
        /// If encryption is enabled, decrypts the content before parsing.
        /// </summary>
        /// <returns>GameData object if load is successful; null otherwise.</returns>
        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new(fullPath, FileMode.Open))
                    {
                        using StreamReader reader = new(stream);
                        dataToLoad = reader.ReadToEnd();
                    }

                    if (useEncryption)
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error occured when trying to load data to file: " + fullPath + "\n" + ex);
                }
            }
            return loadedData;
        }

        /// <summary>
        /// Saves the provided GameData object to a file.
        /// If encryption is enabled, encrypts the content before writing.
        /// </summary>
        /// <param name="data">The GameData object to save.</param>
        public void Save(GameData data)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            try
            {
                Debug.Log("Created SaveData at :" + fullPath);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(data, true);

                if (useEncryption)
                    dataToStore = EncryptDecrypt(dataToStore);

                using FileStream stream = new(fullPath, FileMode.Create);
                using StreamWriter writer = new(stream);
                writer.Write(dataToStore);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + ex);
            }
        }

        /// <summary>
        /// Performs simple XOR encryption/decryption.
        /// </summary>
        /// <param name="data">The data string to encrypt or decrypt.</param>
        /// <returns>The encrypted or decrypted string.</returns>
        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            return modifiedData;
        }

    }

}