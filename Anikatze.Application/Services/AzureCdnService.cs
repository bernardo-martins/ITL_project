/* using System;
using System.Collections.Generic;
using Microsoft.Azure.Management.Cdn;
using Microsoft.Azure.Management.Cdn.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Identity.Client;
using Microsoft.Rest;

namespace Anikatze.Application.Services
{
    public class AzureCdnService
    {
        private readonly string _tenantId = "your-tenant-id";
        private readonly string _clientId = "your-client-id";
        private readonly string _clientSecret = "your-client-secret";
        private readonly string _subscriptionId = "your-subscription-id";
        private readonly string _resourceGroupName = "your-resource-group";
        private readonly string _cdnProfileName = "cdn-profile-name";
        private readonly string _cdnEndpointName = "cdn-endpoint-name";
        private readonly string _storageAccountName = "anikatzetest";

        private readonly string _storageAccountKey =
            "/Po134FhTBLqmVTX3cr8UT9v+6AopOOwoXxJPFlEcBcJp6z1cHMX6I+bI1vxkgApaPjWtDxzwC5N+AStvFAMKw==";

        private readonly string _containerName = "bitmovin";

        private IAzure Authenticate()
        {
            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(_clientId, _clientSecret, _tenantId, AzureEnvironment.AzureGlobalCloud);

            return Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .Authenticate(credentials)
                .WithSubscription(_subscriptionId);
        }

        public async Task CreateCdnEndpointAsync()
        {
            var azure = Authenticate();

            // Create CDN Profile
            var cdnProfile = await azure.CdnProfiles.Define(_cdnProfileName)
                .WithRegion(Region.USEast)
                .WithExistingResourceGroup(_resourceGroupName)
                .WithStandardAkamaiSku()
                .CreateAsync();

            Console.WriteLine($"CDN Profile '{_cdnProfileName}' created.");

            // Create CDN Endpoint
            var cdnEndpoint = await cdnProfile.Endpoints.Define(_cdnEndpointName)
                .WithOrigin(_storageAccountName + ".blob.core.windows.net", 80)
                .WithOriginHostHeader(_storageAccountName + ".blob.core.windows.net")
                .WithOriginPath("/" + _containerName)
                .WithStandardAkamaiCachingRules()
                .CreateAsync();

            Console.WriteLine($"CDN Endpoint '{_cdnEndpointName}' created.");

            // Retrieve CDN URL
            var cdnUrl = $"https://{_cdnEndpointName}.azureedge.net/{_containerName}/";
            Console.WriteLine($"CDN URL: {cdnUrl}");
        }
    }
    */