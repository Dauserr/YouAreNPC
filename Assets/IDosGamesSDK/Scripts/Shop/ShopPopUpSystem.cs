using System;
using UnityEngine;

namespace IDosGames
{
    public class ShopPopUpSystem : MonoBehaviour
    {
        [SerializeField] private GameObject _shopWindow;
        [SerializeField] private PopUp _vipPopUp;
        [SerializeField] private PopUp _tokenPopUp;
        [SerializeField] private PopUp _coinPopUp;
        [SerializeField] private PopUp _wkPopUp;
        [SerializeField] private PopUp _spinTicketPopUp;
        [SerializeField] private PopUpConfirmation _confirmationPopUp;

        private void Awake()
        {
            HideAllPopUps();
        }

        public void HideAllPopUps()
        {
            SetActivatePopUp(_vipPopUp, false);
            SetActivatePopUp(_spinTicketPopUp, false);
            SetActivatePopUp(_wkPopUp, false);
            SetActivatePopUp(_tokenPopUp, false);
            SetActivatePopUp(_coinPopUp, false);
            SetActivatePopUp(_confirmationPopUp, false);
            
            // Resume game when all popups are hidden
            Time.timeScale = 1f;
        }

        private void SetActivatePopUp(PopUp popUp, bool active)
        {
            if (popUp == null)
            {
                return;
            }

            popUp.gameObject.SetActive(active);
            
            // Freeze/unfreeze game when showing/hiding popup
            if (active)
            {
                Time.timeScale = 0f; // Freeze when opening
            }
            else
            {
                Time.timeScale = 1f; // Resume when closing
            }
        }

        public void ShowShopWindow()
        {
            _shopWindow.SetActive(true);
        }

        public void ShowTokenPopUp()
        {
            SetActivatePopUp(_tokenPopUp, true);
        }

        public void ShowCoinPopUp()
        {
            SetActivatePopUp(_coinPopUp, true);
        }

        public void ShowWKPopUp()
        {
            SetActivatePopUp(_wkPopUp, true);
        }

        public void ShowSpinTicketPopUp()
        {
            SetActivatePopUp(_spinTicketPopUp, true);
        }

        public void ShowVIPPopUp()
        {
            SetActivatePopUp(_vipPopUp, true);
        }

        public void ShowConfirmationPopUp(Action confirmAction, string productName, string price, Sprite currencyIcon)
        {
            _confirmationPopUp.FullSet(confirmAction, productName, price, currencyIcon);
            SetActivatePopUp(_confirmationPopUp, true);
        }
    }
}
