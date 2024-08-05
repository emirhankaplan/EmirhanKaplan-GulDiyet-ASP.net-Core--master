using System;

namespace GulDiyet.Core.Application.ViewModels.LaboratoryTests
{
    public class TestResultViewModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public DateTime Date { get; set; }
    }
}
