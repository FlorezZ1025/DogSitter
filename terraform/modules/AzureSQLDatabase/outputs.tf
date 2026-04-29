output "db_host" {
  value     = azurerm_postgresql_flexible_server.main.fqdn
  sensitive = true
}

output "db_name" {
  value = azurerm_postgresql_flexible_server_database.main.name
}