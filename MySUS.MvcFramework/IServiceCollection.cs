using System;
using System.Collections.Generic;
using System.Text;

namespace MySUS.MvcFramework
{
    public interface IServiceCollection
    {
        //void Add<IUsersService, UsersService>
        void Add<TSource, TDestination>();

        object CreateInstance(Type type);
    }
}
