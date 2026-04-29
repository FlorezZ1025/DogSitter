resource "azurerm_container_registry" "main" {
  name                = replace("acr${var.project_name}${var.environment}", "-", "")
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = "Basic"  # El más económico, suficiente para PRB
  admin_enabled       = true     # Necesario para que Container Apps pueda hacer pull
}