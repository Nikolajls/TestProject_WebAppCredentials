output "keyvaultUrl" {
  value = module.keyvault.key-vault-url
}

output "keyvaultId" {
  value = module.keyvault.key-vault-id
}

output "resourceGroup" {
  value = azurerm_resource_group.rg.name
}

output "containerName" {
  value = azurerm_container_group.app.name
}


output "WebappUrl" {
  value = azurerm_container_group.app.fqdn
}
