using System;
using System.Collections.Generic;
using Pulse8Models;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Configuration;


namespace Pulse8.Client
{
    public class Pulse8WebEF : IPulse8WebClient
    {
        string connectionType {get => ConfigurationManager.AppSettings["ef"]; }

        public List<MemberInfo> GetAllMemberInfo()
        {
            using (var client = new HttpClient { BaseAddress = new Uri(connectionType) })
            {
                var response = client.GetAsync("GetAllMembers").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<MemberInfo>>().Result;
                }
            }
            return new List<MemberInfo>();
        }

        public List<MemberInfo> GetMemberInfo(int id)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(connectionType) })
            {
                var json = new ObjectContent<int>(id, new JsonMediaTypeFormatter());
                var response = client.PostAsync("getMember", json).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<MemberInfo>>().Result;
                }
            }
            return new List<MemberInfo>();
        }
    }
}
