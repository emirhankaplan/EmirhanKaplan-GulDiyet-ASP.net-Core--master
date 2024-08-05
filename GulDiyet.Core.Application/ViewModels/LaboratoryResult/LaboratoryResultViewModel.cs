namespace GulDiyet.Core.Application.ViewModels.LaboratoryResult
{
    public class LaboratoryResultViewModel
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string PatientIdNumber { get; set; }
        public int LaboratoryTestId { get; set; }
        public string LaboratoryTestName { get; set; }
        public bool? IsCompleted { get; set; }

    }
}
