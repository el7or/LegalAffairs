using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Common
{
    public static class PrintHelper
    {
        /// <summary>
        /// get case parties for print
        /// </summary>
        /// <returns>list of parties as string</returns>
        public static string TransformParties(ICollection<PartyDto> parties)
        {
            var plaintiff = new List<string>();

            foreach (var party in parties)
            {
                if (party.PartyType == PartyTypes.Person)
                {
                    plaintiff.Add(party.Name + " " + EnumExtensions.GetDescription((IdentityTypes)party.IdentityTypeId) + " رقم  " + party.IdentityValue);
                }
                else if (party.PartyType == PartyTypes.CompanyOrInstitution)
                {
                    plaintiff.Add(party.Name + " سجل تجاري رقم " + party.CommertialRegistrationNumber);
                }
                else
                {
                    plaintiff.Add(party.Name);
                }
            }

            return String.Join(" ، ", plaintiff);
        }
    }
}
