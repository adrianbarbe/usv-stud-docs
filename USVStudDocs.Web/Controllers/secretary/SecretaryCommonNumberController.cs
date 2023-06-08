using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Services.CommonNumberService;
using USVStudDocs.Models.Constants;
using USVStudDocs.Models.Secretary;

namespace USVStudDocs.Web.Controllers.secretary
{
    [Route("api/secretaryCommonNumber")]
    public class SecretaryCommonNumberController : Controller
    {
        private readonly ICommonNumberService _commonNumberService;

        public SecretaryCommonNumberController(ICommonNumberService commonNumberService)
        {
            _commonNumberService = commonNumberService;
        }
        
        [HttpGet]
        [Route("getLast")]
        [Authorize(Policy = Policies.Secretary)]
        public CommonNumber GetLast()
        {
            return _commonNumberService.GetLatest();
        }
        
        [HttpGet]
        [Route("getToday")]
        [Authorize(Policy = Policies.Secretary)]
        public CommonNumber GetToday()
        {
            return _commonNumberService.GetToday();
        }
        
        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.Secretary)]
        public void SaveNew([FromBody] CommonNumber model)
        {
            _commonNumberService.Save(model);
        }
    }
}