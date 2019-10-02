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
