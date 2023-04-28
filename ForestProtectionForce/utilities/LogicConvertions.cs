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

        public static int getSuperAdminOfProvince(UserData userData)
        {
             if(userData.username == "superadmin_jammu")
            {
                return 1;
            }else if (userData.username == "superadmin_kashmir")
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

    }
}
