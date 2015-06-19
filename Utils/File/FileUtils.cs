using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;

namespace DMSoftware.File
{
    public class FileUtils
    {
        public async void WriteObject<T>(T t, string fileName, bool encrypt)
        {
            var json = JsonConvert.SerializeObject(t);
            await PclStorageSample("", fileName, json);
        }

        public async Task PclStorageSample(string folderName, string fileName, string data)
        {
            
            var rootFolder = FileSystem.Current.LocalStorage;
            var folder = await rootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(data);
        }
    }
}
