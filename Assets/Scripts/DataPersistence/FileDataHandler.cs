using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler {
    private string dataDirParth = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "Fuhai";
    public FileDataHandler(string dataDirParth, string dataFileName, bool useEncryption){
        this.dataDirParth=dataDirParth;
        this.dataFileName=dataFileName;
        this.useEncryption=useEncryption;
    }
    public void Save(GameData data, string profileId){
        if(profileId == null)
            return;
        string fullpath = Path.Combine(dataDirParth,profileId,dataFileName);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
            string dataToStoreJson = JsonUtility.ToJson(data,true);
            if(useEncryption){
                dataToStoreJson = EncryptDecrypt(dataToStoreJson);
            }
            using (FileStream stream = new FileStream(fullpath,FileMode.Create)){
                using (StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStoreJson);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Erro ao tentar salvar para o arquivo: "+fullpath+"\n"+ e);
        }
    }
    public GameData Load(string profileId){
        if(profileId ==null){
            return null;
        }
        string fullpath = Path.Combine(dataDirParth,profileId,dataFileName);
        GameData loadedData=null;
        if(File.Exists(fullpath)){
            try{
                string dataToLoadString ="";
                using(FileStream stream = new FileStream(fullpath,FileMode.Open)){
                    using(StreamReader reader = new StreamReader(stream)){
                        dataToLoadString = reader.ReadToEnd();
                        //Debug.Log(dataToLoad);
                    }
                }
                if(useEncryption){
                    dataToLoadString = EncryptDecrypt(dataToLoadString);
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoadString);
            }
            catch(Exception e){
                Debug.LogError("Erro ocorreu ao tentar carregar do arquivo: "+fullpath+"\n"+ e);
            }
        }
        return loadedData;
    }

    private string EncryptDecrypt(string data){
        string modifiedData = "";
        for(int i=0;i<data.Length;i++){
            modifiedData += (char) (data[i]^encryptionCodeWord[i%encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
    public Dictionary<string,GameData> LoadAllProfiles(){
        Dictionary<string,GameData> profileDictionary = new Dictionary<string, GameData>();

        //loop over all the directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirParth).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos){
            string profileId = dirInfo.Name;
            //defensive programing - check if the data file exists
            //if it doesn't then this folder should be skipped
            string fullpath = Path.Combine(dataDirParth,profileId,dataFileName);
            if(!File.Exists(fullpath)){
                Debug.LogWarning("Skipping directory when loading because it does not contain data: " + profileId);
                continue;
            }
            GameData profileData = Load(profileId);
            //defensive programming - check if the data isn't null, because if it is something went wrong
            if(profileData==null){
                Debug.LogError("Tried to load profile data but something went wrong. ProfileId: "+ profileId);
            }
            else{
                profileDictionary.Add(profileId,profileData);
            }
        }
        return profileDictionary;
    }
    public string GetMostRecentProfileId(){
        string mostRecentProfileId = null;
        Dictionary<string,GameData> profilesGameData = LoadAllProfiles();
        foreach(KeyValuePair<string,GameData> pair in profilesGameData){
            string profileId = pair.Key;
            GameData gameData = pair.Value;
            //não deveria ser null, mas vai que né
            if(gameData==null){
                continue;
            }
            if(mostRecentProfileId == null){
                mostRecentProfileId = profileId;
            }
            else{
                DateTime mostRecentGameTime = DateTime.FromBinary( profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                if(newDateTime> mostRecentGameTime){
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }
    public void Delete(string profileId){
        if(profileId == null){
            return;
        }
        string fullpath = Path.Combine(dataDirParth,profileId,dataFileName);
        try{
            if(File.Exists(fullpath)){
                Directory.Delete(Path.GetDirectoryName(fullpath),true);
            }
            else{
                Debug.LogWarning("Tried to delete a data file but there was no file. Fileid: "+ profileId);
            }
        }
        catch(Exception e){
            Debug.LogError("Failed while trying to delete profile "+ profileId+ " at path "+ fullpath +"\n"+e);
        }
    }

}