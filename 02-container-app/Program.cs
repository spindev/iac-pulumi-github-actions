using Pulumi;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;

return await Pulumi.Deployment.RunAsync(() =>
{
    var resourceGroup = new ResourceGroup("resourceGroup");

    var workspace = new Workspace("loganalytics", new WorkspaceArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Sku = new WorkspaceSkuArgs { Name = "PerGB2018" },
        RetentionInDays = 30,
    });
    
    var workspaceSharedKeys = Output.Tuple(resourceGroup.Name, workspace.Name).Apply(items =>
        GetSharedKeys.InvokeAsync(new GetSharedKeysArgs
        {
            ResourceGroupName = items.Item1,
            WorkspaceName = items.Item2,
        }));
    
    var managedEnvironment = new ManagedEnvironment("env", new ManagedEnvironmentArgs
    {
        ResourceGroupName = resourceGroup.Name,
        AppLogsConfiguration = new AppLogsConfigurationArgs
        {
            Destination = "log-analytics",
            LogAnalyticsConfiguration = new LogAnalyticsConfigurationArgs
            {
                CustomerId = workspace.CustomerId,
                SharedKey = workspaceSharedKeys.Apply(r => r.PrimarySharedKey)
            }
        }
    });

    var containerApp = new ContainerApp("containerApp", new ContainerAppArgs
    {
        ResourceGroupName = resourceGroup.Name,
        ManagedEnvironmentId = managedEnvironment.Id,
        Configuration = new ConfigurationArgs{
            Ingress = new IngressArgs {
                External = true,
                TargetPort = 80
            }
        }
    });
});