using System.Collections.Generic;

namespace InsurancePolicyRepository
{
    interface IPolicyProvider
    {
        List<PolicyInfo> GetPoliciesThatMatchNameFragment(string nameFragment);
    }
}
