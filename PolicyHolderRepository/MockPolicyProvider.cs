using System;
using System.Collections.Generic;
using System.Linq;
using WeatherStation;

namespace InsurancePolicyRepository
{
    public class MockPolicyProvider : IPolicyProvider
    {
        public List<PolicyInfo> GetPoliciesThatMatchNameFragment(string nameFragment)
        {
            var list = from p in GetAllPolicies()
                        where p.PolicyHolderName.ToLower().Contains(nameFragment)
                        orderby p.PolicyNumber
                        select p;
            return list.ToList();
        }

        private List<PolicyInfo> GetAllPolicies()
        {
            return new List<PolicyInfo>
                       {
                           new PolicyInfo("Abraham Lincoln", "123456", new Address("48823")),
                           new PolicyInfo("George Washington", "654321", new Address("98027"))
                       };
        }
    }
}
