variable "resourceGroupName" {
  default     = ""
  description = "Name of the resource group"
}

# azure region
variable "location" {
  default     = ""
  description = "Azure region where the resource group will be created"
}


variable "azure_environment" {
  description = "Environment for the IaC"
  default     = ""
}

variable "ASPNETCORE_ENVIRONMENT" {
  default     = ""
  description = "The environment that the ASPNET Core is being instructed to run in"
}

variable "Image_repo_server" {
  default     = ""
  description = "The repository server for the docker image"
}

variable "tenant-id" {
  default     = ""
  sensitive   = true
  description = "Tenant Id in Azure for key vault"
}


variable "Image_repo_user" {
  default     = ""
  description = "The username being used to access image repo which is in Github"
  sensitive   = true
}

variable "Image_repo_image_path" {
  default     = ""
  description = "The actual image path that the container should pull and spin up."
}

variable "domain_name_label" {
  default     = ""
  description = "The domainname to add to the container instance"
}



