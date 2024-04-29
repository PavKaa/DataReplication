using Domain.Entity;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
	public interface IPersonDataService : IService<PersonData, PersonDataDTO>
	{
	}
}
