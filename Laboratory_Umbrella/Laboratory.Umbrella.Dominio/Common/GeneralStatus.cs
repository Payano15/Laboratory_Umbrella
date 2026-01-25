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

    public static class TypeClient
    {
        public enum TypeClients
        {
            Client = 0,
            Provider = 1,
        }
    }

    public static class GlobalStatus
    {
        public enum Status
        {
            Activo = 1,
            Inactivo = 2,
        }
    }
}