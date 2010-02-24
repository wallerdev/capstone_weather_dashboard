using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using InsurancePolicyRepository;

namespace CapstoneWeatherDashboard.Controllers
{
    public class PolicyHolderNamesController : Controller
    {
        //
        // GET: /PolicyHolderNames/

        public ActionResult Index()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["q"]))
            {
                string partialName = Request.QueryString["q"];
                var policyProvider = new MockPolicyProvider();
                List<PolicyInfo> matchedPolicies = policyProvider.GetPoliciesThatMatchNameFragment(partialName);

                var returnString = new StringBuilder();
                foreach (var policyHolder in matchedPolicies)
                {
                    returnString.AppendLine(policyHolder.PolicyHolderName);
                }


                return Content(returnString.ToString());
            }
            else
            {
                throw new ArgumentException("Query parameter 'q' is missing or empty.");
            }
        }

    }
}
