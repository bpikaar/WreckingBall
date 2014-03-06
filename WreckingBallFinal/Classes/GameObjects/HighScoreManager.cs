using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WreckingBallFinal.Classes.GameObjects
{
    class HighScoreManager : GameComponent
    {
        private  StorageContainer container;
        private string filename = "savegame.sav";

        public HighScoreManager(Game game) : base (game)
        {
            IAsyncResult resultTest = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice device = StorageDevice.EndShowSelector(resultTest);

            // Open a storage container.
            IAsyncResult result = device.BeginOpenContainer("WreckingBall", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();
        }

        public void SaveData(SaveGameData data)
        {
            // Check to see whether the save exists.
            if (container.FileExists(filename))
            {
                // Delete it so that we can create one fresh.
                container.DeleteFile(filename);
            }
            // Create the file.
            Stream stream = container.CreateFile(filename);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));

            serializer.Serialize(stream, data);

            stream.Close();
            container.Dispose();
        }

        public SaveGameData ReadData()
        {
            // Check to see whether the save exists.
            if (!container.FileExists(filename))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                return new SaveGameData() { Score = 0 };
            }

            // Open the file.
            Stream stream = container.OpenFile(filename, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData data = (SaveGameData)serializer.Deserialize(stream);
            stream.Close();
            container.Dispose();

            return data;
        }
    }

    public struct SaveGameData
    {
        //public string PlayerName;
        //public Vector2 AvatarPosition;
        //public int Level;
        public int Score;
    }
}
