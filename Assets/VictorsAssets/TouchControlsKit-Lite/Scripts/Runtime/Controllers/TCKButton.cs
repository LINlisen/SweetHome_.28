/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

namespace TouchControlsKit
{
    public class TCKButton : ControllerBase,
        IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
    {
        public bool swipeOut = false;

        [Label("Normal Button")]
        public Sprite normalSprite;
        [Label("Pressed Button")]
        public Sprite pressedSprite;

        Hashtable hash;
        private InputUIManagement inputUIManagement;

        public Color32 pressedColor = new Color32(255, 255, 255, 165);

        int pressedFrame = -1
            , releasedFrame = -1
            , clickedFrame = -1;


        // isPRESSED
        internal bool isPRESSED { get { return touchDown; } }
        // isDOWN
        internal bool isDOWN { get { return (pressedFrame == Time.frameCount - 1); } }
        // isUP
        internal bool isUP { get { return (releasedFrame == Time.frameCount - 1); } }
        // isCLICK
        internal bool isCLICK { get { return (clickedFrame == Time.frameCount - 1); } }

        private void Start()
        {
            hash = PhotonNetwork.LocalPlayer.CustomProperties;
            inputUIManagement = GetComponentInParent<InputUIManagement>();
            Debug.Log("input");
            if (gameObject.name == "skillBtn")
            {
                Debug.Log(gameObject.name);
                switch ((int)hash["Charactor"])
                {
                    case 1:
                        normalSprite = inputUIManagement.CandySprite;
                        pressedSprite = inputUIManagement.CandySprite;
                        break;
                    case 2:
                        normalSprite = inputUIManagement.ChocolateSprite;
                        pressedSprite = inputUIManagement.ChocolateSprite;
                        break;
                }
            }
            if(gameObject.name == "dashBtn")
            {
                normalSprite = inputUIManagement.DashSprite;
            }
        }

        // Update Position
        protected override void UpdatePosition(Vector2 touchPos)
        {
            base.UpdatePosition(touchPos);

            if (touchDown == false)
            {
                touchDown = true;
                touchPhase = ETouchPhase.Began;
                pressedFrame = Time.frameCount;

                ButtonDown();
            }
        }


        // Button Down
        protected void ButtonDown()
        {
            baseImage.sprite = pressedSprite;
            baseImage.color = visible ? pressedColor : (Color32)Color.clear;
        }

        // Button Up
        protected void ButtonUp()
        {
            baseImage.sprite = normalSprite;
            baseImage.color = visible ? baseImageColor : (Color32)Color.clear;
        }

        // Control Reset
        protected override void ControlReset()
        {
            base.ControlReset();

            releasedFrame = Time.frameCount;
            ButtonUp();
        }

        // OnPointer Down
        public void OnPointerDown(PointerEventData pointerData)
        {
            if (touchDown == false)
            {
                touchId = pointerData.pointerId;
                UpdatePosition(pointerData.position);
            }
        }

        // OnDrag
        public void OnDrag(PointerEventData pointerData)
        {
            if (Input.touchCount >= touchId && touchDown)
            {
                UpdatePosition(pointerData.position);
                Debug.Log(pointerData.position);
                Debug.Log(pointerData.scrollDelta);
            }
        }

        // OnPointer Exit
        public void OnPointerExit(PointerEventData pointerData)
        {
            if (swipeOut == false)
            {
                OnPointerUp(pointerData);
            }
        }

        // OnPointer Up
        public void OnPointerUp(PointerEventData pointerData)
        {
            ControlReset();
        }

        // OnPointer Click
        public void OnPointerClick(PointerEventData pointerData)
        {
            clickedFrame = Time.frameCount;
        }
    };
}