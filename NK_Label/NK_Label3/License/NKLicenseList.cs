namespace NK_Label.License
{
    public static class NKLicenseList
    {
        // 'hasp_net_windows.dll' has rebuilded on 2018.05.15 only for this NK-Label project
        public const string Code_YIOBI =
            "BudbQeTWEZvRe2QYHBHkTrFux2aiOVkFaHggKdMHwzj60kPnxsSzcjesi6bRZ3XoCe/4o9if6kZSXHbq"
            + "uLTIho5qxtXONXN2fvlnJMcmllR1V586gyhANdNe+0YUkDLlMsgv2l6MSnf+LlqhW6oK7tWgAgtPR7KQ"
            + "X63FM9UoMMHN0AKKKRWlP11d+GXapSg71pnNjCLnW/zKaOyUcdua/Ck4S6GG9Sb/K32aCkNIf4FkKA50"
            + "/flcVoMQRnn1/JY8qapUGrD5BAIRTTSM64v5UYuLsnDddAyE2C+80frNiYFBMfOaytfyvQDhxWXCc0Ty"
            + "7zx1rfLMjC3Ar8QQZEGiV82Mx1E2UJUYJl9sucE2OE0u0NwXu9vUmkf44EAjjCkr/LSg/bOVfeCWP05h"
            + "8cXC/puJq4wO4vwQOME8FRywu8RsmMtHdhP5cZL6ks/DagoMip3lMqArWrMNMimXXvmdFtfWDFPJSEgZ"
            + "Ln7SchOZdN5UgY4S4GrFLtrSUKSyk3zo2SpblB+Hjd5QByAVUySqg3WQ9mUjqFN9kE/Bo4JEudUGyR0Y"
            + "EEEBUM95eMYubXlXpEnenTlQoHvQfeyNC350QmEoQLCJDc6/AYSqbp7JQrz9vlt7N2NBKGq6dpXAbqJx"
            + "fvTdW/rVemEtdniY7VxCO/ysYBTxGhIj9rheJ+oPoAambyuF/A+Nq+4p0GYyPbSKZ4YlKvLA0oBAzvAN"
            + "qUInv1/tqok35WTe7hIVTXo+6lI6pI9vv554T8/MQ1eI2Zl4Jjaf2tpX4zPHQpZ3tUR/TYzEaf3+JLsD"
            + "g40Lr24NNxVxdM27aoOZQGsA2dihMV21zvSiqVUtvPUJ8Jl94MT2xJjjft8nimGNqXpQwRMmH/vfp+r0"
            + "LyYUEKb3xw5GTKrEOmRrYUOEKDRTj64HeMoNCtlmYY05DiIfhL2XspewAiK5pt+N8lYVKPeIkmXrx0jo"
            + "Whj9W9iHgqOOoJTBk6zLDg==";

        public enum Feature_YIOBI
        {
            Viewer = 4,
            CyberOptics = 6,
            EDAS = 7,
            Mounter = 8,
            SPI_DEMO = 9,
            AOI_DEMO = 11,
            NanoSystem = 13,
            NKBMI = 16,
            FoolProofSystem = 20,
            NK_Label = 21,
        }

        // ePM Product Feature Range -> 1 ~ 39 [YXGMI && YIOBI]
        // MES Product Feature Start Index -> 40 ~ [YXGMI]
        public enum Feature_YXGMI
        {
            NK_MES = 41,
            NK_MONITOR = 42,
            NK_EAP = 43,
        }

        public const string DefaultScope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?> " + "<haspscope/> ";
        public const string LocalScope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?> "
            + "<haspscope> "
            + "   <license_manager hostname =\"localhost\" /> "
            + "</haspscope>";

        public enum MaxMemory
        {
            HL_PRO = 48,
            HL_MAX = 400,
            HL_TIME = 512,
            SRM_PRO = 112,
            SRM_MAX = 4032,
            SRM_TIME = 4032,
        }

        public enum MemoryType
        {
            ePM_Ex = 1,
            Kohyoung = 10,
        }
    }
}
