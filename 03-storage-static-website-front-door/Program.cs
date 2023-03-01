using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.AzureNative.Storage;
using System.Collections.Generic;

return await Pulumi.Deployment.RunAsync(() =>
{
    var resourceGroup = new ResourceGroup("resourceGroup");
    
    var locations = new List<string>()
    {
        "uksouth",
        "westus2",
        "southeastasia"
    };

    foreach(var location in locations)
    {
        var storageAccount = new StorageAccount($"svsa{location.Substring(0,5)}", new StorageAccountArgs
        {
            Location = location,
            ResourceGroupName = resourceGroup.Name,
            Sku = new SkuArgs
            {
                Name = SkuName.Standard_LRS
            },
            Kind = Kind.StorageV2
        });

        var staticWebsite = new StorageAccountStaticWebsite($"staticWebsite-{location}", new StorageAccountStaticWebsiteArgs
        {
            AccountName = storageAccount.Name,
            ResourceGroupName = resourceGroup.Name,
            IndexDocument = "index.html",
            Error404Document = "404.html",
        });

        var index_html = new Blob($"index-{location}.html", new BlobArgs
        {
            BlobName = "index.html",
            ResourceGroupName = resourceGroup.Name,
            AccountName = storageAccount.Name,
            ContainerName = staticWebsite.ContainerName,
            Source = new FileAsset("./wwwroot/index.html"),
            ContentType = "text/html",
        });

        var notfound_html = new Blob($"404-{location}.html", new BlobArgs
        {
            BlobName = "404.html",
            ResourceGroupName = resourceGroup.Name,
            AccountName = storageAccount.Name,
            ContainerName = staticWebsite.ContainerName,
            Source = new FileAsset("./wwwroot/404.html"),
            ContentType = "text/html",
        });
    }
});