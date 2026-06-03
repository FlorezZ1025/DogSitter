resource "azurerm_resource_group" "main" {
  name     = "rg-${var.project_name}-${var.environment}"
  location = var.location
}

module "vnet" {
  source = "./modules/AzureVNet"

  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  environment         = var.environment
  project_name        = var.project_name
  address_space       = var.vnet_address_space
  subnet_prefixes     = var.subnet_prefixes
  subnet_names        = var.subnet_names
}
module "acr" {
  source = "./modules/ACR"

  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  environment         = var.environment
  project_name        = var.project_name
}

module "database" {
  source = "./modules/AzureSQLDatabase"

  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  environment         = var.environment
  project_name        = var.project_name
  admin_username      = var.pg_admin_user
  admin_password      = var.pg_admin_password
  delegated_subnet_id = module.vnet.db_subnet_id
  allowed_ips         = var.allowed_ips  
}

module "aks" {
  source = "./modules/AKS"

  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  environment         = var.environment
  project_name        = var.project_name
  subnet_id           = module.vnet.apps_subnet_id
  acr_id              = module.acr.id
}

module "jump_host" {
  source = "./modules/AzureJumpHost"

  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  environment         = var.environment
  project_name        = var.project_name
  subnet_id           = module.vnet.jump_subnet_id
  admin_username      = "azureuser"
  allowed_ips         = var.allowed_ips
}