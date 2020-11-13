using System;
using System.IO;
using System.Threading.Tasks;

namespace Needfulthings.WriteOnceReadMany
{
    class Program
    {        
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Runner();
        }

        void Runner()
        {
            WriteOnceReadMany writeOnceReadMany = new WriteOnceReadMany();
            Console.Write(writeOnceReadMany.GetFileContent().Result);
        }
    }

    public class WriteOnceReadMany
    {        
        private readonly string FullFilepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "WriteOnceReadMany", "FooMyself.txt");
        private Lazy<Task<string>> FileContent { get; } 

        public WriteOnceReadMany()
        {            
            FileContent = new Lazy<Task<string>>(InitializeStringContent);
        }

        public Task<string> GetFileContent()
        {
            return FileContent.Value;
        }

        private async Task<string> InitializeStringContent()
        {
            string retVal;

            if (File.Exists(FullFilepath))
            {
                retVal = await ReadFileContent();
            }
            else
            {             
                retVal = CreateFileContent();
                await StoreFileContent(retVal);
            }

            return retVal;
        }

        private string CreateFileContent()
        {            
            return $"Here the file content is coming from somewhere. {Guid.NewGuid()}"; 
        }

        private async Task StoreFileContent(string fileContent)
        {            
            if (!Directory.Exists(Path.GetDirectoryName(fileContent)))
            {                
                Directory.CreateDirectory(Path.GetDirectoryName(FullFilepath));
            }

            await File.WriteAllTextAsync(FullFilepath, fileContent);
        }

        private async Task<string> ReadFileContent()
        {               
            return await File.ReadAllTextAsync(FullFilepath);
        }
    }
}
