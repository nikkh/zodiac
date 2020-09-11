using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zodiac.Generator.UI
{
    class UIControllerUtil
    {
        private readonly UIParameters parameters;
        private readonly ZodiacContext _zodiacContext;
        private static CloudStorageAccount cloudStorageAccount;

        internal UIControllerUtil(ZodiacContext zodiacContext)
        {
            _zodiacContext = zodiacContext;
            if (cloudStorageAccount == null) cloudStorageAccount = CloudStorageAccount.Parse(_zodiacContext.UserTestingParametersStorageConnectionString);
        }

        internal async Task<UIParameters> GetParameters(ILogger log)
        {
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("zodiac-generator");
            var dllBlob = container.GetBlockBlobReference("GeneratorParameters.json");
            var parametersAsString = await dllBlob.DownloadTextAsync();
            var parameters = JsonConvert.DeserializeObject<UIParameters>(parametersAsString);
            return parameters;
        }

        internal bool UserSimnulationEnabled(ILogger log = null)
        {
            if (_zodiacContext.UserSimulationEnabled)
            {
                if (log != null) log.LogInformation($"This function will simulate user traffic if specified in configuration.  Current state is enabled");
                return true;
            }
            else
            {
                if (log != null) log.LogWarning($"This function will simulate user traffic if specified in configuration.  Current state is disabled");
                return false;
            }
        }

    }
}
