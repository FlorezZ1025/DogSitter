resource "azurerm_container_registry" "main" {
  # El nombre del ACR solo puede tener letras y números, sin guiones
  name                = "acr${var.project}${var.environment}"
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = var.sku
  admin_enabled       = true

  tags = var.tags
}
