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


                return Json(matchedPolicies,JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new ArgumentException("Query parameter 'q' is missing or empty.");
            }
        }

    }
}
