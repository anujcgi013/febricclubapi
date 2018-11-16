using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductDataAccess;

namespace FebricClubApi.Controllers
{
    public class ProductController : ApiController
    {
        private FebricClubEntities _febClubEntity = new FebricClubEntities();
        public ProductController()
        {
            _febClubEntity.Configuration.ProxyCreationEnabled = false;

        }

        [Route("api/product/")]
        public IEnumerable<ProductDetail> Get()
        {
            return _febClubEntity.ProductDetails;
        }

        [Route("api/product/{srNumber}")]
        public ProductDetail Get(int srNumber)
        {
            return _febClubEntity.ProductDetails.FirstOrDefault(x => x.SNumber == srNumber);
        }

        [HttpPost]
        [Route("api/product/productDetail")]
        public HttpResponseMessage Post([FromBody] ProductDetail productDetail)
        {
            try
            {
                _febClubEntity.ProductDetails.Add(productDetail);
                _febClubEntity.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        [Route("api/product/productDetail/{srNumber}")]
        public HttpResponseMessage Put([FromUri] int srNumber, [FromBody] ProductDetail productDetail)
        {
            try
            {
                var entity = _febClubEntity.ProductDetails.FirstOrDefault(x => x.SNumber == srNumber);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with Id = " + srNumber.ToString());
                }
                else
                {

                    entity.CompanyId = productDetail.CompanyId;
                    entity.BundleNumber = productDetail.BundleNumber;
                    entity.Length = productDetail.Length;
                    entity.Weight = productDetail.Weight;
                    entity.CreatedDate = productDetail.CreatedDate;
                    entity.Comments = productDetail.Comments;
                    _febClubEntity.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
