using Dev.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Services.Contracts
{
    public interface IDevTestServices
    {
        bool SaveDevTestDetails(DevTest entity);
        DevTest GetDevTestDetailsById(int id);
        void UpdateDevTestDetails(DevTest entity);
        void DeleteDevTestDetails(int Id);
        List<DevTest> GetDevTestDataList();
    }
}
