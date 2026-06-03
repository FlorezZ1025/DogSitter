resource "azurerm_mssql_server" "main" {
  name                         = "sql-${var.project}-${var.environment}"
  resource_group_name          = var.resource_group_name
  location                     = var.location
  version                      = "12.0"
  administrator_login          = var.administrator_login
  administrator_login_password = var.administrator_password  # ← nombre corregido

  tags = var.tags
}

resource "azurerm_mssql_database" "main" {
  name      = var.project
  server_id = azurerm_mssql_server.main.id
  sku_name  = var.sku_name

  tags = var.tags
}

resource "azurerm_mssql_firewall_rule" "azure_services" {
  name             = "allow-azure-services"
  server_id        = azurerm_mssql_server.main.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}