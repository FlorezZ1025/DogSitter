variable "resource_group_name"    { type = string }
variable "location"               { type = string }
variable "project"                { type = string }
variable "environment"            { type = string }
variable "administrator_login" {
  type      = string
  sensitive = true
}
variable "administrator_password" {
  type      = string
  sensitive = true
}
variable "sku_name" {
  type    = string
  default = "Basic"   # Basic = ~5 USD/mes, suficiente para dev
}
variable "tags" { type = map(string) }