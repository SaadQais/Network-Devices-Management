using NetworksManagement.Core;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NetworksManagement.Infrastructure.Utils
{
    public class Helper : IHelper
    {
        public List<Interface> GetInterfacesFromNameAddress(List<string> interfacesNames,
            List<string> interfacesAddresses)
        {
            List<Interface> interfaces = new List<Interface>();

            for (int i = 0; i < interfacesNames.Count; i++)
            {
                interfaces.Add(new Interface
                {
                    Name = interfacesNames[i],
                    Address = interfacesAddresses[i]
                });
            }

            return interfaces;
        }

        public static string GetUserId(ClaimsPrincipal user)
        {
            var claimsIdentity = (ClaimsIdentity)user.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim.Value;
        }

        public const string Admin = "Admin";
        public const string User = "User";
    }
}
