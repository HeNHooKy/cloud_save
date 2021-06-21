using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class LastUpdateProvider
    {
        //Мы должны хранить этот словарь в долговременной памяти устройства
        private static Dictionary<string, UserDataRecord> _lastUpdated;
        //Т.к. это не реальный проект, я не буду реализовывать работу с памятью

        public static Dictionary<string, UserDataRecord> Get()
        {
            //Здесь можно реализовать чтение из памяти
            return _lastUpdated;
        }

        public static void Set(Dictionary<string, UserDataRecord> lastUpdated)
        {
            //Здесь можно реализовать запись в память
            _lastUpdated = lastUpdated;
        }

        public static void ClearData()
        {
            _lastUpdated = null;
        }
    }

    
}
