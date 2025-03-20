namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General
{
    public class ResponseModel<T>
    {
        public ResultModel Response { get; set; }
        public T Definition { get; set; }
    }

    public class ResultModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        //public string Code { get; set; }
    }
}
