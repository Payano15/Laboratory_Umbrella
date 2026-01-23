using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Common;

public static class Constants
{
    #region Structs
    public struct Code
    {
        public struct Error
        {
            public static string InvalidToken = "LSRTKN0001";
            public const string InvalidTokenV2 = "LSRTKN0002";
            public static string NotAuthorized = "AUTH0001";
            public static string NotFound = "NFD0001";
            public const string InvalidUserCredentials = "TKNF0002";

            public static string DuplicateRegistration = "BRREG0001";
            public static string BoardAlreadyExists = "BRREG0002";
            public static string InvalidScreenType = "BRREG0003";
            public static string LotobetBoardNotAvailable = "BRREG0004";
            public static string LotobetLinkFailed = "BRREG0005";
            public static string ActivationNotFound = "BRREG0006";
            public static string ConfigurationFailed = "BRREG0007";
        }
    }

    public struct Message
    {
        public struct Error
        {
            public static string InvalidToken = "Unauthorized Token.";
            public const string InvalidTokenV2 = "Unauthorized Token.";
            public static string NotAuthorized = "Not authorized request";
            public static string NotFound = "Resource not found";
            public const string InvalidUserCredentials = "Invalid user credentials.";

            public static string DuplicateRegistration = "Ya existe una solicitud de registro pendiente para este dispositivo";
            public static string BoardAlreadyExists = "Ya existe un board activo con este DeviceId";
            public static string InvalidScreenType = "Tipo de pantalla no válido";
            public static string LotobetBoardNotAvailable = "El board de Lotobet seleccionado no está disponible";
            public static string LotobetLinkFailed = "Error al vincular con la API de Lotobet";
            public static string ActivationNotFound = "No se encontró una activación pendiente para este board";
            public static string ConfigurationFailed = "Error al configurar el board";
        }
    }

    public struct Permission
    {
        public enum Type
        {
            Read,
            Create,
            Edit,
            Cancel
        }
    }
    #endregion
}