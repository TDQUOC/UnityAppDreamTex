using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStruct;
using DreamTex.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace DreamTex.Layout
{
    public class MainLayout : Layout
    {
        [SerializeField] private Text summaryText;
        [SerializeField] private Button checkInBtn;
        [SerializeField] private Button checkOutBtn;

        public override void Show(Action callback = null)
        {
            base.Show((() => SetsumaryText()));
        }

        public void SetsumaryText()
        {
            HTTPManager.Instance.UpdateDaySummary();
            DaySummaryData daySummary = ApplicationController.Instance.DaySummaryData;
            EmployeeData employeeData = ApplicationController.Instance.EmployeeData;
            float totalEmployee = 0;
            float totalStore = 0;

            foreach (var item in daySummary.summaryBill)
            {
                totalStore += item.money;
                if (item.employeeId == employeeData._id)
                    totalEmployee += item.money;
            }

            bool checkCheckIn =
                string.IsNullOrEmpty(daySummary.checkIn.FirstOrDefault(l => l.employeeId == employeeData._id)?.time);
            string checkInTime =
                checkCheckIn
                    ? "Chưa check in"
                    : daySummary.checkIn.FirstOrDefault(l => l.employeeId == employeeData._id)?.time;
            checkInBtn.interactable = checkCheckIn;
            bool checkCheckOut =
                string.IsNullOrEmpty(daySummary.checkOut.FirstOrDefault(l => l.employeeId == employeeData._id)?.time);
            string checkOutTime =
                checkCheckOut
                    ? "Chưa check out"
                    : daySummary.checkOut.FirstOrDefault(l => l.employeeId == employeeData._id)?.time;
            checkOutBtn.interactable = checkCheckOut;
            summaryText.text = $"Tên cửa hàng: {daySummary.storeName}\n" +
                               $"Tên nhân viên: {employeeData.Name}\n" +
                               $"Giờ check in: {checkInTime}\n" +
                               $"Giờ check out: {checkOutTime}\n" +
                               $"Doanh thu ngày:\n" +
                               $"   +Nhân viên: {totalEmployee}\n" +
                               $"   +Cửa hàng: {totalStore}";
        }
    }
}