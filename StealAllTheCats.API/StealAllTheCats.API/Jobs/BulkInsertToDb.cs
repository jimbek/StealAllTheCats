using Coravel.Invocable;
using StealAllTheCats.API.Jobs.Payloads;
using StealAllTheCats.API.Models;
using StealAllTheCats.API.Models.Data;
using StealAllTheCats.API.Services;

namespace StealAllTheCats.API.Jobs
{
    public class BulkInsertToDb : IInvocable, IInvocableWithPayload<BulkInsertToDbPayload?>
    {
        public BulkInsertToDbPayload? Payload { get; set; }

        private ICatService _catService;

        private IUnitOfWork _unitOfWork;

        public BulkInsertToDb(ICatService catService, IUnitOfWork unitOfWork)
        {
            _catService = catService;
            _unitOfWork = unitOfWork;
        }

        public async Task Invoke()
        {
            if (Payload == null)
            {
                throw new ArgumentNullException(nameof(Payload));
            }

            await _unitOfWork.JobRepository.UpdateIfExistsAsync(Payload.Id, Status.Started);
            await _unitOfWork.SaveChangesAsync();

            try
            {
                var images = await _catService.GetImages(15, Payload.ApiKey);

                var cats = new List<CatEntity>();

                foreach (var image in images)
                {
                    var cat = new CatEntity(image);

                    bool catExists = await _unitOfWork.CatRepository.ExistsAsync(cat.CatId);

                    if (!catExists)
                    {
                        await _unitOfWork.CatRepository.AddAsync(cat);
                    }

                    foreach (var tag in cat.TagEntities)
                    {
                        bool tagExists = await _unitOfWork.TagRepository.ExistsAsync(tag.Name);

                        if (!tagExists)
                        {
                            await _unitOfWork.TagRepository.AddAsync(tag);
                        }
                    }

                    cats.Add(cat);
                }

                await _unitOfWork.JobRepository.UpdateIfExistsAsync(Payload.Id, Status.Succeed);
            }
            catch (Exception)
            {
                await _unitOfWork.JobRepository.UpdateIfExistsAsync(Payload.Id, Status.Failed);
            }
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
