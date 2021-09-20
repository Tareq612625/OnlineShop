using OnlineShop.Data;
using System;

namespace OnlineShop.Controllers
{
    internal class AppliccationDbContext
    {
        public object Products { get; internal set; }

        public static implicit operator AppliccationDbContext(ApplicationDbContext v)
        {
            throw new NotImplementedException();
        }
    }
}