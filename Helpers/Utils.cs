namespace RAS823_MC_CiudadMunicipal_FrontEnd.Helpers
{

    public static class ROLEAUDITORIA
    {
        public static readonly string Admin = "Admin";
        public static readonly string SuperAdmin = "Super Admin";
        public static readonly string Usuario = "Usuario";
    }

    public static class COLORTHEME
    {
        public static readonly string Light = "light";
        public static readonly string Dark = "dark";
    }

    public class TYPEPROCCESS
    {
        public static readonly string INTERNAL = "INTERNAL";
        public static readonly string EXTERNAL = "EXTERNAL";
    }

    public class TYPE_PROCCESS_SURVEY
    {
        public static readonly string ALL_SURVEY = "ALL-SURVEY";
        public static readonly string EXTERNAL_SURVEY = "EXTERNAL-SURVEY";
        public static readonly string INTERNAL_SURVEY = "INTERNAL-SURVEY";
    }

    public static class TYPEIDENTIFICATION
    {
        public static readonly string National = "NATIONAL";
        public static readonly string Legal = "LEGAL";
        public static readonly string FOREIGN = "FOREIGN";
    }

    public static class USERTYPE
    {
        public static readonly string INTERNAL = "INTERNAL";
        public static readonly string EXTERNAL = "EXTERNAL";
    }
    public static class USERDEPARTMENT_POSITION
    {
        public static readonly string COLLABORATOR = "COLLABORATOR";
        public static readonly string LEADERSHIPUNIT = "LEADERSHIPUNIT";
        public static readonly string LEADERSHIP = "LEADERSHIP";
    }

    public class PRINCIPALTYPE
    {
        public static readonly string INSTITUTIONALMEMORY = "INSTITUTIONALMEMORY";
        public static readonly string CORRUPTION = "CORRUPTION";
        public static readonly string MANAGEMENT = "MANAGEMENT";
    }

    public static class CustomClaims
    {
        public static readonly string Scope = "http://schemas.microsoft.com/identity/claims/scope";
    }

    public static class ImageProfile
    {
        public static readonly string ImageDefault = "https://www.shutterstock.com/image-vector/vector-flat-illustration-grayscale-avatar-600nw-2264922221.jpg";
    }

    public static class TypesComments
    {
        public static readonly string COMMENT = "COMMENT";
        public static readonly string CHANGEUSER = "CHANGEUSER";
    }
    public static class CODES_RESPONSE
    {
        public static readonly string UNAUTORIZHED = "UNAUTORIZHED";
    }

    public class STATUSMANAGEMENT
    {
        public static readonly string NEW = "NEW";
        public static readonly string REVIEW = "REVIEW";
        public static readonly string PROCCESS = "PROCCESS";
        public static readonly string DONTAPPLY = "DONTAPPLY";
        public static readonly string DONE = "DONE";
        public static readonly string FORTHCOMINGBUDGET = "FORTHCOMINGBUDGET";
    }

    public static class SURVEYQUESTIONTYPE
    {
        public static readonly string SINGLE = "SINGLE_RESPONSE";
        public static readonly string MULTIPLE = "MULTIPLE_RESPONSE";
        public static readonly string SHORT_RESPONSE = "SHORT_RESPONSE";
    }


    public static class USERPOSITIONTASK
    {
        public static readonly string INCHARGE = "INCHARGE";
        public static readonly string COLLABORATOR = "COLLABORATOR";
    }

    public static class PRIORITY
    {
        public const string HIGH = "HIGH";
        public const string MID = "MID";
        public const string LOW = "LOW";
    }


    public static class TABSDATA
    {
        public const string DETAIL = "Detail";
        public const string ATTACHMENTS = "Attachments";
        public const string HISTORY = "History";
    }

    public static class BASELOCATION
    {
        public const string latitude = "9.859986";
        public const string longitude = "-83.920035";
    }
    public static class DAYSTOADD
    {
        public const int days = 9;
    }
    public class CRUDTYPE
    {
        public static readonly string UPDATE = "UPDATE";
        public static readonly string CREATE = "CREATE";
        public static readonly string DELETE = "DELETE";
    }


    public class CONFIG_APROVE
    {
        public static readonly string DENIED = "DENIED";
        public static readonly string INREVIEW = "INREVIEW";
        public static readonly string PUBLISHED = "PUBLISHED";
        public static readonly string NOTPUBLISHED = "NOTPUBLISHED";
        public static readonly string FINISHED = "FINISHED";
    }

    public class TenantIdEnum
    {
        private const string String = "String";
        private const string Number = "Number";
        private const string Decimal = "Decimal";
        private const string Boolean = "Boolean";
        private const string Select = "Select";

        public static string GetTenantIdEnum(int enumVal)
        {
            string enumFound = enumVal switch
            {
                0 => String,
                1 => Number,
                2 => Decimal,
                3 => Boolean,
                4 => Select,
                _ => "",
            };
            return enumFound;
        }
    }

    public static class UtilMethods
    {
        public static string getType(string action)
        {
            if (action == CRUDTYPE.CREATE)
            {
                return "Creado";
            }
            if (action == CRUDTYPE.UPDATE)
            {
                return "Actualizado";
            }
            if (action == CRUDTYPE.DELETE)
            {
                return "Eliminado";
            }
            return "";
        }
    }

}
