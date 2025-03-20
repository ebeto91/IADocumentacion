using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Holidays.Edit
{
    public class HolidaysEditBase : ComponentBase
    {
        [Inject]
        public IHolidaysService _holidayService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Parameter]
        public HolidayManagementDto holidayManagementDto { get; set; } = new HolidayManagementDto();

        [NotNull]
        public Modal? ModalRef { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public async Task HandleFormValid()
        {
            _spinnerService.Show();

            var today = DateTime.Today;

            if (holidayManagementDto.DateSelected.Value < today)
            {
                await _toastService.Error("Acción", "La fecha de cierre no puede ser menor a la fecha de hoy, por favor revisar", autoHide: true);
                _spinnerService.Hide();
                return;
            }

            HolidayDto holidayDto = new HolidayDto()
            {
                Id = holidayManagementDto.Id,
                Day = holidayManagementDto.DateSelected.Value.Day,
                Month = holidayManagementDto.DateSelected.Value.Month,
                Year = holidayManagementDto.DateSelected.Value.Year,
                Description = holidayManagementDto.Description,
                DateSelected = holidayManagementDto.DateSelected.Value.Date,
            };

            var response = await _holidayService.PutHoliday(holidayDto);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información editada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                await CloseModal();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }

        }
        public async Task CloseModal()
        {
            await ModalRef.Close();
            await ActionChild.InvokeAsync(null);
        }
        public async Task CloseModalSend()
        {
            await ActionChild.InvokeAsync(null);
        }
        public async Task OpenModal(HolidayManagementDto holidayManagementDtoParameter)
        {
            await ModalRef.Show();
            holidayManagementDto = holidayManagementDtoParameter;
        }


        public DateTimePicker<DateTime?> _picker1 = default!;
        public async Task<List<DateTime>> OnGetDisabledDaysCallback(DateTime start, DateTime end)
        {

            var ret = new List<DateTime>();
            if (true)
            {
                var day = start;
                while (day <= end)
                {
                    if (day.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
                    {
                        ret.Add(day);
                    }

 

                    day = day.AddDays(1);
                }

                if (DateTime.Today.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
                {
                    // 处理今天是否禁用
                    ret.Add(DateTime.Today);
                }
            }

            await Task.Delay(100);
            return ret;
        }

    }
}
