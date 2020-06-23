using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Enums
{
    public enum RequestStatus
    {
        Waiting = 1,
        UnderConsideration = 2,
        Closed = 3,
        All = 4
    }

    public class RequestStatusHelper
    {
        public static string GetName(RequestStatus status)
        {
            switch (status)
            {
                case RequestStatus.Waiting:
                    return "Ожидает";
                case RequestStatus.UnderConsideration:
                    return "На рассмотрении";
                case RequestStatus.Closed:
                    return "Закрыта";
                case RequestStatus.All:
                    return "Все";
            }
            throw new ArgumentException("Выбранного статуса не найдено");
        }
    }
}