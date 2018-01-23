using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse8.Models
{
    public class MemberInfo
    {
        public int MemberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MostSevereDiagnosisID { get; set; }
        public string MostSevereDiagnosisDescription { get; set; }
        public int CategoryID { get; set; }
        public string CategoryDescription { get; set; }
        public int CategoryScore { get; set; }
        public bool IsMostSevereCategory { get; set; }

        public override string ToString()
        {
            return $"Member ID: {MemberID}{Environment.NewLine}" +
                $"First Name: {FirstName}{Environment.NewLine}" +
                $"LastName: {LastName}{Environment.NewLine}" +
                $"Most Severe Diagnosis ID: {(MostSevereDiagnosisID > 0? MostSevereDiagnosisID.ToString():"")}{Environment.NewLine}" +
                $"Most Severe Diagnosis Description: {MostSevereDiagnosisDescription}{Environment.NewLine}" +
                GetCategoryInfo();
        }
        public string GetCategoryInfo()
        {
            if (CategoryID == -1)
            {
                return $"Is Most Severe Category: {IsMostSevereCategory}{Environment.NewLine}";
            }
            return $"Category ID: {CategoryID}{Environment.NewLine}" +
                    $"Category Description: {CategoryDescription}{Environment.NewLine}" +
                    $"Category Score: {CategoryScore}{Environment.NewLine}" +
                    $"Is Most Severe Category: {IsMostSevereCategory}{Environment.NewLine}";
        }
    }
}
