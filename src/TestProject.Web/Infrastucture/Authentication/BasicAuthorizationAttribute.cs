using Microsoft.AspNetCore.Authorization;


namespace TestProject.Web.Infrastucture.Authentication;

public class BasicAuthorizationAttribute : AuthorizeAttribute
{
  public BasicAuthorizationAttribute()
  {
    Policy = "BasicAuthentication";
  }
}