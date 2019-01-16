using System;
using Abp.Domain.Repositories;

namespace LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Persons
{
    /// <summary>
    /// Person领域层的业务管理
    ///</summary>
    public class PersonManager :YoyoCmsTemplateDomainServiceBase, IPersonManager
    {
    private readonly IRepository<Person,Guid> _personRepository;

        /// <summary>
            /// Person的构造方法
            ///</summary>
        public PersonManager(IRepository<Person, Guid>
personRepository)
            {
            _personRepository =  personRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitPerson()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
