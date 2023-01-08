data "azurerm_client_config" "current" {
}

#Resource group
resource "azurerm_resource_group" "rg" {
  name     = var.resourceGroupName
  location = var.location
}


# log analytics
resource "azurerm_log_analytics_workspace" "law" {
  name                = "loggingworkspace"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "appinsights" {
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  name                = "appinsights-appservice"
  workspace_id        = azurerm_log_analytics_workspace.law.id
  application_type    = "web"
}

#use the module keyvault
locals {

  keyVaultUrl = "https://${var.key_vault_name}.vault.azure.net/"
  secrets-to-add = {
    AuthenticationInfo--Username = {
      value = var.secret-AuthenticationInfo_username
    }

    AuthenticationInfo--Password = {
      value = var.secret-AuthenticationInfo_password
    }

    Image-repo-password = {
      value = var.secret_image_repo_password
    }
  }
}

module "keyvault" {
  source                          = "./modules/keyvault"
  name                            = var.key_vault_name
  resource_group_name             = azurerm_resource_group.rg.name
  location                        = azurerm_resource_group.rg.location
  current_client_object_id        = data.azurerm_client_config.current.object_id
  enabled_for_deployment          = var.kv-vm-deployment
  enabled_for_disk_encryption     = var.kv-disk-encryption
  enabled_for_template_deployment = var.kv-template-deployment

  tags = {
    environment = "${var.azure_environment}"
  }

  policies = {
    apiIdentityRead = {
      tenant_id               = var.tenant_id
      object_id               = azurerm_container_group.app.identity.0.principal_id
      key_permissions         = []
      secret_permissions      = var.kv-secret-permissions-read
      certificate_permissions = []
      storage_permissions     = []
    }
  }

  secrets = local.secrets-to-add


  depends_on = [
    azurerm_container_group.app
  ]
}


resource "azurerm_container_group" "app" {
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  name                = var.container_name
  dns_name_label      = var.domain_name_label
  ip_address_type     = "Public"

  os_type = "Linux"

  image_registry_credential {
    server   = var.Image_repo_server
    username = var.Image_repo_user
    password = var.secret_image_repo_password
  }


  identity {
    type = "SystemAssigned"
  }

  container {
    name   = "webapp"
    image  = var.Image_repo_image_path
    cpu    = 1
    memory = 1.5

    ports {
      port     = 80
      protocol = "TCP"
    }

    ports {
      port     = 443
      protocol = "TCP"
    }



    environment_variables = {
      ASPNETCORE_ENVIRONMENT               = var.ASPNETCORE_ENVIRONMENT
      AppConfiguration__KeyVaultUrl        = "${local.keyVaultUrl}"
      Serilog__WriteTo__0__Name            = "Console"
      Serilog__WriteTo__0__Args__formatter = "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"

      Serilog__WriteTo__1__Name                     = "ApplicationInsights"
      Serilog__WriteTo__1__Args__telemetryConverter = "Serilog.Sinks.ApplicationInsights.TelemetryConverters.TraceTelemetryConverter, Serilog.Sinks.ApplicationInsights"
      Serilog__WriteTo__1__Args__connectionString   = azurerm_application_insights.appinsights.connection_string
    }

  }
}
