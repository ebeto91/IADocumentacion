using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Helpers
{
    public static class DateTimeOperations
    {

        public static DateTime CalculateStartDate(DateTime startDate, List<HolidayDto> holidayDtos)
        {
            DateTime currentDate = startDate;
            int daysAdded = 0;
            var isInLoop = true;
            while (isInLoop)
            {

                var isDateHoliday = holidayDtos.FirstOrDefault(x => x.DateSelected == currentDate);

                if (isDateHoliday != null)
                {
                    currentDate = currentDate.AddDays(1);
                    isInLoop = true;
                }
                else
                {
                    isInLoop = false;
                }
                //volver a validar si es fin de semana
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    isInLoop = false;
                }
                else
                {
                    currentDate = currentDate.AddDays(1);
                    isInLoop = true;
                }
                //currentDate = currentDate.AddDays(1);


            }

            return currentDate;
        }

        public static DateTime CalculateEndDate(DateTime startDate, int businessDays, List<HolidayDto> holidayDtos)
        {
            DateTime currentDate = startDate;
            int daysAdded = 0;

            while (daysAdded < businessDays)
            {
                currentDate = currentDate.AddDays(1);

                // Contar solo días hábiles (excluir sábados y domingos)
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    var isDateHoliday = holidayDtos.FirstOrDefault(x => x.DateSelected == currentDate);

                    if (isDateHoliday == null)
                    {
                        daysAdded++;
                    }
                }


            }

            return currentDate;
        }
    }
}
