variable "resource_group_name"      { type = string }
variable "location"                 { type = string }
variable "environment"              { type = string }
variable "project_name"             { type = string }
variable "infrastructure_subnet_id" { type = string }
variable "container_image"          { type = string }
variable "container_cpu"            { type = number }
variable "container_memory"         { type = string }
variable "db_host"                  { 
    type = string
    sensitive = true 
    }
variable "db_name"                  { type = string }
variable "db_user"                  { 
        type = string
        sensitive = true 
        }
variable "db_password"              { 
        type = string
        sensitive = true
        }
variable "acr_server"   { type = string }
variable "acr_username" { 
    type = string
    sensitive = true
    }
variable "acr_password" { 
    type = string
    sensitive = true 
    }