using Pulumi;
using Pulumi.Azure.Core;
using Pulumi.Azure.Storage;

return await Deployment.RunAsync(() =>
{
    var resourceGroup = new ResourceGroup("resourceGroup");

    var storageAccount = new Account("storage", new AccountArgs
    {
        ResourceGroupName = resourceGroup.Name,
        AccountReplicationType = "LRS",
        AccountTier = "Standard"
    });
});