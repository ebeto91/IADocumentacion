namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Identification { get; set; }

        public string IdentificationTypeId { get; set; }

        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string ImageProfile { get; set; }
        public string UserName { get; set; }

        public bool SharedLocation { get; set; }

        //public string Password { get; set; }
        //public string PasswordResetCode { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool Enabled { get; set; }
        public int AccessFailedCount { get; set; }
    }

    public class UserResponseAssing : UserResponse
    {
        public string Position { get; set; }
        public bool Selected { get; set; }
    }
}
