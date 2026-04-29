resource "azurerm_virtual_network" "main" {
  name                = "vnet-${var.project_name}-${var.environment}"
  location            = var.location
  resource_group_name = var.resource_group_name
  address_space       = var.address_space
}

resource "azurerm_subnet" "subnets" {
  count                = length(var.subnet_names)
  name                 = var.subnet_names[count.index]
  resource_group_name  = var.resource_group_name
  virtual_network_name = azurerm_virtual_network.main.name
  address_prefixes     = [var.subnet_prefixes[count.index]]

  depends_on = [azurerm_virtual_network.main]
   dynamic "delegation" {
    for_each = var.subnet_names[count.index] == "subnet-apps" ? [1] : []
    content {
      name = "delegation-container-apps"
      service_delegation {
        name = "Microsoft.App/environments"
        actions = [
          "Microsoft.Network/virtualNetworks/subnets/join/action"
        ]
      }
    }
  }
}