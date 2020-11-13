using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using Needfulthings.WriteOnceReadMany;

namespace needfulthings.tests
{
    [TestClass]
    public class WriteOnceReadManyTests
    {
        [TestMethod]
        public async Task WriteOnceReadMany_BasicRun()
        {
            string FullFilepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "WriteOnceReadMany", "FooMyself.txt");
            
            for (int i = 0; i < 10; i++)
            {
                WriteOnceReadMany sut = new WriteOnceReadMany();

                var task1 = sut.GetFileContent();
                var task2 = sut.GetFileContent();
                var task3 = sut.GetFileContent();
                var task4 = sut.GetFileContent();

                await Task.WhenAll(task1, task2, task3, task4);

                var result1 = await task1;
                var result2 = await task2;
                var result3 = await task3;
                var result4 = await task4;

                Assert.AreEqual(result1, result2);
                Assert.AreEqual(result1, result3);
                Assert.AreEqual(result1, result4);
                Assert.AreEqual(result2, result4);
                Assert.AreEqual(result3, result4);
            }

            if (Directory.Exists(Path.GetDirectoryName(FullFilepath)))
            {
                Directory.Delete(Path.GetDirectoryName(FullFilepath), true);
            }
        }
    }
}
