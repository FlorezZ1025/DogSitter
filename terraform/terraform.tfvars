# Copia este archivo como terraform.tfvars y rellena los valores sensibles
# NUNCA subas terraform.tfvars al repositorio (está en .gitignore)

project     = "dogsitter"
environment = "dev"
location    = "westus"

# VNet
vnet_address_space = ["10.1.0.0/16"]
aks_subnet_cidr    = "10.1.1.0/24"
db_subnet_cidr     = "10.1.2.0/24"

# ACR
acr_sku = "Basic"

# AKS
kubernetes_version = "1.36.0"
aks_node_count     = 2
aks_node_vm_size   = "Standard_D2s_v3"

# Database — REEMPLAZA ESTOS VALORES
db_admin_login    = "dogsitteradmin"
db_admin_password = "Password1234"
db_sku_name       = "Basic"
