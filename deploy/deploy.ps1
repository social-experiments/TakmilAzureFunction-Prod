param (
	[string]$subscriptionId,
    [string]$resourceGroupName,
    [string]$location,
	[bool]$overwriteResources = $false
)

$templateFile = "template_azfunc_blobtrigger.json"

if (($resourceGroupName -eq "") -or ($location -eq "") -or ($subscriptionId -eq "")) {
	Write-Host 'Usage: deploy.ps1 subscriptionId resourceGroupName location overwriteResources ';
	Write-Host 'Note: overwriteResources is a boolean value - allowed values $true/$false ';
	Write-Host 'Example: deploy.ps1 88888888-3333-2222-1111-000000000000 socexp "West US 2" $true';
	Exit;
}

if (!(Test-Path $templateFile)) {
	Write-Host "Template File does not exist. Please provide a valid file";
	Exit;
}

#Login Prompt
Write-Host "Sign-In with Azure Credentials";
Connect-AzAccount | Out-Null

#Set the active Subscription
Write-Host "Attempting to Set Active Subscription to $subscriptionId";
$context = Get-AzSubscription -SubscriptionId $subscriptionId
if ($null -eq $context) {
	Write-Host "Incorrect SubscriptionId. Please check the Subscription Id";
	Exit;
}

Set-AzContext $context
Write-Host "Successfully Set Active Subscription to $subscriptionId";

############################################################
#Resource Names
############################################################
$trimmedResourceGroupName = $resourceGroupName.ToLower().Replace("_", "")
$deploymentName = $trimmedResourceGroupName + "af" + "deploy"
$storageAccountName = $trimmedResourceGroupName + "af" + "san"


############################################################
# Check if the Resource Group Exists
############################################################
$existingRG = Get-AzResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if (($null -ne $existingRG) -and ($overwriteResources -ne $true)) {
	Write-Host "Resource Group Already Exist.";
	Write-Host "Provide a New Resource Group Name or set overwriteResources parameter to true.";
	Exit;
}

############################################################
# Create a Resource Group
############################################################
if (($null -eq $existingRG)) {
	Write-Host "Creating Resource Group.";
	$existingRG = New-AzResourceGroup -Name $resourceGroupName -Location $location
	if (($null -eq $existingRG)) {
		Write-Host "Not able to create Resource Group. Check if sufficient permissions exist for the account. Exiting"
		Exit;
	}
	Write-Host "Resource Group Successfully Created.";
}

$parameterObject = @{
	"resourceGroupName" = $trimmedResourceGroupName
	"location" = $location
	"storageAccountName" = $storageAccountName
}

############################################################
# Create a Deployment
############################################################
Write-Host "Deploying Resources.";
$deployment = New-AzResourceGroupDeployment -Name $deploymentName -ResourceGroupName $resourceGroupName -TemplateFile $templateFile -TemplateParameterObject $parameterObject
if (($null -eq $deployment)) {
	Write-Host "Error Deploying the Resources. Check if sufficient permissions exist for the account. Exiting"
	Exit;
}
Write-Host "Resource successfully Deployed.";

