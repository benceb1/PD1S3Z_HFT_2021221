using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public class AvgPagesResult
    {
        public string LibraryName { get; set; }
        public double AvgPages { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is AvgPagesResult)
            {
                AvgPagesResult other = obj as AvgPagesResult;
                return this.LibraryName == other.LibraryName &&
                    this.AvgPages == other.AvgPages;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return AvgPages.GetHashCode() + LibraryName.GetHashCode();
        }
    }
}
