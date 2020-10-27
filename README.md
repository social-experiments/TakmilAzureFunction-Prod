# TakmilAzureFunction-Prod

## Overview

This azure function will get triggered when attendance data is uploaded from the Takmil Android Application. The azure function uses cognitive Services API to process the picture and group the students presents in a class.

## Project Structure

### ProcessAttendance

Process Attendance contains the implementation of Azure Function.

### Deploy

To process the image from Attendance Application, the following resources has to be created in an Azure Account. It is always recommended to create a new resource group for the following resources:

1. Storage Account
2. Cognitive Service
3. App Service Plan
4. Azure Function

Every user of this project, has to create these resources. Manually creating these entries is very tedious and error-prone. The deployment folder is an automation for resource deployment. There are scripts and template files that enables the automation

#### install.ps1

Usage: install.ps1

This powershell scripts installs and imports the AZ module, which has all the useful commandlets for resource deployment.

#### deploy.ps1

Usage: deploy.ps1 <subscriptionid> <resourcegroupname> <location> <overwriteresources>
<br>
Example: deploy.ps1 88888888-3333-2222-1111-000000000000 wslp1 "West US 2" \$true
<br>
This powershell script, prompts the user to login with his Azure Account. After login, the script will validate the input subscription ID. If this is a valid subscription ID, set the same as active subscription. Start to create the resource group, if it does not exist. Invokes the deployment process. The deployment process creates the storage account, app service plan and then the azure function. After the resources are created, the azure function code deployment is configured, which pulls the source from github, builds and deploys the code. This script automatically uses the template_takmil.json. The json file should be in the same directory as the script.

##### template_takmil.json

This is the template file that encapsulates all the resources that has to be created for the end to end solution. This template creates an app service plan that would cost roughly 50 USD. There is also a azure function setting to allow always on. This results in instantaneous triggers for the azure function.

## Updating the Code

This deployment handles Continuous Integration. Everytime a change is made to the repository, the new changes will be compiled and deployed to the Azure Function.
To monitor the progress:
1. Navigate to the Resource Group in Azure Portal
2. Click on the Function Name
3. Go to Deployment Section on the Left Side
4. Click on Deployment Center
5. This should show all the deployments happened so far with the timestamp.
6. If recent change is not picked up, click on "Sync" button to deploy the latest changes
<img src="docs\images\azure_function_deployment.jpg" width="1200" height="600">

## Troubleshooting the function trigger

If there are some manual changes done to the resource group and if it looks like the azure function is not getting triggered, the following should help to troubleshoot this issue.

### Launch Azure Function Page
1. Navigate to Azure Function
2. Click Functions->Functions
3. Click the Function Name "ProcessAttendance"
<img src="docs\images\azure_function_launch.jpg" width="1200" height="600">

### Set up the Log Panel
1. Click Code + Test
2. Click Logs in the Bottom of the Screen
3. The logs panel will display a message when the trigger happens.
4. Click Clear to start a new tracing session.
<img src="docs\images\azure_function_log_setup.jpg" width="1200" height="600">

### Upload the Blob
1. Go to Azure Storage in this resource group
2. Navigate to takmil container
3. Upload a sample json file. This can be a simple text file too for troubleshooting.
<img src="docs\images\azure_storage_blob_upload.jpg" width="1400" height="400">

### Monitor the logs for trigger events
The logs panel will display an event if the trigger was successful.
<img src="docs\images\azure_function_log_monitor.jpg" width="1200" height="700">
