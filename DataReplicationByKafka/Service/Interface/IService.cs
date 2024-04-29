using Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
	public interface IService<TData, TModel>
	{
		Task<BaseResponse<TData>> Create(TModel model);

		Task<BaseResponse<List<TData>>> GetAll();
	}
}
