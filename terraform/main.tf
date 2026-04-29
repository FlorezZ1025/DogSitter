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

module "container_apps" {
  source = "./modules/AzureContainerApps"

  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  environment         = var.environment
  project_name        = var.project_name
  infrastructure_subnet_id = module.vnet.apps_subnet_id
  container_cpu       = var.container_cpu
  container_memory    = var.container_memory
  db_host             = module.database.db_host
  db_name             = module.database.db_name
  db_user             = var.pg_admin_user
  db_password         = var.pg_admin_password

  container_image = module.acr.login_server != "" ? var.container_image : var.container_image
  acr_server      = module.acr.login_server      # 👈 nuevo
  acr_username    = module.acr.admin_username     # 👈 nuevo
  acr_password    = module.acr.admin_password     # 👈 nuevo
}