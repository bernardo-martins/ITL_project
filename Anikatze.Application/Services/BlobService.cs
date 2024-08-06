using Azure.Storage.Blobs;
using Azure.Storage;
using Azure.Storage.Sas;
using System;
using System.Security.Cryptography;
using System.Text;
namespace Anikatze.Application.Services;


public class BlobService
{
    private readonly string _storageAccountName = "anikatzetest";
    private readonly string _storageAccountKey = "/Po134FhTBLqmVTX3cr8UT9v+6AopOOwoXxJPFlEcBcJp6z1cHMX6I+bI1vxkgApaPjWtDxzwC5N+AStvFAMKw==";

    public string GenerateSasToken(string containerName)
    {
        var blobServiceClient = new BlobServiceClient(new Uri($"https://anikatzetest.blob.core.windows.net"), new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey));
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            Resource = "c", // c for container
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(60) // Token gültig für 5 Minuten
        };
        sasBuilder.SetPermissions(BlobContainerSasPermissions.Read | BlobContainerSasPermissions.List);

        var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey)).ToString();
        return $"{blobContainerClient.Uri}?{sasToken}";
    }


public string GenerateSecurityToken(string userId)
    {
        var key = Encoding.UTF8.GetBytes("anikatze");
        var message = Encoding.UTF8.GetBytes(userId + DateTime.UtcNow.ToString("yyyyMMddHHmm"));
        using (var hmacsha256 = new HMACSHA256(key))
        {
            var hashMessage = hmacsha256.ComputeHash(message);
            return Convert.ToBase64String(hashMessage);
        }
    }

    private bool IsUserAuthenticated(string userId)
    {
        return true;
    }
}
