param (
    [string]$subscriptionId 
)



#Install the AZ Module If not installed
Write-Host "Installing Azure Module";
Install-Module -Name Az -AllowClobber -Scope AllUsers

#Import the AZ Module
Write-Host "Importing Azure Module";
Import-Module Az

#Login Prompt
Write-Host "Sign-In with Azure Credentials";
Connect-AzAccount | Out-Null



