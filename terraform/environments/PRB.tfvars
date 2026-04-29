environment       = "prb"
project_name      = "dogsitter-api"
location          = "eastus2"

# Red
vnet_address_space = ["10.0.0.0/16"]
subnet_prefixes    = ["10.0.1.0/24", "10.0.2.0/24"]
subnet_names       = ["subnet-apps", "subnet-db"]

container_image   = "acrdogsitterapiprb.azurecr.io/dogsitter-api:latest"
container_cpu    = 0.5
container_memory = "1Gi"

allowed_ips = [
  "201.232.155.20",  
  "191.95.33.8" 
]

# ACR
acr_sku = "Basic"
