variable "resource_group_name" { type = string }
variable "location"            { type = string }
variable "environment"         { type = string }
variable "project_name"        { type = string }
variable "admin_username"      { 
         type = string
         sensitive = true 
         }
variable "admin_password"      { 
 type = string
 sensitive = true 
 }
variable "delegated_subnet_id" { type = string }
variable "allowed_ips" {
  description = "IPs públicas con acceso a PostgreSQL"
  type        = list(string)
  default     = []
}