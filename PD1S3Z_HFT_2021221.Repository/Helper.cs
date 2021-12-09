using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public class Helper
    {
        public static MembershipLevel GetLevelByBookNumber(int booksNum)
        {
            if (booksNum > 4)
            {
                return MembershipLevel.Gold;
            } 
            else if (booksNum > 2)
            {
                return MembershipLevel.Silver;
            } 
            else
            {
                return MembershipLevel.Bronze;
            }
        }
    }
}
