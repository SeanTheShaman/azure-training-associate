# Photo Sharing App Example 

This app gives an example of how to connect an app to an Azure storage account and use the client libraries to interact with it. 



Generate the storage account connection string with the following Azure CLI command: 

az storage account show-connection-string \
  --resource-group learn-7cae0372-89cb-41a5-b8ac-a08993c3bcc4 \
  --query connectionString \
  --name <name>

  Note: The storage account connection string is stored in source control - typically bad practice - but this is simplest for now and only accesses a temporary sandbox account. 