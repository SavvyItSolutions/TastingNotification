using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastingsScheduler
{
    public class AmountsMovements
    {
        private int wineId;
        private string labelName;
        //private string barCode;
        private string customerId;
        private string token;
        private int deviceType;

        public int WineId
        {
            get { return this.wineId; }
            set { this.wineId = value; }
        }
        public string LabelName
        {
            get { return this.labelName; }
            set { this.labelName = value; }
        }
        //public string BarCode
        //{
        //    get { return this.barCode; }
        //    set { this.barCode = value; }
        //}
        public string CustomerId
        {
            get { return this.customerId; }
            set { this.customerId = value; }
        }
        public string Token
        {
            get { return this.token; }
            set { this.token = value; }
        }

        public int DeviceType
        {
            get { return this.deviceType; }
            set { this.deviceType = value; }
        }
    }
}
