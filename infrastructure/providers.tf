terraform {

  required_version = ">=0.12"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.38.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "terraform_state"
    storage_account_name = "webapptestnikolajls"
    container_name       = "terraform-states"
    key                  = "webapp.tfstate"
  }

}

provider "azurerm" {
  features {}
}