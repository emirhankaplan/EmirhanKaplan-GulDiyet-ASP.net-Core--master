using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.DietPlan;
using GulDiyet.Core.Application.ViewModels.Email;
using GulDiyet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Mail;

namespace GulDiyet.Core.Application.Services
{
    public class DietPlanService : IDietPlanService
    {
        private readonly IDietPlanRepository _dietPlanRepository;
        private readonly IEmailService _emailService;
        private readonly IPdfService _pdfService;

        public DietPlanService(IDietPlanRepository dietPlanRepository, IEmailService emailService, IPdfService pdfService)
        {
            _dietPlanRepository = dietPlanRepository;
            _emailService = emailService;
            _pdfService = pdfService;
        }

        public async Task<DietPlanViewModel> CreateDietPlanAsync(SaveDietPlanViewModel saveDietPlanViewModel)
        {
            var dietPlan = new DietPlan
            {
                DiyetisyenId = saveDietPlanViewModel.DiyetisyenId,
                PatientId = saveDietPlanViewModel.PatientId,
                PlanDetails = saveDietPlanViewModel.PlanDetails,
                CreatedDate = saveDietPlanViewModel.CreatedDate
            };

            dietPlan = await _dietPlanRepository.AddAsync(dietPlan);

            var viewModel = new DietPlanViewModel(dietPlan);

            await SendDietPlanByEmail(viewModel);

            return viewModel;
        }

        public async Task UpdateDietPlanAsync(SaveDietPlanViewModel saveDietPlanViewModel)
        {
            var dietPlan = await _dietPlanRepository.GetByIdAsync(saveDietPlanViewModel.Id);
            if (dietPlan != null)
            {
                dietPlan.DiyetisyenId = saveDietPlanViewModel.DiyetisyenId;
                dietPlan.PatientId = saveDietPlanViewModel.PatientId;
                dietPlan.PlanDetails = saveDietPlanViewModel.PlanDetails;
                dietPlan.CreatedDate = saveDietPlanViewModel.CreatedDate;

                await _dietPlanRepository.UpdateAsync(dietPlan);

                var viewModel = new DietPlanViewModel(dietPlan);

                await SendDietPlanByEmail(viewModel);
            }
        }

        public async Task DeleteDietPlanAsync(int id)
        {
            var dietPlan = await _dietPlanRepository.GetByIdAsync(id);
            if (dietPlan != null)
            {
                await _dietPlanRepository.DeleteAsync(dietPlan);
            }
        }

        public async Task<DietPlanViewModel> GetDietPlanByIdAsync(int id)
        {
            var dietPlan = await _dietPlanRepository.GetByIdAsync(id);
            return new DietPlanViewModel(dietPlan);
        }

        public async Task<List<DietPlanViewModel>> GetDietPlansByPatientIdAsync(int patientId)
        {
            var dietPlans = await _dietPlanRepository.GetDietPlansByPatientIdAsync(patientId);
            var viewModelList = new List<DietPlanViewModel>();
            foreach (var plan in dietPlans)
            {
                viewModelList.Add(new DietPlanViewModel(plan));
            }
            return viewModelList;
        }

        public async Task<List<DietPlanViewModel>> GetDietPlansByDiyetisyenIdAsync(int diyetisyenId)
        {
            var dietPlans = await _dietPlanRepository.GetDietPlansByDiyetisyenIdAsync(diyetisyenId);
            var viewModelList = new List<DietPlanViewModel>();
            foreach (var plan in dietPlans)
            {
                viewModelList.Add(new DietPlanViewModel(plan));
            }
            return viewModelList;
        }

        private async Task SendDietPlanByEmail(DietPlanViewModel dietPlan)
        {
            var pdfContent = await _pdfService.GenerateDietPlanPdf(new List<DietPlanViewModel> { dietPlan });
            var emailViewModel = new SaveEmailViewModel
            {
                To = dietPlan.PatientName,
                Subject = "Diyet Planınız",
                Body = "Yeni diyet planınız hazır. Ekteki PDF dosyasından detayları inceleyebilirsiniz.",
                Attachments = new List<Attachment> { new Attachment(new MemoryStream(pdfContent), "DiyetPlanı.pdf") }
            };

            await _emailService.SendEmailAsync(emailViewModel);
            _emailService.NotifyNewDietPlan(emailViewModel);
        }
    }
}
