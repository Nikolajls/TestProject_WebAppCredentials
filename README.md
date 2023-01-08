# TestProject_WebAppCredentials

Container being hosted in Azure [here](http://nikolajwebapptest.westeurope.azurecontainer.io)

## Docker

To build container run following command from src folder:

```powershell

docker build -t testweb-image -f TestProject.Web/Dockerfile .
```

To spin up container you can run (this does not run HTTPS currently):

```powershell

docker run -it --rm -p 8080:80 -p 4431:443  `
-e ASPNETCORE_ENVIRONMENT=Development  `
--name testweb-container  `
testweb-image
```
