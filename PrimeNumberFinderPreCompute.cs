using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NthPrimeNumber
{
    public class PrimeNumberFinderPreCompute : PrimeNumberFinderPrimeIterator
    {
        public void PreCompute(int limit)
        {
            base.FindNthPrime(limit);

            var current = new int[1000];
            var index = 0;
            for (var i = 0; i < Primes.Count; i++)
            {
                current[index++] = Primes[i];
                if ((i + 1) % 1000 == 0)
                {
                    index = 0;
                    var docName = $"primes_{FloorOneThousand(i)}";
                    Store(current, docName);
                    current = new int[1000];
                }
            }

            return;
        }
        
        private int FloorOneThousand(int num)
        {
            if (num < 1000)
            {
                return 0;
            }

            for (var i = 0; i < 3; i++)
            {
                num /= 10;
            }

            return (int)(num * Math.Pow(10, 3));
        }

        public override int FindNthPrime(int n)
        {
            n--;
            var container = GetContainer();
            var docName = $"primes_{FloorOneThousand(n)}";
            var blob = container.GetBlobReference(docName);
            using (var stream = blob.OpenRead())
            {
                var reader = new StreamReader(stream);
                var str = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<int[]>(str);
                return data[n % 1000];
            }
        }

        private void Store(int[] data, string name)
        {
            var container = GetContainer();

            var blockBlob = container.GetBlockBlobReference(name);

            var text = JsonConvert.SerializeObject(data);

            blockBlob.UploadText(text);
        }

        private CloudBlobContainer GetContainer()
        {
            var appSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("appsettings.json"));

            var connectionString = appSettings["ConnectionString"];
            var containerName = appSettings["ContainerName"];
            
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            
            var blobClient = storageAccount.CreateCloudBlobClient();
                       
            var container = blobClient.GetContainerReference(containerName);

            container.CreateIfNotExists();

            return container;
        }
    }
}
