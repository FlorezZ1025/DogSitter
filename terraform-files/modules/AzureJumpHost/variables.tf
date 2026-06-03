variable "resource_group_name" { type = string }
variable "location"            { type = string }
variable "environment"         { type = string }
variable "project_name"        { type = string }
variable "subnet_id"           { type = string }
variable "admin_username"      { type = string }
variable "allowed_ips"         { type = list(string) }