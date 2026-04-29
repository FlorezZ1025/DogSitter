output "resource_group_name" {
  value = azurerm_resource_group.main.name
}

output "acr_login_server" {
  value = module.acr.login_server
}

output "acr_admin_username" {
  value     = module.acr.admin_username
  sensitive = true
}

output "acr_admin_password" {
  value     = module.acr.admin_password
  sensitive = true
}
output "db_host" {
  value     = module.database.db_host
  sensitive = true
}

output "db_name" {
  value = module.database.db_name
}

output "container_app_url" {
  value = module.container_apps.app_url
}