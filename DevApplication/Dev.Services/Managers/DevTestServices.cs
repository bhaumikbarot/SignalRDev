using Dev.BO.Models;
using Dev.Data.UOW;
using Dev.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Services.Managers
{
    public class DevTestServices : IDevTestServices
    {
        #region Fields
        private DevTest _devTest = new DevTest();
        #endregion

        private readonly IUnitOfWork _unitOfWork;
        public DevTestServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool SaveDevTestDetails(DevTest entity)
        {
            try
            {
                _unitOfWork.DevTestRepository.Add(entity);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DevTest GetDevTestDetailsById(int id)
        {
            return _unitOfWork.DevTestRepository.GetBy(u => u.Id == id).AsQueryable().FirstOrDefault();
        }

        public void UpdateDevTestDetails(DevTest entity)
        {
            var objDevTest = GetDevTestDetailsById(entity.Id);
            if (objDevTest != null)
            {
                objDevTest.AffiliateName = entity.AffiliateName;
                objDevTest.CampaignName = entity.CampaignName;
                objDevTest.Clicks = entity.Clicks;
                objDevTest.Conversions = entity.Conversions;
                objDevTest.Date = entity.Date;
                objDevTest.Impressions = entity.Impressions;
                _unitOfWork.DevTestRepository.Update(objDevTest);
                _unitOfWork.Commit();
            }
        }

        public void DeleteDevTestDetails(int Id)
        {
            _unitOfWork.DevTestRepository.Delete(Id);
            _unitOfWork.Commit();
        }
        public List<DevTest> GetDevTestDataList()
        {
            return _unitOfWork.DevTestRepository.GetAll().ToList();
        }
    }
}
