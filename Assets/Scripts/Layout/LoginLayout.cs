using System;
using System.Collections;
using System.Linq;
using DataStruct;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DreamTex.Layout
{
    public class LoginLayout : Layout
    {
        public static System.Action<ResponeData> OnLoginRes;

        [SerializeField] private InputField usernameInputField;
        [SerializeField] private InputField passwordInputField;
        [SerializeField] private Button loginButton;

        private void OnEnable()
        {
            OnLoginRes += OnLoginResponse;
        }

        private void OnDisable()
        {
            OnLoginRes -= OnLoginResponse;
        }

        private void OnLoginResponse(ResponeData obj)
        {
            if (obj.IsSuccess)
            {
                Debug.Log(obj.Data.ToString());
                JObject jsonObject = JObject.Parse(obj.Data.ToString());

                string employeeData = jsonObject.Properties().FirstOrDefault(item => item.Name.ToLower() == "employee")
                    ?.Value.ToString();
                string daySummaryDataString = jsonObject.Properties()
                    .FirstOrDefault(item => item.Name.ToLower() == "daysummary")?.Value.ToString();
                EmployeeData data = JsonConvert.DeserializeObject<EmployeeData>(employeeData);
                DaySummaryData daySummaryData = JsonConvert.DeserializeObject<DaySummaryData>(daySummaryDataString);
                ApplicationController.Instance.SetEmployeeData(data);
                ApplicationController.Instance.SetDaySummaryData(daySummaryData);

                LayoutController.Instance.ShowNotifyLayout("Đăng nhập thành công!",
                    () => { ApplicationController.Instance.ShowMainLayout(); });
            }
            else
            {
                LayoutController.Instance.ShowNotifyLayout("Đăng nhập thất bại!");
                Debug.LogWarning(obj.Msg.ToString());
            }
        }


        private void Start()
        {
            loginButton.onClick.AddListener(OnLickLogin);

            Show();
        }

        public void OnLickLogin()
        {
            try
            {
                LayoutController.Instance.ShowNotifyLayout("Đang đăng nhập...", null, false);
                string userName = usernameInputField.text;
                string password = passwordInputField.text;
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    Debug.LogWarning("Không được để trống tài khoản hoặc mật khẩu!");
                    return;
                }

                StartCoroutine(IESendLogin(userName, password));

            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            IEnumerator IESendLogin(string userName, string password)
            {
                yield return new WaitForSeconds(1);
                HTTPManager.Instance.Login(userName, password);
            }
        }
    }
}