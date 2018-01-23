﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Config = System.Configuration.ConfigurationManager;

namespace Pulse8.Models
{
    public class Data
    {
        private string ConnectionString { get; set; } = Config.AppSettings["Pulse8"];
        public List<MemberInfo> getMemberInfoById(int id)
        {
            List<MemberInfo> memberInfo = new List<MemberInfo>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("Pulse8_GetMemberByID", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@MemberID", id));
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var IsMostSevereCategory = Convert.ToBoolean(reader["Is Most Severe Category"]);
                        var MemberID = (int)reader["Member ID"];
                        var MostSevereDiagnosisID = reader["Most Severe Diagnosis ID"] as int? ?? -1;
                        memberInfo.Add(new MemberInfo
                        {
                            MemberID = (int)reader["Member ID"],
                            FirstName = reader["First Name"] as string,
                            LastName = reader["Last Name"] as string,
                            MostSevereDiagnosisID = reader["Most Severe Diagnosis ID"] as int? ?? -1,
                            MostSevereDiagnosisDescription = reader["Most Severe Diagnosis Description"] as string,
                            CategoryID = reader["Category ID"] as int? ?? -1,
                            CategoryDescription = reader["Category Description"] as string,
                            CategoryScore = reader["Category Score"] as int? ?? -1,
                            IsMostSevereCategory = Convert.ToBoolean(reader["Is Most Severe Category"])
                        });
                    }
                }
            }
            return memberInfo;
        }
    }
}
