output "server_fqdn"   { value = azurerm_mssql_server.main.fully_qualified_domain_name }
output "server_name"   { value = azurerm_mssql_server.main.name }
output "database_name" { value = azurerm_mssql_database.main.name }