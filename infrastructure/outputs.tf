output "keyvaultUrl" {
  value = module.keyvault.key-vault-url
}

output "keyvaultId" {
  value = module.keyvault.key-vault-id
}

output "WebappUrl" {
  value = azurerm_container_group.app.fqdn
}