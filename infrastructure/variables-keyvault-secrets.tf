#keyvault name
variable "key_vault_name" {
  default = ""
}

#secrets to add to the keyvault
variable "secret-AuthenticationInfo_username" {
  default     = ""
  description = ""
  sensitive   = true
}

variable "secret-AuthenticationInfo_password" {
  default     = ""
  description = ""
  sensitive   = true
}

variable "secret_image_repo_password" {
  default     = ""
  description = ""
  sensitive   = true
}


