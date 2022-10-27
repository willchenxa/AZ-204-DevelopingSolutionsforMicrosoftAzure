using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;

public class Program
{
 private const string blobServiceEndpoint = "https://mediastorwilliam.blob.core.windows.net/";
 private const string storageAccountName = "mediastorwilliam";
 private const string storageAccountKey = "JXzSzuOE3uXqAG4diMbV4y9KKgmhjlGO/KnVQ/iStdk5c93NY3poUl2f9PA8DBnMAF9+wbneOGjT+ASt5HisEg==";

 public static async Task Main(string[] args)
 {
  StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

  BlobServiceClient serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);

  AccountInfo info = await serviceClient.GetAccountInfoAsync();

  await Console.Out.WriteLineAsync($"Connected to Azure Storage Account");
  await Console.Out.WriteLineAsync($"Account name:\t{storageAccountName}");
  await Console.Out.WriteLineAsync($"Account kind:\t{info?.AccountKind}");
  await Console.Out.WriteLineAsync($"Account sku:\t{info?.SkuName}");

  await EnumerateContainersAsync(serviceClient);

  string existingContainerName = "raster-graphics";
  await EnumerateBlobsAsync(serviceClient, existingContainerName);
  await EnumerateBlobsAsync(serviceClient, "vector-graphics");

  string newContainerName = "vector-graphics";
  BlobContainerClient containerClient = await GetContainerAsync(serviceClient, newContainerName);

  string uploadedBlobName = "graph.svg";
  BlobClient blobClient = await GetBlobAsync(containerClient, uploadedBlobName);

  await Console.Out.WriteLineAsync($"Blob Url:\t{blobClient.Uri}");
 }

 private static async Task EnumerateContainersAsync(BlobServiceClient client)
 {
  await foreach (BlobContainerItem container in client.GetBlobContainersAsync())
  {
   await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
  }
 }

 private static async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
 {
  BlobContainerClient container = client.GetBlobContainerClient(containerName);

  await Console.Out.WriteLineAsync($"Searching:\t{container.Name}");

  await foreach (BlobItem blob in container.GetBlobsAsync())
  {
   await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
  }
 }

 private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
 {
  BlobContainerClient container = client.GetBlobContainerClient(containerName);

  await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

  await Console.Out.WriteLineAsync($"New Container:\t{container.Name}");

  return container;
 }

 private static async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
 {
  //this line just create blob client, you can either upload or download, doesn't mean blob exists in the storage account
  BlobClient blob = client.GetBlobClient(blobName);
  if (await blob.ExistsAsync())
  {
   await Console.Out.WriteLineAsync($"Blob Found:\t{blob.Name}");
  }
  else
  {
   await Console.Out.WriteLineAsync($"Blob Not Found:\t{blob.Name}");
  }
  return blob;
 }
}
