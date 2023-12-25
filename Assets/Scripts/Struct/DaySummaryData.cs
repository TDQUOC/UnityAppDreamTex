using System.Collections.Generic;

namespace DataStruct
{
    [System.Serializable]
    public class DaySummaryData
    {
        public string _id;
        public string date;
        public string storeId;
        public string storeName;
        public List<SummaryBill> summaryBill;
        public List<ImportOrExport> importOrExport;
        public List<CheckIn> checkIn;
        public List<CheckOut> checkOut;
    }

    [System.Serializable]
    public class CheckOut
    {
        public string employeeId;
        public string employeeName;
        public string time;
        public string image;
        public string imageFullPath;
    }

    [System.Serializable]
    public class CheckIn
    {
        public string employeeId;
        public string employeeName;
        public string time;
        public string image;
        public string imageFullPath;
    }

    [System.Serializable]
    public class ImportOrExport
    {
        public string employeeId;
        public string employeeName;
        public string importOrExport;
        public List<DetailProduct> detail;
        public string note;
        public string time;
    }

    [System.Serializable]
    public class SummaryBill
    {
        public string billId;
        public string employeeId;
        public string employeeName;
        public string guestId;
        public string guestName;
        public List<DetailProduct> detail;
        public float money;
        public string note;
    }

    [System.Serializable]
    public class DetailProduct
    {
        public string productId;
        public string productName;
        public int quantity;
        public float price;
        public float money;
    }
}