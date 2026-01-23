namespace Laboratory.Umbrella.Dominio.Common;

public static class GeneralStatus
{
    public static class ClientStatus
    {
        public enum StatusClient
        {
            INACTIVE = 0,
            ACTIVE = 1,
            SUSPENDED = 2,
            DELETED = 3
        }
    }

}