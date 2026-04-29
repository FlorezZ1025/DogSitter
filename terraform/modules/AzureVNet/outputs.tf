output "vnet_id" {
  value = azurerm_virtual_network.main.id
}

output "apps_subnet_id" {
  value = azurerm_subnet.subnets[0].id
}

output "db_subnet_id" {
  value = azurerm_subnet.subnets[1].id
}