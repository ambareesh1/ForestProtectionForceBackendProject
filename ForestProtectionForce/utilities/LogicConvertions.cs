using ForestProtectionForce.Models;
using Newtonsoft.Json;

namespace ForestProtectionForce.utilities
{
    public static class LogicConvertions
    {

       public static UserData getUserDetails(string details)
        {
            var user = JsonConvert.DeserializeObject<UserData>(details);
            return user ?? new UserData();
        }


    }
}
