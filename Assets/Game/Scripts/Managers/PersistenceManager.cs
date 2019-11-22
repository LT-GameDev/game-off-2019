#pragma warning disable 649

using System;
using System.IO;
using Game.Models;
using UnityEngine;

namespace Game.Managers
{
    public interface IPersistenceManager<TGameData>
    {
        bool HasSave();
        
        void Load(Action<TGameData> onLoaded, Action onError);

        void Save(TGameData data);
    }
    
    public class PersistenceManager : IPersistenceManager<SaveData>, IDisposable
    {
        public event Action SaveBegin;
        public event Action SaveCompleted;
        
        private string SaveDirectory => Application.persistentDataPath + "/Saves/";
        private string SaveFileName => SaveDirectory + "savegame.cgs";

        public bool HasSave()
        {
            return File.Exists(SaveFileName);
        }
        
        public void Load(Action<SaveData> loaded, Action onError)
        {
            LoadAsync();
            
            
            async void LoadAsync()
            {
                try
                {
                    using (var str = new StreamReader(SaveFileName))
                    {
                        var content = await str.ReadToEndAsync();

                        var saveData = JsonUtility.FromJson<SaveData>(content);

                        loaded?.Invoke(saveData);
                    }
                }
                catch (Exception ex)
                {
                    // LOG?
                    onError?.Invoke();
                }
            }
        }

        public void Save(SaveData data)
        {
            SaveAsync();
            
            
            async void SaveAsync()
            {
                SaveBegin?.Invoke();
                
                try
                {
                    if (!HasSave())
                    {
                        if (!Directory.Exists(SaveDirectory))
                        {
                            Directory.CreateDirectory(SaveDirectory);
                        }
                        
                        File.Create(SaveFileName).Close();
                    }
                    
                    using (var stw = new StreamWriter(SaveFileName))
                    {
                        var json = JsonUtility.ToJson(data);
                        
                        await stw.WriteLineAsync(json);
                    }
                }
                catch (Exception ex)
                {
                    // LOG?
                }
                
                SaveCompleted?.Invoke();
            }
        }

        public void Dispose()
        {
            
        }
    }
}