using System.Collections.Generic;
using Pulse8IBusiness.Interfaces;
using Pulse8Models;
using System.Linq;
using System.Data;

namespace Pulse8Business.Repositories
{
    public class EntityRepository : IPulse8Repository
    {
        pulse8Data.Models.Pulse8Entities context = new pulse8Data.Models.Pulse8Entities();

        public List<MemberInfo> GetAllMemberInfo()
        {
            var membersNoData = context.Members.Where(m => !context.MemberDiagnosis.Any( dx => dx.MemberID == m.MemberID)).Select(m => new MemberInfo { MemberID = m.MemberID, LastName = m.LastName, FirstName = m.FirstName, IsMostSevereCategory = true });
            var query = from mem in context.Members
                        join mdx in context.MemberDiagnosis on mem.MemberID equals mdx.MemberID
                        join dcm in context.DiagnosisCategoryMaps on mdx.DiagnosisID equals dcm.DiagnosisID
                        join cat in context.DiagnosisCategories on dcm.DiagnosisCategoryID equals cat.DiagnosisCategoryID
                        join dx in context.Diagnosis on mdx.DiagnosisID equals dx.DiagnosisID
                        select new
                        {
                            cat.CategoryDescription,
                            cat.DiagnosisCategoryID,
                            cat.CategoryScore,
                            mem.FirstName,
                            mem.LastName,
                            mem.MemberID,
                            dx.DiagnosisID,
                            dx.DiagnosisDescription,
                        };
            //create new list with members that have no data to sort.
            var filteredSet = membersNoData.ToList();

            //filter members with data to return only worst diagnosis and each category.
            var distinctMembers = query.GroupBy(m => m.MemberID);
            foreach (var memberDataSet in distinctMembers)
            {
                var mostSevereDx = memberDataSet.Min(m => m.DiagnosisID);
                var mostSevereCat = memberDataSet.Min(m => m.DiagnosisCategoryID);

                ///A seaparate categories model would be most appropriate here but to keep
                //but to keep data sets similar, we will keep one memberInfo object.
                for (int i = 0; i < memberDataSet.Count(); i++)
                {
                    var member = memberDataSet.ElementAt(i);
                    //skip duplicate categories
                    if (filteredSet.Any(mInfo => mInfo.MemberID == member.MemberID && mInfo.CategoryID == member.DiagnosisCategoryID))
                        continue;

                    filteredSet.Add(new MemberInfo
                    {
                        MemberID = member.MemberID,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        MostSevereDiagnosisID = mostSevereDx,
                        MostSevereDiagnosisDescription = memberDataSet.Where(m => m.DiagnosisID == mostSevereDx).Select(m => m.DiagnosisDescription).FirstOrDefault(),
                        CategoryID = member.DiagnosisCategoryID,
                        CategoryDescription = member.CategoryDescription,
                        CategoryScore = member.CategoryScore,
                        IsMostSevereCategory = mostSevereCat == member.DiagnosisCategoryID
                    });
                }
            }

            return filteredSet;
        }

        public List<MemberInfo> GetMemberInfo(int memberId)
        {
            return GetAllMemberInfo().Where(m => m.MemberID == memberId).ToList();
        }
    }
}
