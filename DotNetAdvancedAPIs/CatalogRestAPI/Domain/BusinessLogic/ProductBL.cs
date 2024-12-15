using ProductDomain.Entities;
using HelperUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDomain.BusinessLogic
{
    public class ProductBL
    {
        public delegate void ProductUpdated(ProductBL sender, Product product);
        public event ProductUpdated OnProductUpdated;
        public event ProductUpdated OnProductUpdateFailed;
        public Product Product { get; private set; }

        public ProductBL(Product product = null)
        {
            Product = product;
        }

        public void RaiseProductUpdatedEvent(DBOperationStatus status)
        {
            switch (status)
            {
                case DBOperationStatus.Success:
                    OnProductUpdated?.Invoke(this, Product);
                    break;
                case DBOperationStatus.Fail:
                    OnProductUpdateFailed?.Invoke(this, Product);
                    break;
            }            
        }

    }
}
